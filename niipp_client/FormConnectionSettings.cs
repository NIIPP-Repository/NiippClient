using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using NIIPP.DatabaseClient.DataStorage;

namespace NiippClient
{
    public partial class FormConnectionSettings : Form
    {
        public FormConnectionSettings()
        {
            InitializeComponent();
        }

        private bool SaveConnectionSettings()
        {
            String path = "connectionSettings.txt";

            // удаляем старый файл
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
                fi.Delete();

            try
            {
                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine("Server: " + ConnectionSettings.ServerIp);
                sw.WriteLine("User ID: " + ConnectionSettings.UserId);
                sw.WriteLine("Password: " + ConnectionSettings.Password);
                sw.WriteLine("Database: " + ConnectionSettings.DatabaseName);
                sw.Flush();
                sw.Close();

                return true;
            }
            catch (IOException e)
            {
                MessageBox.Show("Не удалось сохранить файл с настройками " + e.Message, "Ошибка");
                return false;
            }
        }

        private void buttonConnection_Click(object sender, EventArgs e)
        {
            ConnectionSettings.ServerIp = textBoxNameOfServer.Text;
            ConnectionSettings.DatabaseName = textBoxDataBase.Text;
            ConnectionSettings.UserId = textBoxUserID.Text;
            ConnectionSettings.Password = textBoxPassword.Text;

            SaveConnectionSettings();
            FormMain.Instance.ShowServerStatus();
        }
        
        private void formConnectionSettings_Load(object sender, EventArgs e)
        {
            textBoxNameOfServer.Text = ConnectionSettings.ServerIp;
            textBoxDataBase.Text = ConnectionSettings.DatabaseName;
            textBoxUserID.Text = ConnectionSettings.UserId;
            textBoxPassword.Text = ConnectionSettings.Password;
        }
    }
}
