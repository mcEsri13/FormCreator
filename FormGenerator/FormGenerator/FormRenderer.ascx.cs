using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FormGenerator
{
    public partial class FormRenderer : System.Web.UI.UserControl
    {
        public string ItemID { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            ifPageView.Attributes.Add("src", "FormGenerator.aspx?ID=" + ItemID);
        }
    }
}