using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FormGenerator.Controls
{
    public partial class CrossBrowser_Button : System.Web.UI.UserControl
    {
        public event System.EventHandler ehButtonClicked;

        private string iD = "";
        private string cssClass = "";
        private string controlName = "";
        private string successMessage = "";
        private string errorMessage = "";

        public string Id { get { return iD; } set { iD = value; } }
        public string CssClass { get { return cssClass; } set { cssClass = value; } }
        public string SuccessMessage { get { return successMessage; } set { successMessage = value; } }
        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; } }
        public string ControlName { get { return controlName; } set { controlName = value; } }
        public Button txtCrossBrowserSubmit;
        public Label lblSuccessMessage;
        public Label lblErrorMessage;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonClicked(object sender, EventArgs e) 
        {
            if (this.ehButtonClicked != null)
                this.ehButtonClicked(this, new EventArgs());
        }
    }
}