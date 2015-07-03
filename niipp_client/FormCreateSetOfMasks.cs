using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;

using NIIPP.DatabaseClient.Library;
using NIIPP.DatabaseClient.DataStorage;
using NIIPP.DatabaseClient.NetworkFileManager;

namespace NiippClient
{
    public partial class FormCreateSetOfMasks : Form
    {
        public FormCreateSetOfMasks()
        {
            InitializeComponent();
        }

        private readonly string[] _nameOfColumns =
        {
            "№",
            "Метка",
            "Назначение",
            "Коэффициент",
            "Дата",
            "Комментарий",
            "Наличие золота"
        };

        private String
            _pathToTtWord = "",
            _pathToTtScan = "",
            _pathToMap = "",
            _pathToFolderWithMasks = "",
            _pathToAttachedFile = "";

        private SqlRecord _newRecord;
        private SqlTable _tbMasks;

        void InitializeDgv()
        {
            dataGridViewLayers.ColumnCount = _nameOfColumns.Length;
            for (int i = 0; i < _nameOfColumns.Length; i++)
                dataGridViewLayers.Columns[i].Name = _nameOfColumns[i];

            dataGridViewLayers.ReadOnly = true;
            dataGridViewLayers.Font = new Font("Microsoft Sans Serif", 9);
            dataGridViewLayers.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            dataGridViewLayers.BackgroundColor = SystemColors.Control;
            dataGridViewLayers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewLayers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewLayers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridViewLayers.AutoResizeColumns();
        }

        void AddLayer(DataGridView dg, String mark, String purpose, String coeff, DateTime time, String comment)
        {
            dg.RowCount++;
            int n = dg.RowCount - 2;
            dg.Rows[n].Cells[0].Value = (n + 1).ToString();
            dg.Rows[n].Cells[1].Value = mark;
            dg.Rows[n].Cells[2].Value = purpose;
            dg.Rows[n].Cells[3].Value = coeff;
            dg.Rows[n].Cells[4].Value = time.ToString(CultureInfo.CurrentCulture);
            dg.Rows[n].Cells[5].Value = comment;
            dg.Rows[n].Cells[6].Value = checkBoxAurum.Checked ? "да" : "нет";
        }

        string GetFileID(string nameOfFile, string addition)
        {
            return Path.GetFileNameWithoutExtension(nameOfFile) + " (" + addition + ")" + Path.GetExtension(nameOfFile);
        }

        String SaveSetOfMasks()
        {
             string nameSetOfMasks = "";
            // корректно вводим в базу данных id - имя комплекта фотошаблонов
            switch (buttonSaveMasks.Text)
            {
                case "Создать":
                    {
                        if (textBoxNameSetOfMasks.Text == "")
                            return "Вы не ввели информацию о названии комплекта фотошаблонов";
                        nameSetOfMasks = textBoxNameSetOfMasks.Text;

                        _newRecord = new SqlRecord(TbSetOfMasks.Name, TbSetOfMasks.NameOfSetOfMasksId, nameSetOfMasks);
                        if (_newRecord.Exist)
                            return "Указанный комплект фотошаблонов уже существует";

                        _tbMasks = new SqlTable(TbMasks.GetName(nameSetOfMasks));
                        CreateSetOfMaskTable();

                        break;
                    }
                case "Cохранить":
                    {
                        if (textBoxNameSetOfMasks.Text == "")
                            return "Вы не ввели информацию о названии комплекта фотошаблонов";
                        nameSetOfMasks = textBoxNameSetOfMasks.Text;

                        _newRecord.Remove();
                        _newRecord = new SqlRecord(TbSetOfMasks.Name, TbSetOfMasks.NameOfSetOfMasksId, nameSetOfMasks);
                        if (_newRecord.Exist)
                            return "Указанный комплект фотошаблонов уже существует";

                        _tbMasks.Remove();
                        _tbMasks = new SqlTable(TbMasks.GetName(nameSetOfMasks));
                        CreateSetOfMaskTable();

                        break;
                    }
            }

            // если выбраны файлы и папки, то заливаем их в БД
            if (_pathToTtWord != "")
            {
                string fileId = GetFileID(textBoxTTWord.Text, nameSetOfMasks + " @ TT_word");
                ClientLibrary.UploadFileToServer(_pathToTtWord, fileId);
                _newRecord.SetField(TbSetOfMasks.RecordIdofTrWordFile, fileId);
            }
            if (_pathToTtScan != "")
            {
                string fileId = GetFileID(textBoxTTScan.Text, nameSetOfMasks + " @ TT_scan");
                ClientLibrary.UploadFileToServer(_pathToTtScan, fileId);
                _newRecord.SetField(TbSetOfMasks.RecordIdofTrScanFile, fileId);
            }
            if (_pathToMap != "")
            {
                string fileId = GetFileID(textBoxMap.Text, nameSetOfMasks + " @ TT_map");
                ClientLibrary.UploadFileToServer(_pathToMap, fileId);
                _newRecord.SetField(TbSetOfMasks.RecordIdOfMapFile, fileId);
            }
            if (_pathToAttachedFile != "")
            {
                string fileId = GetFileID(textBoxAttachedFile.Text, nameSetOfMasks + " @ attached_file");
                ClientLibrary.UploadFileToServer(_pathToAttachedFile, fileId);
                _newRecord.SetField(TbSetOfMasks.RecordIdOfAttachedFile, fileId);
            }

            // архивация папки и загрузка архива в БД
            if (_pathToFolderWithMasks != "")
            {
                string programFolder = Path.GetDirectoryName(Application.ExecutablePath);
                string pathToSaveFolder = String.Format("{0}\\cache {1}", programFolder,
                    ClientLibrary.GetAuthorOfComputer());

                string destination = pathToSaveFolder + "\\" + Path.GetFileName(_pathToFolderWithMasks) + ".zip";
                FileInfo fi = new FileInfo(destination);
                if (fi.Exists)
                    fi.Delete();

                ZipFile.CreateFromDirectory(_pathToFolderWithMasks, destination, CompressionLevel.NoCompression, false, Encoding.UTF8);
                string fileId = GetFileID(textBoxFolderWithMasks.Text, nameSetOfMasks + " @ folder_masks") + ".zip";
                ClientLibrary.UploadFileToServer(destination, fileId);

                _newRecord.SetField(TbSetOfMasks.RecordIdOfFolderWithMasks, fileId);
            }

            // вводим в базу данных остальную информацию
            _newRecord.SetField(TbSetOfMasks.Description, richTextBoxComment.Text);
            _newRecord.SetField(TbSetOfMasks.Technology, comboBoxTechnology.Text);
            _newRecord.SetField(TbSetOfMasks.TimeOfRecordCreation, dateTimePickerRecordSetOfMasks.Value);
            _newRecord.SetField(TbSetOfMasks.Developer, comboBoxDeveloper.Text);
            _newRecord.SetField(TbSetOfMasks.Author, cbAuthor.Text);
            _newRecord.SetField(TbSetOfMasks.NameOfTableWithMasks, TbMasks.GetName(nameSetOfMasks));
            _newRecord.SetField(TbSetOfMasks.TechProc, cbTechProc.Text);
            _newRecord.SetField(TbSetOfMasks.TimeOfSetOfMasksCreation, dtpTimeOfSetOfMasksCreation.Value);

            _newRecord.Save();

            return "correct";
        }

