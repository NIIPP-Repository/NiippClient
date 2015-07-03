using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NIIPP.DatabaseClient.Library;
using NIIPP.DatabaseClient.DataStorage;

namespace NiippClient
{
    public partial class FormMaterialSearch : Form
    {
        private readonly string[] _nameOfColumns = {
                                     "№",
                                     "Партия",
                                     "Номер",
                                     "Техн.",
                                     "В запуске",
                                     "Дата",
                                     "Просмотр"
                                 };
        private readonly string[] _indexOfColumns = {
                                     "",
                                     TbMaterials.NumberOfParcel,
                                     TbMaterials.NumberOfWafer,
                                     TbMaterials.Technology,
                                     TbMaterials.LaunchedStatus,
                                     TbMaterials.CreationDate,
                                     ""
                                 };

        public FormMaterialSearch()
        {
            InitializeComponent();
        }

        private void InitDgvShowResults()
        {
            dgvShow.ColumnCount = _nameOfColumns.Length;
            for (int i = 0; i < _nameOfColumns.Length; i++)
                dgvShow.Columns[i].Name = _nameOfColumns[i];

            dgvShow.ReadOnly = true;
            dgvShow.Font = new Font("Microsoft Sans Serif", 9);
            dgvShow.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            dgvShow.BackgroundColor = SystemColors.Control;
            dgvShow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvShow.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvShow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvShow.Columns[4].Visible = false;

            dgvShow.AutoResizeColumns();
        }

        private void ResultsToDgvShow(DataTable dt)
        {
            int count = 0;
            foreach (DataRow nextLine in dt.Rows)
            {
                dgvShow.RowCount++;
                int currRow = dgvShow.RowCount - 2;

                for (int i = 0; i < _indexOfColumns.Length; i++)
                {
                    if (_indexOfColumns[i] == "")
                        continue;
                    int currIndex = dt.Columns.IndexOf(_indexOfColumns[i]);
                    dgvShow.Rows[currRow].Cells[i].Value = nextLine.ItemArray[currIndex].ToString();
                }

                int index = dt.Columns.IndexOf(TbMaterials.TechProc);
                if (nextLine.ItemArray[index].ToString() != "")
                    dgvShow.Rows[currRow].Cells[3].Value = String.Format("{0}-{1}", dgvShow.Rows[currRow].Cells[3].Value, nextLine.ItemArray[index]);
                dgvShow.Rows[currRow].Cells[0].Value = (++count).ToString();
                dgvShow.Rows[currRow].Cells[6].Value = "Открыть";
            }

            // срезаем нули в формате времени
            for (int i = 0; i < dgvShow.Rows.Count - 1; i++)
                if (dgvShow.Rows[i].Cells[5].Value.ToString().Length >= 10)
                    dgvShow.Rows[i].Cells[5].Value = dgvShow.Rows[i].Cells[5].Value.ToString().Substring(0, 10);
            dgvShow.AutoResizeColumns();

            // раскраска строк
            for (int i = 0; i < dgvShow.Rows.Count - 1; i++)
            {
                if (dgvShow.Rows[i].Cells[4].Value.ToString() == "YES")
                    dgvShow.Rows[i].Cells[0].Style.BackColor = Color.LightGreen;
                if (dgvShow.Rows[i].Cells[4].Value.ToString() == "NO")
                    dgvShow.Rows[i].Cells[0].Style.BackColor = Color.LightSalmon;
            }
            dgvShow.ClearSelection();

            lblInfo.Text = String.Format("Найдено {0} записей", count);
        }

        private void FormMaterialSearch_Load(object sender, EventArgs e)
        {
            InitDgvShowResults();
        }

        private string ProcessInputText(string str)
        {
            string res = str.Remove(' ');
            res = res.Replace('.', ',');
            return str;
        }

