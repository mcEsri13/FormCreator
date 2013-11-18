using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using esri.com.Saml;
using Esri.ICASTokenManager;
using log4net;

namespace FormGeneratorAdmin
{
    public partial class Consume : System.Web.UI.Page
    {
        public const string IEsriSessionidCookie = "IESRISESSIONID";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Consume));

        private void WriteIcasCookie(string tokenId)
        {
            //write Icas session cookie
            var cookie = new HttpCookie(IEsriSessionidCookie)
            {
                Domain = ".esri.com",
                Value = tokenId,
                Path = "/",
                Expires = DateTime.MinValue
            };

            Response.Cookies.Add(cookie);
        }

        private void PrintUser(User user)
        {

            Response.Write(user.Name);
            Response.Write("<ul>");
            foreach (var pair in user.Attributes)
            {
                Response.Write(string.Format("<li>{0}: {1}</li>", pair.Key, pair.Value));
            }
            Response.Write("</ul>");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string certificate = ConfigurationManager.AppSettings["OktaCertificate"];
            string idpSsoTargetUrl = ConfigurationManager.AppSettings["IdpSsoTargetUrlKey"];

            if (string.IsNullOrEmpty(Request.Form["SAMLResponse"]) == false)
            {
                var accountSettings = new AccountSettings(certificate, idpSsoTargetUrl);
                var samlResponse = new Response(accountSettings);
                samlResponse.LoadXmlFromBase64(Request.Form["SAMLResponse"]);

                if (samlResponse.IsValid())
                {
                    User myuser = samlResponse.GetUser();

                    if (Logger.IsDebugEnabled)
                    {
                        PrintUser(myuser);
                    }

                    string employeeNumber;
                    myuser.Attributes.TryGetValue("employeeNumber", out employeeNumber);

                    if (employeeNumber == null)
                    {
                        const string message =
                            "Employee number not found. Either the SAML response is missing the employeeNumber assertion, the value is null, or the assertion could not be parsed.";
                        Logger.Error(message);

                        throw new InvalidOperationException(
                           "Cannot create ICAS session if the employee number is missing. " + message);
                    }
                    //create ICAS token
                    IcasTokenService svc = IcasTokenService.Instance();
                    string tokenId = svc.CreateIcasToken(employeeNumber);
                    //write the token in the cookie 
                    WriteIcasCookie(tokenId);

                    string relayState = Utils.ParseRelayState(Request);



                    if (Uri.IsWellFormedUriString(relayState, uriKind: UriKind.Absolute))
                    {
                        Response.Redirect(relayState);
                        Logger.Warn(string.Format("RelayState Url is not well formed. RelayState={0}", relayState));
                    }

                }
                else
                {
                    const string message = "SAML Assertion not found.";
                    Logger.Warn(message);
                    throw (new Exception(message));
                }
            }
            else
            {
                Response.Redirect("/");
            }
        }

        protected void UpdateAppSetting(string key, string value)
        {
            ExeConfigurationFileMap FileMap = new ExeConfigurationFileMap();

            FileMap.ExeConfigFilename = Server.MapPath(@"~\Web.config");
            Configuration Config = ConfigurationManager.OpenMappedExeConfiguration(FileMap, ConfigurationUserLevel.None);

            Config.AppSettings.Settings.Add(key, value);
            Config.Save(ConfigurationSaveMode.Modified);        
        }
    }
}