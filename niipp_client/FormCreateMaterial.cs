using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NiippClient.Properties;
using NIIPP.DatabaseClient.Library;
using NIIPP.DatabaseClient.DataStorage;

namespace NiippClient
{
    public partial class FormCreateMaterial : Form
    {
        // абсолютные пути к файлам
        String _pathToSomeFile = "",
               _pathToPassFile = "";

        /// <summary>
        /// Записи пластин в БД
        /// </summary>
        private readonly List<SqlRecord> _records = new List<SqlRecord>();

        /// <summary>
        /// Массив пар (название таблицы с эпит. структурой - стек слоев данной структуры)
        /// </summary>
        private Dictionary<string, string> _epitTablesInfo = new Dictionary<string, string>();





        public FormCreateMaterial()
        {
            InitializeComponent();
        }

        private bool RecordOfNewWafers()
        {
            List<int> numbers;
            try
            {
                numbers = GetNumbersOfWafers(textBoxWaferNumber.Text);
            }
            catch 
            {
                return false;
            }

            // сохранение данных
            foreach (int i in numbers)
            {
                // создаем запись и сохраняем первичные данные
                SqlRecord record = new SqlRecord(TbMaterials.Name, TbMaterials.MaterialId, String.Format("{0}_{1}", textBoxParcelNumber.Text, i));
                if (!record.Exist)
                {
                    record.SetField(TbMaterials.NumberOfParcel, textBoxParcelNumber.Text);
                    record.SetField(TbMaterials.NumberOfWafer, i);
                    _records.Add(record);
                }
                else
                {
                    _records.Clear();
                    MessageBox.Show("Одна из указанных пластин уже существует!");
                    return false;
                }
            }

            textBoxParcelNumber.ReadOnly = true;
            textBoxWaferNumber.ReadOnly = true;

            return true;
        }

        private List<int> GetNumbersOfWafers(string text)
        {
            List<int> res = new List<int>();
            string[] tokens = text.Split(',');
            foreach (string token in tokens)
                res.AddRange(ProcessToken(token));

            res = res.Distinct().ToList();
            res.Sort();

            return res;
        }

        private List<int> ProcessToken(string token)
        {
            List<int> res = new List<int>();

            int divisor = token.IndexOf('-');
            if (divisor != -1)
            // парсим группу пластин
            {
                int first = Int32.Parse(token.Substring(0, divisor));
                int last = Int32.Parse(token.Substring(divisor + 1, token.Length - divisor - 1));

                if (last < first)
                {
                    int copy = first;
                    first = last;
                    last = copy;
                }
                for (int i = first; i <= last; i++)
                    res.Add(i);
            }
            // парсим одну пластину
            else
            {
                res.Add(Int32.Parse(token));
            }

            return res;
        }

        private void SwitchingOfCreateButton()
        {
            switch (buttonCreate.Text)
            {
                case "Создать":
                    {
                        if (!RecordOfNewWafers())
                            return;
                        OnCreateState();
                        break;
                    }
                case "Изменить":
                    {
                        OnChangeState();
                        break;
                    }
                case "Подтвердить изменение":
                    {
                        OnConfirmState();
                        break;
                    }
            }
        }

        string GetFileId(string nameOfFile, string addition)
        {
            return Path.GetFileNameWithoutExtension(nameOfFile) + " (" + addition + ")" + Path.GetExtension(nameOfFile);
        }

