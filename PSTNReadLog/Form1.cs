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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Timer = System.Windows.Forms.Timer;

namespace PSTNReadLog
{
    public partial class Form1 : Form
    {
        ISqlQueryExecutor sqlQueryExecutor;

        public Form1()
        {
            InitializeComponent();
            txtTDHFilePath.Text = @"\\10.10.30.35\Log\THUONGDINH\BMMLLog1.txt";
            txtTDHBackupFilePath.Text = @"\\10.10.30.35\Log\THUONGDINH\BMMLLog1.bak";
            txtTTIFilePath.Text = @"\\10.10.30.35\Log\THANHTRI\BMMLLog2.txt";
            txtTTIBackupFilePath.Text = @"\\10.10.30.35\Log\THANHTRI\BMMLLog2.bak";
            txtIpAddress.Text = "10.10.117.167";
            txtDatabase.Text = "WebPSTN";
            txtUser.Text = "sa";
            txtPass.Text = "11aDminDhm@";

            timer1 = new Timer();
            timer1.Tick += timer1_Tick;
            timer1.Interval = 180000;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            //var pathTest = @"E:\HNI\PSTN Log\statdlu tdh.txt";
            //var maxTime = DateTime.Now.AddMinutes(-30);
            //var dtMaxTimeTDH = GetMaxTimeEvent("TDH");
            //var maxtimeTDH = dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString() != "" ? Convert.ToDateTime(dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);
            //Thread t = new Thread(() =>
            //{
            //    var alarmTDH = ReadLog(pathTest, "TDH", maxtimeTDH > maxTime ? maxtimeTDH : maxTime);
            //});
            //t.Start();
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
            Properties.Settings.Default.LogTDHFilePath = @"\\10.10.30.35\thuongdinh\BMMLLog1.txt";
            Properties.Settings.Default.LogBackUpTDHFilePath = @"\\10.10.30.35\thuongdinh\BMMLLog1.bak";
            Properties.Settings.Default.LogTTIFilePath = @"\\10.10.30.35\thanhtri\BMMLLog2.txt";
            Properties.Settings.Default.LogBackUpTTIFilePath = @"\\10.10.30.35\thanhtri\BMMLLog2.bak";
            Properties.Settings.Default.Save();
            MessageBox.Show("Lưu cấu hình thành công");
            this.Refresh();
        }

