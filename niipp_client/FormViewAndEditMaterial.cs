using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using NIIPP.DatabaseClient.DataStorage;
using NIIPP.DatabaseClient.Library;

namespace NiippClient
{
    public partial class FormViewAndEditMaterial : Form
    {
        private bool _editMode = true;
        private string _materialRecordId;

        private SqlSelect _currSqlSelect;
        private string _pathToAttFile;
        private string _pathToScanPassFile;

        private readonly string[] _nameOfColumns = {
                                     "№",
                                     "Тип слоя",
                                     "Материал",
                                     "Толщина, мкм",
                                     "Концентрация, см^-3"
                                 };

        private readonly string[] _indexOfColumns = {
                                     TbEpitStructure.Number,
                                     TbEpitStructure.TypeOfLayer,
                                     TbEpitStructure.Material,
                                     TbEpitStructure.Thickness,
                                     TbEpitStructure.Concentration
                                 };

        public FormViewAndEditMaterial()
        {
            InitializeComponent();
        }

        public void SetMaterialId(string id)
        {
            _materialRecordId = id;
            _currSqlSelect = new SqlSelect(TbMaterials.Name);
            _currSqlSelect.Equal(TbMaterials.MaterialId, id);
            _currSqlSelect.RetrieveData();

            ConstructFormWithValue();
        }

        private void InitDgvEpitStructure()
        {
            dgvEpitStructure.ColumnCount = _nameOfColumns.Length;
            for (int i = 0; i < _nameOfColumns.Length; i++)
                dgvEpitStructure.Columns[i].Name = _nameOfColumns[i];
            dgvEpitStructure.ReadOnly = true;
            dgvEpitStructure.Font = new Font("Microsoft Sans Serif", 8);
            dgvEpitStructure.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8);
            dgvEpitStructure.BackgroundColor = SystemColors.Control;
            dgvEpitStructure.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEpitStructure.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEpitStructure.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            string nameOfTableWithEpitStructure = (string) _currSqlSelect.GetValueOfField(TbMaterials.NameOfTableWithEpitStructure);
            SqlSelect sqlSelect = new SqlSelect(nameOfTableWithEpitStructure)
            {
                OrderByField = TbEpitStructure.Number,
                SortIsAscending = false
            };
            sqlSelect.RetrieveData();

            dgvEpitStructure.RowCount = sqlSelect.CountOfRows + 1;
            for (int i = 0; i < sqlSelect.CountOfRows; i++)
            {
                for (int col = 0; col < _nameOfColumns.Length; col++)
                {
                    string res = sqlSelect.GetValueOfField(_indexOfColumns[col], i).ToString();
                    if (col == 4)
                        res = ((double) sqlSelect.GetValueOfField(_indexOfColumns[col], i)).ToString("0.0##E+00");
                    if (col == 0 && res == "0")
                        res = "Подложка";

                    dgvEpitStructure.Rows[i].Cells[col].Value = res;
                }
            }

            dgvEpitStructure.AutoResizeColumns();
        }

