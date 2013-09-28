using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using FormGenerator.Controls;

namespace FormGenerator
{
    public partial class FormGenerator : System.Web.UI.Page
    {
        private string templatePath = string.Empty;
        private string pageID = string.Empty;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            FormGeneratorData d = new FormGeneratorData();
            string itemID = "";

            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                itemID = Request.QueryString["ID"].ToString();

                DataTable dtPage = d.GetPageBySitecoreItemID(itemID);
                pageID = dtPage.Rows[0]["Page_ID"].ToString();
                this.MasterPageFile = dtPage.Rows[0]["TemplatePath"].ToString();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            String[] css = new String[] { "http://code.jquery.com/ui/1.10.0/themes/base/jquery-ui.css"
                                        , "../css/Common.css"
                                        , "../css/OneColumn.css"
                                        , "../css/ibmcognosdemo.css"
                                        , "../css/Controls.css" };

            String[] js = new String[] { "http://api.demandbase.com/autocomplete/widget.js"
                                       , "http://code.jquery.com/jquery-1.8.3.js"
                                       , "http://code.jquery.com/ui/1.10.0/jquery-ui.js"
                                       , "../js/DemandBase.js"
                                       , "../js/FormGenerator.js" };

            for (int i = 0; i < css.Length; i++)
            {
                HtmlLink link = new HtmlLink();
                link.Attributes.Add("href", Page.ResolveClientUrl(css[i]));
                link.Attributes.Add("type", "text/css");
                link.Attributes.Add("rel", "stylesheet");
                Page.Header.Controls.Add(link);
            }

