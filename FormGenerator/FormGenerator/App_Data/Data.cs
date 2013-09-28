using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FormGenerator
{
    public class Data
    {
        #region Variables
        private SqlConnection con;
        #endregion

        #region Constructors
        public Data()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["SandBox"].ToString());
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

        public DataTable GetTemplates()
        {
            DataSet ds = SQL_SP_Exec("[spr_GetTemplates]", con, null, null);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public bool AddControlToPlaceHolder(string controlList_ID, string pageID, string pageholder_ID, string fieldName, string watermark)
        {
            String[] paramNames = { "ControlList_ID", "PageID", "Pageholder_ID", "FieldName", "Watermark" };
            Object[] paramValues = { controlList_ID, pageID, pageholder_ID, fieldName, watermark };

            DataSet ds = SQL_SP_Exec("[spr_AddControlToPlaceHolder]", con, paramNames, paramValues);

            if (ds.Tables.Count > 0)
                return Convert.ToBoolean(ds.Tables[0].Rows[0][0]);
            else
                return false;
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

        public DataTable GetPages()
        {
            DataSet ds = SQL_SP_Exec("[spr_GetPages]", con, null, null);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable GetPageControlsByPlaceholderID(string placeholder_ID)
        {
            String[] paramNames = { "Placeholder_ID" };
            Object[] paramValues = { placeholder_ID };

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

        public void SetPageControlOrder(string delimitedIDs, string delimiter)
        {
            String[] paramNames = { "DelimitedPageControl_IDs", "Delimiter" };
            Object[] paramValues = { delimitedIDs, delimiter };

            DataSet ds = SQL_SP_Exec("[spr_SetPageControlOrder]", con, paramNames, paramValues);
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

        #endregion
    }
}