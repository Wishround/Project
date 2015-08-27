using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WishroundProject.DataAccess;
using WishroundProject.Models;
using Microsoft.AspNet.Identity;
using WishroundProject.Tools.Parser.Objects;
using Newtonsoft.Json;

namespace WishroundProject.Controllers
{
    [Authorize]
    public class WishController : Controller
    {
        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateWishModel());
        }

        [HttpPost]
        public ActionResult Create(CreateWishModel model)
        {
            if (ModelState.IsValid)
            {
                var uniqueKey = Guid.NewGuid().ToString().GetHashCode().ToString("x");
                var html = new HtmlDocument();
                html.LoadHtml(new WebClient().DownloadString(model.URL));
                var root = html.DocumentNode;
                var nodes = root.Descendants();
                var totalNodes = nodes.Count();
                HtmlNode name = html.DocumentNode.Descendants("script").Where(x => x.InnerHtml.Contains("dataLayer.push({\"pageType\":")).First();
                string json = name.InnerHtml;
                var startIndex = json.IndexOf("dataLayer.push({\"pageType\":");
                json = json.Substring(startIndex + 15, json.Length - (startIndex + 15) - 2);
                json = json.Substring(0, json.Length - 2);
                Product product = JsonConvert.DeserializeObject<Product>(json);
                WebClient client = new WebClient();
                string imageUrl = "/Content/WishImages/" + Guid.NewGuid().ToString() + ".png";
                client.DownloadFile(new Uri(product.productImageURL), Server.MapPath("~") + imageUrl);
                var productName = product.productName.IndexOf('.') > 0 ? product.productName.Substring(0, product.productName.IndexOf('.')) : product.productName;
                WishAccess wAccess = new WishAccess(new Guid(User.Identity.GetUserId()));
                Wish wish = wAccess.InsertWish(productName, product.productID.ToString(), product.productPrice, product.Currency, imageUrl);
                if (wish != null)
                {
                    return RedirectToAction("Index", "Wish", new { id = wish.PublicId });
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var wish = WishAccess.GetWishByPublishId(id);
            return View(new WishInfoModel { WishId = id.ToString(), Name = wish.Name, Code = wish.Code, ImageUrl = wish.ImageURL, Cost = (float)wish.Cost, Currency = wish.Currency });
        }

        public ActionResult All()
        {
            AllWishesModel model = new AllWishesModel { Wishes = new List<WishPreview>() };
            WishAccess wAccess = new WishAccess(new Guid(User.Identity.GetUserId()));
            foreach (var wish in wAccess.GetAllWishes())
            {
                model.Wishes.Add(new WishPreview { PublicId = wish.PublicId, Name = wish.Name, Cost = wish.Cost, Currency = wish.Currency, ImageUrl = wish.ImageURL });
            }
            return View(model);
        }
    }
}