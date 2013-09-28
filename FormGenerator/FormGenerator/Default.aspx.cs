using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

namespace FormGenerator
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = "Layouts/OneColumn.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FormGeneratorData d = new FormGeneratorData();
            MasterPage mp = Page.Master;

            DataTable dtControls = d.GetPageControlsByPageID("1");

            foreach (DataRow dr in dtControls.Rows)
            {
                string phID = dr["ASP_ID"].ToString();
                string type = dr["TypeName"].ToString();
                PlaceHolder ph = (PlaceHolder)mp.FindControl(phID);

                if (type == "TextBox")
                {
                    TextBox tb = new TextBox();
                    tb.ID = dr["Unique_ID"].ToString();
                    tb.CssClass = dr["CssClass"].ToString();
                    tb.Attributes.Add("placeholder", dr["Placeholder"].ToString());

                    ph.Controls.Add(tb);
                }

                if (type == "ListBox")
                {
                    ListBox lb = new ListBox();
                    lb.ID = dr["Unique_ID"].ToString();
                    lb.CssClass = dr["CssClass"].ToString();

                    ph.Controls.Add(lb);
                }
                else if (type == "DropDownList")
                {
                    DropDownList ddl = new DropDownList();
                    ddl.ID = dr["Unique_ID"].ToString();
                    ddl.CssClass = dr["CssClass"].ToString();

                    ph.Controls.Add(ddl);
                }
                else if (type == "Multi-line")
                {
                    TextBox tb = new TextBox();
                    tb.ID = dr["Unique_ID"].ToString();
                    tb.CssClass = dr["CssClass"].ToString();
                    tb.TextMode = TextBoxMode.MultiLine;
                    tb.Attributes.Add("placeholder", dr["Placeholder"].ToString());

                    ph.Controls.Add(tb);
                }
                

                ph.Controls.Add(new LiteralControl("<br />"));
            }

        }
    }
}