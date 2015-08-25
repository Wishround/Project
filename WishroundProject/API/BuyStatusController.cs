using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WishroundProject.API.Objects;

namespace WishroundProject.API
{
    public class BuyStatusController : ApiController
    {
        // GET api/buystatus/5
        [HttpGet]
        [ActionName("get")]
        public IHttpActionResult GetStatus(string orderId)
        {
            return Ok("success");
        }

        // POST api/buystatus
        [HttpPost]
        [ActionName("set")]
        public IHttpActionResult SetStatus([FromBody]Status status)
        {
            var i = status.data;
            return Ok("ok");
        }
    }
}
