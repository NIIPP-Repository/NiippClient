using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using NIIPP.DatabaseClient.Library;
using NIIPP.DatabaseClient.DataStorage;
using NIIPP.DatabaseClient.NetworkFileManager;

namespace NiippClient
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        public static FormMain Instance { get; private set; }

        /// <summary>
        /// Выводит на форму состояние пдключения с сервером
        /// </summary>
        public void ShowServerStatus()
        {
            bool ok = ClientLibrary.CheckServerStatus();
            if (ok)
            {
                labelServerStatus.Text = "Access Granted";
                labelServerStatus.ForeColor = Color.Green;
            }
            else
            {
                labelServerStatus.Text = "Access Denied";
                labelServerStatus.ForeColor = Color.Red;
            }
        }

        bool LoadConnectionSettings()
        {
            String path = "connectionSettings.txt";

            try
            {
                StreamReader sr = new StreamReader(path);

                ConnectionSettings.ServerIp = sr.ReadLine().Substring(8);
                ConnectionSettings.UserId = sr.ReadLine().Substring(9);
                ConnectionSettings.Password = sr.ReadLine().Substring(10);
                ConnectionSettings.DatabaseName = sr.ReadLine().Substring(10);

                sr.Close();

                return true;
            }
            catch (IOException e)
            {
                MessageBox.Show("Не удалось считать файл с настройками подключения \n" + e.Message, "Ошибка");
                return false;
            }
        }

        /// <summary>
        /// Cоздаем папку с кэшем, если уже есть то чистим
        /// </summary>
        private static void ProcessCache()
        {
            string programFolder = Path.GetDirectoryName(Application.ExecutablePath);
            string pathToSaveFolder = String.Format("{0}\\cache {1}", programFolder,
                ClientLibrary.GetAuthorOfComputer());

            DirectoryInfo di = new DirectoryInfo(pathToSaveFolder);
            if (!di.Exists)
            {
                di.Create();
            }
            else
            {
                foreach (FileInfo fi in di.GetFiles())
                {
                    try
                    {
                        if (!fi.IsReadOnly && fi.Name.ToLower() != "thumbs.db")
                            fi.Delete();
                    }
                    finally
                    {
                    }
                }
            }
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            Instance = this;
            LoadConnectionSettings();
            ShowServerStatus();
            ProcessCache();
        }

        private void pictureBoxCreateMaterial_Click(object sender, EventArgs e)
        {
            Form winCreateMaterial = new FormCreateMaterial();
            winCreateMaterial.Show();
        }

        private void подключениеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form winConnection = new FormConnectionSettings();
            winConnection.Show();
        }

        private void закрытьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBoxCreateTemplate_Click(object sender, EventArgs e)
        {
            Form formSetOfMasks = new FormCreateSetOfMasks();
            formSetOfMasks.Show();
        }

        private void FormMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void pictureBoxEditTemplate_Click(object sender, EventArgs e)
        {
            FormCreateRouteList winCreateWaferDoc = new FormCreateRouteList();
            winCreateWaferDoc.Show();
        }

        private void pbSearch_Click(object sender, EventArgs e)
        {
            FormMaterialSearch winMaterialSearch = new FormMaterialSearch();
            winMaterialSearch.Show();
        }

    }
}
