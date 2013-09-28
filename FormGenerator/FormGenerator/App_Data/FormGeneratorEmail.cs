using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Configuration;

namespace FormGenerator
{
    public class FormGeneratorEmail
    {
        public void Send(DataTable dt, string fieldName, string fieldValue, string subject, string aprimoID, bool isTest)
        {
            MailMessage message = new MailMessage();
            string aprimoEmail = "";

            if(isTest)
                aprimoEmail = ConfigurationManager.AppSettings["AprimoEmailTest"];
            else
                aprimoEmail = ConfigurationManager.AppSettings["AprimoEmailLive"];

            message.Subject = "WEBFORM: " + subject.Replace('-', ' ');
            message.From = new MailAddress("noreply@esri.com");
            string xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine + CreateAprimoXml(dt, fieldName, fieldValue, aprimoID).ToString();
            message.Body = HttpUtility.HtmlDecode(xml);
            message.To.Add(new MailAddress(aprimoEmail));

            SmtpClient smtp = new SmtpClient("SMTP.esri.com", 25);

            smtp.Send(message);

        }

        private XDocument CreateAprimoXml(DataTable dt, string fieldName, string fieldValue, string aprimoID)
        {
            return new XDocument(
                new XDeclaration("1.0", "utf-16", "yes"),
                new XElement("xml",
                    new XElement("target",
                        new XElement("tag_pairs",
                                CreateXElementArray(dt, fieldName, fieldValue),
                            new XElement("tag_pair",
                                new XElement("name", "DS_Flag"),
                                new XElement("values",
                                    new XElement("value", aprimoID)
                                )
                            ),
                            new XElement("tag_pair",
                                new XElement("name", "DS_Flag_Updated"),
                                new XElement("values",
                                    new XElement("value", DateTime.Now.ToString())
                                )
                            )
                        )
                    )
                )
            );
        }

        private XElement[] CreateXElementArray(DataTable dt, string FieldName, string fieldValue)
        {
            XElement[] returnArray = new XElement[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(dt.Rows[i][fieldValue].ToString()))
                {
                    XElement element = new XElement("tag_pair",
                        new XElement("name", dt.Rows[i][FieldName].ToString().Replace(' ', '_')),
                            new XElement("values",
                                new XElement("value", dt.Rows[i][fieldValue].ToString())));

                    returnArray[i] = element;
                }
            }

            return returnArray;
        }
    }
}