        private void ConstructSelectQuery(ref SqlSelect sqlSelectQuery)
        {
            if (tbGlobalSearch.Text != "")
            {
                string str = tbGlobalSearch.Text;
                sqlSelectQuery.TypeOfLogicIsAnd = false;
                sqlSelectQuery.Like(TbMaterials.MaterialId, str);
                sqlSelectQuery.Like(TbMaterials.AuthorOfRecord, str);
                sqlSelectQuery.Like(TbMaterials.Comment, str);
                sqlSelectQuery.Like(TbMaterials.ConcentrationInWafer, str);
                sqlSelectQuery.Like(TbMaterials.Corresponds, str);
                sqlSelectQuery.Like(TbMaterials.CreationDate, str);
                sqlSelectQuery.Like(TbMaterials.Ingot, str);
                sqlSelectQuery.Like(TbMaterials.LaunchedStatus, str);
                sqlSelectQuery.Like(TbMaterials.MaterialOfWafer, str);
                sqlSelectQuery.Like(TbMaterials.RecordIdOfAttachmentFile, str);
                sqlSelectQuery.Like(TbMaterials.RecordIdOfEpitStructureFile, str);
                sqlSelectQuery.Like(TbMaterials.Technology, str);
                sqlSelectQuery.Like(TbMaterials.TechProc, str);
                sqlSelectQuery.Like(TbMaterials.StructureManufacturer, str);
                sqlSelectQuery.Like(TbMaterials.ThicknessOfWafer, str);
                sqlSelectQuery.Like(TbMaterials.TypeOfWafer, str);
                sqlSelectQuery.Like(TbMaterials.VerificationStatus, str);
                sqlSelectQuery.Like(TbMaterials.WaferDiameter, str);
                sqlSelectQuery.Like(TbMaterials.WaferManufacturer, str);
                sqlSelectQuery.Like(TbMaterials.WaferResistance, str);

                return;
            }

            if (tbNumberOfParty.Text != "")
                sqlSelectQuery.Equal(TbMaterials.NumberOfParcel, tbNumberOfParty.Text);

            if (!rbDontWantWNFilter.Checked)
            {
                if (tbNumberOfWafers.Enabled)
                {
                    int x = Int32.Parse(tbNumberOfWafers.Text);
                    sqlSelectQuery.Equal(TbMaterials.NumberOfWafer, x);
                }
                if (tbNumberOfWafersStart.Enabled)
                {
                    int x1 = Int32.Parse(tbNumberOfWafersStart.Text);
                    sqlSelectQuery.MoreOrEqual(TbMaterials.NumberOfWafer, x1);
                }
                if (tbNumberOfWafersFinish.Enabled)
                {
                    int x2 = Int32.Parse(tbNumberOfWafersFinish.Text);
                    sqlSelectQuery.LessOrEqual(TbMaterials.NumberOfWafer, x2);
                }
            }

            if (!rbDontWantDateFilter.Checked)
            {
                if (dtpCreationTime.Enabled)
                    sqlSelectQuery.Equal(TbMaterials.CreationDate, dtpCreationTime.Value.Date);
                if (dtpCreationStart.Enabled)
                    sqlSelectQuery.MoreOrEqual(TbMaterials.CreationDate, dtpCreationStart.Value.Date);
                if (dtpCreationFinish.Enabled)
                    sqlSelectQuery.LessOrEqual(TbMaterials.CreationDate, dtpCreationFinish.Value.Date);
            }
            if (cbAuthorOfRecord.Text != "")
                sqlSelectQuery.Equal(TbMaterials.AuthorOfRecord, cbAuthorOfRecord.Text);
            if (cbDefinitionOfWafer.Text != "")
                sqlSelectQuery.Equal(TbMaterials.WaferManufacturer, cbDefinitionOfWafer.Text);
            if (cbTypeOfWafer.Text != "")
                sqlSelectQuery.Equal(TbMaterials.TypeOfWafer, cbTypeOfWafer.Text);
            if (cbMaterialOfWafer.Text != "")
                sqlSelectQuery.Equal(TbMaterials.MaterialOfWafer, cbMaterialOfWafer.Text);
            if (!rbDontWantConcFilter.Checked)
            {
                if (tbConcOfWafer.Enabled)
                    sqlSelectQuery.Equal(TbMaterials.ConcentrationInWafer, Double.Parse(tbConcOfWafer.Text));
                if (tbConcOfWaferStart.Enabled)
                    sqlSelectQuery.MoreOrEqual(TbMaterials.ConcentrationInWafer, Double.Parse(tbConcOfWaferStart.Text));
                if (tbConcOfWaferFinish.Enabled)
                    sqlSelectQuery.LessOrEqual(TbMaterials.ConcentrationInWafer, Double.Parse(tbConcOfWaferFinish.Text));
            }
            if (!rbDontWantWTFilter.Checked)
            {
                if (tbThicknessOfWafer.Enabled)
                {
                    double thickness = Double.Parse(tbThicknessOfWafer.Text);
                    if (cbWaferThicknessDim.Text == "нм")
                        thickness /= 1000;
                    sqlSelectQuery.Equal(TbMaterials.ThicknessOfWafer, thickness);
                }
                if (tbThicknessOfWaferStart.Enabled)
                {
                    double thickness = Double.Parse(tbThicknessOfWaferStart.Text);
                    if (cbWaferThicknessDimStart.Text == "нм")
                        thickness /= 1000;
                    sqlSelectQuery.MoreOrEqual(TbMaterials.ThicknessOfWafer, thickness);
                }
                if (tbThicknessOfWaferFinish.Enabled)
                {
                    double thickness = Double.Parse(tbThicknessOfWaferFinish.Text);
                    if (cbWaferThicknessDimFinish.Text == "нм")
                        thickness /= 1000;
                    sqlSelectQuery.LessOrEqual(TbMaterials.ThicknessOfWafer, thickness);
                }
            }
            if (!rbStatusAll.Checked)
            {
                if (rbStatusLaunched.Checked)
                    sqlSelectQuery.Equal(TbMaterials.LaunchedStatus, "YES");
                if (rbStatusNoLaunched.Checked)
                    sqlSelectQuery.Equal(TbMaterials.LaunchedStatus, "NO");
            }

            if (cbTechnology.Text != "")
            {
                sqlSelectQuery.Equal(TbMaterials.Technology, cbTechnology.Text);
            }
            if (cbTechProc.Text != "")
            {
                sqlSelectQuery.Equal(TbMaterials.TechProc, cbTechProc.Text);
            }
            if (cbManufacturer.Text != "")
            {
                sqlSelectQuery.Equal(TbMaterials.StructureManufacturer, cbManufacturer.Text);
            }
            if (tbIngot.Text != "")
            {
                sqlSelectQuery.Equal(TbMaterials.Ingot, tbIngot.Text);
            }
            if (tbCorresponds.Text != "")
            {
                sqlSelectQuery.Equal(TbMaterials.Corresponds, tbCorresponds.Text);
            }
        }

