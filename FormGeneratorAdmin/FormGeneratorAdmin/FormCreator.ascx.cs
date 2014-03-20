using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Esri.ICASTokenManager;
using esri.com.Saml;

namespace FormGeneratorAdmin
{
    public partial class FormCreator : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //HttpCookie myCookie = new HttpCookie("IESRISESSIONID");
            //myCookie = Request.Cookies["IESRISESSIONID"];

            //// Read the cookie information and display it.
            //if (myCookie != null)
            //{
            //    FormGeneratorData formData = new FormGeneratorData();

            //    FormGeneratorTools.BindObject(ddlFormList, formData.GetForms(), "Name", "Form_ID", "Select Form");
            //    FormGeneratorTools.BindObject(ddlActions, formData.GetControlActionTypes(), "Name", "ControlActionType_ID", "Select Action");
            //    FormGeneratorTools.BindObject(ddlLayout, formData.GetTemplates(), "friendlyname", "template_id", "Select Layout");
            //    FormGeneratorTools.BindObject(ddlStyle, formData.GetStyles(), "Name", "StyleType_ID", "Select Style");
            //    FormGeneratorTools.BindObject(ddlControlTypes, formData.GetCustomizableControlTypes(), "Name", "ControlType_ID", "Select Control Type");

            //    //load controllists
            //    DataTable fields = formData.GetControls();
            //    int i = 1;
            //    foreach (DataRow fieldRow in fields.Rows)
            //    {
            //        HtmlGenericControl span = new HtmlGenericControl("span");
            //        span.Attributes.Add("clID", fieldRow["ControlList_ID"].ToString());
            //        span.Attributes.Add("ctype", fieldRow["ControlType"].ToString());
            //        span.Attributes.Add("class", "listItem");
            //        span.InnerHtml = fieldRow["Name"].ToString();


            //        pnlFields.Controls.Add(span);

            //        i++;
            //    }
            //}
            //else
            //{
            //    string hasChecked = (string)Session["HasChecked"];

            //    if (hasChecked == null)
            //    {
            //        string certificate = ConfigurationManager.AppSettings[AccountSettings.CertificateKey];
            //        string idpSsoTargetUrl = ConfigurationManager.AppSettings[AccountSettings.IdpSsoTargetUrlKey];
            //        string assertionConsumerUrl = ConfigurationManager.AppSettings[AppSettings.AssertionConsumerServiceUrlKey];
            //        string issuer = ConfigurationManager.AppSettings[AppSettings.IssuerKey];

            //        var accountSettings = new AccountSettings(certificate, idpSsoTargetUrl);
            //        var samlResponse = new Response(accountSettings);
            //        AuthRequest req = new AuthRequest(new AppSettings(assertionConsumerUrl, issuer), accountSettings);

            //        var redirectUrl = accountSettings.IdpSsoTargetUrl + "?SAMLRequest=" + Server.UrlEncode(req.GetRequest(AuthRequest.AuthRequestFormat.Base64));
            //        string relayState = Utils.ParseRelayState(Request);

            //        Session["HasChecked"] = "true";

            //        Response.Redirect(redirectUrl);
            //    }
            //    else
            //    {
            //        Session["HasChecked"] = null;
            //        Response.Write("Not Authorized");
            //        Response.End();
            //    }
            //}

            // for local dev
            FormGeneratorData formData = new FormGeneratorData();

            FormGeneratorTools.BindObject(ddlFormList, formData.GetForms(), "Name", "Form_ID", "Select Form");
            FormGeneratorTools.BindObject(ddlActions, formData.GetControlActionTypes(), "Name", "ControlActionType_ID", "Select Action");
            FormGeneratorTools.BindObject(ddlLayout, formData.GetTemplates(), "friendlyname", "template_id", "Select Layout");
            FormGeneratorTools.BindObject(ddlStyle, formData.GetStyles(), "Name", "StyleType_ID", "Select Style");
            FormGeneratorTools.BindObject(ddlControlTypes, formData.GetCustomizableControlTypes(), "Name", "ControlType_ID", "Select Control Type");
            FormGeneratorTools.BindObject(ddlCustomControlFunctions, formData.GetCustomControlFunctions(), "Name", "CustomControlFunction_ID", "Select Function (optional)");

            //load controllists
            DataTable fields = formData.GetControls();
            int i = 1;
            foreach (DataRow fieldRow in fields.Rows)
            {
                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes.Add("clID", fieldRow["ControlList_ID"].ToString());
                span.Attributes.Add("ctype", fieldRow["ControlType"].ToString());
                span.Attributes.Add("class", "listItem draggable");
                span.InnerHtml = fieldRow["Name"].ToString();


                pnlFields.Controls.Add(span);

                i++;
            }   

        }

    }
}