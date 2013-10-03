using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

namespace FormGeneratorAdmin
{
    public partial class FormCreator : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormGeneratorData formData = new FormGeneratorData();

                FormGeneratorTools.BindObject(ddlFormList, formData.GetForms(), "Name", "Form_ID", "Select Form");
                FormGeneratorTools.BindObject(ddlActions, formData.GetControlActionTypes(), "Name", "ControlActionType_ID", "Select Action");
                FormGeneratorTools.BindObject(ddlLayout, formData.GetTemplates(), "friendlyname", "template_id", "Select Layout");
                FormGeneratorTools.BindObject(ddlStyle, formData.GetStyles(), "Name", "StyleType_ID", "Select Style");

                //load controllists
                DataTable fields = formData.GetControls();
                int i = 1;
                foreach (DataRow fieldRow in fields.Rows)
                {
                    HtmlGenericControl span = new HtmlGenericControl("span");
                    span.Attributes.Add("clID", fieldRow["ControlList_ID"].ToString());
                    span.Attributes.Add("ctype", fieldRow["ControlType"].ToString());
                    span.Attributes.Add("class", "listItem");
                    span.InnerHtml = fieldRow["Name"].ToString();


                    pnlFields.Controls.Add(span);

                    i++;
                }
            }
        }
    }
}