        private void dtpCreationStart_ValueChanged(object sender, EventArgs e)
        {
            dtpCreationFinish.Value = dtpCreationStart.Value;
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            dgvShow.Rows.Clear();
            dgvShow.AutoResizeColumns();

            lblInfo.Text = "<info>";
        }

        private void rbDontWantWNFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDontWantWNFilter.Checked)
            {
                tbNumberOfWafers.Enabled = false;
                tbNumberOfWafersFinish.Enabled = false;
                tbNumberOfWafersStart.Enabled = false;
            }
        }

        private void rbWNFilterPoint_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWNFilterPoint.Checked)
            {
                tbNumberOfWafers.Enabled = true;
                tbNumberOfWafersFinish.Enabled = false;
                tbNumberOfWafersStart.Enabled = false;
            }
        }

        private void rbWNFilterInterval_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWNFilterInterval.Checked)
            {
                tbNumberOfWafers.Enabled = false;
                tbNumberOfWafersFinish.Enabled = true;
                tbNumberOfWafersStart.Enabled = true;
            }
        }

        private void rbWNFilterStart_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWNFilterStart.Checked)
            {
                tbNumberOfWafers.Enabled = false;
                tbNumberOfWafersFinish.Enabled = false;
                tbNumberOfWafersStart.Enabled = true;
            }

        }

        private void rbWNFilterFinish_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWNFilterFinish.Checked)
            {
                tbNumberOfWafers.Enabled = false;
                tbNumberOfWafersFinish.Enabled = true;
                tbNumberOfWafersStart.Enabled = false;
            }
        }

        private void rbDontWantDateFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDontWantDateFilter.Checked)
            {
                dtpCreationFinish.Enabled = false;
                dtpCreationStart.Enabled = false;
                dtpCreationTime.Enabled = false;
            }
        }

        private void rbWantDateFilterPoint_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWantDateFilterPoint.Checked)
            {
                dtpCreationTime.Enabled = true;
                dtpCreationStart.Enabled = false;
                dtpCreationFinish.Enabled = false;
            }
        }

        private void rbWantDateFilterInterval_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWantDateFilterInterval.Checked)
            {
                dtpCreationTime.Enabled = false;
                dtpCreationStart.Enabled = true;
                dtpCreationFinish.Enabled = true;
            }
        }

        private void rbDateFilterStart_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDateFilterStart.Checked)
            {
                dtpCreationTime.Enabled = false;
                dtpCreationStart.Enabled = true;
                dtpCreationFinish.Enabled = false;
            }
        }

        private void rbDateFilterFinish_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDateFilterFinish.Checked)
            {
                dtpCreationTime.Enabled = false;
                dtpCreationStart.Enabled = false;
                dtpCreationFinish.Enabled = true;
            }
        }

        private void rbDontWantConcFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDontWantConcFilter.Checked)
            {
                tbConcOfWafer.Enabled = false;
                tbConcOfWaferStart.Enabled = false;
                tbConcOfWaferFinish.Enabled = false;
            }
        }

        private void rbConcPointFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbConcPointFilter.Checked)
            {
                tbConcOfWafer.Enabled = true;
                tbConcOfWaferStart.Enabled = false;
                tbConcOfWaferFinish.Enabled = false;
            }
        }

        private void rbConcIntervalFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbConcIntervalFilter.Checked)
            {
                tbConcOfWafer.Enabled = false;
                tbConcOfWaferStart.Enabled = true;
                tbConcOfWaferFinish.Enabled = true;
            }
        }

        private void rbConcFilterStart_CheckedChanged(object sender, EventArgs e)
        {
            if (rbConcFilterStart.Checked)
            {
                tbConcOfWafer.Enabled = false;
                tbConcOfWaferStart.Enabled = true;
                tbConcOfWaferFinish.Enabled = false;
            }
        }

        private void rbConcFilterFinish_CheckedChanged(object sender, EventArgs e)
        {
            if (rbConcFilterFinish.Checked)
            {
                tbConcOfWafer.Enabled = false;
                tbConcOfWaferStart.Enabled = false;
                tbConcOfWaferFinish.Enabled = true;
            }
        }

        private void rbDontWantWTFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDontWantWTFilter.Checked)
            {
                tbThicknessOfWafer.Enabled = false;
                tbThicknessOfWaferFinish.Enabled = false;
                tbThicknessOfWaferStart.Enabled = false;
                cbWaferThicknessDim.Enabled = false;
                cbWaferThicknessDimFinish.Enabled = false;
                cbWaferThicknessDimStart.Enabled = false;
            }
        }

        private void rbWTFilterPoint_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWTFilterPoint.Checked)
            {
                tbThicknessOfWafer.Enabled = true;
                tbThicknessOfWaferFinish.Enabled = false;
                tbThicknessOfWaferStart.Enabled = false;
                cbWaferThicknessDim.Enabled = true;
                cbWaferThicknessDimFinish.Enabled = false;
                cbWaferThicknessDimStart.Enabled = false;
            }
        }

        private void rbWTFilterInterval_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWTFilterInterval.Checked)
            {
                tbThicknessOfWafer.Enabled = false;
                tbThicknessOfWaferFinish.Enabled = true;
                tbThicknessOfWaferStart.Enabled = true;
                cbWaferThicknessDim.Enabled = false;
                cbWaferThicknessDimFinish.Enabled = true;
                cbWaferThicknessDimStart.Enabled = true;
            }
        }

        private void rbWTFilterStart_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWTFilterStart.Checked)
            {
                tbThicknessOfWafer.Enabled = false;
                tbThicknessOfWaferFinish.Enabled = false;
                tbThicknessOfWaferStart.Enabled = true;
                cbWaferThicknessDim.Enabled = false;
                cbWaferThicknessDimFinish.Enabled = false;
                cbWaferThicknessDimStart.Enabled = true;
            }
        }

        private void rbWTFilterFinish_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWTFilterFinish.Checked)
            {
                tbThicknessOfWafer.Enabled = false;
                tbThicknessOfWaferFinish.Enabled = true;
                tbThicknessOfWaferStart.Enabled = false;
                cbWaferThicknessDim.Enabled = false;
                cbWaferThicknessDimFinish.Enabled = true;
                cbWaferThicknessDimStart.Enabled = false;
            }
        }

        private void dgvShow_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex >= dgvShow.Rows.Count - 1)
                return;
            if (dgvShow.Columns[e.ColumnIndex].Name == "Просмотр")
            {
                string id = String.Format("{0}_{1}", dgvShow.Rows[e.RowIndex].Cells[1].Value, dgvShow.Rows[e.RowIndex].Cells[2].Value);
                FormViewAndEditMaterial formViewAndEditMaterial = new FormViewAndEditMaterial();
                formViewAndEditMaterial.Show();
                formViewAndEditMaterial.SetMaterialId(id);
            }
        }

        private void btnStartSearchAdd_Click(object sender, EventArgs e)
        {
            SqlSelect sqlSelectQuery = new SqlSelect(TbMaterials.Name);

            ConstructSelectQuery(ref sqlSelectQuery);
            sqlSelectQuery.OrderByField = TbMaterials.MaterialId;
            sqlSelectQuery.RetrieveData();

            ResultsToDgvShow(sqlSelectQuery.DataTable);
        }

        private void btnStartNewSearch_Click(object sender, EventArgs e)
        {
            dgvShow.Rows.Clear();

            SqlSelect sqlSelectQuery = new SqlSelect(TbMaterials.Name);

            ConstructSelectQuery(ref sqlSelectQuery);
            sqlSelectQuery.OrderByField = TbMaterials.MaterialId;
            sqlSelectQuery.RetrieveData();

            ResultsToDgvShow(sqlSelectQuery.DataTable);
        }
    }
}
