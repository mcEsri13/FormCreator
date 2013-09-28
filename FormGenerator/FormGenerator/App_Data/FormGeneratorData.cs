using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FormGenerator
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


        public DataTable GetPageControlsByPageID(string pageID)
        {
            String[] paramNames = { "Page_ID" };
            Object[] paramValues = { pageID };

            DataSet ds = SQL_SP_Exec("[spr_GetPageControlsByPageID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public string Login(string username, string password)
        {
            String[] paramNames = { "Username", "Password" };
            Object[] paramValues = { username, password };

            DataSet ds = SQL_SP_Exec("[spr_Login]", con, paramNames, paramValues);

            return ds.Tables[0].Rows[0][0].ToString();
        }

        public void LogAction(string admin_ID, string message)
        {
            String[] paramNames = { "Admin_ID", "Message" };
            Object[] paramValues = { admin_ID, message };

            DataSet ds = SQL_SP_Exec("[spr_LogAction]", con, paramNames, paramValues);
        }

        public int AddPage(string templateID, string name, string itemID)
        {
            String[] paramNames = { "TemplateID", "Name", "ItemID" };
            Object[] paramValues = { templateID, name, itemID };

            DataSet ds = SQL_SP_Exec("[spr_AddPage]", con, paramNames, paramValues);

            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        public DataTable GetPageByPageID(string pageID)
        {
            String[] paramNames = { "Page_ID" };
            Object[] paramValues = { pageID };

            DataSet ds = SQL_SP_Exec("[spr_GetPageByPageID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetAprimoInfoByPage_ID(string pageID)
        {
            String[] paramNames = { "Page_ID" };
            Object[] paramValues = { pageID };

            DataSet ds = SQL_SP_Exec("[spr_GetAprimoInfoByPage_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetPageBySitecoreItemID(string itemID)
        {
            String[] paramNames = { "ItemID" };
            Object[] paramValues = { itemID };

            DataSet ds = SQL_SP_Exec("[spr_GetPageBySitecoreItemID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetTemplateByPageID(string pageID)
        {
            String[] paramNames = { "Page_ID" };
            Object[] paramValues = { pageID };

            DataSet ds = SQL_SP_Exec("[spr_GetTemplateByPageID]", con, paramNames, paramValues);

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

        public DataTable GetAvalableControlsByPage_ID(string pageID)
        {
            String[] paramNames = { "Page_ID" };
            Object[] paramValues = { pageID };

            DataSet ds = SQL_SP_Exec("[spr_GetAvalableControlsByPage_ID]", con, paramNames, paramValues);

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

        public int AddControlToPlaceHolder(string controlList_ID, string pageID, string pageholder_ID, string pageControl_ID, string displayText)
        {
            String[] paramNames = { "ControlList_ID", "PageID", "Pageholder_ID", "PageControl_ID", "DisplayText" };
            Object[] paramValues = { controlList_ID, pageID, pageholder_ID, pageControl_ID, displayText };

            DataSet ds = SQL_SP_Exec("[spr_AddControlToPlaceHolder]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            else
                return 0;
        }

        public int SavePageControlSetting(object PageControl_ID, string ControlProperty_ID, string settingValue)
        {
            String[] paramNames = { "PageControl_ID", "ControlProperty_ID", "SettingValue"};
            Object[] paramValues = { PageControl_ID, ControlProperty_ID, settingValue };

            DataSet ds = SQL_SP_Exec("[spr_SavePageControlSetting]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            else
                return 0;
        }

        public string GetControlProperty(object PageControl_ID, string PropertyName)
        {
            String[] paramNames = { "PageControl_ID", "PropertyName" };
            Object[] paramValues = { PageControl_ID, PropertyName };

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

        public bool GetECASValueByPage_ID(string page_ID)
        {
            String[] paramNames = { "Page_ID" };
            Object[] paramValues = { page_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetECASValueByPage_ID]", con, paramNames, paramValues);

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

        public bool DoesPagehaveECASControlAction(string page_ID)
        {
            String[] paramNames = { "Page_ID" };
            Object[] paramValues = { page_ID };

            DataSet ds = SQL_SP_Exec("[spr_DoesPagehaveECASControlAction]", con, paramNames, paramValues);

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

        public DataTable GetPages()
        {
            DataSet ds = SQL_SP_Exec("[spr_GetPages]", con, null, null);

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

        public DataTable GetPageControlsByPlaceholderID(string placeholder_ID, string page_ID)
        {
            String[] paramNames = { "Placeholder_ID", "Page_ID" };
            Object[] paramValues = { placeholder_ID, page_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetPageControlsByPlaceholderID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public void RemovePageControl(string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID" };
            Object[] paramValues = { pageControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_RemovePageControl]", con, paramNames, paramValues);
        }

        public void RemovePageByPage_ID(string page_ID)
        {
            String[] paramNames = { "Page_ID" };
            Object[] paramValues = { page_ID };

            DataSet ds = SQL_SP_Exec("[spr_RemovePageByPage_ID]", con, paramNames, paramValues);
        }

        public DataTable GetAllControlActionDataByPageControl_ID(string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID" };
            Object[] paramValues = { pageControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetAllControlActionDataByPageControl_ID]", con, paramNames, paramValues);

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

        public DataTable GetControlActionsByPageControl_ID(string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID" };
            Object[] paramValues = { pageControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetControlActionsByPageControl_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public string GetPage_IDByPageControl_ID(string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID" };
            Object[] paramValues = { pageControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetPage_IDByPageControl_ID]", con, paramNames, paramValues);

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

        public void GetPageControlByPageControl_ID(string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID" };
            Object[] paramValues = { pageControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetPageControlByPageControl_ID]", con, paramNames, paramValues);
        }

        public void AddControlAction(string pageControl_ID, string controlActionType_ID)
        {
            String[] paramNames = { "PageControl_ID", "ControlActionType_ID" };
            Object[] paramValues = { pageControl_ID, controlActionType_ID };

            DataSet ds = SQL_SP_Exec("[spr_AddControlAction]", con, paramNames, paramValues);
        }

        public void UpdateRequiredByPageControl_ID(string pageControl_ID, bool isRequired)
        {
            String[] paramNames = { "PageControl_ID", "IsRequired" };
            Object[] paramValues = { pageControl_ID, isRequired };

            DataSet ds = SQL_SP_Exec("[spr_UpdateRequiredByPageControl_ID]", con, paramNames, paramValues);
        }

        public void UpdateECAS(string page_ID, bool value)
        {
            String[] paramNames = { "Page_ID", "Value" };
            Object[] paramValues = { page_ID, value };

            DataSet ds = SQL_SP_Exec("[spr_UpdateECAS]", con, paramNames, paramValues);
        }

        public void UpdateReturnURLByPage_ID(string page_ID, string returnURL)
        {
            String[] paramNames = { "Page_ID", "ReturnURL" };
            Object[] paramValues = { page_ID, returnURL };

            DataSet ds = SQL_SP_Exec("[spr_UpdateReturnURLByPage_ID]", con, paramNames, paramValues);
        }

        public string GetReturnURLByPage_ID(string page_ID)
        {
            String[] paramNames = { "Page_ID"};
            Object[] paramValues = { page_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetReturnURLByPage_ID]", con, paramNames, paramValues);

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

        public void UpdateAprimoInfo(string page_ID, string subject, string aprimoID)
        {
            String[] paramNames = { "Page_ID", "Subject", "ID" };
            Object[] paramValues = { page_ID, subject, aprimoID };

            DataSet ds = SQL_SP_Exec("[spr_UpdateAprimoInfo]", con, paramNames, paramValues);
        }

        public void SavePageControlDefaultOption(string defaultOption, string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID", "DefaultOption" };
            Object[] paramValues = { pageControl_ID, defaultOption };

            DataSet ds = SQL_SP_Exec("[spr_SavePageControlDefaultOption]", con, paramNames, paramValues);
        }

        public void UpdateControlPropertySetting(string pageControl_ID, string propertyName, string value)
        {
            String[] paramNames = { "PageControl_ID", "PropertyName", "Value" };
            Object[] paramValues = { pageControl_ID, propertyName, value };

            DataSet ds = SQL_SP_Exec("[spr_UpdateControlPropertySetting]", con, paramNames, paramValues);
        }

        public void AddControlOption(string pageControl_ID, string text, string value)
        {
            String[] paramNames = { "PageControl_ID", "Text", "Value" };
            Object[] paramValues = { pageControl_ID, text, value };

            DataSet ds = SQL_SP_Exec("[spr_AddControlOption]", con, paramNames, paramValues);
        }

        public void UpdateControlOption(string controlOption_ID, string text, string value)
        {
            String[] paramNames = { "ControlOption_ID", "Text", "Value" };
            Object[] paramValues = { controlOption_ID, text, value };

            DataSet ds = SQL_SP_Exec("[spr_UpdateControlOption]", con, paramNames, paramValues);
        }

        public DataTable GetPageControlPropertyValuesByPageControl_ID(string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID" };
            Object[] paramValues = { pageControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetPageControlPropertyValuesByPageControl_ID]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public string GetDefaultOptionByPageControl_ID(string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID" };
            Object[] paramValues = { pageControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetDefaultOptionByPageControl_ID]", con, paramNames, paramValues);

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

        public DataTable GetControlInfoByPageControl_ID(string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID" };
            Object[] paramValues = { pageControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetControlInfoByPageControl_ID]", con, paramNames, paramValues);

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

        public void SetPageControlOrder(string delimitedIDs, string delimiter)
        {
            String[] paramNames = { "DelimitedPageControl_IDs", "Delimiter" };
            Object[] paramValues = { delimitedIDs, delimiter };

            DataSet ds = SQL_SP_Exec("[spr_SetPageControlOrder]", con, paramNames, paramValues);
        }

        public void SetControlOptionOrder(string delimitedIDs, string delimiter)
        {
            String[] paramNames = { "DelimitedPageControl_IDs", "Delimiter" };
            Object[] paramValues = { delimitedIDs, delimiter };

            DataSet ds = SQL_SP_Exec("[spr_SetControlOptionOrder]", con, paramNames, paramValues);
        }

        public DataTable GetControlOptionsByPageControl_ID(string pageControl_ID)
        {
            String[] paramNames = { "PageControl_ID" };
            Object[] paramValues = { pageControl_ID };

            DataSet ds = SQL_SP_Exec("[spr_GetControlOptionsByPageControl_ID]", con, paramNames, paramValues);

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

        public DataTable UpdatePageInfo(string pageID, string styleTypeID, string campaign, string page, string source)
        {
            String[] paramNames = { "Page_ID", "StyleType_ID", "Tracking_Campaign", "Tracking_Page", "Tracking_Source" };
            Object[] paramValues = { pageID, styleTypeID, campaign, page, source };

            DataSet ds = SQL_SP_Exec("[spr_UpdatePageInfo]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        #endregion
    }
}