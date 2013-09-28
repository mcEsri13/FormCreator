using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FormGenerator.Controls
{
    public partial class CrossBrowser_TextBox : System.Web.UI.UserControl
    {
        //private string iD = "";
        private string watermark = "";
        private string _for = "";
        //private string validate = "";
        //private string cssClass = "";
        //private string controlName = "";
        //private string toolTip = "";

        //public string Id { get { return tbxCrossBrowser.ID; } set { tbxCrossBrowser.ID = value; } }
        public string Watermark { get { return watermark; } set { watermark = value; } }
        public string For { get { return _for; } set { _for = value; } }
        //public string Validate { get { return validate; } set { validate = value; } }
        //public string CssClass { get { return cssClass; } set { cssClass = value; } }
        //public string ControlName { get { return controlName; } set { controlName = value; } }
        //public string ToolTip { get { return toolTip; } set { toolTip = value; } }
        public TextBox tbxCrossBrowser;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}