        private void ConstructFormWithValue()
        {
            tbPartyNumber.Text = _currSqlSelect.GetValueOfField(TbMaterials.NumberOfParcel).ToString();
            tbWaferNumber.Text = _currSqlSelect.GetValueOfField(TbMaterials.NumberOfWafer).ToString();
            rtbComment.Text = _currSqlSelect.GetValueOfField(TbMaterials.Comment).ToString();
            cbAuthor.Text = _currSqlSelect.GetValueOfField(TbMaterials.AuthorOfRecord).ToString();
            cbTechnology.Text = _currSqlSelect.GetValueOfField(TbMaterials.Technology).ToString();
            tbIngot.Text = _currSqlSelect.GetValueOfField(TbMaterials.Ingot).ToString();
            tbCorrespond.Text =  _currSqlSelect.GetValueOfField(TbMaterials.Corresponds).ToString();
            cbWaferManufacturer.Text = _currSqlSelect.GetValueOfField(TbMaterials.WaferManufacturer).ToString();
            cbMaterialOfWafer.Text = _currSqlSelect.GetValueOfField(TbMaterials.MaterialOfWafer).ToString();
            cbTypeOfWafer.Text = _currSqlSelect.GetValueOfField(TbMaterials.TypeOfWafer).ToString();
            tbConcOfWafer.Text = _currSqlSelect.GetValueOfField(TbMaterials.ConcentrationInWafer).ToString();
            tbThickOfWafer.Text =  _currSqlSelect.GetValueOfField(TbMaterials.ThicknessOfWafer).ToString();
            dtpRecordMaterial.Value = (DateTime) _currSqlSelect.GetValueOfField(TbMaterials.CreationDate);
            cbManufacturer.Text =  _currSqlSelect.GetValueOfField(TbMaterials.StructureManufacturer).ToString();

            string strResistance =  _currSqlSelect.GetValueOfField(TbMaterials.WaferResistance).ToString();
            if (strResistance != "")
            {
                double resistance = Double.Parse(strResistance);
                cbWaferResistance.Text = resistance.ToString("0.0##E+00");
            }

            cbWaferDiameter.Text =  _currSqlSelect.GetValueOfField(TbMaterials.WaferDiameter).ToString();
            cbTechProc.Text =  _currSqlSelect.GetValueOfField(TbMaterials.TechProc).ToString();

            // получение стека эпитаксиальных структур
            string nameOfTableWithEpitStructure =  _currSqlSelect.GetValueOfField(TbMaterials.NameOfTableWithEpitStructure).ToString();
            SqlSelect sqlSelect = new SqlSelect(TbEpitHost.Name);
            sqlSelect.Equal(TbEpitHost.NameOfEpitStructureTable, nameOfTableWithEpitStructure);
            sqlSelect.RetrieveData();
            tbTypeOfEpitStruct.Text = sqlSelect.GetValueOfField(TbEpitHost.UserName).ToString();


            InitDgvEpitStructure();

            // обновляем контролы без изменения БД
            bool copy = _editMode;
            _editMode = false;
            string userValidation = _currSqlSelect.GetValueOfField(TbMaterials.VerificationStatus).ToString();
            cbUserValidation.CheckState = userValidation == "YES" ? CheckState.Checked : CheckState.Unchecked;
            string isLaunched = _currSqlSelect.GetValueOfField(TbMaterials.LaunchedStatus).ToString();
            cbIsLaunched.CheckState = isLaunched == "YES" ? CheckState.Checked : CheckState.Unchecked;
            _editMode = copy;

            string recordIdOfEpitStructureFile =  _currSqlSelect.GetValueOfField(TbMaterials.RecordIdOfEpitStructureFile).ToString();
            if (recordIdOfEpitStructureFile != "")
            {
                SqlSelect select1 = new SqlSelect(TbFilesPool.Name);
                select1.Equal(TbFilesPool.FileID, recordIdOfEpitStructureFile);
                select1.RetrieveData();
                lblOpenScanOfPassFile.Text = select1.GetValueOfField(TbFilesPool.NameOfFile).ToString();
                _pathToScanPassFile = ClientLibrary.DownloadFileFromServer(recordIdOfEpitStructureFile);
                if (_pathToScanPassFile != null)
                    pbEpitStructImage.Load(_pathToScanPassFile);
            }

            string recordIdOfAttachmentFile = _currSqlSelect.GetValueOfField(TbMaterials.RecordIdOfAttachmentFile).ToString();
            if (recordIdOfAttachmentFile != "")
            {
                SqlSelect select2 = new SqlSelect(TbFilesPool.Name);
                select2.Equal(TbFilesPool.FileID, recordIdOfAttachmentFile);
                select2.RetrieveData();
                lblOpenAttFile.Text = select2.GetValueOfField(TbFilesPool.NameOfFile).ToString();
                _pathToAttFile = ClientLibrary.DownloadFileFromServer(recordIdOfAttachmentFile);
            }
            
            MakeControlsReadOnly();
        }

