using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FormGenerator.Controls
{
    public partial class CrossBrowser_Dropdown : System.Web.UI.UserControl
    {
        private string iD = "";
        private string placeholder = "";
        private string validate = "";
        private string cssClass = "";
        private string controlName = "";
        private string toolTip = "";

        public string Id { get { return iD; } set { iD = value; } }
        public string Placeholder { get { return placeholder; } set { placeholder = value; } }
        public string Validate { get { return validate; } set { validate = value; } }
        public string CssClass { get { return cssClass; } set { cssClass = value; } }
        public string ControlName { get { return controlName; } set { controlName = value; } }
        public string ToolTip { get { return toolTip; } set { toolTip = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}