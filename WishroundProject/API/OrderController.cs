using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using WishroundProject.API.Objects;
using WishroundProject.DataAccess;
using WishroundProject.Tools.Parser.Objects;

namespace WishroundProject.API
{
    public class OrderController : ApiController
    {
        // GET api/buystatus/5
        [HttpGet]
        [ActionName("getstatus")]
        public IHttpActionResult GetStatus(Guid orderId)
        {
            OrderAccess oAccess = new OrderAccess();
            string status = oAccess.GetStatusByOrderId(orderId);

            if (!string.IsNullOrEmpty(status))
            {
                return Ok(new { orderId = orderId, status = status });
            }
            else
            {
                return BadRequest("Order not found");
            }
        }

        // POST api/buystatus
        [HttpPost]
        [ActionName("setstatus")]
        public IHttpActionResult SetStatus([FromBody]Status status)
        {
            var jsonData = Base64Decode(status.data);
            BuyInfo info = JsonConvert.DeserializeObject<BuyInfo>(jsonData);
            var privateKey = Config.LiqPayPrivateKey;
            var hash = System.Convert.ToBase64String(sha1Hash(privateKey + status.data + privateKey));
            if (!hash.Equals(status.signature))
            {
                return BadRequest("Signature is'n correct");
            }
            OrderAccess oAccess = new OrderAccess();
            bool result = oAccess.SetStatusByOrderId(info.order_id, info.status);
            if (result)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Data is'n correct");
            }
        }

        [HttpPost]
        [ActionName("create")]
        public IHttpActionResult Create([FromBody]WishroundProject.API.Objects.Wish wish)
        {
            OrderAccess oAccess = new OrderAccess();
            var newOrder = oAccess.CreateForWish(wish.wishId);

            if (newOrder != null)
            {
                return Ok<WishroundProject.API.Objects.Order>(new Objects.Order { orderId = newOrder.PublicId });
            }
            else
            {
                return BadRequest("Wish not found");
            }
        }

        [HttpPost]
        [ActionName("getsignature")]
        public IHttpActionResult GetSignature(Data data)
        {
            var privateKey = Config.LiqPayPrivateKey;

            var hash = System.Convert.ToBase64String(sha1Hash(privateKey + data.data + privateKey));

            if (!string.IsNullOrEmpty(hash))
            {
                return Ok(new { signature = hash });
            }
            else
            {
                return BadRequest("Error");
            }
        }

        Byte[] sha1Hash(string password)
        {
            return SHA1CryptoServiceProvider.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
