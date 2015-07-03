using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using NIIPP.DatabaseClient.DataStorage;
using NIIPP.DatabaseClient.Library;
using Word = Microsoft.Office.Interop.Word;

namespace NiippClient
{
    public partial class FormCreateRouteList : Form
    {
        Word.Application _app;

        public FormCreateRouteList()
        {
            InitializeComponent();
        }

        private void FormCreateWaferDoc_Load(object sender, EventArgs e)
        {
            cbPartyNumber.DropDownHeight = 300;

            ClientLibrary.InitComboBoxFromDb(ref cbTechnology, TbMaterials.Name, TbMaterials.Technology);
            ClientLibrary.InitComboBoxFromDb(ref cbSetOfMasks, TbSetOfMasks.Name, TbSetOfMasks.NameOfSetOfMasksId);
            ClientLibrary.InitComboBoxFromDb(ref cbTechProc, TbMaterials.Name, TbMaterials.TechProc);
            cbTechnologist.Text = ClientLibrary.GetAuthorOfComputer();

            SetValuesOfCbPartyNumber();
        }

        private void ReplaceAllTemplateInDoc(string path, object[] findText, object[] replaceWith)
        {
            if (_app == null)
                _app = new Word.Application();

            Word.Document doc = null;
            object fileName = path;
            object falseValue = false;
            object trueValue = true;
            object missing = Type.Missing;

            doc = _app.Documents.Open(ref fileName, ref missing, ref falseValue,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing);
            _app.Selection.Find.ClearFormatting();
            _app.Selection.Find.Replacement.ClearFormatting();
            Word.Range wordRange = doc.Sections[1].Range;
            Word.Find wordFindObj = wordRange.Find;

            for (int k = 0; k < findText.Length; k++)
            {
                object[] wordFindParameters = new object[15] { findText[k], missing, missing, missing, missing, missing, missing, missing, missing, replaceWith[k], Word.WdReplace.wdReplaceAll, missing, missing, missing, missing };
                wordFindObj.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod, null, wordFindObj, wordFindParameters);
            }

            doc.Close(trueValue, missing, missing);
        }

        private void buttonChooseDocTemplate_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //String pathToFile = "";
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    pathToFile = ofd.FileName;
            //}
            //labelDocTemplateName.Text = Path.GetFileName(pathToFile);
            //String folder = Path.GetDirectoryName(pathToFile);

            //int first = 0,
            //    last = 0;
            //try
            //{
            //    first = Int32.Parse(textBoxFirstNumber.Text);
            //    last = Int32.Parse(textBoxLastNumber.Text);
            //}
            //catch
            //{
            //    MessageBox.Show("Данные введены не корректно!");
            //    return;
            //}
            //String author = comboBoxAuthor.Text,
            //       numberOfOrder = comboBoxNumberOfOrder.Text,
            //       setOfMasks = comboBoxSetOfMasks.Text;
            //DateTime now = DateTime.Now;

            //FileInfo fi = new FileInfo(pathToFile);

            //for (int i = first; i <= last; i++)
            //{
            //    Object[] findText = { "@NN/YY", "@партия", "@название материала", "@заказ", "@пластина", "@слои", "@процесс", "@технолог" };
            //    Object[] replaceWith = { i.ToString() + "/" + now.Year.ToString().Substring(2), "@партия", "@название материала", numberOfOrder, "@пластина", "@слои", "@процесс", author };

            //    String currPath = folder + i.ToString() + "_created_by_pro.doc";
            //    fi.CopyTo(currPath);
            //    ReplaceAllTemplateInDoc(currPath, findText, replaceWith);
            //}
            //app.Quit();
        }

        private void cbPartyNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                return;

            SetValuesOfCbPartyNumber();
            cbPartyNumber.DroppedDown = true;
        }

        private void SetValuesOfCbPartyNumber()
        {
            DataTable dt = ClientLibrary.GetLikeValuesOfField(TbMaterials.Name, TbMaterials.NumberOfParcel, cbPartyNumber.Text);
            var query = from DataRow row in dt.Rows
                where cbTechnology.Text == "" || row[TbMaterials.Technology].ToString() == cbTechnology.Text
                where cbTechProc.Text == "" || row[TbMaterials.TechProc].ToString() == cbTechProc.Text
                select row[TbMaterials.NumberOfParcel].ToString();

            int pos = cbPartyNumber.SelectionStart;
            cbPartyNumber.Items.Clear();
            cbPartyNumber.Items.AddRange(query.Distinct().ToArray());
            cbPartyNumber.SelectionStart = pos;
        }

        private void cbPartyNumber_TextChanged(object sender, EventArgs e)
        {
            SqlSelect sqlSelect = new SqlSelect(TbMaterials.Name);
            sqlSelect.Equal(TbMaterials.NumberOfParcel, cbPartyNumber.Text);
            sqlSelect.RetrieveData();
            DataTable dt = sqlSelect.DataTable;

            cbWaferNumber.Text = "";
            cbWaferNumber.Items.Clear();

            var query = 
                from DataRow row in dt.Rows
                select row[TbMaterials.NumberOfWafer].ToString();
            cbWaferNumber.Items.AddRange( query.ToArray() );
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlRecord rec = new SqlRecord(TbRouteLists.Name, TbRouteLists.NameOfList, tbRouteListName.Text);
            rec.SetField(TbRouteLists.NumberOfOrder, Int32.Parse(tbNumberOfOrder.Text));
            rec.SetField(TbRouteLists.CountOfMaterials, Int32.Parse(tbCountOfWafers.Text));
            rec.SetField(TbRouteLists.SetOfMasks, cbSetOfMasks.Text);
            rec.SetField(TbRouteLists.Materials, cbPartyNumber.Text + "_" + cbWaferNumber.Text);
            rec.SetField(TbRouteLists.Technologist, cbTechnologist.Text);
            rec.SetField(TbRouteLists.DateOfCreation, dtpTimeOfCreation.Value);
            rec.Save();
        }

        private void cbTechnology_TextChanged(object sender, EventArgs e)
        {
            cbPartyNumber.Text = "";
            SetValuesOfCbPartyNumber();
        }

        private void cbTechProc_TextChanged(object sender, EventArgs e)
        {
            cbPartyNumber.Text = "";
            SetValuesOfCbPartyNumber();
        }

    }
}
