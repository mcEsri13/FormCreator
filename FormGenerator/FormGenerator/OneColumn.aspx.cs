using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace FormGenerator
{
    public partial class OneColumn : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = "Layouts/OneColumn.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FormGeneratorData d = new FormGeneratorData();
            MasterPage mp = Page.Master;
            this.Form.ID = d.GetPageByPageID("1").Rows[0]["FormName"].ToString();

            DataTable dtControls = d.GetPageControlsByPageID("1");

            foreach (DataRow dr in dtControls.Rows)
            {
                string phID = dr["ASP_ID"].ToString();
                string type = dr["TypeName"].ToString();
                PlaceHolder ph = (PlaceHolder)mp.FindControl(phID);

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


                    ph.Controls.Add(lit);
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

            email.Send(data, "Field_ID", "Field_Value", Form.ID, "", true);
        }
    }
}