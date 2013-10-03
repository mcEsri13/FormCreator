using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
//using Esri.Web.FormGenerator.Utilities;
using System.Data;
using System.Collections.Specialized;
//using Esri.EdCommunity;

namespace Esri.Web.FormGenerator.Data
{
    public class FormGeneratorHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var results = context.Request.Params.Get("results");
            Json jsonResults = new JavaScriptSerializer().Deserialize<Json>(results);

            //validation for action..
            foreach (KeyValuePair<string, string> actionValue in jsonResults.Actions)
            {
                if (actionValue.Value.ToLower().Equals("send data to aprimo (test)"))
                {
                    //Send APrimo Function call (test)
                    ToAPrimoEventTest(jsonResults.Form, jsonResults.DB, jsonResults.Groups);
                }
                if (actionValue.Value.ToLower().Equals("send data to aprimo (live)"))
                {
                    //Send APrimo Function call (live)
                    ToAPrimoEvent(jsonResults.Form, jsonResults.DB, jsonResults.Groups);
                }
            }
        }

        private void ToAPrimoEventTest(Dictionary<string, string> form, Dictionary<string, string> demandBase, Dictionary<string, string> groups)
        {
            DataTable dbResults = new DataTable();
            dbResults.Columns.Add("Name");
            dbResults.Columns.Add("Value");

            ToDataTable(form, ref dbResults);
            ToDataTable(groups, ref dbResults);
            ToDataTable(demandBase, ref dbResults);

            //FormGeneratorEmail email = new FormGeneratorEmail();
            //email.Send(dbResults, "Name", "Value", "Test", "TestID.");
        }

        private void ToAPrimoEvent(Dictionary<string, string> form, Dictionary<string, string> demandBase, Dictionary<string, string> groups)
        {
            DataTable dbResults = new DataTable();
            dbResults.Columns.Add("Name");
            dbResults.Columns.Add("Value");

            ToDataTable(form, ref dbResults);
            ToDataTable(groups, ref dbResults);
            ToDataTable(demandBase, ref dbResults);

            //FormGeneratorEmail email = new FormGeneratorEmail();
            //email.Send(dbResults, "Name", "Value", "Test", "TestID.");
        }

        public void ToDataTable(Dictionary<string, string> list, ref DataTable result)
        {
            if (list.Count == 0)
                return;

            foreach (KeyValuePair<string, string> col in list)
            {
                DataRow dr = result.NewRow();

                dr["Name"] = col.Key;
                dr["Value"] = col.Value;

                result.Rows.Add(dr);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class Json
        {
            public Dictionary<string, string> Form { get; set; }
            public Dictionary<string, string> DB { get; set; }
            public Dictionary<string, string> Actions { get; set; }
            public Dictionary<string, string> FormActions { get; set; }
            public Dictionary<string, string> Groups { get; set; }
        }
    }
}