        bool SaveInfoAboutMaterial()
        {
            // Проверка числовых данных на валидность
            // проверка толщины
            double? thickness = ClientLibrary.DoubleParse(cbThicknessOfWafer.Text);
            if (thickness == null)
                return false;
            if (comboBoxChooseWaferThickDimension.Text == "нм")
                thickness /= 1000;
            // проверка концентрации
            double? concentration = ClientLibrary.DoubleParse(cbConcOfWafer.Text);
            if (concentration == null)
                 return false;
            // проверка диаметра
            double? diameter = ClientLibrary.DoubleParse(cbWaferDiameter.Text);
            if (diameter == null)
                return false;
            // проверка сопротивления
            double? resistance = ClientLibrary.DoubleParse(cbWaferResistance.Text);
            if (resistance == null)
                return false;
            // проверка технологического процесса
            int? techProc = ClientLibrary.IntParse(cbTechProc.Text);
            if (techProc == null)
                return false;
            
            // сохраняем данные в записи
            foreach (SqlRecord rec in _records)
            {
                rec.SetField(TbMaterials.CreationDate, dateTimePickerRecordMaterial.Value.Date);
                rec.SetField(TbMaterials.AuthorOfRecord, comboBoxAuthor.Text);
                rec.SetField(TbMaterials.Comment, richTextBoxComment.Text);
                rec.SetField(TbMaterials.Technology, comboBoxTechnology.Text);
                rec.SetField(TbMaterials.StructureManufacturer, cbManufacturerOfStructure.Text);
                rec.SetField(TbMaterials.WaferManufacturer, cbWaferManufacturer.Text);
                rec.SetField(TbMaterials.TypeOfWafer, comboBoxTypeOfWafer.Text);
                rec.SetField(TbMaterials.Ingot, textBoxIngot.Text);
                rec.SetField(TbMaterials.MaterialOfWafer, comboBoxMaterialOfWafer.Text);
                rec.SetField(TbMaterials.Corresponds, cbCorrespond.Text);
                if (cbThicknessOfWafer.Text != "")
                    rec.SetField(TbMaterials.ThicknessOfWafer, (double) thickness);
                if (cbConcOfWafer.Text != "")
                    rec.SetField(TbMaterials.ConcentrationInWafer, (double) concentration);
                if (cbTechProc.Text != "")
                    rec.SetField(TbMaterials.TechProc, (int) techProc);
                if (cbWaferDiameter.Text != "")
                    rec.SetField(TbMaterials.WaferDiameter, (double) diameter);
                if (cbWaferResistance.Text != "")
                    rec.SetField(TbMaterials.WaferResistance, (double) resistance);
                rec.Save();
            }

            // если выбраны файлы, то загружаем их на сервер
            string id = textBoxParcelNumber.Text + "_" + textBoxWaferNumber.Text;

            // загружаем файл со сканом эпитаксиальной структуры
            if (_pathToPassFile != "")
            {
                string idPassScanFile = GetFileId(Path.GetFileName(_pathToPassFile), id + " @ pass_scan");
                bool successOfUpload = ClientLibrary.UploadFileToServer(_pathToPassFile, idPassScanFile);
                // создаем ссылки для каждой пластины
                if (successOfUpload)
                    foreach (SqlRecord rec in _records)
                        rec.SetField(TbMaterials.RecordIdOfEpitStructureFile, idPassScanFile);
            }

            // загружаем дополнительный файл
            if (_pathToSomeFile != "")
            {
                string idSomeFile = GetFileId(Path.GetFileName(_pathToSomeFile), id + " @ some_file");
                bool successOfUpload = ClientLibrary.UploadFileToServer(_pathToSomeFile, idSomeFile);
                // создаем ссылки для каждой пластины
                if (successOfUpload)
                    foreach (SqlRecord rec in _records)
                        rec.SetField(TbMaterials.RecordIdOfAttachmentFile, idSomeFile);
            }
            
            return true;
        }

        private void ActionToTextChangedInMainInfo(object sender, EventArgs e)
        {
            buttonSaveMaterial.Text = "Сохранить *";
        }

        private void SaveEpitStructureRecord()
        {
            foreach (SqlRecord rec in _records)
            {
                rec.SetField(TbMaterials.NameOfTableWithEpitStructure, 
                    TbEpitStructure.Name(String.Format("{0}_{1}", textBoxParcelNumber.Text, textBoxWaferNumber.Text)));
                rec.Save();
            }
        }

