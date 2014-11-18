using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace FormGeneratorAdmin
{
    public partial class ajax : System.Web.UI.Page
    {
        [WebMethod]
        public static string LogIn(string username, string password)
        {
            FormGeneratorData data = new FormGeneratorData();

            string encryptedUsername = Crypto.Encrypt(username);
            string encryptedPassword = Crypto.Encrypt(password);

            DataTable dtReturn = data.Login(encryptedUsername, encryptedPassword);

            return GetJson(dtReturn);
        }

        [WebMethod]
        public static void RemoveElement(string formControlID)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.RemoveFormControl(formControlID);
        }

        [WebMethod]
        public static void SetElementValidation(string formControlID, string isChecked)
        {
            FormGeneratorData data = new FormGeneratorData();

            bool ischecked = Convert.ToBoolean(isChecked);

            data.UpdateRequiredByFormControl_ID(formControlID, ischecked);
        }
        
        [WebMethod]
        public static void SaveMultipleActionParamsByID(string FormControl_ID, string ControlActionType_ID, string DelimData)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.SaveMultipleActionParamsByID(FormControl_ID, ControlActionType_ID, DelimData);
        }
        
        [WebMethod]
        public static void SetTabOrder(string formControlID, string tabOrder)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.SetTabOrder(formControlID, tabOrder);
        }
        
        [WebMethod]
        public static void SaveSubmitText(string formControl_ID, string text)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.SaveSubmitText(formControl_ID, text);
        }

        [WebMethod]
        public static string AddFormControlGroupItem(string formControl_ID, string text, string value, string formControlGroup_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson(data.AddFormControlGroupItem(formControl_ID, text, value, formControlGroup_ID));
        }

        [WebMethod]
        public static void SaveCustomFieldInfo(string formControl_ID, string customLabel, string customControlType, string aprimoColumn, string isSpecial, string customControlFunction_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.SaveCustomFieldInfo(formControl_ID, customLabel, customControlType, aprimoColumn, isSpecial, customControlFunction_ID);
        }

        [WebMethod]
        public static string GetCustomFieldInfo(string formControl_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            DataSet ds = data.GetCustomFieldInfo(formControl_ID);

            ds.Tables[0].TableName = "formElements";
            ds.Tables[1].TableName = "formControlOptions";

            return JsonConvert.SerializeObject(ds, Formatting.Indented);
        }

        [WebMethod]
        public static string SetElementAction(string formControlID, string controlActionTypeID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson(data.AddControlAction(formControlID, controlActionTypeID));
        }

        [WebMethod]
        public static void RemoveFormControlGroupItem(string formControlGroup_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.RemoveFormControlGroupItem(formControlGroup_ID);
        }

        [WebMethod]
        public static string AddElementOption(string formControl_ID, string text, string value)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson(data.AddControlOption(formControl_ID, text, value));
        }

        [WebMethod]
        public static string SaveElementOption(string formControl_ID, string text, string value)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson(data.SaveControlOption(formControl_ID, text, value));
        }

        [WebMethod]
        public static string AddElementToContainer(string controlList_ID, string formID, string placeholderName, string formControl_ID, string text)
        {
            FormGeneratorData data = new FormGeneratorData();

            return data.AddControlToPlaceHolder(controlList_ID, formID, placeholderName, formControl_ID, text);
        }

        [WebMethod]
        public static string AddControlToPagePlaceHolder(string controlList_ID, string formID, string placeholderName, string formControl_ID, string text, string pageName)
        {
            FormGeneratorData data = new FormGeneratorData();

            return data.AddControlToPagePlaceHolder(controlList_ID, formID, placeholderName, formControl_ID, text, pageName);
        }

        [WebMethod]
        public static string GetElementActions(string formControl_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson( data.GetAllControlActionDataByFormControl_ID(formControl_ID));
        }

        [WebMethod]
        public static string GetAllFormControlGroupItemsByFormControl_ID(string formControl_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson(data.GetAllFormControlGroupItemsByFormControl_ID(formControl_ID));
        }

        [WebMethod]
        public static string GetAprimoInfo(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson( data.GetAprimoInfoByForm_ID(formID));
        }
        
        [WebMethod]
        public static string GetAllFormDataByFormID(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();

            DataSet ds = data.GetAllFormDataByFormID(formID);

            ds.Tables[0].TableName = "formData";
            ds.Tables[1].TableName = "formContainers";
            ds.Tables[2].TableName = "formElements";
            ds.Tables[3].TableName = "formChildElements";
            ds.Tables[4].TableName = "elementActions";
            ds.Tables[5].TableName = "formActions";
            ds.Tables[6].TableName = "elementBehaviors";

            return JsonConvert.SerializeObject(ds, Formatting.Indented);
        }

        [WebMethod]
        public static string GetAllFormDataByFormID_Pagination(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();

            DataSet ds = data.GetAllFormDataByFormID_Pagination(formID);

            ds.Tables[0].TableName = "formData";
            ds.Tables[1].TableName = "formPages";
            ds.Tables[2].TableName = "formContainers";
            ds.Tables[3].TableName = "formElements";
            ds.Tables[4].TableName = "formChildElements";
            ds.Tables[5].TableName = "elementActions";
            ds.Tables[6].TableName = "formActions";
            ds.Tables[7].TableName = "elementBehaviors";
            ds.Tables[8].TableName = "CustomControlOptions";
            ds.Tables[9].TableName = "LookupData";

            return JsonConvert.SerializeObject(ds, Formatting.Indented);
        }

        [WebMethod]
        public static string GetFormBySitecoreItemID(string itemID)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.GetFormBySitecoreItemID(itemID);

            return GetJson(data.GetFormBySitecoreItemID(itemID));
        }

        [WebMethod]
        public static string AddForm(string formID, string formName, string sitecoreID, string trackingCampaign, string trackingSource, string trackingForm, string header, string templateID, string styleID, string aprimoID, string aprimoSubject, string confirmationURL)
        {
            FormGeneratorData data = new FormGeneratorData();

            DataTable dtResult = data.AddForm(formID, formName, sitecoreID, trackingCampaign, trackingSource, trackingForm, header, templateID, styleID, aprimoID, aprimoSubject, confirmationURL);

            return GetJson(dtResult);
        }

        [WebMethod]
        public static string UpdateFormInfo(string formID, string styleTypeID, string tracking_campaign, string tracking_form, string tracking_source)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson( data.UpdateFormInfo(formID, styleTypeID, tracking_campaign, tracking_form, tracking_source));
        }

        [WebMethod]
        public static string GetFormByFormID(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson( data.GetFormByFormID(formID));
        }

        [WebMethod]
        public static void UpdateAprimoInfo(string formID, string subject, string aprimoID)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.UpdateAprimoInfo(formID, subject, aprimoID);
        }

        [WebMethod]
        public static string GetLayoutByFormID(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();
            return GetJson( data.GetTemplateByFormID(formID));
        }

        [WebMethod]
        public static void RemoveElementAction(string controlAction_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.RemoveControlAction(controlAction_ID);
        }

        [WebMethod]
        public static void RemoveElementOption(string controlOption_ID)
        {
            FormGeneratorData data = new FormGeneratorData();
            data.RemoveElementOption(controlOption_ID);
        }

        [WebMethod]
        public static void SetFormElementOrder(string delimitedIDs, string delimiter)
        {
            FormGeneratorData data = new FormGeneratorData();
            data.SetFormControlOrder(delimitedIDs, delimiter);
        }

        [WebMethod]
        public static void RemoveFormByForm_ID(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();
            data.RemoveFormByForm_ID(formID);
        }

        [WebMethod]
        public static void SetElementOptionOrder(string delimitedIds, string delimiter)
        {
            FormGeneratorData data = new FormGeneratorData();
            data.SetControlOptionOrder(delimitedIds, delimiter);
        }

        [WebMethod]
        public static string GetElementActionTypes()
        {
            FormGeneratorData data = new FormGeneratorData();
            return GetJson( data.GetControlActionTypes());
         }

        [WebMethod]
        public static int SaveCustomGroupInfo(string formControl_ID, string customLabel, string aprimoColumn)
        {
            FormGeneratorData data = new FormGeneratorData();
            return data.SaveCustomGroupInfo(formControl_ID, customLabel, aprimoColumn);
        }
        
        [WebMethod]
        public static string GetCustomGroupInfoByFormControl_ID(string formControl_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson( data.GetCustomGroupInfoByFormControl_ID(formControl_ID));
        }

        [WebMethod]
        public static string GetControlActionParametersByControlAction_ID(string ControlAction_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            DataSet ds = data.GetControlActionParametersByControlAction_ID(ControlAction_ID);

            ds.Tables[0].TableName = "ActionParams";

            return JsonConvert.SerializeObject(ds, Formatting.Indented);
        }

        [WebMethod]
        public static int SaveFormElementSetting(string formControl_ID, string controlProperty_ID, string settingValue)
        {
            FormGeneratorData data = new FormGeneratorData();
            return data.SaveFormControlSetting(formControl_ID, controlProperty_ID, settingValue);
        }

        [WebMethod]
        public static string GetReturnURLByForm_ID(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();
            return data.GetReturnURLByForm_ID(formID);
        }

        [WebMethod]
        public static void UpdateReturnURLByForm_ID(string formID, string returnURL)
        {
            FormGeneratorData data = new FormGeneratorData();
            data.UpdateReturnURLByForm_ID(formID, returnURL);
        }

        [WebMethod]
        public static void SaveFormElementDefaultOption(string defaultOption, string FormControl_ID)
        {
            FormGeneratorData data = new FormGeneratorData();
            data.SaveFormControlDefaultOption(defaultOption, FormControl_ID);
        }

        [WebMethod]
        public static void UpdateElementPropertySetting(string formControl_ID, string propertyName, string value)
        {
            FormGeneratorData data = new FormGeneratorData();
            data.UpdateControlPropertySetting(formControl_ID, propertyName, value);
        }

        [WebMethod]
        public static void GetAvalableElementsByForm_ID(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();
            data.GetAvalableControlsByForm_ID(formID);
        }

        [WebMethod]
        public static string GetElementInfoByFormElement_ID(string formControl_ID)
        {
            FormGeneratorData data = new FormGeneratorData();
            return GetJson( data.GetControlInfoByFormControl_ID(formControl_ID));
        }

        [WebMethod]
        public static string GetFormElementsByContainerID(string containerID, string formID)
        {
            FormGeneratorData data = new FormGeneratorData();
            return GetJson( data.GetFormControlsByPlaceholderID(containerID, formID));
        }

        [WebMethod]
        public static string GetElementOptionsByFormElement_ID(string formcontrolID)
        {
            FormGeneratorData data = new FormGeneratorData();
            return GetJson( data.GetControlOptionsByFormControl_ID(formcontrolID));
        }

        [WebMethod]
        public static string GetFormElementPropertyValuesByFormElement_ID(string formControl_ID)
        {
            FormGeneratorData data = new FormGeneratorData();
            return GetJson( data.GetFormControlPropertyValuesByFormControl_ID(formControl_ID));
        }


        #region helpers
        private static string GetJson(DataTable dt)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();

                foreach (DataColumn col in dt.Columns)
                    row.Add(col.ColumnName.Trim(), dr[col]);

                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
        #endregion

    }
}