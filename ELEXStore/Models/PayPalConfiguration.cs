using System.Collections.Generic;
using System.Net;
using PayPal.Api;

namespace ELEXStore.Models
{
    public static class Configuration
    {
        //these variables will store the clientID and clientSecret
        //by reading them from the web.config
        public static readonly string ClientId;
        public static readonly string ClientSecret;

        static Configuration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            // getting accesstocken from paypal               
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            string accessToken = new OAuthTokenCredential
        (ClientId, ClientSecret, GetConfig()).GetAccessToken();

            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken()) {Config = GetConfig()};
            return apiContext;
        }
    }
}