        private bool CreateEpitStructure()
        {
            string nameOfEpitStructureTable = TbEpitStructure.Name(textBoxParcelNumber.Text + "_" + textBoxWaferNumber.Text);

            // создаем имя структуры
            string nameForUser = GetEpitStack();
            // делаем запись в таблице 'TbEpitHost'
            SqlRecord recEpitHost = new SqlRecord(TbEpitHost.Name, TbEpitHost.NameOfEpitStructureTable, nameOfEpitStructureTable);
            recEpitHost.SetField(TbEpitHost.UserName, nameForUser);
            recEpitHost.SetField(TbEpitHost.TimeOfCreation, DateTime.Now);
            recEpitHost.Save();

            // создаем и заполняем таблицу 'TbEpitStructure'
            SqlTable tbEpitStruct = new SqlTable(nameOfEpitStructureTable);
            if (tbEpitStruct.Exist)
                tbEpitStruct.Remove();
            tbEpitStruct.Create(TbEpitStructure.Number, typeof (int));

            for (int i = 0; i < dataGridViewLayers.RowCount - 1; i++)
            {
                int num = dataGridViewLayers.RowCount - i - 2;

                SqlRecord recEpitStruct = tbEpitStruct.GetRecord(TbEpitStructure.Number, num);
                recEpitStruct.CreateFieldsIfNotExist = true;

                // валидация данных
                double thickness = 0,
                       conc = 0;
                double? readThickness = ClientLibrary.DoubleParse(dataGridViewLayers.Rows[i].Cells[3].Value.ToString()),
                        readConc = ClientLibrary.DoubleParse(dataGridViewLayers.Rows[i].Cells[4].Value.ToString());
                if (readThickness == null || readConc == null)
                {
                    tbEpitStruct.Remove();
                    recEpitHost.Remove();
                    return false;
                }
                thickness = (double) readThickness;
                conc = (double) readConc;

                recEpitStruct.SetField(TbEpitStructure.TypeOfLayer, dataGridViewLayers.Rows[i].Cells[1].Value.ToString());
                recEpitStruct.SetField(TbEpitStructure.Material, dataGridViewLayers.Rows[i].Cells[2].Value.ToString());
                recEpitStruct.SetField(TbEpitStructure.Thickness, thickness);
                recEpitStruct.SetField(TbEpitStructure.Concentration, conc);
                recEpitStruct.Save();
            }

            return true;
        }

        private string GetEpitStack()
        {
            string nameForUser = "";
            for (int i = 0; i <= dataGridViewLayers.RowCount - 2; i++)
            {
                nameForUser += dataGridViewLayers.Rows[i].Cells[1].Value.ToString();
                if (i < dataGridViewLayers.RowCount - 2)
                    nameForUser += " / ";
            }
            nameForUser += "_sub";
            return nameForUser;
        }

        private void ActionToTextChangedInEpitStr(object sender, EventArgs e)
        {
            buttonSaveEpitStruct.Text = "Сохранить *";
        }

        private void ChooseLayerUpDownPicture()
        {
            pictureBoxLayerOrder.Image = radioButtonToWafer.Checked ? Resources.layerDownPicture : Resources.layerUpPicture;
        }

        private void AddLayerFromWafer()
        {
            String layerName,
                   layerMaterial;
            double thick = 0,
                   conc;

            try
            {
                layerName = cbLayerType.Text;
                layerMaterial = comboBoxLayerMaterial.Text;
                if (comboBoxChooseLayerThickDimension.Text == "мкм")
                    thick = Double.Parse(ClientLibrary.DoubleProcess(textBoxLayerThick.Text));
                if (comboBoxChooseLayerThickDimension.Text == "нм")
                    thick = Double.Parse(ClientLibrary.DoubleProcess(textBoxLayerThick.Text)) / 1000.0;
                conc = Double.Parse(ClientLibrary.DoubleProcess(textBoxLayerConc.Text));
            }
            catch
            {
                MessageBox.Show("Проверьте корректность введеных данных", "Ошибка!");
                return;
            }

            dataGridViewLayers.RowCount++;
            ClientLibrary.MoveRowsDown(dataGridViewLayers);
            dataGridViewLayers.Rows[0].Cells[0].Value = (dataGridViewLayers.RowCount - 2).ToString() + " слой";
            dataGridViewLayers.Rows[0].Cells[1].Value = layerName;
            dataGridViewLayers.Rows[0].Cells[2].Value = layerMaterial;
            dataGridViewLayers.Rows[0].Cells[3].Value = thick.ToString();
            dataGridViewLayers.Rows[0].Cells[4].Value = conc.ToString("0.0##E+00");
        }

