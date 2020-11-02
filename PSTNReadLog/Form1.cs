using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSTNReadLog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtTDHFilePath.Text = Properties.Settings.Default.LogTDHFilePath;
            txtTDHBackupFilePath.Text = Properties.Settings.Default.LogBackUpTDHFilePath;
            txtTTIFilePath.Text = Properties.Settings.Default.LogTTIFilePath;
            txtTTIBackupFilePath.Text = Properties.Settings.Default.LogBackUpTTIFilePath;
            txtIpAddress.Text = Properties.Settings.Default.Server;
            txtDatabase.Text = Properties.Settings.Default.Database;
            txtUser.Text = Properties.Settings.Default.User;
            txtPass.Text = Properties.Settings.Default.Pass;
        }

        private void btnLogTDHFileBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Log files (*.txt, *.bak)|*.txt;*.bak";
            DialogResult result = openFile.ShowDialog(); 

            if (result == DialogResult.OK) 
            {
                this.txtTDHFilePath.Text = openFile.FileName;
            }
        }

        private void btnLogTDHBackupFileBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Log files (*.txt, *.bak)|*.txt;*.bak";
            DialogResult result = openFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.txtTDHBackupFilePath.Text = openFile.FileName;
            }
        }

        private void btnLogTTIFileBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Log files (*.txt, *.bak)|*.txt;*.bak";
            DialogResult result = openFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.txtTTIFilePath.Text = openFile.FileName;
            }
        }

        private void btnLogTTIBackupFileBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Log files (*.txt, *.bak)|*.txt;*.bak";
            DialogResult result = openFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.txtTTIBackupFilePath.Text = openFile.FileName;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LogTDHFilePath = txtTDHFilePath.Text;
            Properties.Settings.Default.LogBackUpTDHFilePath = txtTDHBackupFilePath.Text;
            Properties.Settings.Default.LogTTIFilePath = txtTTIFilePath.Text;
            Properties.Settings.Default.LogBackUpTTIFilePath = txtTTIBackupFilePath.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Lưu cấu hình thành công");
            this.Refresh();
        }
        public List<ALARM> ReadLogTDH()
        {
            string log = System.IO.File.ReadAllText(txtTDHFilePath.Text);
            List<ALARM> alarms = new List<ALARM>();

            String[] splitString = Regex.Split(log, "END JOB");
            foreach (var item in splitString)
            {
                if (!Regex.IsMatch(item, @".*HISTORY-FILE-INFORMATION.*"))
                {
                    ALARM alarm = new ALARM();
                    //DateTime
                    string patternDate = @"\d{2}-.*-.*:.*:\d{2}";
                    if (Regex.IsMatch(item, patternDate))
                    {
                        MatchCollection mcDate = Regex.Matches(item, patternDate);
                        string stringDate = "20" + mcDate[0].Value.ToString();


                        var datetime = DateTime.Parse(stringDate);

                        string pattern = @"\sDLU.*EAL.*\d{1}";
                        bool isMatch = Regex.IsMatch(item, pattern);
                        if (isMatch == true)
                        {
                            MatchCollection mc = Regex.Matches(item, pattern);
                            string stringERL = mc[0].Value.ToString();

                            if (!Regex.IsMatch(item, @".*END OF ENVIRONMENTAL ALARM.*"))
                            {
                                String[] splitDLU = Regex.Split(stringERL, "  ");
                                alarm.DLU = splitDLU[1];
                                alarm.ERL = splitDLU[5];

                                alarm.startDate = datetime;
                                alarm.endDate = null;
                                alarm.isSuccess = false;
                                alarms.Add(alarm);
                            }
                            else
                            {
                                String[] splitDLU = Regex.Split(stringERL, "  ");
                                alarm.DLU = Regex.Split(splitDLU[0], "=")[1];
                                alarm.ERL = Regex.Split(splitDLU[3], "=")[1];

                                alarm.startDate = null;
                                alarm.endDate = datetime;
                                alarm.isSuccess = false;
                                alarms.Add(alarm);
                            }
                        }
                    }
                }
            }
            return alarms;
        }
        public class ALARM
        {
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string DLU { get; set; }
            public string ERL { get; set; }
            public bool isSuccess { get; set; }
        }
        private void StartLogTDH_Click(object sender, EventArgs e)
        {
            ReadLogTDH();
        }
        public void SaveSourceSetting(string ip, string dataBase, string userName, string passWord)
        {
            Properties.Settings.Default.Server = ip;
            Properties.Settings.Default.Database = dataBase;
            Properties.Settings.Default.Pass = passWord;
            Properties.Settings.Default.User = userName;
            Properties.Settings.Default.Save();
            this.Refresh();
        }

        private void txtTestConect_Click(object sender, EventArgs e)
        {
            string ip = this.txtIpAddress.Text;
            string dataBase = this.txtDatabase.Text;
            string userName = this.txtUser.Text;
            string passWord = this.txtPass.Text;
            string connectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ip, dataBase, userName, passWord);
            TestConnect(connectionString, "sqlserver");
            SaveSourceSetting(ip, dataBase, userName, passWord);
        }
        public void TestConnect(string connectionString, string databaseType)
        {
            string factoryName = String.Empty;

            switch (databaseType.ToLower())
            {
                case "mysql":
                    factoryName = "MySql.Data.MySqlClient";
                    break;
                case "sqlserver":
                    factoryName = "System.Data.SqlClient";
                    break;
            }
            if (String.IsNullOrEmpty(factoryName))
            {
                MessageBox.Show("Không biết loại cơ sở dữ liệu", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DbProviderFactory factory = DbProviderFactories.GetFactory(factoryName);
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection successfull", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Không thế kết nối tới Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
