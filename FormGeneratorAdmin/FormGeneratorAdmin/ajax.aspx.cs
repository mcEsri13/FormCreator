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
        public static void SetElementAction(string formControlID, string controlActionTypeID)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.AddControlAction(formControlID, controlActionTypeID);
        }

        [WebMethod]
        public static void SetSelectOption(string formControlID, string text, string value)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.AddControlOption(formControlID, text, value);
        }

        [WebMethod]
        public static void SetFieldToContainer(string controlList_ID, string formID, string placeholderID, string formControl_ID, string text)
        {
            FormGeneratorData data = new FormGeneratorData();

            data.AddControlToPlaceHolder(controlList_ID, formID, placeholderID, formControl_ID, text);
        }

        [WebMethod]
        public static DataTable GetElementActions(string formControl_ID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return data.GetAllControlActionDataByFormControl_ID(formControl_ID);
        }

        [WebMethod]
        public static DataTable GetAprimoInfo(string formID)
        {
            FormGeneratorData data = new FormGeneratorData();

            return data.GetAprimoInfoByForm_ID(formID);
        }


        [WebMethod]
        public static string AddForm(string formName, string sitecoreID)
        {
            FormGeneratorData data = new FormGeneratorData();

            //DataTable dtResult = data.AddForm(formName, sitecoreID);

            //string json = GetJson(dtResult);

            //return json;

            return "[{\"Form_ID\":\"11111\",\"Name\":\"testName\", \"ItemID\":\"{245234-2452345-23534-235435}\",\"ModificationDate\":\"1/1/2014\",\"Tracking_Campaign\":\"1111111\",\"Tracking_Source\":\"2222222\",\"Tracking_Form\":\"3333333\"}]";
        }

        [WebMethod]
        public static void AJAX_SaveControlOrder(string delimPageControl_IDs)
        {

        }


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
    }
}