namespace PSTNReadLog
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.nghiepvu = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnReadLog = new System.Windows.Forms.Button();
            this.btnLogTTIBackupFileBrowser = new System.Windows.Forms.Button();
            this.btnLogTTIFileBrowser = new System.Windows.Forms.Button();
            this.txtTTIFilePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnLogTDHBackupFileBrowser = new System.Windows.Forms.Button();
            this.txtTDHBackupFilePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLogTDHFileBrowser = new System.Windows.Forms.Button();
            this.txtTTIBackupFilePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTDHFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtTestConect = new System.Windows.Forms.Button();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDLU = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tbnSearch = new System.Windows.Forms.Button();
            this.dtfromDate = new System.Windows.Forms.DateTimePicker();
            this.cbHOST = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dttoDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.nghiepvu.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nghiepvu
            // 
            this.nghiepvu.Controls.Add(this.tabPage2);
            this.nghiepvu.Controls.Add(this.tabPage3);
            this.nghiepvu.Controls.Add(this.tabPage1);
            this.nghiepvu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nghiepvu.Location = new System.Drawing.Point(0, 0);
            this.nghiepvu.Name = "nghiepvu";
            this.nghiepvu.SelectedIndex = 0;
            this.nghiepvu.Size = new System.Drawing.Size(800, 450);
            this.nghiepvu.TabIndex = 0;
            this.nghiepvu.SelectedIndexChanged += new System.EventHandler(this.nghiepvu_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnReadLog);
            this.tabPage2.Controls.Add(this.btnLogTTIBackupFileBrowser);
            this.tabPage2.Controls.Add(this.btnLogTTIFileBrowser);
            this.tabPage2.Controls.Add(this.txtTTIFilePath);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btnLogTDHBackupFileBrowser);
            this.tabPage2.Controls.Add(this.txtTDHBackupFilePath);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.btnSave);
            this.tabPage2.Controls.Add(this.btnLogTDHFileBrowser);
            this.tabPage2.Controls.Add(this.txtTTIBackupFilePath);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtTDHFilePath);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 424);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cấu hình";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnReadLog
            // 
            this.btnReadLog.Location = new System.Drawing.Point(11, 242);
            this.btnReadLog.Name = "btnReadLog";
            this.btnReadLog.Size = new System.Drawing.Size(211, 51);
            this.btnReadLog.TabIndex = 14;
            this.btnReadLog.Text = "Start";
            this.btnReadLog.UseVisualStyleBackColor = true;
            this.btnReadLog.Click += new System.EventHandler(this.btnReadLog_Click);
            // 
            // btnLogTTIBackupFileBrowser
            // 
            this.btnLogTTIBackupFileBrowser.Location = new System.Drawing.Point(626, 119);
            this.btnLogTTIBackupFileBrowser.Name = "btnLogTTIBackupFileBrowser";
            this.btnLogTTIBackupFileBrowser.Size = new System.Drawing.Size(48, 21);
            this.btnLogTTIBackupFileBrowser.TabIndex = 13;
            this.btnLogTTIBackupFileBrowser.Text = "....";
            this.btnLogTTIBackupFileBrowser.UseVisualStyleBackColor = true;
            this.btnLogTTIBackupFileBrowser.Click += new System.EventHandler(this.btnLogTTIBackupFileBrowser_Click);
            // 
            // btnLogTTIFileBrowser
            // 
            this.btnLogTTIFileBrowser.Location = new System.Drawing.Point(626, 92);
            this.btnLogTTIFileBrowser.Name = "btnLogTTIFileBrowser";
            this.btnLogTTIFileBrowser.Size = new System.Drawing.Size(48, 21);
            this.btnLogTTIFileBrowser.TabIndex = 12;
            this.btnLogTTIFileBrowser.Text = "....";
            this.btnLogTTIFileBrowser.UseVisualStyleBackColor = true;
            this.btnLogTTIFileBrowser.Click += new System.EventHandler(this.btnLogTTIFileBrowser_Click);
            // 
            // txtTTIFilePath
            // 
            this.txtTTIFilePath.Location = new System.Drawing.Point(199, 92);
            this.txtTTIFilePath.Name = "txtTTIFilePath";
            this.txtTTIFilePath.Size = new System.Drawing.Size(410, 20);
            this.txtTTIFilePath.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "File log host Thanh Trì";
            // 
            // btnLogTDHBackupFileBrowser
            // 
            this.btnLogTDHBackupFileBrowser.Location = new System.Drawing.Point(626, 38);
            this.btnLogTDHBackupFileBrowser.Name = "btnLogTDHBackupFileBrowser";
            this.btnLogTDHBackupFileBrowser.Size = new System.Drawing.Size(48, 21);
            this.btnLogTDHBackupFileBrowser.TabIndex = 9;
            this.btnLogTDHBackupFileBrowser.Text = "....";
            this.btnLogTDHBackupFileBrowser.UseVisualStyleBackColor = true;
            this.btnLogTDHBackupFileBrowser.Click += new System.EventHandler(this.btnLogTDHBackupFileBrowser_Click);
            // 
            // txtTDHBackupFilePath
            // 
            this.txtTDHBackupFilePath.Location = new System.Drawing.Point(199, 38);
            this.txtTDHBackupFilePath.Name = "txtTDHBackupFilePath";
            this.txtTDHBackupFilePath.Size = new System.Drawing.Size(410, 20);
            this.txtTDHBackupFilePath.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "File backup log host Thượng Đình:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(11, 182);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLogTDHFileBrowser
            // 
            this.btnLogTDHFileBrowser.Location = new System.Drawing.Point(626, 11);
            this.btnLogTDHFileBrowser.Name = "btnLogTDHFileBrowser";
            this.btnLogTDHFileBrowser.Size = new System.Drawing.Size(48, 21);
            this.btnLogTDHFileBrowser.TabIndex = 4;
            this.btnLogTDHFileBrowser.Text = "....";
            this.btnLogTDHFileBrowser.UseVisualStyleBackColor = true;
            this.btnLogTDHFileBrowser.Click += new System.EventHandler(this.btnLogTDHFileBrowser_Click);
            // 
            // txtTTIBackupFilePath
            // 
            this.txtTTIBackupFilePath.Location = new System.Drawing.Point(199, 120);
            this.txtTTIBackupFilePath.Name = "txtTTIBackupFilePath";
            this.txtTTIBackupFilePath.Size = new System.Drawing.Size(410, 20);
            this.txtTTIBackupFilePath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "File backup log host Thanh Trì";
            // 
            // txtTDHFilePath
            // 
            this.txtTDHFilePath.Location = new System.Drawing.Point(199, 11);
            this.txtTDHFilePath.Name = "txtTDHFilePath";
            this.txtTDHFilePath.Size = new System.Drawing.Size(410, 20);
            this.txtTDHFilePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File log host Thượng Đình:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtTestConect);
            this.tabPage3.Controls.Add(this.txtDatabase);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.txtUser);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.txtPass);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.txtIpAddress);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(792, 424);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Database";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtTestConect
            // 
            this.txtTestConect.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTestConect.Location = new System.Drawing.Point(80, 99);
            this.txtTestConect.Margin = new System.Windows.Forms.Padding(5);
            this.txtTestConect.Name = "txtTestConect";
            this.txtTestConect.Size = new System.Drawing.Size(153, 45);
            this.txtTestConect.TabIndex = 23;
            this.txtTestConect.Text = "Kiểm tra kết nối";
            this.txtTestConect.UseVisualStyleBackColor = true;
            this.txtTestConect.Click += new System.EventHandler(this.txtTestConect_Click);
            // 
            // txtDatabase
            // 
            this.txtDatabase.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDatabase.Location = new System.Drawing.Point(80, 60);
            this.txtDatabase.Margin = new System.Windows.Forms.Padding(5);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(270, 29);
            this.txtDatabase.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(-4, 68);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 21);
            this.label5.TabIndex = 21;
            this.label5.Text = "Database:";
            // 
            // txtUser
            // 
            this.txtUser.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUser.Location = new System.Drawing.Point(494, 17);
            this.txtUser.Margin = new System.Windows.Forms.Padding(5);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(288, 29);
            this.txtUser.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(374, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 21);
            this.label6.TabIndex = 19;
            this.label6.Text = "User:";
            // 
            // txtPass
            // 
            this.txtPass.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPass.Location = new System.Drawing.Point(494, 60);
            this.txtPass.Margin = new System.Windows.Forms.Padding(5);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(288, 29);
            this.txtPass.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(374, 68);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 21);
            this.label7.TabIndex = 17;
            this.label7.Text = "Pass:";
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIpAddress.Location = new System.Drawing.Point(80, 22);
            this.txtIpAddress.Margin = new System.Windows.Forms.Padding(5);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(270, 29);
            this.txtIpAddress.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 25);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 21);
            this.label9.TabIndex = 13;
            this.label9.Text = "Server:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Nghiệp vụ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 61);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(786, 360);
            this.panel2.TabIndex = 13;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(786, 360);
            this.dataGridView1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbDLU);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.tbnSearch);
            this.panel1.Controls.Add(this.dtfromDate);
            this.panel1.Controls.Add(this.cbHOST);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.dttoDate);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(786, 58);
            this.panel1.TabIndex = 12;
            // 
            // cbDLU
            // 
            this.cbDLU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDLU.FormattingEnabled = true;
            this.cbDLU.Location = new System.Drawing.Point(358, 31);
            this.cbDLU.Name = "cbDLU";
            this.cbDLU.Size = new System.Drawing.Size(187, 21);
            this.cbDLU.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(705, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 45);
            this.button1.TabIndex = 1;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbnSearch
            // 
            this.tbnSearch.Location = new System.Drawing.Point(601, 5);
            this.tbnSearch.Name = "tbnSearch";
            this.tbnSearch.Size = new System.Drawing.Size(87, 45);
            this.tbnSearch.TabIndex = 11;
            this.tbnSearch.Text = "Tìm kiếm";
            this.tbnSearch.UseVisualStyleBackColor = true;
            // 
            // dtfromDate
            // 
            this.dtfromDate.Location = new System.Drawing.Point(65, 5);
            this.dtfromDate.Name = "dtfromDate";
            this.dtfromDate.Size = new System.Drawing.Size(200, 20);
            this.dtfromDate.TabIndex = 3;
            // 
            // cbHOST
            // 
            this.cbHOST.FormattingEnabled = true;
            this.cbHOST.Items.AddRange(new object[] {
            "TDH",
            "TTI"});
            this.cbHOST.Location = new System.Drawing.Point(358, 5);
            this.cbHOST.Name = "cbHOST";
            this.cbHOST.Size = new System.Drawing.Size(187, 21);
            this.cbHOST.TabIndex = 10;
            this.cbHOST.SelectedIndexChanged += new System.EventHandler(this.cbHOST_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Từ ngày";
            // 
            // dttoDate
            // 
            this.dttoDate.Location = new System.Drawing.Point(65, 31);
            this.dttoDate.Name = "dttoDate";
            this.dttoDate.Size = new System.Drawing.Size(200, 20);
            this.dttoDate.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(315, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "HOST";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Đến ngày";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(315, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "DLU";
            // 
            // timer1
            // 
            this.timer1.Interval = 6000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.nghiepvu);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.nghiepvu.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl nghiepvu;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtTTIBackupFilePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTDHFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLogTDHFileBrowser;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLogTTIFileBrowser;
        private System.Windows.Forms.TextBox txtTTIFilePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLogTDHBackupFileBrowser;
        private System.Windows.Forms.TextBox txtTDHBackupFilePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLogTTIBackupFileBrowser;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button txtTestConect;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtfromDate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dttoDate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button tbnSearch;
        private System.Windows.Forms.ComboBox cbHOST;
        private System.Windows.Forms.ComboBox cbDLU;
        private System.Windows.Forms.Button btnReadLog;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

