using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WishroundProject.Tools.Parser.Objects
{
    public class BuyInfo
    {
        public string public_key { get; set; }

        public Guid order_id { get; set; }

        public string status { get; set; }
    }
}