using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Esri.FormGenerator;

namespace Esri.Web.FormGenerator.Data
{
    public class FormGeneratorData
    {
        #region Global

        private static string Connection = ConfigurationManager.ConnectionStrings["FormGenerator"].ToString();
        private DataSet Collection = new DataSet();
        String[] paramNames = { "SitecoreID" };

        #endregion

        #region Events

        public DataSet GetData(string id)
        {
            //Update the param values object with the pass string value..
            Object[] paramValues = { id };

            //Create the connection
            using (SqlConnection sCon = new System.Data.SqlClient.SqlConnection(Connection))
            {
                //Set command type to stored procedure and call sproc against sql connection
                using (SqlCommand sCmd = new System.Data.SqlClient.SqlCommand("[spr_GetAllFormDataByItemID]", sCon))
                {
                    sCmd.CommandType = CommandType.StoredProcedure;
                    //Create data adpater and store the connection command type through and prepare iteration process.
                    using (SqlDataAdapter sAdapater = new System.Data.SqlClient.SqlDataAdapter(sCmd))
                    {
                        //iteration through each paramter set...
                        for (int i = 0; i < paramNames.Length; i++)
                            sCmd.Parameters.Add(paramNames[i], paramValues[i]);

                        //open connnection, fill data and close (optional)
                        sCon.Open();
                        sAdapater.Fill(Collection);
                        sCon.Close();
                    }
                }
            }
            //return data
            return Collection;
        }

        public DataTable GetConstantValues(int controlListID)
        {
            //Create the connection
            using (SqlConnection sCon = new System.Data.SqlClient.SqlConnection(Connection))
            {
                //Set command type to stored procedure and call sproc against sql connection
                using (SqlCommand sCmd = new System.Data.SqlClient.SqlCommand("[spr_GetControlConstantValues]", sCon))
                {
                    sCmd.CommandType = CommandType.StoredProcedure;
                    //Create data adpater and store the connection command type through and prepare iteration process.
                    using (SqlDataAdapter sAdapater = new System.Data.SqlClient.SqlDataAdapter(sCmd))
                    {
                        //open connnection, fill data and close (optional)
                        sCmd.Parameters.AddWithValue("ControlList_ID", controlListID);
                        sCon.Open();
                        sAdapater.Fill(Collection);
                        sCon.Close();
                    }
                }
                return Collection.Tables[0];
            }
        }
        #endregion
    }
}