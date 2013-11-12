using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using System.Web.UI;
//using FormGenerator.Controls;

namespace FormGeneratorAdmin
{
    public static class FormGeneratorTools
    {
        public static void BindObject(ListControl ddl, DataTable data, string textColumn, string valueConlumn, string defaultText)
        {
            ddl.DataSource = data;
            ddl.DataTextField = textColumn;
            ddl.DataValueField = valueConlumn;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(defaultText, "-1"));
        }

        //public static DataTable GenerateFieldList(ControlCollection controls)
        //{
        //    DataTable data = new DataTable();
        //    data.Columns.Add("Field_ID");
        //    data.Columns.Add("Field_Value");

        //    foreach (Control control in (ControlCollection)controls)
        //    {
        //        if (control.GetType() == typeof(PlaceHolder))
        //        {
        //            PlaceHolder ph = (PlaceHolder)control;

        //            foreach (Control phc in ph.Controls)
        //            {
        //                DataRow dr = data.NewRow();

        //                string test = typeof(CrossBrowser_TextBox).ToString();

        //                //if (phc.GetType() == typeof(TextBox))
        //                if (phc.GetType().ToString() == "ASP.controls_crossbrowser_textbox_ascx")
        //                {
        //                    TextBox tb = ((CrossBrowser_TextBox)phc).tbxCrossBrowser;
        //                    dr["Field_ID"] = tb.ID;
        //                    dr["Field_Value"] = tb.Text;
        //                }
        //                else if (phc.GetType().ToString() == "ASP.controls_crossbrowser_textarea_ascx")
        //                {
        //                    TextBox tb = ((CrossBrowser_TextArea)phc).tbxCrossBrowser;
        //                    dr["Field_ID"] = tb.ID;
        //                    dr["Field_Value"] = tb.Text;
        //                }
        //                else if (phc.GetType() == typeof(DropDownList))
        //                {
        //                    DropDownList ddl = (DropDownList)phc;
        //                    dr["Field_ID"] = ddl.ID;
        //                    dr["Field_Value"] = ddl.Text;
        //                }
        //                else if (phc.GetType() == typeof(ListBox))
        //                {
        //                    ListBox lb = (ListBox)phc;
        //                    dr["Field_ID"] = lb.ID;

        //                    foreach (ListItem li in lb.Items)
        //                    {
        //                        if(li.Selected)
        //                            dr["Field_Value"] += li.Text + ",";
        //                    }
        //                }

        //                data.Rows.Add(dr);
        //            }
        //        }
        //    }

        //    return data;
        //}

        public static bool SendEmail(DataTable data, string subject, string to, string from)
        {
            MailMessage message = new MailMessage();

            string bodyMessage = "<table>";

            foreach (DataRow dr in data.Rows)
            {
                bodyMessage += "<tr>";
                bodyMessage += "<td>" + dr["Field_ID"].ToString() + "</td><td>" + dr["Field_Value"].ToString() + "</td>";
                bodyMessage += "</tr>";
            }

            bodyMessage += "</table>";

            message.To.Add(to);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(bodyMessage, null, "text/html");
            message.AlternateViews.Add(htmlView);

            message.Subject = subject;
            message.From = new MailAddress(from);
            message.Body = bodyMessage;

            SmtpClient smtp = new SmtpClient("SMTP.esri.com", 25);

            smtp.Send(message);

            return true;
        }

        
    }
}