        private void AddLayerToWafer()
        {
            String layerName,
                   layerMaterial;
            double thick = 0,
                   conc;

            try
            {
                layerName = cbLayerType.Text;
                layerMaterial = comboBoxLayerMaterial.Text;
                if (comboBoxChooseLayerThickDimension.Text == "мкм")
                    thick = Double.Parse(ClientLibrary.DoubleProcess(textBoxLayerThick.Text));
                if (comboBoxChooseLayerThickDimension.Text == "нм")
                    thick = Double.Parse(ClientLibrary.DoubleProcess(textBoxLayerThick.Text)) / 1000.0;
                conc = Double.Parse(ClientLibrary.DoubleProcess(textBoxLayerConc.Text));
            }
            catch
            {
                MessageBox.Show("Проверьте корректность введеных данных", "Ошибка!");
                return;
            }

            dataGridViewLayers.RowCount++;
            int count = dataGridViewLayers.RowCount - 1;

            ClientLibrary.CopyRows(dataGridViewLayers, count - 1, count - 2);
            
            dataGridViewLayers.Rows[count - 2].Cells[1].Value = layerName;
            dataGridViewLayers.Rows[count - 2].Cells[2].Value = layerMaterial;
            dataGridViewLayers.Rows[count - 2].Cells[3].Value = thick.ToString();
            dataGridViewLayers.Rows[count - 2].Cells[4].Value = conc.ToString("0.0##E+00");

            int num = 0;
            for (int i = count - 2; i >= 0; i--)
            {
                num++;
                dataGridViewLayers.Rows[i].Cells[0].Value = num + " слой";
            }
        }

        private void AddWaferLayer()
        {
            double thick,
                   conc;
            try
            {
                thick = Double.Parse(ClientLibrary.DoubleProcess(cbThicknessOfWafer.Text));
                if (comboBoxChooseWaferThickDimension.Text == "нм")
                    thick /= 1000.0;
            }
            catch
            {
                thick = 0;
            }

            try
            {
                conc = Double.Parse(ClientLibrary.DoubleProcess(cbConcOfWafer.Text));
            }
            catch
            {
                conc = 0;
            }

            string layerName = comboBoxTypeOfWafer.Text;
            string layerMaterial = comboBoxMaterialOfWafer.Text;

            dataGridViewLayers.RowCount++;
            ClientLibrary.MoveRowsDown(dataGridViewLayers);
            dataGridViewLayers.Rows[0].Cells[0].Value = "Подложка";
            dataGridViewLayers.Rows[0].Cells[1].Value = layerName;
            dataGridViewLayers.Rows[0].Cells[2].Value = layerMaterial;
            dataGridViewLayers.Rows[0].Cells[3].Value = thick.ToString();
            dataGridViewLayers.Rows[0].Cells[4].Value = conc.ToString("0.0##E+00");

        }

        void AcceptRecord()
        {
            // ставим все подложки проверенными
            foreach (SqlRecord rec in _records)
            {
                rec.SetField(TbMaterials.VerificationStatus, "YES");
                rec.Save();
            }
            MessageBox.Show("Запись успешно сохранена", "Подтверждение");

            // обнуляем все элементы управления
            InitializeForNewRecord();
            tabControlCreateMaterial.SelectedIndex = 0;

            this.Close();
        }

        private bool ValidationBeforeEpitStructureConfigEnter()
        {
            if (buttonCreate.Text == "Создать" || buttonCreate.Text == "Подтвердить изменение")
            {
                tabControlCreateMaterial.SelectedIndex = 0;
                MessageBox.Show("Запись не была создана", "Предупреждение!");
                return false;
            }
            if (buttonSaveMaterial.Text == "Сохранить *")
            {
                tabControlCreateMaterial.SelectedIndex = 0;
                MessageBox.Show("Информация введенная во вкладке `Главная информация` не была сохранена", "Предупреждение!");
                return false;
            }
            return true;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            SwitchingOfCreateButton();
        }

