using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormGeneratorAdmin
{
    public class Utils
    {
        public static string ParseRelayState(HttpRequest request)
        {
            string relayState = request.QueryString["RelayState"];
            if (string.IsNullOrEmpty(relayState))
            {
                relayState = request.Form["RelayState"];
            }
            return relayState;
        }
    }
}