        private void MakeControlsReadOnly()
        {
            foreach (var next in Controls)
            {
                if (next.GetType() == typeof (GroupBox))
                {
                    GroupBox gb = (GroupBox) next;
                    foreach (var control in gb.Controls)
                    {
                        if (control.GetType() == typeof (TextBox))
                        {
                            TextBox tb = (TextBox) control;
                            tb.ReadOnly = true;
                        }
                        if (control.GetType() == typeof (DateTimePicker))
                        {
                            DateTimePicker dtp = (DateTimePicker) control;
                            dtp.Enabled = false;
                        }
                        if (control.GetType() == typeof(RichTextBox))
                        {
                            RichTextBox rtb = (RichTextBox)control;
                            rtb.ReadOnly = true;
                        }
                        if (control.GetType() == typeof(ComboBox))
                        {
                            ComboBox cb = (ComboBox)control;
                            string currText = cb.Text;
                            cb.Items.Clear();
                            cb.Items.Add(currText);
                            cb.Text = currText;
                            cb.KeyPress += delegate(object sender, KeyPressEventArgs e)
                            {
                                e.KeyChar = (char)Keys.None;
                            };
                            cb.TextChanged += delegate(object sender, EventArgs args)
                            {
                                ComboBox comboBox = (ComboBox) sender;
                                comboBox.Text = comboBox.Items[0].ToString();
                            };
                            cb.BackColor = SystemColors.Control;
                        }
                    }
                }
            }
        }

        private void FormViewAndEditMaterial_Load(object sender, EventArgs e)
        {

        }

        private void lblOpenScanOfPassFile_MouseEnter(object sender, EventArgs e)
        {
            lblOpenScanOfPassFile.ForeColor = Color.Blue;
        }

        private void lblOpenScanOfPassFile_MouseLeave(object sender, EventArgs e)
        {
            lblOpenScanOfPassFile.ForeColor = Color.Black;
        }

        private void lblOpenAttFile_MouseEnter(object sender, EventArgs e)
        {
            lblOpenAttFile.ForeColor = Color.Blue;
        }

        private void lblOpenAttFile_MouseLeave(object sender, EventArgs e)
        {
            lblOpenAttFile.ForeColor = Color.Black;
        }

        private void lblOpenScanOfPassFile_Click(object sender, EventArgs e)
        {
            if ( ((Label) sender).Text != "<undefined>" )
                ClientLibrary.OpenExplorerAndChooseFile(_pathToScanPassFile);
        }
        
        private void lblOpenAttFile_Click(object sender, EventArgs e)
        {
            if (((Label)sender).Text != "<undefined>")
                ClientLibrary.OpenExplorerAndChooseFile(_pathToAttFile);
        }

        private void cbUserValidation_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbUserValidation.Checked)
            {
                cbUserValidation.Text = "Запись подтверждена";
                cbUserValidation.ForeColor = Color.Green;
            }
            else
            {
                cbUserValidation.Text = "Запись не подтверждена";
                cbUserValidation.ForeColor = Color.Red;
            }

            if (_editMode)
            {
                SqlRecord rec = new SqlRecord(TbMaterials.Name, TbMaterials.MaterialId, _materialRecordId);

                rec.SetField(TbMaterials.VerificationStatus, cbUserValidation.Checked ? "YES" : "NO");
                rec.Save();
            }
        }

        private void cbIsLaunched_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbIsLaunched.Checked)
            {
                cbIsLaunched.Text = "Пластина запущена";
                cbIsLaunched.ForeColor = Color.Green;
            }
            else
            {
                cbIsLaunched.Text = "Пластина не запущена";
                cbIsLaunched.ForeColor = Color.Red;
            }

            if (_editMode)
            {
                SqlRecord rec = new SqlRecord(TbMaterials.Name, TbMaterials.MaterialId, _materialRecordId);

                rec.SetField(TbMaterials.LaunchedStatus, cbIsLaunched.Checked ? "YES" : "NO");
                rec.Save();
            }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void печатьПаспортаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_pathToScanPassFile))
                return;

            PrintDocument pd = new PrintDocument 
            {OriginAtMargins = true, DefaultPageSettings = {Landscape = false}};

            pd.PrintPage += pd_PrintPage;
            pd.Print();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            double cmToUnits = 100 / 2.54;
            e.Graphics.DrawImage(new Bitmap(_pathToScanPassFile), 0, 0, (float)(6.5 * cmToUnits), (float)(11.5 * cmToUnits));
        }
    }
}