        private void formCreateMaterial_Load(object sender, EventArgs e)
        {
            InitializeControls();
        }

        private void buttonSaveMaterial_Click(object sender, EventArgs e)
        {
            // запись информации о материале в tb_materials
            if (!SaveInfoAboutMaterial())
            {
                return;
            }

            buttonSaveMaterial.Text = "Сохранить";
        }

        private void comboBoxManufacturer_TextChanged(object sender, EventArgs e)
        {
            labelManufacturerShow.Text = cbManufacturerOfStructure.Text;
        }

        private void buttonSaveEpitStruct_Click(object sender, EventArgs e)
        {
            bool ok = CreateEpitStructure();

            if (ok)
            {
                SaveEpitStructureRecord();
                buttonSaveEpitStruct.Text = "Сохранить";
            }
        }

        public void LoadEpitTableToDataGrid(String table, DataGridView dgv)
        {
            SqlSelect sqlSelect = new SqlSelect(table);
            sqlSelect.OrderByField = TbEpitStructure.Number;
            sqlSelect.SortIsAscending = false;
            sqlSelect.RetrieveData();

            dgv.Rows.Clear();
            dgv.RowCount = sqlSelect.CountOfRows + 1;

            for (int i = 0; i < sqlSelect.CountOfRows; i++)
            {
                int number = (int)sqlSelect.GetValueOfField(TbEpitStructure.Number, i);
                dgv.Rows[i].Cells[0].Value = number != 0 ? number + " слой" : "Подложка";
                dgv.Rows[i].Cells[1].Value = sqlSelect.GetValueOfField(TbEpitStructure.TypeOfLayer, i);
                dgv.Rows[i].Cells[2].Value = sqlSelect.GetValueOfField(TbEpitStructure.Material, i);
                dgv.Rows[i].Cells[3].Value = sqlSelect.GetValueOfField(TbEpitStructure.Thickness, i) + "?";
                dgv.Rows[i].Cells[4].Value =
                    ((double)sqlSelect.GetValueOfField(TbEpitStructure.Concentration, i)).ToString("0.0##E+00") + "?";
            }
        }

        private void comboBoxListOfEpitStructure_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxAddLayer.Enabled = false;
            groupBoxLayerNumeration.Enabled = false;

            string epitStack = comboBoxListOfEpitStructure.Text;
            string nameOfOriginTable = _epitTablesInfo.FirstOrDefault(x => x.Value == epitStack).Key;

            LoadEpitTableToDataGrid(nameOfOriginTable, dataGridViewLayers);
        }

        private void buttonAddLayer_Click(object sender, EventArgs e)
        {
            groupBoxLayerNumeration.Enabled = false;
            comboBoxListOfEpitStructure.Enabled = false;
            buttonSaveEpitStruct.Text = "Сохранить *";

            if (radioButtonFromWafer.Checked)
                AddLayerFromWafer();
            else
                AddLayerToWafer();
        }

        private void tabPageFinalView_Enter(object sender, EventArgs e)
        {
            if (!ValidationBeforeInterToFinalView())
                return;

            PrepareInfoForFinalView();
        }

        private void tabPageEpitStructureConfig_Enter(object sender, EventArgs e)
        {
            if (!ValidationBeforeEpitStructureConfigEnter())
                return;
            if (dataGridViewLayers.Rows.Count <= 2)
            {
                dataGridViewLayers.Rows.Clear();
                AddWaferLayer();
            }
        }

        private void buttonAcceptRecord_Click(object sender, EventArgs e)
        {
            AcceptRecord();
        }

        private void pictureBoxPassport_Click(object sender, EventArgs e)
        {
            ClientLibrary.OpenFile(_pathToPassFile);
        }

        private void radioButtonFromWafer_CheckedChanged(object sender, EventArgs e)
        {
            ChooseLayerUpDownPicture();
        }

        private void labelPassportScan_Click(object sender, EventArgs e)
        {
            ClientLibrary.OpenExplorerAndChooseFile(_pathToPassFile);
        }

