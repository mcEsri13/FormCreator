using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.Net.Mail;

namespace FormGenerator
{
    public class SendAprimoEmail
    {
        public void Send(DataTable dt, string fieldName, string fieldValue, string formName)
        {
            MailMessage message = new MailMessage();

            message.Subject = "WEBFORM: " + formName.Replace('-', ' ');  
            message.From = new MailAddress("noreply@esri.com");
            message.Body = CreateAprimoXml(dt, fieldName, fieldValue).ToString();
            message.To.Add(new MailAddress("jesquibel@esri.com"));

            SmtpClient smtp = new SmtpClient("SMTP.esri.com", 25);

            smtp.Send(message);
      
        }

        private XDocument CreateAprimoXml(DataTable dt, string fieldName, string fieldValue)
        {
            return new XDocument(new XDeclaration("1.0", "utf-16", "yes"),
                new XElement("xml",
                    new XElement("targets",
                        new XElement("tag_pairs",
                                CreateXElementArray(dt, fieldName, fieldValue),
                            new XElement("tag_pair",
                                new XElement("name", "DS_Flag_Updated"),
                                new XElement("values",
                                    new XElement("value", DateTime.Now.ToString())
                                )
                            ),
                            new XElement("tag_pair",
                                new XElement("name", "date_stamp"),
                                new XElement("values",
                                    new XElement("value", DateTime.Now.ToString("d"))
                                )
                            ),
                            new XElement("tag_pair",
                                new XElement("name", "time_stamp"),
                                new XElement("values",
                                    new XElement("value", DateTime.Now.ToString("t"))
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
