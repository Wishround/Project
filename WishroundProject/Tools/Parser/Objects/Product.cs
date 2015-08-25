using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WishroundProject.Tools.Parser.Objects
{
    public class Product
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public float productPrice { get; set; }

        public string productImageURL { get; set; }

        public string Currency
        {
            get
            {
                return "USD";
            }
        }
    }
}