        private void labelAttachedFile_Click(object sender, EventArgs e)
        {
            ClientLibrary.OpenExplorerAndChooseFile(_pathToSomeFile);
        }

        private void textBoxParcelNumber_TextChanged(object sender, EventArgs e)
        {
            labelParcelShow.Text = textBoxParcelNumber.Text;
        }

        private void pictureBoxSomeFile_Click(object sender, EventArgs e)
        {
            _pathToSomeFile = ClientLibrary.ChooseFile(textBoxFile);
        }

        private void pictureBoxPassportScan_Click(object sender, EventArgs e)
        {
            _pathToPassFile = ClientLibrary.ChooseFile(textBoxPassportScan);
            if (_pathToPassFile != "")
                ClientLibrary.LoadImageToPictureBox(_pathToPassFile, pictureBoxPassport);
        }

        private void OnConfirmState()
        {
            buttonCreate.Text = "Изменить";
            gbStructureInfo.Enabled = true;
            gbWaferInfo.Enabled = true;
            gbMainInfo.Enabled = true;
            groupBoxCreateEpitStructure.Enabled = true;
            groupBoxEpitMain.Enabled = true;
            buttonSaveMaterial.Enabled = true;
            textBoxParcelNumber.ReadOnly = true;
            textBoxWaferNumber.ReadOnly = true;
            buttonSaveMaterial.Text = "Сохранить *";
        }

        private void OnChangeState()
        {
            textBoxParcelNumber.ReadOnly = false;
            textBoxWaferNumber.ReadOnly = false;
            buttonCreate.Text = "Подтвердить изменение";
            gbStructureInfo.Enabled = false;
            gbWaferInfo.Enabled = false;
            gbMainInfo.Enabled = false;
            groupBoxCreateEpitStructure.Enabled = false;
            groupBoxEpitMain.Enabled = false;
            buttonSaveMaterial.Enabled = false;
        }

        private void OnCreateState()
        {
            buttonCreate.Text = "Изменить";
            gbStructureInfo.Enabled = true;
            gbWaferInfo.Enabled = true;
            gbMainInfo.Enabled = true;
            groupBoxCreateEpitStructure.Enabled = true;
            groupBoxEpitMain.Enabled = true;
            buttonSaveMaterial.Enabled = true;
        }

        private Dictionary<string, string> GetEpitTablesInfo()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            SqlSelect sqlSelect = new SqlSelect(TbEpitHost.Name);
            sqlSelect.RetrieveData();
            DataTable dt = sqlSelect.DataTable;

            foreach (DataRow row in dt.Rows)
                res.Add(row[TbEpitHost.NameOfEpitStructureTable].ToString(), row[TbEpitHost.UserName].ToString());

