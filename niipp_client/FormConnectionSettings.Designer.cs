namespace NiippClient
{
    partial class FormConnectionSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxDataBase = new System.Windows.Forms.TextBox();
            this.buttonConnection = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxUserID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNameOfServer = new System.Windows.Forms.TextBox();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.label7);
            this.groupBoxMain.Controls.Add(this.textBoxDataBase);
            this.groupBoxMain.Controls.Add(this.buttonConnection);
            this.groupBoxMain.Controls.Add(this.label6);
            this.groupBoxMain.Controls.Add(this.textBoxPassword);
            this.groupBoxMain.Controls.Add(this.label5);
            this.groupBoxMain.Controls.Add(this.textBoxUserID);
            this.groupBoxMain.Controls.Add(this.label1);
            this.groupBoxMain.Controls.Add(this.textBoxNameOfServer);
            this.groupBoxMain.Location = new System.Drawing.Point(24, 23);
            this.groupBoxMain.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBoxMain.Size = new System.Drawing.Size(458, 392);
            this.groupBoxMain.TabIndex = 40;
            this.groupBoxMain.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Candara", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(30, 223);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 37);
            this.label7.TabIndex = 47;
            this.label7.Text = "Database :";
            // 
            // textBoxDataBase
            // 
            this.textBoxDataBase.Font = new System.Drawing.Font("Arial Unicode MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxDataBase.Location = new System.Drawing.Point(213, 223);
            this.textBoxDataBase.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxDataBase.Name = "textBoxDataBase";
            this.textBoxDataBase.Size = new System.Drawing.Size(196, 40);
            this.textBoxDataBase.TabIndex = 46;
            this.textBoxDataBase.Text = "db_wafers";
            // 
            // buttonConnection
            // 
            this.buttonConnection.Font = new System.Drawing.Font("Candara", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonConnection.Location = new System.Drawing.Point(37, 296);
            this.buttonConnection.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonConnection.Name = "buttonConnection";
            this.buttonConnection.Size = new System.Drawing.Size(372, 50);
            this.buttonConnection.TabIndex = 45;
            this.buttonConnection.Text = "Сохранить настройки";
            this.buttonConnection.UseVisualStyleBackColor = true;
            this.buttonConnection.Click += new System.EventHandler(this.buttonConnection_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Candara", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(30, 165);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 37);
            this.label6.TabIndex = 44;
            this.label6.Text = "Password :";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Arial Unicode MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPassword.Location = new System.Drawing.Point(213, 166);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(196, 40);
            this.textBoxPassword.TabIndex = 43;
            this.textBoxPassword.Text = "user4pass";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Candara", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(30, 108);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 37);
            this.label5.TabIndex = 42;
            this.label5.Text = "User ID :";
            // 
            // textBoxUserID
            // 
            this.textBoxUserID.Font = new System.Drawing.Font("Arial Unicode MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxUserID.Location = new System.Drawing.Point(213, 108);
            this.textBoxUserID.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxUserID.Name = "textBoxUserID";
            this.textBoxUserID.Size = new System.Drawing.Size(196, 40);
            this.textBoxUserID.TabIndex = 41;
            this.textBoxUserID.Text = "user4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Candara", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(30, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 37);
            this.label1.TabIndex = 40;
            this.label1.Text = "Server :";
            // 
            // textBoxNameOfServer
            // 
            this.textBoxNameOfServer.Font = new System.Drawing.Font("Arial Unicode MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxNameOfServer.Location = new System.Drawing.Point(213, 50);
            this.textBoxNameOfServer.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxNameOfServer.Name = "textBoxNameOfServer";
            this.textBoxNameOfServer.Size = new System.Drawing.Size(196, 40);
            this.textBoxNameOfServer.TabIndex = 39;
            this.textBoxNameOfServer.Text = "localhost";
            // 
            // FormConnectionSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(514, 448);
            this.Controls.Add(this.groupBoxMain);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FormConnectionSettings";
            this.Text = "Настройки подключения";
            this.Load += new System.EventHandler(this.formConnectionSettings_Load);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxDataBase;
        private System.Windows.Forms.Button buttonConnection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxUserID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNameOfServer;
    }
}