        public List<ALARM> ReadLog(string filePath, string host, DateTime maxTimeEvent)
        {
            try
            {
                var datenow = DateTime.Now;
                string log = System.IO.File.ReadAllText(filePath);
                List<ALARM> alarms = new List<ALARM>();
                String[] splitString = Regex.Split(log, " JOB ");
                foreach (var item in splitString)
                {
                    string patternDate1 = @"\d{2}-.*-.*:.*:\d{2}";
                    string stringDate1 = "";
                    if (Regex.IsMatch(item, patternDate1))
                    {
                        MatchCollection mcDate1 = Regex.Matches(item, patternDate1);
                        stringDate1 = "20" + mcDate1[0].Value.ToString();
                        var datetime1 = Convert.ToDateTime(stringDate1);

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
                                        alarm.dateStart = datenow;
                                        alarm.endDate = null;
                                        alarm.isSuccess = false;
                                        alarm.isBegin = true;
                                        alarm.HOST = host;
                                        alarms.Add(alarm);
                                        InsertAlarm(alarm);
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
                                        alarm.dateStart = datenow;
                                        alarm.isSuccess = false;
                                        alarm.isBegin = false;
                                        alarm.HOST = host;
                                        //alarm.LastString = lastString;
                                        alarms.Add(alarm);
                                        UpdateAlarmEnd(alarm);
                                        
                                    }
                                }

                            }

                            string patternBeginNETM = @"\sX25LINK.*FROM  ACT  TO  UNA.*";
                            bool isMatchNETM = Regex.IsMatch(item, patternBeginNETM);
                            if (isMatchNETM == true)
                            {
                                MatchCollection mcDate = Regex.Matches(item, patternDate);
                                string stringDate = "20" + mcDate[mcDate.Count - 1].Value.ToString();

                                var datetime = Convert.ToDateTime(stringDate);
                                if (datetime > maxTimeEvent)
                                {

                                    MatchCollection content = Regex.Matches(item, patternBeginNETM);
                                    alarm.isTongDai = true;
                                    alarm.DLU = content[0].Value.ToString().Replace("FROM  ACT  TO  UNA", "").Trim();
                                    alarm.ERL = "6";
                                    alarm.dateEvent = datetime;
                                    alarm.startDate = datetime;
                                    alarm.dateStart = datenow;
                                    alarm.endDate = null;
                                    alarm.isSuccess = false;
                                    alarm.isBegin = true;
                                    alarm.HOST = host;
                                    alarms.Add(alarm);
                                    InsertAlarm(alarm);
                                }


                            }
                            string patternEndNETM = @"\sX25LINK.*FROM  UNA  TO  ACT.*";
                            bool isMatchEndNETM = Regex.IsMatch(item, patternEndNETM);
                            if (isMatchEndNETM == true)
                            {
                                MatchCollection mcDate = Regex.Matches(item, patternDate);
                                string stringDate = "20" + mcDate[mcDate.Count - 1].Value.ToString();

                                var datetime = Convert.ToDateTime(stringDate);
                                if (datetime > maxTimeEvent)
                                {

                                    MatchCollection content = Regex.Matches(item, patternEndNETM);
                                    alarm.isTongDai = true;
                                    alarm.DLU = content[0].Value.ToString().Replace("FROM  UNA  TO  ACT","").Trim();
                                    alarm.ERL = "6";
                                    alarm.dateEvent = datetime;
                                    alarm.startDate = null;
                                    alarm.endDate = datetime;
                                    alarm.dateStart = datenow;
                                    alarm.isSuccess = false;
                                    alarm.isBegin = true;
                                    alarm.HOST = host;
                                    alarms.Add(alarm);
                                    UpdateAlarmEnd(alarm);
                                }
                            }

                            string patternBeginOMT = @"\sOMT-.*FROM  ACT  TO  UNA.*";
                            bool isMatchOMT = Regex.IsMatch(item, patternBeginOMT);
                            if (isMatchOMT == true)
                            {
                                MatchCollection mcDate = Regex.Matches(item, patternDate);
                                string stringDate = "20" + mcDate[mcDate.Count - 1].Value.ToString();

                                var datetime = Convert.ToDateTime(stringDate);
                                if (datetime > maxTimeEvent)
                                {

                                    MatchCollection content = Regex.Matches(item, patternBeginOMT);
                                    alarm.isTongDai = true;
                                    alarm.DLU = content[0].Value.ToString().Replace("FROM  ACT  TO  UNA", "").Trim();
                                    alarm.ERL = "7";
                                    alarm.dateEvent = datetime;
                                    alarm.startDate = datetime;
                                    alarm.dateStart = datenow;
                                    alarm.endDate = null;
                                    alarm.isSuccess = false;
                                    alarm.isBegin = true;
                                    alarm.HOST = host;
                                    alarms.Add(alarm);
                                    InsertAlarm(alarm);
                                }


                            }

                            string patternEndOMT = @"\sOMT-.*FROM  UNA  TO  ACT.*";
                            bool isMatchEndOMT = Regex.IsMatch(item, patternEndOMT);
                            if (isMatchEndOMT == true)
                            {
                                MatchCollection mcDate = Regex.Matches(item, patternDate);
                                string stringDate = "20" + mcDate[mcDate.Count - 1].Value.ToString();

                                var datetime = Convert.ToDateTime(stringDate);
                                if (datetime > maxTimeEvent)
                                {

                                    MatchCollection content = Regex.Matches(item, patternEndOMT);
                                    alarm.isTongDai = true;
                                    alarm.DLU = content[0].Value.ToString().Replace("FROM  UNA  TO  ACT", "").Trim();
                                    alarm.ERL = "7";
                                    alarm.dateEvent = datetime;
                                    alarm.startDate = null;
                                    alarm.endDate = datetime;
                                    alarm.dateStart = datenow;
                                    alarm.isSuccess = false;
                                    alarm.isBegin = true;
                                    alarm.HOST = host;
                                    alarms.Add(alarm);
                                    UpdateAlarmEnd(alarm);
                                }
                            }

                            //Cảnh báo môi trường tổng đài

                            if (Regex.IsMatch(item, @".*EXTERNAL ALARM EXCHANGE.*") 
                                && !Regex.IsMatch(item, @".*END OF ENVIRONMENTAL ALARM.*") && !Regex.IsMatch(item, @".*DATA:*"))
                            {
                                MatchCollection mcDate = Regex.Matches(item, patternDate);
                                string stringDate = "20" + mcDate[mcDate.Count - 1].Value.ToString();

                                var datetime = Convert.ToDateTime(stringDate);
                                if (datetime > maxTimeEvent)
                                {

                                    alarm.isTongDai = true;
                                    alarm.DLU = host;
                                    alarm.ERL = Regex.Match(Regex.Match(item, @".*EAL.*\d*").Value.Trim(), @"\d+").Value.ToString().Trim() ;
                                    alarm.dateEvent = datetime;
                                    alarm.startDate = datetime;
                                    alarm.dateStart = datenow;
                                    alarm.endDate = null;
                                    alarm.isSuccess = false;
                                    alarm.isBegin = true;
                                    alarm.HOST = host;
                                    alarms.Add(alarm);
                                    InsertAlarm(alarm);
                                }
                            }
                            if (Regex.IsMatch(item, @".*END OF ENVIRONMENTAL ALARM.*") && !Regex.IsMatch(item, @".*DATA:*"))
                            {
                                MatchCollection mcDate = Regex.Matches(item, patternDate);
                                string stringDate = "20" + mcDate[mcDate.Count - 1].Value.ToString();

                                var datetime = Convert.ToDateTime(stringDate);
                                if (datetime > maxTimeEvent)
                                {

                                    alarm.isTongDai = true;
                                    alarm.DLU = host;
                                    alarm.ERL = Regex.Match(Regex.Match(item, @".*EAL=.*\d*").Value.Trim(), @"\d+").Value.ToString().Trim();
                                    alarm.dateEvent = datetime;
                                    alarm.dateStart = datenow;
                                    alarm.startDate = null;
                                    alarm.endDate = datetime;
                                    alarm.isSuccess = false;
                                    alarm.isBegin = true;
                                    alarm.HOST = host;
                                    alarms.Add(alarm);
                                    UpdateAlarmEnd(alarm); 
                                }
                            }

                            ///Check side
                            ///
                            if(Regex.IsMatch(item, @".*DLU.*DLUC-OST.*LTG.*DEG.*") && datetime1 > DateTime.Now.AddMinutes(5))
                            {
                                string patternSide = @"\d+\s+\w+\s+\d+-\s*\d+.*";
                                MatchCollection sides = Regex.Matches(item, patternSide);
                                foreach(var side in sides)
                                {
                                    var dlu = Regex.Match(side.ToString().Trim(), @"\d+\s").Value.Trim();
                                    MatchCollection content = Regex.Matches(side.ToString().Trim(), @"\w+\s+\d+-\s*\d+\s+\w*");
                                    var side_0 = content[0].Value.ToString().Trim();
                                    var side_1 = "";
                                    if(content.Count > 1)
                                    {
                                        side_1 = content[1].Value.ToString().Trim();
                                    }
                                    string checkSql = String.Format("SELECT * FROM SIDE WHERE DLU = '{0}' AND TONGDAI = '{1}'",
                                        dlu, host);

                                    sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                                    DataTable lsSide = new DataTable();
                                    lsSide = sqlQueryExecutor.Execute(checkSql);
                                    if(lsSide.Rows.Count > 0)
                                    {
                                        string sql = String.Format("UPDATE SIDE SET SIDE_0 = '{0}', SIDE_1 = '{1}', TIMEUPDATE = getdate() WHERE DLU = '{2}' AND TONGDAI = '{3}'",
                                        side_0, side_1, dlu, host
                                        );
                                        sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                                        sqlQueryExecutor.ExecuteUpdate(sql);
                                    }
                                    else
                                    {
                                        string sql = String.Format("INSERT INTO SIDE(DLU, SIDE_0, SIDE_1, TIMEUPDATE,TONGDAI) VALUES('{0}','{1}','{2}', getdate(),'{3}')",
                                        dlu, side_0, side_1, host
                                        );
                                        sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                                        sqlQueryExecutor.ExecuteUpdate(sql);
                                    }
                                    
                                }
                            }


                        }

                    }

                }
                var time = DateTime.Now - datenow;
                return alarms.OrderBy(x => x.dateEvent).ToList();
                
            }catch(Exception ex)
            {
                return null;
            }
            

        }
        public List<ALARM> ReadLogOld(string filePath, string host, DateTime maxTimeEvent)
        {
            string log = System.IO.File.ReadAllText(filePath);
            log.Replace("'", "''");
            string lastStringDB = GetLastString(host);
            if (lastStringDB == null || lastStringDB == "")
            {
                string lastStringUpdate = log.Substring(log.Length - 590, 590);
                string sql = "update ALARM set [DateEvent] = GETDATE(), [LastString] = '" + Base64Encode(lastStringUpdate) + "' where ID = (select MAX(ID) from [WebPSTN].[dbo].[ALARM] where HOST = '" + host + "')";
                sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                sqlQueryExecutor.ExecuteUpdate(sql);
                return null;
            }
            else
            {
                String[] lognews = Regex.Split(log, Base64Decode(lastStringDB));
                if (lognews.Length > 1)
                {
                    string lastString = log.Substring(log.Length - 590, 590);
                    string sql = "update ALARM set [DateEvent] = GETDATE(), [LastString] = '" + Base64Encode(lastString) + "' where ID = (select MAX(ID) from [WebPSTN].[dbo].[ALARM] where HOST = '" + host + "')";
                    sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                    sqlQueryExecutor.ExecuteUpdate(sql);
                    string lognew = lognews[1].ToString();
                    List<ALARM> alarms = new List<ALARM>();

                    String[] splitString = Regex.Split(lognew, "END JOB");
                    foreach (var item in splitString)
                    {
                        string patternDate1 = @"\d{2}-.*-.*:.*:\d{2}";
                        string stringDate1 = "";
                        if (Regex.IsMatch(item, patternDate1))
                        {
                            MatchCollection mcDate1 = Regex.Matches(item, patternDate1);
                            stringDate1 = "20" + mcDate1[0].Value.ToString();
                        }

                        var datetime1 = stringDate1 != "" ? Convert.ToDateTime(stringDate1) : DateTime.Now.Date;
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
                                        alarm.LastString = Base64Encode(lastString);
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
                                        alarm.LastString = lastString;
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
                else
                {
                    List<ALARM> alarms = new List<ALARM>();
                    string lastString = log.Substring(log.Length - 590, 590);
                    string sql = "update ALARM set [DateEvent] = GETDATE(), [LastString] = '" + Base64Encode(lastString) + "' where ID = (select MAX(ID) from [WebPSTN].[dbo].[ALARM] where HOST = '" + host + "')";
                    sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                    sqlQueryExecutor.ExecuteUpdate(sql);
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

                        var datetime1 = stringDate1 != "" ? Convert.ToDateTime(stringDate1) : DateTime.Now.Date;
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
                                        alarm.LastString = Base64Encode(lastString);
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
                                        alarm.LastString = lastString;
                                        alarms.Add(alarm);
                                        XuatPhieuAlarm(alarm);
                                        UpdateAlarmEnd(alarm);
                                    }
                                }

                            }
                        }
                    }
                    return alarms.OrderBy(x => x.dateEvent).ToList();
                }
            }

        }

        public class ALARM
        {
            public DateTime dateEvent { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? dateStart { get; set; }
            public DateTime? endDate { get; set; }
            public string DLU { get; set; }
            public string ERL { get; set; }
            public bool isSuccess { get; set; }
            public int LevelAlarm { get; set; }
            public int Status { get; set; }
            public string HOST { get; set; }
            public string LastString { get; set; }
            public bool isTongDai  { get; set; }
            public bool isBegin { get; set; }
        }
        public void InsertAlarm(ALARM alarm)
        {
            if (CheckConnect() == "OK")
            {
                InsertAlarmCCSM(alarm);
                XuatPhieuAlarm(alarm);
                string sql = String.Format("INSERT INTO ALARM(DLU,ERL,StartDate,EndDate,IsSuccess,LevelAlarm,Status,HOST,DateEvent,LastString) VALUES('{0}','{1}', CONVERT(datetime, '{2}', 103), CONVERT(datetime, '{3}', 103), '{4}', '{5}', '{6}', '{7}', CONVERT(datetime, '{8}', 103), '{9}')",
                alarm.DLU, alarm.ERL, alarm.startDate, alarm.endDate, alarm.isSuccess, alarm.LevelAlarm, alarm.Status, alarm.HOST, alarm.dateEvent, alarm.dateStart.Value.ToString("dd/MM/yyyy HH:mm:ss"));
                sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                sqlQueryExecutor.ExecuteUpdate(sql);
                
                
            }
        }
        public DataTable GetAlarmBegin(ALARM alarm)
        {
            if (CheckConnect() == "OK")
            {
                string sql = String.Format("SELECT * FROM ALARM WHERE DLU = '{0}' AND ERL = '{1}' AND HOST = '{2}' AND IsSuccess = 0 ORDER BY ID desc",
                                        alarm.DLU, alarm.ERL, alarm.HOST);

                sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                DataTable lsAlarm = new DataTable();
                lsAlarm = sqlQueryExecutor.Execute(sql);
                return lsAlarm;
            }
            else
            {
                return null;
            }
            
        }
        public void UpdateAlarmEnd(ALARM alarm)
        {
            if (CheckConnect() == "OK")
            {
                var lsAlarm = GetAlarmBegin(alarm);
                if (lsAlarm.Rows.Count > 0)
                {
                    XuatPhieuAlarm(alarm);
                    UpdateAlarmEndCCSM(alarm);
                    string sql = String.Format("UPDATE ALARM SET EndDate = CONVERT(datetime, '{0}', 103), IsSuccess = 1, DateEvent = CONVERT(datetime, '{1}', 103) WHERE ID = {2}",
                    alarm.endDate, alarm.dateEvent, lsAlarm.Rows[0]["ID"]);
                    sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                    sqlQueryExecutor.ExecuteUpdate(sql);
                }
                
            }
            
        }
        public void InsertAlarmCCSM(ALARM alarm)
        {
            if (CheckConnect() == "OK")
            {
                if (alarm.isTongDai == true)
                {
                    var dmAlarm = GetDMAlarm(alarm.ERL, alarm.HOST, "TONGDAI");
                    if (dmAlarm.Rows.Count > 0)
                    {
                        var levelalarm = "";
                        if (dmAlarm.Rows[0]["CapDo"].ToString() == "2")
                        {
                            levelalarm = "MJ";
                        }
                        if (dmAlarm.Rows[0]["CapDo"].ToString() == "3")
                        {
                            levelalarm = "CR";
                        }
                        var result = PostAlarmToCCSM(dmAlarm.Rows[0]["LoaiCCSM"].ToString(), alarm.HOST , levelalarm,
                            alarm.HOST + ":" + dmAlarm.Rows[0]["Name"].ToString() + "_" + alarm.DLU,
                            alarm.HOST + ":" + dmAlarm.Rows[0]["Name"].ToString() + "_" + alarm.DLU,
                            alarm.startDate.Value.ToString("dd-MM-yyyy HH:mm:ss"),
                            "", "PSTN"
                            );
                        //var result = Util.WebService.SendAlarmCCSM(
                        //    dmAlarm.Rows[0]["LoaiCCSM"].ToString(),
                        //    alarm.HOST,
                        //    levelalarm, 
                        //    alarm.HOST + ":" + dmAlarm.Rows[0]["Name"].ToString() + "_" + alarm.DLU,
                        //    alarm.startDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                        //    "",
                        //    alarm.HOST +  alarm.DLU + dmAlarm.Rows[0]["LoaiCCSM"].ToString()
                        //    );
                        LogXuatPhieu(result, alarm.HOST + "_" + alarm.DLU + "_" + dmAlarm.Rows[0]["LoaiCCSM"].ToString() + "_" + alarm.dateEvent.ToString("dd/MM/yyyy HH:mm:ss"));
                    }
                }
                else
                {
                    var dmAlarm = GetDMAlarm(alarm.ERL, alarm.HOST, "VETINH");
                    var dmVeTinh = GetDMVeTinh(alarm.DLU, alarm.HOST);
                    if (dmAlarm.Rows.Count > 0 && dmVeTinh.Rows.Count > 0)
                    {
                        var levelalarm = "";
                        if (dmAlarm.Rows[0]["CapDo"].ToString() == "2")
                        {
                            levelalarm = "MJ";
                        }
                        if (dmAlarm.Rows[0]["CapDo"].ToString() == "3")
                        {
                            levelalarm = "CR";
                        }
                        var result = PostAlarmToCCSM(dmAlarm.Rows[0]["LoaiCCSM"].ToString(), alarm.HOST + "." + dmVeTinh.Rows[0]["MaVT"].ToString() + "." + alarm.DLU, levelalarm,
                            dmVeTinh.Rows[0]["MaVT"].ToString() + ":" + dmAlarm.Rows[0]["Name"].ToString(), dmVeTinh.Rows[0]["MaVT"].ToString() + ":" + dmAlarm.Rows[0]["Name"].ToString(),
                            alarm.startDate.Value.ToString("dd-MM-yyyy HH:mm:ss"),
                            "", "PSTN"
                            );
                        //var result = Util.WebService.SendAlarmCCSM(
                        //    dmAlarm.Rows[0]["LoaiCCSM"].ToString(),
                        //    alarm.HOST + "." + dmVeTinh.Rows[0]["MaVT"].ToString() + "." + alarm.DLU,
                        //    levelalarm, dmVeTinh.Rows[0]["MaVT"].ToString() + ":" + dmAlarm.Rows[0]["Name"].ToString(),
                        //    alarm.startDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                        //    "",
                        //    alarm.HOST + dmVeTinh.Rows[0]["MaVT"].ToString() + alarm.DLU + dmAlarm.Rows[0]["LoaiCCSM"].ToString()
                        //    );
                        LogXuatPhieu(result, alarm.HOST + "_" + dmVeTinh.Rows[0]["MaVT"].ToString() + "_" + alarm.DLU + "_" + dmAlarm.Rows[0]["LoaiCCSM"].ToString() + "_" + alarm.dateEvent.ToString("dd/MM/yyyy HH:mm:ss"));
                    }
                }
            }
        }
        public DataTable GetAlarmBeginCCSM(ALARM alarm)
        {
            if (CheckConnect() == "OK")
            {
                string sql = String.Format("SELECT * FROM PSTNAlarmTapTrung WHERE NAlarm like '{0}%' AND TrangThaiEnd = 0 ORDER BY MaSend desc",
                                        alarm.HOST + "_" + alarm.DLU + "_" + alarm.ERL + "_");

                sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                DataTable lsAlarm = new DataTable();
                lsAlarm = sqlQueryExecutor.Execute(sql);
                return lsAlarm;
            }
            else
            {
                return null;
            }
            
        }
        public void UpdateAlarmEndCCSM(ALARM alarm)
        {
            if (CheckConnect() == "OK")
            {
                if (alarm.isTongDai == true)
                {
                    var dmAlarm = GetDMAlarm(alarm.ERL, alarm.HOST, "TONGDAI");
                    if (dmAlarm.Rows.Count > 0)
                    {
                        var levelalarm = "";
                        if (dmAlarm.Rows[0]["CapDo"].ToString() == "2")
                        {
                            levelalarm = "MJ";
                        }
                        if (dmAlarm.Rows[0]["CapDo"].ToString() == "3")
                        {
                            levelalarm = "CR";
                        }
                        var result =  PostAlarmToCCSM(dmAlarm.Rows[0]["LoaiCCSM"].ToString(), alarm.HOST, levelalarm,
                            alarm.HOST + ":" + dmAlarm.Rows[0]["Name"].ToString() + "_" + alarm.DLU,
                            alarm.HOST + ":" + dmAlarm.Rows[0]["Name"].ToString() + "_" + alarm.DLU,
                            "",
                            alarm.endDate.Value.ToString("dd-MM-yyyy HH:mm:ss"),
                            "PSTN"
                            );
                        //var result = Util.WebService.SendAlarmCCSM(
                        //    dmAlarm.Rows[0]["LoaiCCSM"].ToString(),
                        //    alarm.HOST,
                        //    levelalarm,
                        //    alarm.HOST + ":" + dmAlarm.Rows[0]["Name"].ToString() + "_" + alarm.DLU,
                        //    "",
                        //    alarm.endDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                        //    alarm.HOST + alarm.DLU + dmAlarm.Rows[0]["LoaiCCSM"].ToString()
                        //    );
                        LogXuatPhieu(result, alarm.HOST + "_" + alarm.DLU + "_" + dmAlarm.Rows[0]["LoaiCCSM"].ToString() + "_" + alarm.dateEvent.ToString("dd/MM/yyyy HH:mm:ss"));
                    }
                }
                else
                {
                    var dmAlarm = GetDMAlarm(alarm.ERL, alarm.HOST, "VETINH");
                    var dmVeTinh = GetDMVeTinh(alarm.DLU, alarm.HOST);
                    var levelalarm = "";
                    
                    if (dmAlarm.Rows.Count > 0 && dmVeTinh.Rows.Count > 0)
                    {
                        if (dmAlarm.Rows[0]["CapDo"].ToString() == "2")
                        {
                            levelalarm = "MJ";
                        }
                        if (dmAlarm.Rows[0]["CapDo"].ToString() == "3")
                        {
                            levelalarm = "CR";
                        }
                        var result =  PostAlarmToCCSM(dmAlarm.Rows[0]["LoaiCCSM"].ToString(),
                            alarm.HOST + "." + dmVeTinh.Rows[0]["MaVT"].ToString() + "." + alarm.DLU, levelalarm,
                            dmVeTinh.Rows[0]["MaVT"].ToString() + ":" + dmAlarm.Rows[0]["Name"].ToString(), dmVeTinh.Rows[0]["MaVT"].ToString() + ":" + dmAlarm.Rows[0]["Name"].ToString(),
                            "",
                            alarm.endDate.Value.ToString("dd-MM-yyyy HH:mm:ss"),
                            "PSTN"
                            );
                        //var result = Util.WebService.SendAlarmCCSM(
                        //        dmAlarm.Rows[0]["LoaiCCSM"].ToString(),
                        //        alarm.HOST + "." + dmVeTinh.Rows[0]["MaVT"].ToString() + "." + alarm.DLU,
                        //        levelalarm, dmVeTinh.Rows[0]["MaVT"].ToString() + ":" + dmAlarm.Rows[0]["Name"].ToString(),
                        //        "",
                        //        alarm.endDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                        //        alarm.HOST + dmVeTinh.Rows[0]["MaVT"].ToString() + alarm.DLU + dmAlarm.Rows[0]["LoaiCCSM"].ToString()
                        //        );
                        LogXuatPhieu(result, alarm.HOST + "_" + dmVeTinh.Rows[0]["MaVT"].ToString() + "_" + alarm.DLU + "_" + dmAlarm.Rows[0]["LoaiCCSM"].ToString() + "_" + alarm.dateEvent.ToString("dd/MM/yyyy HH:mm:ss"));

                    }
                }
                
            }
            
        }
        public DataTable GetMaxTimeEvent( string host)
        {
            if (CheckConnect() == "OK")
            {
                string sql = "SELECT MAX(DateEvent) AS MAXDATE FROM ALARM WHERE HOST = '" + host + "'";
                sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                DataTable result = new DataTable();
                result = sqlQueryExecutor.Execute(sql);
                return result;
            }
            else
            {
                return null;
            }
        }
        public string GetLastString(string host)
        {
            if (CheckConnect() == "OK")
            {
                string sql = "SELECT top(10) LastString FROM ALARM WHERE  HOST = '" + host + "' ORDER BY ID DESC";
                sqlQueryExecutor = new SqlServerSqlQueryExecutor(Properties.Settings.Default.Server, 1433, Properties.Settings.Default.Database, Properties.Settings.Default.User, Properties.Settings.Default.Pass);
                DataTable result = new DataTable();
                result = sqlQueryExecutor.Execute(sql);
                return result.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
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
            try
            {
                if (CheckConnect() == "OK")
                {
                    var maxTime = DateTime.Now.AddMinutes(-30);
                    var dtMaxTimeTDH = GetMaxTimeEvent("TDH");
                    var maxtimeTDH = dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString() != "" ? Convert.ToDateTime(dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);
                    Thread t = new Thread(() =>
                    {
                        var alarmTDH = ReadLog(txtTDHBackupFilePath.Text, "TDH", maxtimeTDH > maxTime ? maxtimeTDH : maxTime);
                    });
                    t.Start();
                    Thread t1 = new Thread(() =>
                    {
                        var alarms = ReadLog(txtTDHFilePath.Text, "TDH", maxtimeTDH > maxTime ? maxtimeTDH : maxTime);
                    });
                    t1.Start();

                    //TTI
                    var dtMaxTimeTTI = GetMaxTimeEvent("TTI");
                    var maxtimeTTI = dtMaxTimeTTI.Rows[0]["MAXDATE"].ToString() != "" ? Convert.ToDateTime(dtMaxTimeTTI.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);
                    Thread t2 = new Thread(() =>
                    {
                        var alarmTTIBak = ReadLog(txtTTIBackupFilePath.Text, "TTI", maxtimeTTI > maxTime ? maxtimeTTI : maxTime);
                    });
                    t2.Start();
                    Thread t3 = new Thread(() =>
                    {
                        var alarmTTI = ReadLog(txtTTIFilePath.Text, "TTI", maxtimeTTI > maxTime ? maxtimeTTI : maxTime);
                    });
                    t3.Start();

                    //var pathTest = @"E:\HNI\PSTN Log\ALARM_EWSD.txt";
                    //var maxTime = DateTime.Now.AddMinutes(-30);
                    //var dtMaxTimeTDH = GetMaxTimeEvent("TDH");
                    //var maxtimeTDH = dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString() != "" ? Convert.ToDateTime(dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);
                    //Thread t = new Thread(() =>
                    //{
                    //    var alarmTDH = ReadLog(pathTest, "TDH", maxtimeTDH > maxTime ? maxtimeTDH : maxTime);
                    //});
                    //t.Start();
                }
            }
            catch(Exception ex)
            {
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var maxTime = DateTime.Now.AddMinutes(-30);

            var dtMaxTimeTDH = GetMaxTimeEvent("TDH");
            var maxtimeTDH = Convert.ToDateTime(dtMaxTimeTDH.Rows[0]["MAXDATE"]) > DateTime.Now.AddMinutes(-30) ? Convert.ToDateTime(dtMaxTimeTDH.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);
            //var maxtimeTDH =  DateTime.Now.AddMinutes(-50);

            var alarmTDH = ReadLog(txtTDHBackupFilePath.Text, "TDH", maxtimeTDH);
            var alarms = ReadLog(txtTDHFilePath.Text, "TDH", maxtimeTDH);


            //TTI
            var dtMaxTimeTTI = GetMaxTimeEvent("TTI");

            var maxtimeTTI = Convert.ToDateTime(dtMaxTimeTTI.Rows[0]["MAXDATE"]) > DateTime.Now.AddMinutes(-30) ? Convert.ToDateTime(dtMaxTimeTTI.Rows[0]["MAXDATE"].ToString()) : DateTime.Now.AddMinutes(-30);
            //var maxtimeTTI =  DateTime.Now.AddMinutes(-30);

            var alarmTTIBak = ReadLog(txtTTIBackupFilePath.Text, "TTI", maxtimeTTI);
            var alarmTTI = ReadLog(txtTTIFilePath.Text, "TTI", maxtimeTTI);
        }

        private void btnReadLog_Click(object sender, EventArgs e)
        {
            //timer1.Enabled = !timer1.Enabled;
            //btnReadLog.Text = btnReadLog.Text == "Stop" ? "Start" : "Stop";
            var alarms = ReadLog(txtTDHFilePath.Text, "TDH", DateTime.Now.AddMinutes(-30));
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
            if(alarm.isTongDai == true)
            {

            }
            else
            {
                var dmAlarm = GetDMAlarm(alarm.ERL, alarm.HOST, "VETINH");
                var dmVeTinh = GetDMVeTinh(alarm.DLU, alarm.HOST);
                string url = "http://10.10.20.49/dhscInternalApi/api/v1/Service/CanhBaoSuCoMangNew";
                string result = "";
                if (dmAlarm.Rows.Count > 0 && dmVeTinh.Rows.Count > 0 && alarm.ERL != "5")
                {
                    if (alarm.isBegin == true)
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
                            alarm.HOST + "_" + dmVeTinh.Rows[0]["MaVT"] + "_" + alarm.ERL,
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
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string PostAlarmToCCSM(string loaialarm, string mathietbi, string levelalarm, string contentalarm,
            string description, string ngaybatdau_string, string ngayketthuc_string, string nhomsuco)
        {
            string result = "";
            try
            {
                var URL = "http://10.10.16.3:2022/api/CCSM/insert_db_ccsm";
                var DATA = @"{""loaialarm"":""" + loaialarm + "\"," +
                            @"""mathietbi"":""" + mathietbi + "\"," +
                            @"""levelalarm"":""" + levelalarm + "\"," +
                            @"""contentalarm"":""" + contentalarm + "\"," +
                            @"""description"":""" + description + "\"," +
                            @"""ngaybatdau_string"":""" + ngaybatdau_string + "\"," +
                            @"""ngayketthuc_string"":""" + ngayketthuc_string + "\"," +
                            @"""nhomsuco"":""" + nhomsuco + "\"" +
                "}";
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.BaseAddress = new System.Uri(URL);

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiY3Vvbmdscy5iZ2dAdm5wdC52biIsImp0aSI6IjlhOWQ4NWQzLTZhZjYtNGVhOC05MTQ2LTk0YmE1NjY1NDYxZSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6ImNjc21fYWRtaW4iLCJleHAiOjE3NTI5OTg5NzYsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.kQFotHWsraa3817o7MaXWy6ILIvNdurNFdjqY8zJSJo");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");
                HttpResponseMessage messge = client.PostAsync(URL, content).Result;
                if (messge.IsSuccessStatusCode)
                {
                    result = messge.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    result = "put data faild";
                }
            }catch(Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
