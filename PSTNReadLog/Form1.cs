using PSTNReadLog.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PSTNReadLog
{
    public partial class Form1 : Form
    {
        ISqlQueryExecutor sqlQueryExecutor;

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

            timer1 = new Timer();
            timer1.Tick += timer1_Tick;
            timer1.Interval = 18000;
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
        public List<ALARM> ReadLog(string filePath, string host, DateTime maxTimeEvent)
        {
            string log = System.IO.File.ReadAllText(filePath);
            List<ALARM> alarms = new List<ALARM>();

            String[] splitString = Regex.Split(log, "END JOB");
            foreach (var item in splitString)
            {
                string patternDate1 = @"\d{2}-.*-.*:.*:\d{2}";
                string stringDate1 = "";
                if (Regex.IsMatch(item, patternDate1))
                {
                    MatchCollection mcDate1 = Regex.Matches(item, patternDate1);
                    stringDate1 = "20" + mcDate1[0].Value.ToString();
                }
                
                var datetime1 = stringDate1 != "" ?  Convert.ToDateTime(stringDate1) : DateTime.Now.Date;
                if (!Regex.IsMatch(item, @".*HISTORY-FILE-INFORMATION.*") && datetime1 > maxTimeEvent)
                {
                    ALARM alarm = new ALARM();
                    //DateTime
                    string patternDate = @"\d{2}-.*-.*:.*:\d{2}";

                    string patternBegin = @"\sDLU.*EAL\s*\d*";
                    bool isMatch = Regex.IsMatch(item, patternBegin);
                    if (isMatch == true)
                    {
                        MatchCollection mcDate = Regex.Matches(item, patternDate);
                        string stringDate = "20" + mcDate[mcDate.Count - 1].Value.ToString();

                        var datetime = Convert.ToDateTime(stringDate);
                        if (datetime > maxTimeEvent)
                        {
                            MatchCollection mc = Regex.Matches(item, patternBegin);
                            string stringERL = mc[0].Value.ToString();

                            if (Regex.IsMatch(item, @".*EXTERNAL ALARM DLU.*") && !Regex.IsMatch(item, @".*END OF ENVIRONMENTAL ALARM.*") 
                                && !Regex.IsMatch(item, @".*EXTERNAL ALARM DLU END*") && !Regex.IsMatch(item, @".*DATA:*"))
                            {
                                String[] splitDLU = Regex.Split(stringERL, "       ");
                                alarm.DLU = Regex.Match(splitDLU[0], @"\d+").Value;
                                alarm.ERL = Regex.Match(splitDLU[1], @"\d+").Value;
                                alarm.dateEvent = datetime;
                                alarm.startDate = datetime;
                                alarm.endDate = null;
                                alarm.isSuccess = false;
                                alarm.isBegin = true;
                                alarm.HOST = host;
                                alarms.Add(alarm);
                                InsertAlarm(alarm);
                                InsertAlarmCCSM(alarm);
                                XuatPhieuAlarm(alarm);
                            }
                        }


                    }
                    string patternEnd = @"\sDLU.*EAL=\S*";
                    bool isMatchEnd = Regex.IsMatch(item, patternEnd);
                    if (isMatchEnd == true)
                    {
                        MatchCollection mcDate = Regex.Matches(item, patternDate);
                        string stringDate = "20" + mcDate[mcDate.Count - 1].Value.ToString();

                        var datetime = Convert.ToDateTime(stringDate);
                        if (datetime > maxTimeEvent)
                        {
                            MatchCollection mc = Regex.Matches(item, patternEnd);
                            string stringERL = mc[0].Value.ToString();
                            if (Regex.IsMatch(item, @".*END OF ENVIRONMENTAL ALARM.*"))
                            {
                                String[] splitDLU = Regex.Split(stringERL, "=");

                                alarm.DLU = Regex.Match(splitDLU[1], @"\d+").Value;
                                alarm.ERL = Regex.Match(splitDLU[2], @"\d+").Value;
                                alarm.dateEvent = datetime;
                                alarm.startDate = null;
                                alarm.endDate = datetime;
                                alarm.isSuccess = false;
                                alarm.isBegin = false;
                                alarm.HOST = host;
                                alarms.Add(alarm);
                                XuatPhieuAlarm(alarm);
                                UpdateAlarmEnd(alarm);
                                UpdateAlarmEndCCSM(alarm);
                            }
                        }

                    }
                }
            }
            return alarms.OrderBy(x => x.dateEvent).ToList();
        }
        public class ALARM
        {
            public DateTime dateEvent { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string DLU { get; set; }
            public string ERL { get; set; }
            public bool isSuccess { get; set; }
            public int LevelAlarm { get; set; }
            public int Status { get; set; }
            public string HOST { get; set; }
            public bool isBegin { get; set; }
        }
        public void InsertAlarm(ALARM alarm)
        {
            string sql = String.Format("INSERT INTO ALARM VALUES('{0}','{1}', CONVERT(datetime, '{2}', 103), CONVERT(datetime, '{3}', 103), '{4}', '{5}', '{6}', '{7}', CONVERT(datetime, '{8}', 103))",
                alarm.DLU, alarm.ERL, alarm.startDate, alarm.endDate, alarm.isSuccess, alarm.LevelAlarm, alarm.Status, alarm.HOST, alarm.dateEvent);
            sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
            sqlQueryExecutor.ExecuteUpdate(sql);
            
        }
        public DataTable GetAlarmBegin(ALARM alarm)
        {
            string sql = String.Format("SELECT * FROM ALARM WHERE DLU = '{0}' AND ERL = '{1}' AND HOST = '{2}' AND IsSuccess = 0 ORDER BY ID desc",
                                        alarm.DLU, alarm.ERL, alarm.HOST);

            sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
            DataTable lsAlarm = new DataTable();
            lsAlarm = sqlQueryExecutor.Execute(sql);
            return lsAlarm;
        }
        public void UpdateAlarmEnd(ALARM alarm)
        {
            var lsAlarm = GetAlarmBegin(alarm);
            if(lsAlarm.Rows.Count > 0)
            {
                string sql = String.Format("UPDATE ALARM SET EndDate = CONVERT(datetime, '{0}', 103), IsSuccess = 1, DateEvent = CONVERT(datetime, '{1}', 103) WHERE ID = {2}",
                alarm.endDate, alarm.dateEvent, lsAlarm.Rows[0]["ID"]);
                sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                sqlQueryExecutor.ExecuteUpdate(sql);
            }
        }
        public void InsertAlarmCCSM(ALARM alarm)
        {
            var dmAlarm = GetDMAlarm(alarm.ERL, alarm.HOST, "VETINH");
            var dmVeTinh = GetDMVeTinh(alarm.DLU, alarm.HOST);
            
            if (dmAlarm.Rows.Count > 0 && dmVeTinh.Rows.Count >0)
            {
                string sql = String.Format("INSERT INTO PSTNAlarmTapTrung VALUES('{0}','{1}', '', CONVERT(datetime, '{2}', 103),'{3}', '{4}', '{5}', '{6}', '', '{7}', '{7}', '', '{8}', '{9}', '{10}', '')",
                alarm.HOST, dmVeTinh.Rows[0]["MaVT"], alarm.dateEvent, "",
                "Cap " + dmAlarm.Rows[0]["CapDo"], dmVeTinh.Rows[0]["MaVT"] + ":" + dmAlarm.Rows[0]["Name"], alarm.startDate.Value.ToString("dd/MM/yyyy HH:mm:ss"), alarm.isSuccess, alarm.DLU, dmAlarm.Rows[0]["LoaiCCSM"],
                alarm.HOST + "_" + alarm.DLU + "_" + alarm.ERL + "_" + alarm.dateEvent.ToString("yyyyMMddHHmmss"));
                sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                sqlQueryExecutor.ExecuteUpdate(sql);

                var lsAlarm = GetAlarmBeginCCSM(alarm);
                if (lsAlarm.Rows.Count > 0)
                {
                    string sql1 = String.Format("UPDATE PSTNAlarmTapTrung SET MaClear = '{0}' WHERE MaSend = {1}",
                    alarm.HOST + lsAlarm.Rows[0]["MaSend"], lsAlarm.Rows[0]["MaSend"]);
                    sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                    sqlQueryExecutor.ExecuteUpdate(sql1);
                }
            }

        }
        public DataTable GetAlarmBeginCCSM(ALARM alarm)
        {
            string sql = String.Format("SELECT * FROM PSTNAlarmTapTrung WHERE NAlarm like '{0}%' AND TrangThaiEnd = 0 ORDER BY MaSend desc",
                                        alarm.HOST + "_" + alarm.DLU + "_" + alarm.ERL + "_");

            sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
            DataTable lsAlarm = new DataTable();
            lsAlarm = sqlQueryExecutor.Execute(sql);
            return lsAlarm;
        }
        public void UpdateAlarmEndCCSM(ALARM alarm)
        {
            var lsAlarm = GetAlarmBeginCCSM(alarm);
            if (lsAlarm.Rows.Count > 0)
            {
                string sql = String.Format("UPDATE PSTNAlarmTapTrung SET TimeEnd = '{0}', TrangThaiEnd = 1, TrangThaiXL = 0 WHERE MaSend = {1}",
                alarm.endDate.Value.ToString("dd/MM/yyyy HH:mm:ss"), lsAlarm.Rows[0]["MaSend"]);
                sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                sqlQueryExecutor.ExecuteUpdate(sql);
            }
        }
        public DataTable GetMaxTimeEvent( string host)
        {
            string sql = "SELECT MAX(DateEvent) AS MAXDATE FROM ALARM WHERE HOST = '" + host + "'";
            sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
            DataTable result = new DataTable();
            result = sqlQueryExecutor.Execute(sql);
            return result;
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
        public string CheckConnect()
        {
            string ip = this.txtIpAddress.Text;
            string dataBase = this.txtDatabase.Text;
            string userName = this.txtUser.Text;
            string passWord = this.txtPass.Text;
            string connectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", ip, dataBase, userName, passWord);
            string result = String.Empty;

            
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                try
                {
                    connection.Open();
                    result = "OK";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Không thế kết nối tới Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = "NOT OK";
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CheckConnect() == "OK")
            {
                var maxTime = DateTime.Now.AddMinutes(-30);

                var dtMaxTimeTDH = GetMaxTimeEvent("TDH");
                var maxtimeTDH = dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString() != "" ? Convert.ToDateTime(dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);

                var alarmTDH = ReadLog(txtTDHBackupFilePath.Text, "TDH", maxtimeTDH > maxTime ? maxtimeTDH : maxTime);
                var alarms = ReadLog(txtTDHFilePath.Text, "TDH", maxtimeTDH > maxTime ? maxtimeTDH : maxTime);


                //TTI
                var dtMaxTimeTTI = GetMaxTimeEvent("TTI");
                var maxtimeTTI = dtMaxTimeTTI.Rows[0]["MAXDATE"].ToString() != "" ? Convert.ToDateTime(dtMaxTimeTTI.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);

                var alarmTTIBak = ReadLog(txtTTIBackupFilePath.Text, "TTI", maxtimeTTI > maxTime ? maxtimeTTI : maxTime);
                var alarmTTI = ReadLog(txtTTIFilePath.Text, "TTI", maxtimeTTI > maxTime ? maxtimeTTI : maxTime);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var maxTime = DateTime.Now.AddMinutes(-30);

            var dtMaxTimeTDH = GetMaxTimeEvent("TDH");
            var maxtimeTDH = dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString() != "" ? Convert.ToDateTime(dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);

            var alarmTDH = ReadLog(txtTDHBackupFilePath.Text, "TDH", maxtimeTDH);
            var alarms = ReadLog(txtTDHFilePath.Text, "TDH", maxtimeTDH);


            //TTI
            var dtMaxTimeTTI = GetMaxTimeEvent("TTI");
            var maxtimeTTI = dtMaxTimeTTI.Rows[0]["MAXDATE"].ToString() != "" ? Convert.ToDateTime(dtMaxTimeTTI.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);

            var alarmTTIBak = ReadLog(txtTTIBackupFilePath.Text, "TTI", maxtimeTTI);
            var alarmTTI = ReadLog(txtTTIFilePath.Text, "TTI", maxtimeTTI);
         }

        private void btnReadLog_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            btnReadLog.Text = btnReadLog.Text == "Stop" ? "Start" : "Stop";
        }
        

        public void LoadDLUCombobox(string host)
        {
            string sql = String.Format("SELECT UR, TenVT FROM QuanLyVT WHERE Host = '{0}' UNION SELECT 0 MaDLU, N'Tất cả' Name", host);

            sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
            DataTable dt = new DataTable();
            dt = sqlQueryExecutor.Execute(sql);
            

            cbDLU.ValueMember = "UR";

            cbDLU.DisplayMember = "TenVT";

            cbDLU.DataSource = dt;
        }

        private void cbHOST_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDLUCombobox(cbHOST.Text);
        }
        public DataTable GetDMAlarm(string maAlarm, string host, string loai)
        {
            string sql = String.Format("SELECT * FROM DM_ALARM WHERE MaAlarm = '{0}' AND HOST = '{1}' AND Loai = '{2}'",
                                        maAlarm, host, loai);

            sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
            DataTable lsAlarm = new DataTable();
            lsAlarm = sqlQueryExecutor.Execute(sql);
            return lsAlarm;
        }
        public DataTable GetDMVeTinh(string dlu, string host)
        {
            string sql = String.Format("SELECT * FROM QuanLyVT WHERE UR = '{0}' AND Host = '{1}'", dlu, host);

            sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
            DataTable lsAlarm = new DataTable();
            lsAlarm = sqlQueryExecutor.Execute(sql);
            return lsAlarm;
        }
        public void XuatPhieuAlarm(ALARM alarm)
        {
            var dmAlarm = GetDMAlarm(alarm.ERL, alarm.HOST, "VETINH");
            var dmVeTinh = GetDMVeTinh(alarm.DLU, alarm.HOST);
            string url = "http://10.10.20.49/dhscInternalApi/api/v1/Service/CanhBaoSuCoMangNew";
            string result = "";
            if (dmAlarm.Rows.Count > 0 && dmVeTinh.Rows.Count > 0 && alarm.ERL != "5")
            {
                if(alarm.isBegin == true)
                {
                    string input = String.Format("\"MaSuCo\": \"{0}\",\"MaVeTinh\": \"{1}\",\"LoaiMang\":\"{2}\",\"LoaiCanhBao\":\"{3}\",\"CapDoId\": \"{4}\",\"TgSuCo\": \"{5}\",\"TgClr\":\"{6}\",\"MaTinhThanh\":\"{7}\",\"TrangThai\": \"{8}\",\"HeThongId\": \"{9}\",\"NoiDungCanhBao\":\"{10}\"",
                    //alarm.HOST + "_" + dmVeTinh.Rows[0]["MaVT"] + "_" + alarm.DLU + "_" + alarm.ERL+ "_" + alarm.startDate.Value.ToString("ddMMyyyyHHmmss"),
                    alarm.HOST + "_" + dmVeTinh.Rows[0]["MaVT"] + "_" + alarm.ERL,
                    dmVeTinh.Rows[0]["MaVT"],
                    40112,
                    dmAlarm.Rows[0]["LoaiCanhBao"],
                    dmAlarm.Rows[0]["CapDo"],
                    alarm.dateEvent.ToString("dd/MM/yyyy HH:mm:ss"),
                    "",
                    "HNI",
                    1,
                    1,
                    dmVeTinh.Rows[0]["MaVT"] + ":" + alarm.DLU + ":" + dmAlarm.Rows[0]["Name"]
                    );
                    result = POST(url, "{" + input + "}");
                    LogXuatPhieu(result, input);
                }
                else
                {
                    var lsAlarm = GetAlarmBegin(alarm);
                    if (lsAlarm.Rows.Count > 0)
                    {
                        string input = String.Format("\"MaSuCo\": \"{0}\",\"MaVeTinh\": \"{1}\",\"LoaiMang\":\"{2}\",\"LoaiCanhBao\":\"{3}\",\"CapDoId\": \"{4}\",\"TgSuCo\": \"{5}\",\"TgClr\":\"{6}\",\"MaTinhThanh\":\"{7}\",\"TrangThai\": \"{8}\",\"HeThongId\": \"{9}\",\"NoiDungCanhBao\":\"{10}\"",
                        //alarm.HOST + "_" + dmVeTinh.Rows[0]["MaVT"] + "_" + alarm.DLU + "_" + alarm.ERL + "_" + Convert.ToDateTime(lsAlarm.Rows[0]["StartDate"].ToString()).ToString("ddMMyyyyHHmmss"),
                        alarm.HOST + "_" + dmVeTinh.Rows[0]["MaVT"] +  "_" + alarm.ERL,
                        dmVeTinh.Rows[0]["MaVT"],
                        40112,
                        dmAlarm.Rows[0]["LoaiCanhBao"],
                        dmAlarm.Rows[0]["CapDo"],
                        Convert.ToDateTime(lsAlarm.Rows[0]["StartDate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                        alarm.dateEvent.ToString("dd/MM/yyyy HH:mm:ss"),
                        "HNI",
                        3,
                        1,
                        dmVeTinh.Rows[0]["MaVT"] + ":" + alarm.DLU + ":" + dmAlarm.Rows[0]["Name"]
                        );
                        result = POST(url, "{" + input + "}");
                        LogXuatPhieu(result, input);
                    }
                }
            }
        }
        
        public static string POST(string url, string jsonContent)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            client.BaseAddress = new System.Uri(url);
            byte[] cred = UTF8Encoding.UTF8.GetBytes("DhscApiUsername:@@Dhsc@pIP@ssw0rd@@");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            System.Net.Http.HttpContent content = new StringContent(jsonContent, UTF8Encoding.UTF8, "application/json");
            HttpResponseMessage messge = client.PostAsync(url, content).Result;
            string description = string.Empty;
            if (messge.IsSuccessStatusCode)
            {
                string result = messge.Content.ReadAsStringAsync().Result;
                description = result;
            }
            return description;
        }
        public static string CallRestMethod(string url, string method, string input)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "POST";
            webrequest.ContentType = "text/xml;charset=UTF-8";
            webrequest.Accept = "text/xml";
            webrequest.Headers.Add("SOAPAction", method);

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byte1 = encoding.GetBytes(input);

            webrequest.GetRequestStream().Write(byte1, 0, byte1.Length);

            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            result = xmlDoc.InnerText;
            webresponse.Close();
            return result;
        }

        private void nghiepvu_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbHOST.Text = "TDH";
            LoadDLUCombobox(cbHOST.Text);
        }
        public void LogXuatPhieu(string status, string content)
        {
            string sql = String.Format("INSERT INTO LogXuatPhieu VALUES(CONVERT(datetime, '{0}', 103),'{1}', '{2}')",
                DateTime.Now , status, content);
            sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
            sqlQueryExecutor.ExecuteUpdate(sql);
        }
    }
}