            return res;
        }

        private void InitializeControls()
        {
            comboBoxAuthor.Text = ClientLibrary.GetAuthorOfComputer();
            
            ClientLibrary.InitComboBoxFromDb(ref cbWaferManufacturer, TbMaterials.Name, TbMaterials.WaferManufacturer);
            ClientLibrary.InitComboBoxFromDb(ref comboBoxTypeOfWafer, TbMaterials.Name, TbMaterials.TypeOfWafer);
            ClientLibrary.InitComboBoxFromDb(ref comboBoxMaterialOfWafer, TbMaterials.Name, TbMaterials.MaterialOfWafer);
            ClientLibrary.InitComboBoxFromDb(ref cbWaferDiameter, TbMaterials.Name, TbMaterials.WaferDiameter);
            ClientLibrary.InitComboBoxFromDb(ref cbConcOfWafer, TbMaterials.Name, TbMaterials.ConcentrationInWafer);
            ClientLibrary.InitComboBoxFromDb(ref cbWaferResistance, TbMaterials.Name, TbMaterials.WaferResistance);
            ClientLibrary.InitComboBoxFromDb(ref cbThicknessOfWafer, TbMaterials.Name, TbMaterials.ThicknessOfWafer);

            ClientLibrary.InitComboBoxFromDb(ref comboBoxTechnology, TbMaterials.Name, TbMaterials.Technology);
            ClientLibrary.InitComboBoxFromDb(ref cbTechProc, TbMaterials.Name, TbMaterials.TechProc);
            ClientLibrary.InitComboBoxFromDb(ref cbManufacturerOfStructure, TbMaterials.Name, TbMaterials.StructureManufacturer);
            ClientLibrary.InitComboBoxFromDb(ref cbCorrespond, TbMaterials.Name, TbMaterials.Corresponds);
            ClientLibrary.InitComboBoxFromDb(ref comboBoxAuthor, TbMaterials.Name, TbMaterials.AuthorOfRecord);


            _epitTablesInfo = GetEpitTablesInfo();
            comboBoxListOfEpitStructure.Items.Clear();
            comboBoxListOfEpitStructure.Items.AddRange( _epitTablesInfo.Values.Distinct().ToArray() );

            dataGridViewLayers.ColumnCount = 5;
            dataGridViewLayers.Columns[0].Name = "№";
            dataGridViewLayers.Columns[1].Name = "Тип слоя";
            dataGridViewLayers.Columns[2].Name = "Материал";
            dataGridViewLayers.Columns[3].Name = "Толщина, мкм";
            dataGridViewLayers.Columns[4].Name = "Концетрация, см^-3";
            dataGridViewLayers.BackgroundColor = SystemColors.Control;
            dataGridViewLayers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewLayers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewLayers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewLayers.AutoResizeColumns();

            dataGridViewShowLayers.BackgroundColor = SystemColors.Control;
            dataGridViewShowLayers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewShowLayers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewShowLayers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewShowLayers.AutoResizeColumns();

            gbStructureInfo.Enabled = false;
            gbMainInfo.Enabled = false;
            gbWaferInfo.Enabled = false;
            groupBoxEpitMain.Enabled = false;
            groupBoxCreateEpitStructure.Enabled = false;
            buttonSaveMaterial.Enabled = false;

            // делаем так, что при каждом изменении текста появляется звездочка нв кнопке сохранить
            cbWaferManufacturer.TextChanged += ActionToTextChangedInMainInfo;
            comboBoxTechnology.TextChanged += ActionToTextChangedInMainInfo;
            cbWaferManufacturer.TextChanged += ActionToTextChangedInMainInfo;
            textBoxIngot.TextChanged += ActionToTextChangedInMainInfo;
            cbCorrespond.TextChanged += ActionToTextChangedInMainInfo;
            textBoxPassportScan.TextChanged += ActionToTextChangedInMainInfo;
            textBoxFile.TextChanged += ActionToTextChangedInMainInfo;
            richTextBoxComment.TextChanged += ActionToTextChangedInMainInfo;
            dateTimePickerRecordMaterial.TextChanged += ActionToTextChangedInMainInfo;
            comboBoxAuthor.TextChanged += ActionToTextChangedInMainInfo;
            comboBoxTypeOfWafer.TextChanged += ActionToTextChangedInMainInfo;
            comboBoxMaterialOfWafer.TextChanged += ActionToTextChangedInMainInfo;
            cbConcOfWafer.TextChanged += ActionToTextChangedInMainInfo;
            cbThicknessOfWafer.TextChanged += ActionToTextChangedInMainInfo;
            cbTechProc.TextChanged += ActionToTextChangedInMainInfo;
            comboBoxListOfEpitStructure.TextChanged += ActionToTextChangedInEpitStr;

            labelPassportScan.MouseEnter += ClientLibrary.OnMouseEnterLabel;
            labelAttachedFile.MouseEnter += ClientLibrary.OnMouseEnterLabel;
            labelPassportScan.MouseLeave += ClientLibrary.OnMouseLeaveLabel;
            labelAttachedFile.MouseLeave += ClientLibrary.OnMouseLeaveLabel;
        }

        private bool ValidationBeforeInterToFinalView()
        {
            if (buttonCreate.Text == "Создать" || buttonCreate.Text == "Подтвердить изменение")
            {
                tabControlCreateMaterial.SelectedIndex = 0;
                MessageBox.Show("Запись не была создана", "Предупреждение!");
                return false;
            }
            if (buttonSaveMaterial.Text == "Сохранить *")
            {
                tabControlCreateMaterial.SelectedIndex = 0;
                MessageBox.Show("Информация введенная во вкладке `Главная информация` не была сохранена", "Предупреждение!");
                return false;
            }
            if (buttonSaveEpitStruct.Text == "Сохранить *")
            {
                tabControlCreateMaterial.SelectedIndex = 1;
                MessageBox.Show("Информация введенная во вкладке `Конфигурация эпит. структур` не была сохранена", "Предупреждение!");
                return false;
            }
            return true;
        }

        private void PrepareInfoForFinalView()
        {
            // общая информация
            labelParcelNumber.Text = textBoxParcelNumber.Text;
            labelNumberOfWafer.Text = textBoxWaferNumber.Text;
            labelTimeOfCreation.Text = dateTimePickerRecordMaterial.Value.ToShortDateString();
            labelPassportScan.Text = textBoxPassportScan.Text;
            labelAttachedFile.Text = textBoxFile.Text;
            labelComments.Text = richTextBoxComment.Text;
            labelAuthorOfMaterial.Text = comboBoxAuthor.Text;

            // информация о структуре
            labelNameOfTechnology.Text = comboBoxTechnology.Text;
            lblTechProc.Text = cbTechProc.Text;
            labelManufacturer.Text = cbManufacturerOfStructure.Text;
            labelCorrespond.Text = cbCorrespond.Text;
            labelTypeOfEpitStructure.Text = GetEpitStack();

            // информация о подложке
            labelIngot.Text = textBoxIngot.Text;
            labelWaferType.Text = comboBoxTypeOfWafer.Text;
            labelWaferConc.Text = cbConcOfWafer.Text;
            labelWafer.Text = cbWaferManufacturer.Text;
            labelWaferThick.Text = cbThicknessOfWafer.Text;
            labelWaferMaterial.Text = comboBoxMaterialOfWafer.Text;
            lblWaferDiameter.Text = cbWaferDiameter.Text;
            lblWaferResistance.Text = cbWaferResistance.Text;


            ClientLibrary.CopyInfoFromDataGridView(dataGridViewShowLayers, dataGridViewLayers);
        }

        private void InitializeForNewRecord()
        {
            _records.Clear();
            _epitTablesInfo.Clear();
            _pathToPassFile = "";
            _pathToSomeFile = "";

            buttonCreate.Text = "Создать";
            textBoxParcelNumber.ReadOnly = false;
            textBoxWaferNumber.ReadOnly = false;

            textBoxParcelNumber.Text = "";
            textBoxWaferNumber.Text = "";
            labelManufacturerShow.Text = "";
            labelParcelShow.Text = "";
            textBoxPassportScan.Text = "";
            cbLayerType.Text = "";
            textBoxLayerThick.Text = "";
            textBoxLayerConc.Text = "";
            cbCorrespond.Text = "";
            textBoxFile.Text = "";

            comboBoxAuthor.Text = "";
            textBoxIngot.Text = "";
            cbWaferManufacturer.Text = "";
            comboBoxTechnology.Text = "";
            cbManufacturerOfStructure.Text = "";
            dataGridViewLayers.Rows.Clear();
            dataGridViewShowLayers.Rows.Clear();
            dataGridViewShowLayers.Rows.Clear();

            richTextBoxComment.Text = "";

            gbStructureInfo.Enabled = false;
            gbWaferInfo.Enabled = false;
            gbMainInfo.Enabled = false;
            groupBoxEpitMain.Enabled = false;
            groupBoxCreateEpitStructure.Enabled = false;
            buttonSaveMaterial.Enabled = false;

            buttonSaveEpitStruct.Text = "Сохранить";
            buttonSaveMaterial.Text = "Сохранить";
        }

        private void dataGridViewLayers_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            buttonSaveEpitStruct.Text = "Сохранить *";
        }

        private void tabPageMainInfo_Click(object sender, EventArgs e)
        {

        }
    }
}
