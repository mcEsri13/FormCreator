using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Web.Script.Serialization;

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
        public static void AddElementAction(string formControlID, string controlActionTypeID)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.AddControlAction(formControlID, controlActionTypeID);
        }

        [WebMethod]
        public static void AddElementOption(string formControlID, string text, string value)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.AddControlOption(formControlID, text, value);
        }

        [WebMethod]
        public static void AddElementToContainer(string controlList_ID, string formID, string placeholderID, string formControl_ID, string text)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.AddControlToPlaceHolder(controlList_ID, formID, placeholderID, formControl_ID, text);
        }

        [WebMethod]
        public static string GetElementActions(string formControl_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson( data.GetAllControlActionDataByFormControl_ID(formControl_ID));
        }

        [WebMethod]
        public static string GetAprimoInfo(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson( data.GetAprimoInfoByForm_ID(formID));
        }


        [WebMethod]
        public static string AddForm(string formName, string sitecoreID)
        {
            FormGeneratorData data = new FormGeneratorData();

            //DataTable dtResult = data.AddForm(formName, sitecoreID);

            //string json = GetJson(dtResult);

            //return GetJson( json;

            return "[{\"Form_ID\":\"11111\",\"Name\":\"testName\", \"ItemID\":\"{245234-2452345-23534-235435}\",\"ModificationDate\":\"1/1/2014\",\"Tracking_Campaign\":\"1111111\",\"Tracking_Source\":\"2222222\",\"Tracking_Form\":\"3333333\"}]";
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
        public static string RemoveElementAction(string controlAction_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return GetJson( data.RemoveControlAction(controlAction_ID));
        }

        [WebMethod]
        public static void RemoveElementOption(string controlOption_ID)
        {
            FormGeneratorData data = new FormGeneratorData();
            data.RemoveControlOption(controlOption_ID);
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