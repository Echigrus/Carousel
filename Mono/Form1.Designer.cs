namespace Mono
{
    partial class Mono
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mono));
            this.browserSelect = new System.Windows.Forms.DomainUpDown();
            this.tasksCheck = new System.Windows.Forms.CheckBox();
            this.robberyCheck = new System.Windows.Forms.CheckBox();
            this.repairCheck = new System.Windows.Forms.CheckBox();
            this.grayLabel = new System.Windows.Forms.Label();
            this.graySelect = new System.Windows.Forms.DomainUpDown();
            this.specialLabel = new System.Windows.Forms.Label();
            this.specialBox = new System.Windows.Forms.TextBox();
            this.specialSelect = new System.Windows.Forms.DomainUpDown();
            this.thingsLabel = new System.Windows.Forms.Label();
            this.timingLabel = new System.Windows.Forms.Label();
            this.timingBar = new System.Windows.Forms.TrackBar();
            this.start = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.BW = new System.ComponentModel.BackgroundWorker();
            this.precBox = new System.Windows.Forms.TextBox();
            this.precLabel = new System.Windows.Forms.Label();
            this.logpassGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.timingBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logpassGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // browserSelect
            // 
            this.browserSelect.Location = new System.Drawing.Point(282, 12);
            this.browserSelect.Name = "browserSelect";
            this.browserSelect.ReadOnly = true;
            this.browserSelect.Size = new System.Drawing.Size(120, 20);
            this.browserSelect.TabIndex = 0;
            this.browserSelect.TabStop = false;
            this.browserSelect.Text = "browserSel";
            // 
            // tasksCheck
            // 
            this.tasksCheck.AutoSize = true;
            this.tasksCheck.Location = new System.Drawing.Point(283, 38);
            this.tasksCheck.Name = "tasksCheck";
            this.tasksCheck.Size = new System.Drawing.Size(70, 17);
            this.tasksCheck.TabIndex = 5;
            this.tasksCheck.Text = "Наводки";
            this.tasksCheck.UseVisualStyleBackColor = true;
            // 
            // robberyCheck
            // 
            this.robberyCheck.AutoSize = true;
            this.robberyCheck.Location = new System.Drawing.Point(283, 59);
            this.robberyCheck.Name = "robberyCheck";
            this.robberyCheck.Size = new System.Drawing.Size(68, 17);
            this.robberyCheck.TabIndex = 6;
            this.robberyCheck.Text = "Патруль";
            this.robberyCheck.UseVisualStyleBackColor = true;
            // 
            // repairCheck
            // 
            this.repairCheck.AutoSize = true;
            this.repairCheck.Checked = true;
            this.repairCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.repairCheck.Location = new System.Drawing.Point(283, 81);
            this.repairCheck.Name = "repairCheck";
            this.repairCheck.Size = new System.Drawing.Size(69, 17);
            this.repairCheck.TabIndex = 48;
            this.repairCheck.Text = "Починка";
            this.repairCheck.UseVisualStyleBackColor = true;
            // 
            // grayLabel
            // 
            this.grayLabel.AutoSize = true;
            this.grayLabel.Location = new System.Drawing.Point(280, 144);
            this.grayLabel.Name = "grayLabel";
            this.grayLabel.Size = new System.Drawing.Size(79, 13);
            this.grayLabel.TabIndex = 49;
            this.grayLabel.Text = "Личные вещи:";
            // 
            // graySelect
            // 
            this.graySelect.Location = new System.Drawing.Point(283, 160);
            this.graySelect.Name = "graySelect";
            this.graySelect.ReadOnly = true;
            this.graySelect.Size = new System.Drawing.Size(110, 20);
            this.graySelect.TabIndex = 50;
            this.graySelect.TabStop = false;
            this.graySelect.Text = "browserSel";
            // 
            // specialLabel
            // 
            this.specialLabel.AutoSize = true;
            this.specialLabel.Location = new System.Drawing.Point(280, 185);
            this.specialLabel.Name = "specialLabel";
            this.specialLabel.Size = new System.Drawing.Size(40, 13);
            this.specialLabel.TabIndex = 51;
            this.specialLabel.Text = "Кроме";
            // 
            // specialBox
            // 
            this.specialBox.Location = new System.Drawing.Point(283, 201);
            this.specialBox.Multiline = true;
            this.specialBox.Name = "specialBox";
            this.specialBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.specialBox.Size = new System.Drawing.Size(110, 84);
            this.specialBox.TabIndex = 52;
            // 
            // specialSelect
            // 
            this.specialSelect.Location = new System.Drawing.Point(283, 312);
            this.specialSelect.Name = "specialSelect";
            this.specialSelect.ReadOnly = true;
            this.specialSelect.Size = new System.Drawing.Size(110, 20);
            this.specialSelect.TabIndex = 54;
            this.specialSelect.TabStop = false;
            this.specialSelect.Text = "browserSel";
            // 
            // thingsLabel
            // 
            this.thingsLabel.AutoSize = true;
            this.thingsLabel.Location = new System.Drawing.Point(280, 296);
            this.thingsLabel.Name = "thingsLabel";
            this.thingsLabel.Size = new System.Drawing.Size(94, 13);
            this.thingsLabel.TabIndex = 53;
            this.thingsLabel.Text = "Не личные вещи:";
            // 
            // timingLabel
            // 
            this.timingLabel.AutoSize = true;
            this.timingLabel.Location = new System.Drawing.Point(283, 345);
            this.timingLabel.Name = "timingLabel";
            this.timingLabel.Size = new System.Drawing.Size(61, 13);
            this.timingLabel.TabIndex = 55;
            this.timingLabel.Text = "Задержка:";
            // 
            // timingBar
            // 
            this.timingBar.LargeChange = 200;
            this.timingBar.Location = new System.Drawing.Point(286, 361);
            this.timingBar.Maximum = 2000;
            this.timingBar.Name = "timingBar";
            this.timingBar.Size = new System.Drawing.Size(104, 45);
            this.timingBar.SmallChange = 200;
            this.timingBar.TabIndex = 56;
            this.timingBar.TickFrequency = 200;
            this.timingBar.Value = 1000;
            this.timingBar.Scroll += new System.EventHandler(this.BarScrolled);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(283, 419);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(110, 23);
            this.start.TabIndex = 57;
            this.start.Text = "Старт";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(283, 446);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(110, 23);
            this.stop.TabIndex = 58;
            this.stop.Text = "Стоп";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // BW
            // 
            this.BW.WorkerSupportsCancellation = true;
            this.BW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BW_DoWork);
            // 
            // precBox
            // 
            this.precBox.Location = new System.Drawing.Point(334, 117);
            this.precBox.Name = "precBox";
            this.precBox.Size = new System.Drawing.Size(59, 20);
            this.precBox.TabIndex = 59;
            this.precBox.Text = "2000";
            this.precBox.WordWrap = false;
            // 
            // precLabel
            // 
            this.precLabel.AutoSize = true;
            this.precLabel.Location = new System.Drawing.Point(283, 120);
            this.precLabel.Name = "precLabel";
            this.precLabel.Size = new System.Drawing.Size(45, 13);
            this.precLabel.TabIndex = 60;
            this.precLabel.Text = "Точный";
            // 
            // logpassGrid
            // 
            this.logpassGrid.AllowUserToAddRows = false;
            this.logpassGrid.AllowUserToDeleteRows = false;
            this.logpassGrid.AllowUserToResizeColumns = false;
            this.logpassGrid.AllowUserToResizeRows = false;
            this.logpassGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logpassGrid.ColumnHeadersVisible = false;
            this.logpassGrid.EnableHeadersVisualStyles = false;
            this.logpassGrid.Location = new System.Drawing.Point(12, 12);
            this.logpassGrid.Name = "logpassGrid";
            this.logpassGrid.RowHeadersVisible = false;
            this.logpassGrid.RowHeadersWidth = 110;
            this.logpassGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.logpassGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.logpassGrid.ShowCellErrors = false;
            this.logpassGrid.ShowCellToolTips = false;
            this.logpassGrid.ShowEditingIcon = false;
            this.logpassGrid.ShowRowErrors = false;
            this.logpassGrid.Size = new System.Drawing.Size(240, 457);
            this.logpassGrid.TabIndex = 61;
            // 
            // Mono
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(414, 479);
            this.Controls.Add(this.logpassGrid);
            this.Controls.Add(this.precLabel);
            this.Controls.Add(this.precBox);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.start);
            this.Controls.Add(this.timingBar);
            this.Controls.Add(this.timingLabel);
            this.Controls.Add(this.specialSelect);
            this.Controls.Add(this.thingsLabel);
            this.Controls.Add(this.specialBox);
            this.Controls.Add(this.specialLabel);
            this.Controls.Add(this.graySelect);
            this.Controls.Add(this.grayLabel);
            this.Controls.Add(this.repairCheck);
            this.Controls.Add(this.robberyCheck);
            this.Controls.Add(this.tasksCheck);
            this.Controls.Add(this.browserSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Mono";
            this.Text = "Carousel5";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.timingBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logpassGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DomainUpDown browserSelect;
        private System.Windows.Forms.CheckBox tasksCheck;
        private System.Windows.Forms.CheckBox robberyCheck;
        private System.Windows.Forms.CheckBox repairCheck;
        private System.Windows.Forms.Label grayLabel;
        private System.Windows.Forms.DomainUpDown graySelect;
        private System.Windows.Forms.Label specialLabel;
        private System.Windows.Forms.TextBox specialBox;
        private System.Windows.Forms.DomainUpDown specialSelect;
        private System.Windows.Forms.Label thingsLabel;
        private System.Windows.Forms.Label timingLabel;
        private System.Windows.Forms.TrackBar timingBar;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button stop;
        private System.ComponentModel.BackgroundWorker BW;
        private System.Windows.Forms.TextBox precBox;
        private System.Windows.Forms.Label precLabel;
        private System.Windows.Forms.DataGridView logpassGrid;
    }
}

