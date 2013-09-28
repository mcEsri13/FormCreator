using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;

namespace FormGeneratorAdmin
{
    public class FormGeneratorData
    {
        #region Variables
        private SqlConnection con;
        #endregion

        #region Constructors
        public FormGeneratorData()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["FormGenerator"].ToString());
        }
        #endregion

        #region Helpers

        private DataSet SQL_SP_Exec(string spName, SqlConnection con, String[] paramNames, Object[] paramValues)
        {
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 14400;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            if (paramNames != null && paramValues != null)
                for (int i = 0; i < paramNames.Length; i++)
                    cmd.Parameters.Add(new SqlParameter(paramNames[i], paramValues[i]));

            con.Open();
            da.Fill(ds);
            con.Close();

            return ds;
        }

        #endregion

        #region Data Calls


        public DataTable GetFormControlsByFormID(string FormID)
        {
            String[] paramNames = { "Form_ID" };
            Object[] paramValues = { FormID };

            DataSet ds = SQL_SP_Exec("[spr_GetFormControlsByFormID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable Login(string username, string password)
        {
            String[] paramNames = { "Username", "Password" };
            Object[] paramValues = { username, password };

            DataSet ds = SQL_SP_Exec("[spr_Login]", con, paramNames, paramValues);

            return ds.Tables[0];
        }

        public void LogAction(string admin_ID, string message)
        {
            String[] paramNames = { "Admin_ID", "Message" };
            Object[] paramValues = { admin_ID, message };

            DataSet ds = SQL_SP_Exec("[spr_LogAction]", con, paramNames, paramValues);
        }

        public DataTable AddForm(string name, string itemID, string trackingCampaign, string trackingSource, string trackingForm)
        {
            String[] paramNames = { "Name", "ItemID", "TrackingCampaign", "TrackingSource", "TrackingForm" };
            Object[] paramValues = { name, itemID, trackingCampaign, trackingSource, trackingForm };

            DataSet ds = SQL_SP_Exec("[spr_AddForm]", con, paramNames, paramValues);

            return ds.Tables[0];
        }

        public DataTable GetFormByFormID(string FormID)
        {
            String[] paramNames = { "Form_ID" };
            Object[] paramValues = { FormID };

            DataSet ds = SQL_SP_Exec("[spr_GetFormByFormID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetAprimoInfoByForm_ID(string FormID)
        {
            String[] paramNames = { "Form_ID" };
            Object[] paramValues = { FormID };

            DataSet ds = SQL_SP_Exec("[spr_GetAprimoInfoByForm_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetFormBySitecoreItemID(string itemID)
        {
            String[] paramNames = { "ItemID" };
            Object[] paramValues = { itemID };

            DataSet ds = SQL_SP_Exec("[spr_GetFormBySitecoreItemID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetTemplateByFormID(string FormID)
        {
            String[] paramNames = { "Form_ID" };
            Object[] paramValues = { FormID };

            DataSet ds = SQL_SP_Exec("[spr_GetTemplateByFormID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetControls()
        {
            DataSet ds = SQL_SP_Exec("[spr_GetControls]", con, null, null);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetAvalableControlsByForm_ID(string FormID)
        {
            String[] paramNames = { "Form_ID" };
            Object[] paramValues = { FormID };

            DataSet ds = SQL_SP_Exec("[spr_GetAvalableControlsByForm_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetControlActionTypes()
        {
            DataSet ds = SQL_SP_Exec("[spr_GetControlActionTypes]", con, null, null);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetTemplates()
        {
            DataSet ds = SQL_SP_Exec("[spr_GetTemplates]", con, null, null);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public int AddControlToPlaceHolder(string controlList_ID, string FormID, string Formholder_ID, string FormControl_ID, string displayText)
        {
            String[] paramNames = { "ControlList_ID", "FormID", "Formholder_ID", "FormControl_ID", "DisplayText" };
            Object[] paramValues = { controlList_ID, FormID, Formholder_ID, FormControl_ID, displayText };

            DataSet ds = SQL_SP_Exec("[spr_AddControlToPlaceHolder]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            else
                return 0;
        }

        public int SaveFormControlSetting(object FormControl_ID, string ControlProperty_ID, string settingValue)
        {
            String[] paramNames = { "FormControl_ID", "ControlProperty_ID", "SettingValue"};
            Object[] paramValues = { FormControl_ID, ControlProperty_ID, settingValue };

            DataSet ds = SQL_SP_Exec("[spr_SaveFormControlSetting]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            else
                return 0;
        }

        public string GetControlProperty(object FormControl_ID, string PropertyName)
        {
            String[] paramNames = { "FormControl_ID", "PropertyName" };
            Object[] paramValues = { FormControl_ID, PropertyName };

            DataSet ds = SQL_SP_Exec("[spr_GetControlProperty]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0]["SettingValue"].ToString();
                else
                    return "";
            }
            else
                return "";
        }

        public DataTable GetPlaceholdersByTemplateID(string template_ID)
        {
            String[] paramNames = { "Template_ID" };
            Object[] paramValues = { template_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetPlaceholdersByTemplateID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetControlConstantValues(string controlList_ID)
        {
            String[] paramNames = { "ControlList_ID" };
            Object[] paramValues = { controlList_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetControlConstantValues]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public bool GetECASValueByForm_ID(string Form_ID)
        {
            String[] paramNames = { "Form_ID" };
            Object[] paramValues = { Form_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetECASValueByForm_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToBoolean(ds.Tables[0].Rows[0]["ECASOnSubmit"]);
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool DoesFormhaveECASControlAction(string Form_ID)
        {
            String[] paramNames = { "Form_ID" };
            Object[] paramValues = { Form_ID };

            DataSet ds = SQL_SP_Exec("[spr_DoesFormhaveECASControlAction]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToBoolean(ds.Tables[0].Rows[0][0]);
                }
                else
                    return false;
            }
            else
                return false;
        }

        public DataTable GetForms()
        {
            DataSet ds = SQL_SP_Exec("[spr_GetForms]", con, null, null);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetStyles()
        {
            DataSet ds = SQL_SP_Exec("[spr_GetStyles]", con, null, null);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetFormControlsByPlaceholderID(string placeholder_ID, string Form_ID)
        {
            String[] paramNames = { "Placeholder_ID", "Form_ID" };
            Object[] paramValues = { placeholder_ID, Form_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetFormControlsByPlaceholderID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public void RemoveFormControl(string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID" };
            Object[] paramValues = { FormControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_RemoveFormControl]", con, paramNames, paramValues);
        }

        public void RemoveFormByForm_ID(string Form_ID)
        {
            String[] paramNames = { "Form_ID" };
            Object[] paramValues = { Form_ID };

            DataSet ds = SQL_SP_Exec("[spr_RemoveFormByForm_ID]", con, paramNames, paramValues);
        }

        public DataTable GetAllControlActionDataByFormControl_ID(string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID" };
            Object[] paramValues = { FormControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetAllControlActionDataByFormControl_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetControlActionParametersByControlAction_ID(string controlAction_ID)
        {
            String[] paramNames = { "ControlAction_ID" };
            Object[] paramValues = { controlAction_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetControlActionParametersByControlAction_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetControlActionsByFormControl_ID(string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID" };
            Object[] paramValues = { FormControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetControlActionsByFormControl_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public string GetForm_IDByFormControl_ID(string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID" };
            Object[] paramValues = { FormControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetForm_IDByFormControl_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0] != null)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                    return "";
            }
            else
                return "";
        }

        public void GetFormControlByFormControl_ID(string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID" };
            Object[] paramValues = { FormControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetFormControlByFormControl_ID]", con, paramNames, paramValues);
        }

        public void AddControlAction(string FormControl_ID, string controlActionType_ID)
        {
            String[] paramNames = { "FormControl_ID", "ControlActionType_ID" };
            Object[] paramValues = { FormControl_ID, controlActionType_ID };

            DataSet ds = SQL_SP_Exec("[spr_AddControlAction]", con, paramNames, paramValues);
        }

        public void UpdateRequiredByFormControl_ID(string FormControl_ID, bool isRequired)
        {
            String[] paramNames = { "FormControl_ID", "IsRequired" };
            Object[] paramValues = { FormControl_ID, isRequired };

            DataSet ds = SQL_SP_Exec("[spr_UpdateRequiredByFormControl_ID]", con, paramNames, paramValues);
        }

        public void UpdateECAS(string Form_ID, bool value)
        {
            String[] paramNames = { "Form_ID", "Value" };
            Object[] paramValues = { Form_ID, value };

            DataSet ds = SQL_SP_Exec("[spr_UpdateECAS]", con, paramNames, paramValues);
        }

        public void UpdateReturnURLByForm_ID(string Form_ID, string returnURL)
        {
            String[] paramNames = { "Form_ID", "ReturnURL" };
            Object[] paramValues = { Form_ID, returnURL };

            DataSet ds = SQL_SP_Exec("[spr_UpdateReturnURLByForm_ID]", con, paramNames, paramValues);
        }

        public string GetReturnURLByForm_ID(string Form_ID)
        {
            String[] paramNames = { "Form_ID"};
            Object[] paramValues = { Form_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetReturnURLByForm_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ECASReturnURL"] != null)
                {
                    return ds.Tables[0].Rows[0]["ECASReturnURL"].ToString();
                }
                else
                    return "";
            }
            else
                return "";
        }

        public void UpdateAprimoInfo(string Form_ID, string subject, string aprimoID)
        {
            String[] paramNames = { "Form_ID", "Subject", "ID" };
            Object[] paramValues = { Form_ID, subject, aprimoID };

            DataSet ds = SQL_SP_Exec("[spr_UpdateAprimoInfo]", con, paramNames, paramValues);
        }

        public void SaveFormControlDefaultOption(string defaultOption, string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID", "DefaultOption" };
            Object[] paramValues = { FormControl_ID, defaultOption };

            DataSet ds = SQL_SP_Exec("[spr_SaveFormControlDefaultOption]", con, paramNames, paramValues);
        }

        public void UpdateControlPropertySetting(string FormControl_ID, string propertyName, string value)
        {
            String[] paramNames = { "FormControl_ID", "PropertyName", "Value" };
            Object[] paramValues = { FormControl_ID, propertyName, value };

            DataSet ds = SQL_SP_Exec("[spr_UpdateControlPropertySetting]", con, paramNames, paramValues);
        }

        public void AddControlOption(string FormControl_ID, string text, string value)
        {
            String[] paramNames = { "FormControl_ID", "Text", "Value" };
            Object[] paramValues = { FormControl_ID, text, value };

            DataSet ds = SQL_SP_Exec("[spr_AddControlOption]", con, paramNames, paramValues);
        }

        public void UpdateControlOption(string controlOption_ID, string text, string value)
        {
            String[] paramNames = { "ControlOption_ID", "Text", "Value" };
            Object[] paramValues = { controlOption_ID, text, value };

            DataSet ds = SQL_SP_Exec("[spr_UpdateControlOption]", con, paramNames, paramValues);
        }

        public DataTable GetFormControlPropertyValuesByFormControl_ID(string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID" };
            Object[] paramValues = { FormControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetFormControlPropertyValuesByFormControl_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public string GetDefaultOptionByFormControl_ID(string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID" };
            Object[] paramValues = { FormControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetDefaultOptionByFormControl_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["DefaultValue"] != null)
                {
                    return ds.Tables[0].Rows[0]["DefaultValue"].ToString();
                }
                else
                    return "";
            }
            else
                return "";
        }

        public DataTable GetControlInfoByFormControl_ID(string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID" };
            Object[] paramValues = { FormControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetControlInfoByFormControl_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable RemoveControlAction(string controlAction_ID)
        {
            String[] paramNames = { "ControlAction_ID" };
            Object[] paramValues = { controlAction_ID };

            DataSet ds = SQL_SP_Exec("[spr_RemoveControlAction]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable RemoveControlOption(string controlOption_ID)
        {
            String[] paramNames = { "ControlOption_ID" };
            Object[] paramValues = { controlOption_ID };

            DataSet ds = SQL_SP_Exec("[spr_RemoveControlOption]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public void SetFormControlOrder(string delimitedIDs, string delimiter)
        {
            String[] paramNames = { "DelimitedFormControl_IDs", "Delimiter" };
            Object[] paramValues = { delimitedIDs, delimiter };

            DataSet ds = SQL_SP_Exec("[spr_SetFormControlOrder]", con, paramNames, paramValues);
        }

        public void SetControlOptionOrder(string delimitedIDs, string delimiter)
        {
            String[] paramNames = { "DelimitedFormControl_IDs", "Delimiter" };
            Object[] paramValues = { delimitedIDs, delimiter };

            DataSet ds = SQL_SP_Exec("[spr_SetControlOptionOrder]", con, paramNames, paramValues);
        }

        public DataTable GetControlOptionsByFormControl_ID(string FormControl_ID)
        {
            String[] paramNames = { "FormControl_ID" };
            Object[] paramValues = { FormControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetControlOptionsByFormControl_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetControlOptionsByControlOption_ID(string controlOption_ID)
        {
            String[] paramNames = { "ControlOption_ID" };
            Object[] paramValues = { controlOption_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetControlOptionsByControlOption_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable UpdateFormInfo(string FormID, string styleTypeID, string campaign, string Form, string source)
        {
            String[] paramNames = { "Form_ID", "StyleType_ID", "Tracking_Campaign", "Tracking_Form", "Tracking_Source" };
            Object[] paramValues = { FormID, styleTypeID, campaign, Form, source };

            DataSet ds = SQL_SP_Exec("[spr_UpdateFormInfo]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        #endregion
    }
}