            for (int i = 0; i < js.Length; i++)
            {
                HtmlGenericControl script = new HtmlGenericControl("script");
                script.Attributes.Add("type", "text/javascript");
                script.Attributes.Add("src", js[i]);
                Page.Header.Controls.Add(script);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpBrowserCapabilities browser = Request.Browser;
                FormGeneratorData d = new FormGeneratorData();
                MasterPage mp = Page.Master;

                DataTable dtForm = d.GetPageByPageID(pageID);


                if (dtForm.Rows.Count > 0)
                {
                    this.Form.ID = dtForm.Rows[0]["FormName"].ToString();

                    if (dtForm.Rows[0]["CSSClass"] != DBNull.Value)
                        this.Form.Attributes.Add("class", dtForm.Rows[0]["CSSClass"].ToString());

                    if (dtForm.Rows[0]["Tracking_Campaign"] != DBNull.Value)
                    {
                        HiddenField hfCampaign = new HiddenField();
                        hfCampaign.ID = "c";
                        hfCampaign.Value = dtForm.Rows[0]["Tracking_Campaign"].ToString();

                        this.Form.Controls.Add(hfCampaign);
                    }

                    if (dtForm.Rows[0]["Tracking_Source"] != DBNull.Value)
                    {
                        HiddenField hfSource = new HiddenField();
                        hfSource.ID = "s";
                        hfSource.Value = dtForm.Rows[0]["Tracking_Source"].ToString();

                        this.Form.Controls.Add(hfSource);
                    }

                    if (dtForm.Rows[0]["Tracking_Page"] != DBNull.Value)
                    {
                        HiddenField hfPage = new HiddenField();
                        hfPage.ID = "_p";
                        hfPage.Value = dtForm.Rows[0]["Tracking_Page"].ToString();

                        this.Form.Controls.Add(hfPage);
                    }
                }
                else
                    return;

                object aprimoID = dtForm.Rows[0]["Aprimo_ID"];

                object subjectLine = dtForm.Rows[0]["Subject"];

                object isModal = dtForm.Rows[0]["IsModal"];

                if (isModal != null)
                {
                    bool isModalForm = Convert.ToBoolean(isModal);

                    PlaceHolder ph = (PlaceHolder)mp.FindControl("phHidden");

                    HtmlGenericControl hidden = new HtmlGenericControl();
                    hidden.TagName = "input";
                    hidden.ID = "hidIsModal";
                    hidden.Attributes.Add("type", "hidden");

                    if (isModalForm)
                        hidden.Attributes.Add("value", "true");
                    else
                        hidden.Attributes.Add("value", "false");

                    ph.Controls.Add(hidden);
                }

                if (aprimoID != null)
                    this.Form.Attributes.Add("AprimoID", aprimoID.ToString());

                if (subjectLine != null)
                    this.Form.Attributes.Add("AprimoSubject", subjectLine.ToString());

                DataTable dtControls = d.GetPageControlsByPageID(pageID);

                foreach (DataRow dr in dtControls.Rows)
                {
                    string pageControl_ID = dr["pageControl_ID"].ToString();
                    string phID = dr["ASP_ID"].ToString();
                    string type = dr["TypeName"].ToString();
                    PlaceHolder ph = (PlaceHolder)mp.FindControl(phID);
                    DataTable dtProperties = d.GetPageControlPropertyValuesByPageControl_ID(pageControl_ID);
                    string isRequired = dr["IsRequired"].ToString();

                    if (type == "TextBox")
                    {
                        CrossBrowser_TextBox c = (CrossBrowser_TextBox)LoadControl("Controls/CrossBrowser_TextBox.ascx");

                        string controlName = dr["Type"].ToString().ToLower();

                        if (isRequired == "True")
                            c.Watermark = dr["Watermark"].ToString() + " *";
                        else
                            c.Watermark = dr["Watermark"].ToString();

                        c.tbxCrossBrowser.ID = dr["AprimoFieldName"].ToString();
                        c.tbxCrossBrowser.CssClass = dr["CssClass"].ToString();
                        c.tbxCrossBrowser.Attributes.Add("validate", dr["DoValidate"].ToString());
                        c.tbxCrossBrowser.Attributes.Add("controlname", dr["Type"].ToString().ToLower());

                        if (controlName == "company")
                        {
                            c.tbxCrossBrowser.ClientIDMode = ClientIDMode.Static;
                            c.tbxCrossBrowser.ID = "company";
                        }

                        ph.Controls.Add(c);

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

                        ph.Controls.Add(new LiteralControl("<br />"));
                    }
                    else if (type == "DropDownList")
                    {
                        DropDownList ddl = new DropDownList();
                        bool hasConstantValues = Convert.ToBoolean(dr["HasConstantValues"]);
                        DataTable dt;

                        if (hasConstantValues)
                        {
                            dt = d.GetControlConstantValues(dr["ControlList_ID"].ToString());

                            FormGeneratorTools.BindObject(ddl, dt, "Text", "Value", dr["type"].ToString());
                        }
                        else
                        {
                            dt = d.GetControlOptionsByPageControl_ID(dr["PageControl_ID"].ToString());

                            string defaultValue = "";

                            if (dr["DefaultValue"] != null)
                                defaultValue = dr["DefaultValue"].ToString();

                            FormGeneratorTools.BindObject(ddl, dt, "Text", "Value", defaultValue);
                        }


                        ddl.ID = dr["AprimoFieldName"].ToString();
                        ddl.CssClass = dr["CssClass"].ToString();

                        ddl.Attributes.Add("validate", dr["DoValidate"].ToString());
                        ddl.Attributes.Add("controlName", dr["Type"].ToString().ToLower());

                        if (isRequired == "True")
                            ddl.Items[0].Text = ddl.Items[0].Text + " *";


                        ph.Controls.Add(ddl);

                        ph.Controls.Add(new LiteralControl("<br />"));
                    }
                    else if (type == "Multi-line")
                    {

                        CrossBrowser_TextArea c = (CrossBrowser_TextArea)LoadControl("Controls/CrossBrowser_TextArea.ascx");
                        c.Watermark = dr["Watermark"].ToString();
                        c.tbxCrossBrowser.ID = dr["AprimoFieldName"].ToString();
                        c.tbxCrossBrowser.CssClass = dr["CssClass"].ToString();
                        c.tbxCrossBrowser.Attributes.Add("validate", dr["DoValidate"].ToString());
                        c.tbxCrossBrowser.Attributes.Add("controlname", dr["Type"].ToString().ToLower());

                        if (isRequired == "True")
                            c.Watermark = dr["Watermark"].ToString() + " *";
                        else
                            c.Watermark = dr["Watermark"].ToString();

                        ph.Controls.Add(c);

                        //ph.Controls.Add(new LiteralControl("<br />"));
                    }
                    else if (type == "Literal")
                    {
                        Literal lit = new Literal();
                        lit.ID = dr["AprimoFieldName"].ToString();

                        SetControlProperties(lit, dtProperties);

                        ph.Controls.Add(lit);

                        ph.Controls.Add(new LiteralControl("<br />"));
                    }

                    else if (type == "Submit")
                    {
                        string errorMessage = "Please fix errors.";
                        string successMessage = "Submitted successfully!";

                        CrossBrowser_Button c = (CrossBrowser_Button)LoadControl("Controls/CrossBrowser_Button.ascx");

                        c.ID = "cbbSubmit";
                        c.ehButtonClicked += new EventHandler(submit_Click);
                        c.txtCrossBrowserSubmit.CssClass = dr["CssClass"].ToString();
                        c.txtCrossBrowserSubmit.Text = d.GetControlProperty(dr["PageControl_ID"].ToString(), "Text");
                        c.txtCrossBrowserSubmit.OnClientClick = "return IsValid()";
                        c.txtCrossBrowserSubmit.ID = dr["AprimoFieldName"].ToString();
                        c.txtCrossBrowserSubmit.Attributes.Add("pageControlID", dr["PageControl_ID"].ToString());
                        c.ErrorMessage = errorMessage;
                        c.SuccessMessage = successMessage;
                        //c.lblErrorMessage.Text = errorMessage;
                        c.lblSuccessMessage.Text = successMessage;

                        ph.Controls.Add(c);
                        ph.Controls.Add(new LiteralControl("<br />"));
                    }

                }

                HtmlGenericControl dbFormHidden = new HtmlGenericControl("div");
                dbFormHidden.ID = "db-form-hidden";
                Page.Controls.Add(dbFormHidden);

                HtmlGenericControl tracking = new HtmlGenericControl("div");
                tracking.ID = "trackingFields";
                Page.Controls.Add(tracking);

                HiddenField hf = new HiddenField();
                hf.ID = "hfDBResults";
                hf.Visible = true;

                HtmlGenericControl wrapper = new HtmlGenericControl("div");
                wrapper.Controls.Add(hf);
                Page.Form.Controls.Add(wrapper);

                Page.Controls.Add(new LiteralControl(@"

                <script type='text/javascript'>
                  $(document).ready(function(){

                        initAutocomplete();
                        getResultsIP();
                        renderHiddenTrackingFields();

                        if($('.ui-autocomplete-input').attr('name') == '')
                            $('.ui-autocomplete-input').hide();
                  })

                </script>

            "));
            }
        }

        protected void submit_Click(Object sender, EventArgs e)
        {
            Response.Write("yeah!");
            //****************************
            FormGeneratorData data = new FormGeneratorData();
            string pageControlID = ((CrossBrowser_Button)sender).txtCrossBrowserSubmit.Attributes["pageControlID"];
            DataTable dtActions = data.GetControlActionsByPageControl_ID(pageControlID);

            foreach (DataRow action in dtActions.Rows)
            {
                string actionName = action["Name"].ToString();
                string controlAction_ID = action["ControlAction_ID"].ToString();

                if (actionName == "Send Data To Aprimo (test)")
                {
                    SendDataToAprimo(true);
                }
                else if (actionName == "Send Data To Aprimo (live)")
                {
                    SendDataToAprimo(false);
                }
            }

            PlaceHolder ph = (PlaceHolder)Page.Master.FindControl("phColumn1");
            CrossBrowser_Button cbb = (CrossBrowser_Button)ph.FindControl("cbbSubmit");

            cbb.lblSuccessMessage.Visible = true;

            ClearFields();
        }

        private void SetControlPropertyByName(object control, string propertyName, string value)
        {
            var controlType = control.GetType();
            controlType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(control, value, null);

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

        private DataTable DelimStringToDataTable(string delim)
        {
            DataTable dtOut = new DataTable();
            dtOut.Columns.Add("Field_ID");
            dtOut.Columns.Add("Field_Value");

            foreach (string delimData in delim.Split('|'))
            {
                DataRow dr = dtOut.NewRow();

                String[] pair = delimData.Split('^');

                dr["Field_ID"] = pair[0];
                dr["Field_Value"] = pair[1];

                dtOut.Rows.Add(dr);
            }
            return dtOut;
        }

        private DataTable MergeDataTables(DataTable dt1, DataTable dt2)
        {
            DataTable dtOut = new DataTable();
            dtOut.Columns.Add("Field_ID");
            dtOut.Columns.Add("Field_Value");

            foreach (DataRow dr1 in dt1.Rows)
            {
                DataRow dr = dtOut.NewRow();

                dr["Field_ID"] = dr1[0];
                dr["Field_Value"] = dr1[1];

                dtOut.Rows.Add(dr);
            }

            foreach (DataRow dr2 in dt2.Rows)
            {
                DataRow dr = dtOut.NewRow();

                dr["Field_ID"] = dr2[0];
                dr["Field_Value"] = dr2[1];

                dtOut.Rows.Add(dr);
            }

            return dtOut;
        }

        private string CreateQueryString(DataTable dtParameters)
        {
            string ret = "?";

            foreach (DataRow dr in dtParameters.Rows)
            {
                ret += dr["Name"] + "=" + dr["Value"] + "&";
            }

            ret = ret.TrimEnd('&');

            return ret;
        }

        private void ClearFields()
        {
            PlaceHolder ph = (PlaceHolder)this.Master.FindControl("phColumn1");

            foreach (Control c in ph.Controls)
            {
                foreach (Control field in c.Controls)
                {
                    if (field is TextBox)
                    {
                        TextBox tb = (TextBox)field;
                        tb.Text = "";
                    }

                    if (field is DropDownList)
                    {
                        DropDownList ddl = (DropDownList)field;
                        ddl.SelectedIndex = 0;
                    }
                }                
            }
        }

        private void SendDataToAprimo(bool isTest)
        {
            DataTable dtDBResults = new DataTable();
            DataTable fieldData = FormGeneratorTools.GenerateFieldList(Form.Controls);

            HiddenField hfDBResults = (HiddenField)this.Form.FindControl("hfDBResults");

            HiddenField hfCampaign = (HiddenField)this.Form.FindControl("c");
            HiddenField hfSource = (HiddenField)this.Form.FindControl("s");
            HiddenField hfPage = (HiddenField)this.Form.FindControl("_p");

            if (hfDBResults.Value != "")
                dtDBResults = DelimStringToDataTable(hfDBResults.Value);

            DataTable dtMerged = MergeDataTables(fieldData, dtDBResults);


            FormGeneratorEmail email = new FormGeneratorEmail();

            string aprimoID = this.Form.Attributes["AprimoID"].ToString();
            string subject = this.Form.Attributes["AprimoSubject"].ToString();

            if (isTest)
                email.Send(dtMerged, "Field_ID", "Field_Value", subject, aprimoID, true);   
            else
                email.Send(dtMerged, "Field_ID", "Field_Value", subject, aprimoID, false);  

        }
    }
}