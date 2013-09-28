using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;

namespace FormGenerator
{
    public partial class TwoColumn1 : System.Web.UI.Page
    {
        private string templatePath = string.Empty;
        private string pageID = string.Empty;
        private string itemID = string.Empty;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            FormGeneratorData d = new FormGeneratorData();

            itemID = "{4C8C460D-40FD-45EA-A76E-B1AD7953371D}";

            //if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            //{
            //    itemID = Request.QueryString["ID"].ToString();
            //}

            DataTable dtPage = d.GetPageBySitecoreItemID(itemID);
            pageID = dtPage.Rows[0]["Page_ID"].ToString();
            this.MasterPageFile = dtPage.Rows[0]["TemplatePath"].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FormGeneratorData d = new FormGeneratorData();
            MasterPage mp = Page.Master;
            this.Form.ID = d.GetPageByPageID(pageID).Rows[0]["FormName"].ToString();

            DataTable dtControls = d.GetPageControlsByPageID(pageID);

            foreach (DataRow dr in dtControls.Rows)
            {
                string pageControl_ID = dr["pageControl_ID"].ToString();
                string phID = dr["ASP_ID"].ToString();
                string type = dr["TypeName"].ToString();
                PlaceHolder ph = (PlaceHolder)mp.FindControl(phID);
                DataTable dtProperties = d.GetPageControlPropertyValuesByPageControl_ID(pageControl_ID);

                if (type == "TextBox")
                {
                    TextBox tb = new TextBox();
                    tb.ID = dr["AprimoFieldName"].ToString();
                    tb.CssClass = dr["CssClass"].ToString();
                    tb.Attributes.Add("placeholder", dr["Watermark"].ToString());
                    tb.Attributes.Add("validate", dr["DoValidate"].ToString());
                    tb.Attributes.Add("controlName", dr["Type"].ToString().ToLower());

                    ph.Controls.Add(tb);
                }
                else if (type == "ListBox")
                {
                    DataTable dt = d.GetControlOptionsByPageControl_ID(dr["PageControl_ID"].ToString());
                    ListBox lb = new ListBox();
                    lb.ID = dr["AprimoFieldName"].ToString();
                    lb.CssClass = dr["CssClass"].ToString();
                    lb.SelectionMode = ListSelectionMode.Multiple;

                    lb.Attributes.Add("validate", dr["DoValidate"].ToString());
                    lb.Attributes.Add("controlName", dr["Type"].ToString().ToLower());

                    FormGeneratorTools.BindObject(lb, dt, "Text", "Value", "Select One");

                    ph.Controls.Add(lb);
                }
                else if (type == "DropDownList")
                {
                    DataTable dt = d.GetControlOptionsByPageControl_ID(dr["PageControl_ID"].ToString());
                    DropDownList ddl = new DropDownList();
                    ddl.ID = dr["AprimoFieldName"].ToString();
                    ddl.CssClass = dr["CssClass"].ToString();

                    ddl.Attributes.Add("validate", dr["DoValidate"].ToString());
                    ddl.Attributes.Add("controlName", dr["Type"].ToString().ToLower());

                    FormGeneratorTools.BindObject(ddl, dt, "Text", "Value", "Select One");

                    ph.Controls.Add(ddl);
                }
                else if (type == "Multi-line")
                {
                    TextBox tb = new TextBox();
                    tb.ID = dr["AprimoFieldName"].ToString();
                    tb.CssClass = dr["CssClass"].ToString();
                    tb.TextMode = TextBoxMode.MultiLine;

                    tb.Attributes.Add("placeholder", dr["Watermark"].ToString());
                    tb.Attributes.Add("controlName", dr["Type"].ToString().ToLower());
                    tb.Attributes.Add("validate", dr["DoValidate"].ToString());

                    ph.Controls.Add(tb);
                }
                else if (type == "Literal")
                {
                    Literal lit = new Literal();
                    lit.ID = dr["AprimoFieldName"].ToString();

                    SetControlProperties(lit, dtProperties);

                    ph.Controls.Add(lit);
                }
                else if (type == "Image")
                {
                    Image img = new Image();

                    SetControlProperties(img, dtProperties);

                    ph.Controls.Add(img);
                }
                else if (type == "Submit")
                {
                    Button submit = new Button();
                    submit.Text = "Submit";
                    submit.OnClientClick = "return IsValid()";
                    submit.Click += new EventHandler(submit_Click);
                    submit.ID = dr["AprimoFieldName"].ToString();


                    ph.Controls.Add(submit);
                }

                ph.Controls.Add(new LiteralControl("<br />"));
            }
        }

        protected void submit_Click(Object sender, EventArgs e)
        {
            DataTable data = FormGeneratorTools.GenerateFieldList(Form.Controls);

            FormGeneratorEmail email = new FormGeneratorEmail();

            email.Send(data, "Field_ID", "Field_Value", Form.ID, "",true);
        }

        private void SetControlPropertyByName(object control, string propertyName, string value)
        {
            var controlType = control.GetType();
            controlType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(control,value,null);

        }

        private void SetControlProperties(Control control, DataTable dtControlProperties)
        {
            foreach (DataRow r in dtControlProperties.Rows)
            {
                string pName = r["PropertyName"].ToString();
                string pValue = r["SettingValue"].ToString();

                SetControlPropertyByName(control, pName, pValue);
            }        
        }
    }
}