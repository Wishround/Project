using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WishroundProject
{
    public static class Config
    {

        public static readonly string LiqPayPrivateKey;

        public static readonly string LiqPayPublicKey;

        public static readonly string FacebookAppId;

        static Config()
        {
            LiqPayPrivateKey = WebConfigurationManager.AppSettings["LPPrivateKey"];
            LiqPayPublicKey = WebConfigurationManager.AppSettings["LPPublicKey"];
            FacebookAppId = WebConfigurationManager.AppSettings["FBAppId"];
        }
    }
}