        private void CreateSetOfMaskTable()
        {
            _tbMasks.Create(TbMasks.Number, SqlType.IntFormat);

            for (int i = 0; i < dataGridViewLayers.RowCount - 1; i++)
            {
                DataGridViewRow nextRow = dataGridViewLayers.Rows[i];
                String currNumber = nextRow.Cells[0].Value.ToString();
                DateTime dt = DateTime.Parse(nextRow.Cells[4].Value.ToString());

                double? dCoeff = ClientLibrary.DoubleParse(nextRow.Cells[3].Value.ToString());
                if (dCoeff == null)
                {
                    _tbMasks.Remove();
                    return;
                }

                SqlRecord newRecord = _tbMasks.GetRecord(TbMasks.Number, currNumber);

                // первый раз создаем поля, затем уже не нужно
                if (i == 0)
                    newRecord.CreateFieldsIfNotExist = true;

                newRecord.SetField(TbMasks.Mark, nextRow.Cells[1].Value.ToString());
                newRecord.SetField(TbMasks.Purpose, nextRow.Cells[2].Value.ToString());
                newRecord.SetField(TbMasks.Coeff, (double) dCoeff);
                newRecord.SetField(TbMasks.TimeOfCreation, dt);
                newRecord.SetField(TbMasks.Comment, nextRow.Cells[5].Value.ToString(), SqlType.TextFormat);
                newRecord.SetField(TbMasks.Aurum, nextRow.Cells[6].Value.ToString() == "да" ? "YES" : "NO");
                newRecord.Save();
            }
        }

        private void buttonAddLayer_Click(object sender, EventArgs e)
        {
            if (checkBoxAurum.Checked && ClientLibrary.DoubleParse(textBoxCoeff.Text) == null)
                return;
            AddLayer(dataGridViewLayers, textBoxMark.Text, textBoxPurpose.Text, textBoxCoeff.Text, dateTimePickerTimeOfLayerCreation.Value, textBoxLayerComment.Text);
        }

        private void FormCreateSetOfMasks_Load(object sender, EventArgs e)
        {
            InitializeDgv();

            cbAuthor.Text = ClientLibrary.GetAuthorOfComputer();
            ClientLibrary.InitComboBoxFromDb(ref comboBoxTechnology, TbMaterials.Name, TbMaterials.Technology);
            ClientLibrary.InitComboBoxFromDb(ref cbTechProc, TbMaterials.Name, TbMaterials.TechProc);
        }

        private void buttonSaveMaterial_Click(object sender, EventArgs e)
        {
            string res = SaveSetOfMasks();
            if (res != "correct")
                MessageBox.Show(res, "Ошибка ввода");
            else
            {
                buttonSaveMasks.Text = "Сохранить";
            }
        }

        private void pictureBoxTTWord_Click(object sender, EventArgs e)
        {
            _pathToTtWord = ClientLibrary.ChooseFile(textBoxTTWord);
        }

        private void pictureBoxTTScan_Click(object sender, EventArgs e)
        {
            _pathToTtScan = ClientLibrary.ChooseFile(textBoxTTScan);
        }

        private void pictureBoxMap_Click(object sender, EventArgs e)
        {
            _pathToMap = ClientLibrary.ChooseFile(textBoxMap);
        }

        private void pictureBoxFolderWithMasks_Click(object sender, EventArgs e)
        {
            _pathToFolderWithMasks = ClientLibrary.ChooseFolder(textBoxFolderWithMasks);
        }

        private void pictureBoxAttachedFile_Click(object sender, EventArgs e)
        {
            _pathToAttachedFile = ClientLibrary.ChooseFile(textBoxAttachedFile);
        }

        private void checkBoxAurum_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAurum.Checked)
            {
                textBoxCoeff.Visible = true;
                labelCoeff.Visible = true;
            }
            else
            {
                textBoxCoeff.Visible = false;
                labelCoeff.Visible = false;
                textBoxCoeff.Text = "";
            }
        }

        private void groupBoxAddLayer_Enter(object sender, EventArgs e)
        {

        }

        private void groupBoxFiles_Enter(object sender, EventArgs e)
        {

        }

    }
}
