using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PSTNReadLog.Util
{
    public class WebService
    {
        public static string SendAlarmCCSM(string loaialarm, string mathietbi, string levelalarm, string contentalarm, string ngaybatdau, string ngayketthuc, string maclear)
        {
            string publishService = @"http://10.10.16.3:8088/AlarmService.asmx";
            string result = "";
            string method = "http://tempuri.org/SendAlarm";
            string s =
                    @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                    <SendAlarm  xmlns=""http://tempuri.org/"">
                                        <alobj>
                                            <id>0</id>
                                                <alarmid>0</alarmid>
                                                <loaialarm>{0}</loaialarm>
                                                <mathietbi>{1}</mathietbi>
                                                <levelalarm>{2}</levelalarm>
                                                <contentalarm>{3}</contentalarm>
                                                <ngaybatdau>{4}</ngaybatdau>
                                                <ngayketthuc>{5}</ngayketthuc>
                                                <madonvixuly></madonvixuly>
                                                <daxuatphieu>0</daxuatphieu>
                                                <danghiemthu>0</danghiemthu>
                                                <xacnhancanhbao>0</xacnhancanhbao>
                                                <noidungxacnhan></noidungxacnhan>
                                                <MaNguyenNhan>0</MaNguyenNhan>
                                                <maclear>{6}</maclear>
                                                <khonghien>0</khonghien>
                                                <ngayxacnhanCOF></ngayxacnhanCOF>
                                                <CountTimeXLAlarm></CountTimeXLAlarm>
                                                <CountTimeXLAlarmOver10></CountTimeXLAlarmOver10>
                                                <STT>0</STT>
                                                <nguoixacnhan></nguoixacnhan>
                                        </alobj>
                                    </SendAlarm>
                                </s:Body>
                                </s:Envelope>";
            string input = string.Format(s, loaialarm, mathietbi, levelalarm, contentalarm, ngaybatdau, ngayketthuc, maclear);
            try
            {
                result = CallRestMethod(publishService, method, input);
            }
            catch (Exception e)
            {
                result = e.ToString();
            }
            
            return result;
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
        
    }
}
