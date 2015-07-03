namespace NiippClient
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.label2 = new System.Windows.Forms.Label();
            this.labelServerStatus = new System.Windows.Forms.Label();
            this.pbCreateMaterial = new System.Windows.Forms.PictureBox();
            this.pbEditTemplate = new System.Windows.Forms.PictureBox();
            this.pbCreateTemplate = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pbCreateMesure = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pbSearch = new System.Windows.Forms.PictureBox();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.закрытьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.подключениеToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbCreateMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEditTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCreateTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCreateMesure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).BeginInit();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(27, 161);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "Материал";
            // 
            // labelServerStatus
            // 
            this.labelServerStatus.AutoSize = true;
            this.labelServerStatus.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelServerStatus.Location = new System.Drawing.Point(10, 205);
            this.labelServerStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelServerStatus.Name = "labelServerStatus";
            this.labelServerStatus.Size = new System.Drawing.Size(111, 28);
            this.labelServerStatus.TabIndex = 8;
            this.labelServerStatus.Text = "<undefined>";
            // 
            // pbCreateMaterial
            // 
            this.pbCreateMaterial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbCreateMaterial.ErrorImage = null;
            this.pbCreateMaterial.Image = global::NiippClient.Properties.Resources.createMaterialPicture;
            this.pbCreateMaterial.InitialImage = null;
            this.pbCreateMaterial.Location = new System.Drawing.Point(14, 31);
            this.pbCreateMaterial.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbCreateMaterial.Name = "pbCreateMaterial";
            this.pbCreateMaterial.Size = new System.Drawing.Size(125, 125);
            this.pbCreateMaterial.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCreateMaterial.TabIndex = 0;
            this.pbCreateMaterial.TabStop = false;
            this.pbCreateMaterial.Click += new System.EventHandler(this.pictureBoxCreateMaterial_Click);
            // 
            // pbEditTemplate
            // 
            this.pbEditTemplate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEditTemplate.ErrorImage = null;
            this.pbEditTemplate.Image = global::NiippClient.Properties.Resources.editTemplatePicture;
            this.pbEditTemplate.InitialImage = null;
            this.pbEditTemplate.Location = new System.Drawing.Point(306, 31);
            this.pbEditTemplate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbEditTemplate.Name = "pbEditTemplate";
            this.pbEditTemplate.Size = new System.Drawing.Size(125, 125);
            this.pbEditTemplate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbEditTemplate.TabIndex = 4;
            this.pbEditTemplate.TabStop = false;
            this.pbEditTemplate.Click += new System.EventHandler(this.pictureBoxEditTemplate_Click);
            // 
            // pbCreateTemplate
            // 
            this.pbCreateTemplate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbCreateTemplate.ErrorImage = null;
            this.pbCreateTemplate.Image = global::NiippClient.Properties.Resources.createTempatePicture;
            this.pbCreateTemplate.InitialImage = null;
            this.pbCreateTemplate.Location = new System.Drawing.Point(162, 31);
            this.pbCreateTemplate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbCreateTemplate.Name = "pbCreateTemplate";
            this.pbCreateTemplate.Size = new System.Drawing.Size(125, 125);
            this.pbCreateTemplate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCreateTemplate.TabIndex = 3;
            this.pbCreateTemplate.TabStop = false;
            this.pbCreateTemplate.Click += new System.EventHandler(this.pictureBoxCreateTemplate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(200, 161);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 28);
            this.label3.TabIndex = 13;
            this.label3.Text = "ФШ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(467, 161);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 28);
            this.label5.TabIndex = 15;
            this.label5.Text = "Измерения";
            // 
            // pbCreateMesure
            // 
            this.pbCreateMesure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbCreateMesure.Image = global::NiippClient.Properties.Resources.createMeasurePicture;
            this.pbCreateMesure.Location = new System.Drawing.Point(452, 31);
            this.pbCreateMesure.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbCreateMesure.Name = "pbCreateMesure";
            this.pbCreateMesure.Size = new System.Drawing.Size(125, 125);
            this.pbCreateMesure.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCreateMesure.TabIndex = 3;
            this.pbCreateMesure.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label6.Location = new System.Drawing.Point(641, 161);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 28);
            this.label6.TabIndex = 17;
            this.label6.Text = "Поиск";
            // 
            // pbSearch
            // 
            this.pbSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbSearch.Image = global::NiippClient.Properties.Resources.searchPicture;
            this.pbSearch.Location = new System.Drawing.Point(604, 31);
            this.pbSearch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.Size = new System.Drawing.Size(125, 125);
            this.pbSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSearch.TabIndex = 6;
            this.pbSearch.TabStop = false;
            this.pbSearch.Click += new System.EventHandler(this.pbSearch_Click);
            // 
            // menuStripMain
            // 
            this.menuStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.menuStripMain.Font = new System.Drawing.Font("Candara", 11.14286F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem1,
            this.настройкиToolStripMenuItem1});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStripMain.Size = new System.Drawing.Size(768, 24);
            this.menuStripMain.TabIndex = 5;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem1
            // 
            this.файлToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.закрытьToolStripMenuItem1});
            this.файлToolStripMenuItem1.Name = "файлToolStripMenuItem1";
            this.файлToolStripMenuItem1.Size = new System.Drawing.Size(53, 22);
            this.файлToolStripMenuItem1.Text = "Файл";
            // 
            // закрытьToolStripMenuItem1
            // 
            this.закрытьToolStripMenuItem1.Name = "закрытьToolStripMenuItem1";
            this.закрытьToolStripMenuItem1.Size = new System.Drawing.Size(130, 22);
            this.закрытьToolStripMenuItem1.Text = "Закрыть";
            this.закрытьToolStripMenuItem1.Click += new System.EventHandler(this.закрытьToolStripMenuItem1_Click);
            // 
            // настройкиToolStripMenuItem1
            // 
            this.настройкиToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.подключениеToolStripMenuItem1});
            this.настройкиToolStripMenuItem1.Name = "настройкиToolStripMenuItem1";
            this.настройкиToolStripMenuItem1.Size = new System.Drawing.Size(90, 22);
            this.настройкиToolStripMenuItem1.Text = "Настройки";
            // 
            // подключениеToolStripMenuItem1
            // 
            this.подключениеToolStripMenuItem1.Name = "подключениеToolStripMenuItem1";
            this.подключениеToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.подключениеToolStripMenuItem1.Text = "Подключение";
            this.подключениеToolStripMenuItem1.Click += new System.EventHandler(this.подключениеToolStripMenuItem1_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label11.Location = new System.Drawing.Point(319, 161);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(103, 28);
            this.label11.TabIndex = 12;
            this.label11.Text = "Технология";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(768, 239);
            this.Controls.Add(this.pbSearch);
            this.Controls.Add(this.pbCreateMesure);
            this.Controls.Add(this.pbEditTemplate);
            this.Controls.Add(this.pbCreateTemplate);
            this.Controls.Add(this.pbCreateMaterial);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelServerStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStripMain);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Клиент для БД пластин";
            this.Load += new System.EventHandler(this.formMain_Load);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.pbCreateMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEditTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCreateTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCreateMesure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).EndInit();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCreateMaterial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.Label labelServerStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pbCreateTemplate;
        private System.Windows.Forms.PictureBox pbEditTemplate;
        private System.Windows.Forms.PictureBox pbCreateMesure;
        private System.Windows.Forms.PictureBox pbSearch;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem закрытьToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem подключениеToolStripMenuItem1;
        private System.Windows.Forms.Label label11;
    }
}

