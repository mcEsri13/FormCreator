using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Web.Services;


namespace FormGenerator
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string admin_ID = "";

                if (Session["ID"] != null)
                {
                    admin_ID = Session["ID"].ToString();

                    pnlLogin.Visible = false;
                    pnlAdmin.Visible = true;

                }
            }

            loadFrame();
        }

        protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPages.SelectedIndex != 0)
            {
                FormGeneratorData data = new FormGeneratorData();

                DataTable dtTemplate = data.GetTemplateByPageID(ddlPages.SelectedValue);
                DataTable dtPages = data.GetPageByPageID(ddlPages.SelectedValue);

                hidItemID.Value = dtPages.Rows[0]["ItemID"].ToString();

                LoadDropdown(ddlStyles, data.GetStyles(), "Name", "StyleType_ID", "Select Style");

                if (ddlPages.SelectedIndex > 0)
                {
                    loadFrame();
                    btnRemovePage.Visible = true;
                    btnSaveFormInfo.Visible = true;

                    ddlStyles.SelectedIndex = 0;
                    txtCampaign.Text = "";
                    txtPage.Text = "";
                    txtSource.Text = "";

                    if(dtPages.Rows[0]["StyleType_ID"] != DBNull.Value)
                        ddlStyles.SelectedValue = dtPages.Rows[0]["StyleType_ID"].ToString();

                    if (dtPages.Rows[0]["Tracking_Campaign"] != DBNull.Value)
                        txtCampaign.Text = dtPages.Rows[0]["Tracking_Campaign"].ToString();

                    if (dtPages.Rows[0]["Tracking_Page"] != DBNull.Value)
                        txtPage.Text = dtPages.Rows[0]["Tracking_Page"].ToString();

                    if (dtPages.Rows[0]["Tracking_Source"] != DBNull.Value)
                        txtSource.Text = dtPages.Rows[0]["Tracking_Source"].ToString();
                }
                else
                {
                    btnRemovePage.Visible = false;
                    btnSaveFormInfo.Visible = false;
                }

                ReloadControlList();

                LoadDropdown(ddlControlList, data.GetAvalableControlsByPage_ID(ddlPages.SelectedValue), "name", "controllist_id", "Select Field");


                pnlControlList.Visible = true;
                pnlEditActions.Visible = false;
                pnlAddSubmitButton.Visible = false;
                pnlAprimoInfo.Visible = true;
                pnlRightSide.Visible = true;
                pnlSetPageFields.Visible = true;
                pnlAddEditDropdown.Visible = false;
                btnAdd.Visible = true;

                pnlPageInfo.Visible = true;
            }
            else
            {
                pnlControlList.Visible = false;
                pnlEditActions.Visible = false;
                pnlAddSubmitButton.Visible = false;
                pnlAprimoInfo.Visible = false;
                pnlRightSide.Visible = false;
                pnlSetPageFields.Visible = false;
                btnRemovePage.Visible = false;
                pnlAddEditDropdown.Visible = false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            string controlText = "";

            if (pnlAddSubmitButton.Visible)
                controlText = txtButtonText.Text;
            else if(pnlAddEditContentBlock.Visible)
                controlText = txtRTE.Text;

            if (ddlControlList.SelectedIndex == 0)
                return;

            int newPageControl_ID = data.AddControlToPlaceHolder(ddlControlList.SelectedValue
                                        , ddlPages.SelectedValue
                                        , "1"
                                        , hfPageControlID_ForEdit.Value
                                        , controlText);

            LogMessage("Added Field '" + controlText + "'");

            if (ddlControlList.SelectedValue == "20")
                pnlAddEditDropdown.Visible = true;

            ClearFields();
            ReloadControlList();

            LoadDropdown(ddlControlList, data.GetAvalableControlsByPage_ID(ddlPages.SelectedValue), "name", "controllist_id", "Select Field");
            ddlControlList.SelectedIndex = 0;

            pnlAddSubmitButton.Visible = false;

            pnlControlList.Visible = true;
        }

        protected void rptrActions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            HiddenField hf = (HiddenField)e.Item.FindControl("hidPageControl_ID");
            string controlActionID = e.CommandArgument.ToString();
            FormGeneratorData data = new FormGeneratorData();

            data.RemoveControlAction(controlActionID);

            LogMessage("Removed Action from Submit Button.");

            DataTable dtActions = data.GetControlActionsByPageControl_ID(hf.Value);

            rptrActions.DataSource = dtActions;
            rptrActions.DataBind();

            if (ddlPages.SelectedIndex != 0)
            {
                if (data.DoesPagehaveECASControlAction(ddlPages.SelectedValue))
                    pnlReturnURL.Visible = true;
                else
                    pnlReturnURL.Visible = false;
            }
        }

        protected void rptrOptions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string controlOption_ID = e.CommandArgument.ToString();
            FormGeneratorData data = new FormGeneratorData();

            if (e.CommandName.ToString() == "delete")
            {
                data.RemoveControlOption(controlOption_ID);

                DataTable dtOptions = data.GetControlOptionsByPageControl_ID(hfPageControlID_DD_ForEdit.Value);

                rptrOptions.DataSource = dtOptions;
                rptrOptions.DataBind();
            }
            else if (e.CommandName.ToString() == "edit")
            {
                hfControlOptionID_ForEdit.Value = e.CommandArgument.ToString();

                DataTable dt = data.GetControlOptionsByControlOption_ID(controlOption_ID);

                txtText.Text = dt.Rows[0]["Text"].ToString();
                txtValue.Text = dt.Rows[0]["Value"].ToString();

                //load default

                btnSaveOption.Text = "Save";
            }

        }

        protected void chkRequired_CheckChanged(object source, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();
            CheckBox cb = (CheckBox)source;
            string pageControl_ID = cb.Attributes["PCID"].ToString();

            data.UpdateRequiredByPageControl_ID(pageControl_ID, cb.Checked); 
        }

        protected void rptrPageControls_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();
            HiddenField hf = (HiddenField)e.Item.FindControl("hidPageControlID");
            DataTable dtProperties = data.GetPageControlPropertyValuesByPageControl_ID(hf.Value);

            if (dtProperties.Rows.Count > 0)
            {
                if (dtProperties.Rows[0]["ControlList_Name"].ToString() == "Content Block")
                {
                    pnlEditActions.Visible = false;
                    pnlAddEditDropdown.Visible = false;
                    pnlAddEditTextBox.Visible = false;
                    pnlAddEditContentBlock.Visible = true;
                    pnlAddSubmitButton.Visible = false;
                    btnAdd.Visible = true;

                    txtRTE.Text = dtProperties.Rows[0]["SettingValue"].ToString();
                    btnRTECancel.Visible = true;
                    btnUpdateRTE.Visible = true;

                    
                }
                else if (dtProperties.Rows[0]["ControlList_Name"].ToString() == "Submit")
                {
                    pnlEditActions.Visible = false;
                    pnlAddEditDropdown.Visible = false;
                    pnlAddEditTextBox.Visible = false;
                    pnlAddEditContentBlock.Visible = false;
                    pnlAddSubmitButton.Visible = true;
                    btnAdd.Visible = true;

                    txtButtonText.Text = dtProperties.Rows[0]["SettingValue"].ToString();
                }

                hfPageControlSetting_ID.Value = dtProperties.Rows[0]["PageControlSetting_ID"].ToString();
                hfControlListID_ForEdit.Value = dtProperties.Rows[0]["ControlList_ID"].ToString();
                hfPageControlID_ForEdit.Value = dtProperties.Rows[0]["PageControl_ID"].ToString();
            }

            if (e.CommandName.ToString() == "delete")
            {
                data.RemovePageControl(e.CommandArgument.ToString());
                LoadDropdown(ddlControlList, data.GetAvalableControlsByPage_ID(ddlPages.SelectedValue), "name", "controllist_id", "Select Field");

                pnlControlList.Visible = true;
                btnAdd.Visible = true;
                pnlAddEditDropdown.Visible = false;
                pnlAddEditContentBlock.Visible = false;
                pnlAddEditTextBox.Visible = false;
                pnlAddSubmitButton.Visible = false;

                LogMessage("Deleted Field From Form.");
            }
            else // Edit
            {

                pnlControlList.Visible = false;

                DataTable dtControlInfo = data.GetControlInfoByPageControl_ID(hf.Value);
                string controlType = dtControlInfo.Rows[0]["ControlType"].ToString();



                if (controlType == "TextBox" || controlType == "Multi-line")
                {
                    pnlEditActions.Visible = false;
                    pnlAddEditDropdown.Visible = false;
                    pnlAddEditTextBox.Visible = true;
                    pnlAddEditContentBlock.Visible = false;
                    btnAdd.Visible = true;

                    hfControlListID_ForEdit.Value = dtControlInfo.Rows[0]["ControlList_ID"].ToString();
                    hfPageControlID_ForEdit.Value = hf.Value;
                }
                else if (controlType == "DropDownList")
                {
                    pnlEditActions.Visible = false;
                    pnlAddEditDropdown.Visible = true;
                    pnlAddEditTextBox.Visible = false;
                    pnlAddEditContentBlock.Visible = false;
                    btnAdd.Visible = false;
                    hfPageControlID_DD_ForEdit.Value = hf.Value;

                    DataTable dtOptions = data.GetControlOptionsByPageControl_ID(hf.Value);
                    string defaultValue = data.GetDefaultOptionByPageControl_ID(hf.Value);

                    txtDefaultOption.Text = defaultValue;
                    rptrOptions.DataSource = dtOptions;
                    rptrOptions.DataBind();
                }
                else if (controlType == "Submit")
                {
                    pnlEditActions.Visible = true;
                    pnlAddSubmitButton.Visible = true;
                    pnlControlList.Visible = false;

                    btnUpdateButtonText.Visible = true;

                    hfPageControlID_Action_ForEdit.Value = hf.Value;
                    hfPageControlID_ForEdit.Value = hf.Value;

                    DataTable dtActionTypes = data.GetControlActionTypes();
                    DataTable dtActions = data.GetControlActionsByPageControl_ID(hf.Value);
                    DataTable dtAprimoInfo = data.GetAprimoInfoByPage_ID(ddlPages.SelectedValue);

                    if (dtAprimoInfo != null)
                    {
                        txtAprimoSubject.Text = dtAprimoInfo.Rows[0]["Subject"].ToString();
                        txtAprimoID.Text = dtAprimoInfo.Rows[0]["Aprimo_ID"].ToString();
                    }

                    LoadDropdown(ddlActions, dtActionTypes, "Name", "ControlActionType_ID", "Select Action");

                    rptrActions.DataSource = dtActions;
                    rptrActions.DataBind();

                    if (ddlPages.SelectedIndex != 0)
                    {
                        if (data.DoesPagehaveECASControlAction(ddlPages.SelectedValue))
                        {
                            string url = "";

                            if (ddlPages.SelectedIndex != 0)
                                url = data.GetReturnURLByPage_ID(ddlPages.SelectedValue);

                            tbxReturnURL.Text = url;

                            pnlReturnURL.Visible = true;
                        }
                    }

                }

            }

            ReloadControlList();
        }

        private void ReloadControlList()
        {
            FormGeneratorData data = new FormGeneratorData();

            DataTable dt = data.GetPageControlsByPlaceholderID("1", ddlPages.SelectedValue);

            if (dt.Rows.Count > 0)
            {
                rptrPageControls.DataSource = dt;
                rptrPageControls.DataBind();

                lblNoData.Visible = false;
            }
            else
            {
                lblNoData.Visible = true;
            }
        }

        private void LoadDropdown(DropDownList ddl, DataTable data, string textColumn, string valueConlumn, string defaultText )
        {
            ddl.DataSource = data;
            ddl.DataTextField = textColumn;
            ddl.DataValueField = valueConlumn;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(defaultText, "-1"));        
        }

        private void ClearForm()
        {
            rptrPageControls.DataSource = null;
            rptrPageControls.DataBind();

            pnlControlList.Visible = false;     
        }

        [WebMethod]
        public static void AJAX_SaveControlOrder(string delimPageControl_IDs)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.SetPageControlOrder(delimPageControl_IDs, "|");

            if (HttpContext.Current.Session["ID"] != null)
                LogMessage("Saved Field Order.");
        }

        [WebMethod]
        public static void AJAX_SaveOptionOrder(string delimControlOption_IDs)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.SetControlOptionOrder(delimControlOption_IDs, "|");

            if (HttpContext.Current.Session["ID"] != null)
                LogMessage("Saved Field Option Order.");            
        }

        protected void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            string path = "";

            imgTemplatePreview.ImageUrl = path;
        }

        protected void btnAddPage_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            if (txtPageName.Text != "" && txtItemID.Text != "")
            {

                int newPageID = data.AddPage("1" //only using single column template for now
                                            , txtPageName.Text
                                            , txtItemID.Text);

                LoadDropdown(ddlPages, data.GetPages(), "Name", "Page_ID", "Select Form");

                if (newPageID != 0)
                {
                    pnlSetPageFields.Visible = true;
                    ddlPages.SelectedValue = newPageID.ToString();

                    LogMessage("Added Form.");
                }
                else
                    LogMessage("Add Form Fail.");

                //Load info box and display
                lblSCID.Text = txtItemID.Text;
                lblName.Text = txtPageName.Text;
                lblPath.Text = "Coming Soon!";
                pnlPageInfoDisplay.Visible = true;

                pnlSetPageFields.Visible = true;

                hidItemID.Value = txtItemID.Text;
                loadFrame();

                //-------------------
                ReloadControlList();

                LoadDropdown(ddlControlList, data.GetAvalableControlsByPage_ID(ddlPages.SelectedValue), "name", "controllist_id", "Select Field");

                pnlControlList.Visible = true;
                //btnNewControl.Visible = true;
                //-------------------

                pnlChoosePage.Visible = false;

                //Clear fields
                txtPageName.Text = "";
                txtItemID.Text = "";
                pnlCreatePage.Visible = false;
                pnlRightSide.Visible = true;
            }

        }

        protected void btnEditExisting_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            LoadDropdown(ddlPages, data.GetPages(), "Name", "Page_ID", "Select Form");

            pnlChoosePage.Visible = true;
            pnlPageInfoDisplay.Visible = false;
            lblNoData.Visible = false;
            pnlSetPageFields.Visible = true;
            pnlCreatePage.Visible = false;
            pnlControlList.Visible = false;
            pnlAddSubmitButton.Visible = false;
            //cbxECAS.Visible = false;

            phRightSide.Controls.Clear();

            rptrPageControls.DataSource = null;
            rptrPageControls.DataBind();
        }

        protected void btnCreatNew_Click(object sender, EventArgs e)
        {
            pnlSetPageFields.Visible = false;
            pnlCreatePage.Visible = true;
            pnlChoosePage.Visible = false;
            pnlAprimoInfo.Visible = false;
            pnlRightSide.Visible = false;

            phRightSide.Controls.Clear();

            rptrPageControls.DataSource = null;
            rptrPageControls.DataBind();
        }

        protected void ddlControlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            if (ddlControlList.SelectedValue != "-1")
            {
                btnAdd.Visible = true;
            }
            else
            {
                pnlAddEditDropdown.Visible = false;
                pnlAddEditTextBox.Visible = false;
                pnlAddEditContentBlock.Visible = false;
                btnAdd.Visible = false;
            }

            if (DoesListContainString(ddlControlList.SelectedValue,"1,4,8,9,10,11,19"))
            {
                pnlAddEditDropdown.Visible = false;
                pnlAddEditTextBox.Visible = true;
                pnlAddEditContentBlock.Visible = false;
            }
            else if (ddlControlList.SelectedValue == "12") // Content Block
            {
                pnlAddEditDropdown.Visible = false;
                pnlAddEditTextBox.Visible = false;
                pnlAddEditContentBlock.Visible = true;
            }
            else if (ddlControlList.SelectedValue == "13") //Submit
            {
                DataTable dtActionTypes = data.GetControlActionTypes();

                LoadDropdown(ddlActions, dtActionTypes, "Name", "ControlActionType_ID", "Select Action");

                btnAdd.Visible = true;
                pnlEditActions.Visible = false;
                pnlAddEditDropdown.Visible = false;
                pnlAddEditTextBox.Visible = false;
                pnlAddEditContentBlock.Visible = false;
            }
            else if (ddlControlList.SelectedValue == "20") //Custom Dropdown
            {
                pnlAddEditDropdown.Visible = false;
                pnlAddEditTextBox.Visible = false;
                pnlAddEditContentBlock.Visible = false;
                btnAdd.Visible = true;
            }
            else if (ddlControlList.SelectedValue == "14") //Image
            {
                pnlAddEditDropdown.Visible = false;
                pnlAddEditTextBox.Visible = false;
                pnlAddEditContentBlock.Visible = false;
            }
        }

        private bool DoesListContainString(string str, string delimValues)
        {
            foreach (string delimValue in delimValues.Split(','))
                if (str == delimValue)
                    return true;

            return false;
        }

        private void ClearFields()
        {
            txtRTE.Text = "";
            ddlControlList.SelectedIndex = 0;
            hfPageControlID_ForEdit.Value = "";

            pnlAddEditDropdown.Visible = false;
            pnlAddEditTextBox.Visible = false;
            pnlAddEditContentBlock.Visible = false;
            btnAdd.Visible = true;
        }

        protected void btnNewControl_Click(object sender, EventArgs e)
        {
            ddlControlList.Visible = true;
            btnAdd.Visible = true;
        }

        protected void btnAddAction_Click(object sender, EventArgs e)
        {
            if (ddlActions.SelectedIndex > 0)
            {
                string pageControlID = hfPageControlID_Action_ForEdit.Value;
                FormGeneratorData data = new FormGeneratorData();

                data.AddControlAction(pageControlID, ddlActions.SelectedValue);

                rptrActions.DataSource = data.GetControlActionsByPageControl_ID(pageControlID);
                rptrActions.DataBind();

                if (ddlPages.SelectedIndex != 0)
                {
                    if (data.DoesPagehaveECASControlAction(ddlPages.SelectedValue))
                        pnlReturnURL.Visible = true;
                }
            }
        }

        protected void btnCancelAction_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            if (txtAprimoID.Text != "" && txtAprimoSubject.Text != "" && ddlPages.SelectedIndex != 0)
                data.UpdateAprimoInfo(ddlPages.SelectedValue, txtAprimoSubject.Text, txtAprimoID.Text);

            if (ddlPages.SelectedIndex != 0)
                data.UpdateReturnURLByPage_ID(ddlPages.SelectedValue, tbxReturnURL.Text);

            LogMessage("Updated Return URL (ECAS Aciton).");

            LogMessage("Updated Aprimo Data.");

            pnlEditActions.Visible = false;
            pnlAddSubmitButton.Visible = false;
            pnlControlList.Visible = true;
            pnlReturnURL.Visible = false;
        }

        protected void btnCancelOptions_Click(object sender, EventArgs e)
        {
            pnlAddEditDropdown.Visible = false;
            //btnNewControl.Visible = true;
            pnlControlList.Visible = true;
            btnAdd.Visible = true;
        }

        protected void btnSaveOption_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            if (txtText.Text != "" && txtValue.Text != "")
            {
                if (hfControlOptionID_ForEdit.Value == "")
                {
                    data.AddControlOption(hfPageControlID_DD_ForEdit.Value, txtText.Text, txtValue.Text);
                    LogMessage("Added Dropdown Option.");
                }
                else
                {
                    data.UpdateControlOption(hfControlOptionID_ForEdit.Value, txtText.Text, txtValue.Text);
                    hfControlOptionID_ForEdit.Value = "";
                    LogMessage("Updated Dropdown Option.");
                }

                txtText.Text = "";
                txtValue.Text = "";
                btnSaveOption.Text = "Add";
            }

            if (txtDefaultOption.Text != "")
                data.SavePageControlDefaultOption(txtDefaultOption.Text, hfPageControlID_DD_ForEdit.Value);

            DataTable dtOptions = data.GetControlOptionsByPageControl_ID(hfPageControlID_DD_ForEdit.Value);

            rptrOptions.DataSource = dtOptions;
            rptrOptions.DataBind();

        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            string encryptedUsername = Crypto.Encrypt(txtUserName.Text);
            string encryptedPassword = Crypto.Encrypt(txtPassword.Text);

            string admin_ID = data.Login(encryptedUsername, encryptedPassword);

            if (admin_ID != "0")
            {
                pnlLogin.Visible = false;
                pnlAdmin.Visible = true;
                Session["ID"] = admin_ID.ToString();

                data.LogAction(admin_ID, "Logged In.");
            }
            else
            {
                pnlLogin.Visible = true;
                pnlAdmin.Visible = false;
                Session["ID"] = "";
                data.LogAction(admin_ID, "Login Fail.");
            }
             
        }

        protected void loadFrame()
        {
            if (hidItemID.Value != "")
            {
                phRightSide.Controls.Clear();
                phRightSide.Controls.Add(new LiteralControl("<iframe src='" + "FormGenerator.aspx?ID=" + hidItemID.Value + "' scrolling='no' class='formViewCSS' frameBorder='0'></iframe>"));
                phRightSide.Controls.Add(new LiteralControl(@"
                
                    <script type='text/javascript'>
                        $('.formViewCSS').load(function () {
                            var newFrameHeight = parseInt($(this).contents().find('html').height()) + 50;
                            var newFrameWidth = parseInt($(this).contents().find('html').width()) + 50;
                            $(this).attr('height', newFrameHeight + 'px');

                            isModal = $(this).contents().find('html').find('#hidIsModal').val();

                            if(isModal == 'true')
							{
                                parentID = $(this).closest('div').attr('id');
                                if(parentID != 'hiddenModalForm')
								     $(this).wrap('<div id=hiddenModalForm />');

                                buttonID = $('#btnShowForm').attr('id');
                                if(buttonID != 'btnShowForm')
								{
								    $('<input id=btnShowForm type=button value=Show&nbsp;Form  />').insertBefore('#hiddenModalForm');
									
									//----------- Dialog Box Stuff
									var d = $('#hiddenModalForm').dialog({
										autoOpen: false,
										height: newFrameHeight + 50,
										width: newFrameWidth,
										modal: true
									});
									d.parent().find('a').find('span').attr('class', 'ui-icon ui-icon-minus');
									d.parent().draggable({
										containment: 'form',
										opacity: 0.70
									});
									$('.ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-draggable.ui-resizable').css('zIndex', 200);

									$('form').append(d.parent());

									$('#btnShowForm').click(function () {
										$('#hiddenModalForm').dialog('open');
									}).next('.ui-widget-overlay')
												  .css('background', '#f00ba2');

									$('body').on('click', '.ui-widget-overlay', function () {
										$('.ui-dialog').filter(function () {
											return $(this).css('display') === 'block';
										}).find('.ui-dialog-content').dialog('close');
									});

									$('#closeDialog').click(function () {
										$('#hiddenModalForm').dialog('close');
									});
									//----------- End Dialog										
								}
									
								
							}
                        });
                    </script>
                
                "));
            }        
        }

        protected void btnRemovePage_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            if (ddlPages.SelectedIndex != 0)
            {
                data.RemovePageByPage_ID(ddlPages.SelectedValue);
                LoadDropdown(ddlPages, data.GetPages(), "Name", "Page_ID", "Select Form");

                pnlControlList.Visible = false;

                rptrPageControls.DataSource = null;
                rptrPageControls.DataBind();

                phRightSide.Controls.Clear();

                lblNoData.Visible = false;

                LogMessage("Removed Form.");
            }
        }

        protected void btnUpdateRTE_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.SavePageControlSetting(hfPageControlID_ForEdit.Value, "4", txtRTE.Text); //4 = Content Block

            pnlAddEditContentBlock.Visible = false;
            pnlControlList.Visible = true;

            hfPageControlID_ForEdit.Value = "";
            hfControlListID_ForEdit.Value = "";

            LogMessage("Updated Content Block");
        }

        protected void ddlControlList_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //if (ddlControlList.SelectedValue == "13") //Submit
            //{
            //    pnlAddSubmitButton.Visible = true;
            //    btnUpdateButtonText.Visible = false;
            //}
            //else 

            if (ddlControlList.SelectedValue == "12") //Content Block
            {
                pnlAddEditContentBlock.Visible = true;
                pnlAddSubmitButton.Visible = false;
                btnUpdateButtonText.Visible = false;
                btnUpdateRTE.Visible = false;
                btnRTECancel.Visible = false;
            }
        }

        protected void btnUpdateButtonText_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.UpdateControlPropertySetting(hfPageControlID_ForEdit.Value, "Text", txtButtonText.Text);

            pnlAddSubmitButton.Visible = false;
            pnlEditActions.Visible = false;
            txtButtonText.Text = "";
            pnlControlList.Visible = true;
            hfPageControlID_ForEdit.Value = "";
            hfControlListID_ForEdit.Value = "";
            hfPageControlSetting_ID.Value = "";

            LogMessage("Updated Button Text");
        }

        protected void btnRTECancel_Click(object sender, EventArgs e)
        {
            pnlAddEditContentBlock.Visible = false;
            pnlControlList.Visible = true;
        }

        //protected void btnUpdateAprimoInfo_Click(object sender, EventArgs e)
        //{
        //    FormGeneratorData data = new FormGeneratorData();

        //    if (txtAprimoID.Text != "" && txtAprimoSubject.Text != "" && ddlPages.SelectedIndex != 0)
        //        data.UpdateAprimoInfo(ddlPages.SelectedValue, txtAprimoSubject.Text, txtAprimoID.Text);

        //    LogMessage("Updated Aprimo Data.");
        //}

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();

            if(ddlPages.SelectedIndex != 0)
                data.UpdateReturnURLByPage_ID(ddlPages.SelectedValue, tbxReturnURL.Text);

            LogMessage("Updated Return URL (ECAS Aciton).");
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["ID"] = null;

            LogMessage("Logged Out.");

            Response.Redirect("Admin.aspx");
        }

        private static void LogMessage(string Message)
        {
            FormGeneratorData data = new FormGeneratorData();

            if (HttpContext.Current.Session["ID"] != null)
            {
                string admin_ID = HttpContext.Current.Session["ID"].ToString();
                data.LogAction(admin_ID, Message);
            }     

        }

        protected void btnSaveFormInfo_Click(object sender, EventArgs e)
        {
            FormGeneratorData data = new FormGeneratorData();


            if (ddlStyles.SelectedIndex != 0 && txtCampaign.Text != "" && txtPage.Text != "" && txtSource.Text != "")
            {
                data.UpdatePageInfo(ddlPages.SelectedValue, ddlStyles.SelectedValue, txtCampaign.Text, txtPage.Text, txtSource.Text);

                LogMessage("Updated page info.");
            }
            
        }
        
    }
}