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
                //if (!Request.Browser.IsMobileDevice)
                //{
                    var html = new HtmlDocument();
                    html.LoadHtml(new WebClient().DownloadString(model.URL));
                    var root = html.DocumentNode;
                    var nodes = root.Descendants();
                    var totalNodes = nodes.Count();
                    HtmlNode name = html.DocumentNode.Descendants("script").Where(x => x.InnerHtml.Contains("dataLayer.push({\"pageType\":")).First();
                    //IEnumerable<HtmlNode> name = html.DocumentNode.Descendants("title").Where(x => x.Attributes.Contains("href"));

                    string json = name.InnerHtml;

                    var startIndex = json.IndexOf("dataLayer.push({\"pageType\":");
                    json = json.Substring(startIndex + 15, json.Length - (startIndex + 15) - 2);
                    json = json.Substring(0, json.Length - 2);
                    Product product = JsonConvert.DeserializeObject<Product>(json);
                    WebClient client = new WebClient();
                    string imageUrl = "/Content/WishImages/" + Guid.NewGuid().ToString()+".png";
                    client.DownloadFile(new Uri(product.productImageURL), Server.MapPath("~")+imageUrl);
                    WishAccess wAccess = new WishAccess(new Guid(User.Identity.GetUserId()));
                    Wish wish = wAccess.InsertWish(product.productName, product.productID.ToString(), product.productPrice, product.Currency, imageUrl);
                    if (wish != null)
                    {
                        return RedirectToAction("Index", "Wish", new { id = wish.PublicId });
                    }
                    //Session[uniqueKey] = new ConfirmWish { Currency = product.Currency, ImageUrl = product.productImageURL, Cost = product.productPrice, Code = product.productID.ToString(), Name = product.productName, URL = model.URL };
                //}
                //else
                //{
                //    ViewData[uniqueKey] = model.URL;
                //}
                //return RedirectToAction("Confirm", new { k = uniqueKey, m = Request.Browser.IsMobileDevice });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var wish = WishAccess.GetWishByPublishId(id);
            return View(new WishInfoModel{ WishId=id.ToString(), Name = wish.Name, Code = wish.Code, ImageUrl = wish.ImageURL, Cost = (float)wish.Cost, Currency = wish.Currency});
        }

        //[HttpPost]
        //public ActionResult Confirm(ConfirmWishModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        WishAccess wAccess = new WishAccess(new Guid(User.Identity.GetUserId()));
        //        Wish wish = wAccess.InsertWish(model.Name, model.Code, model.Cost, model.Currency, model.ImageUrl);
        //        if (wish != null)
        //        {
        //            return RedirectToAction("All");
        //        }
        //    }
        //    return View(model);
        //}

        public ActionResult All()
        {
            AllWishesModel model = new AllWishesModel { Wishes = new List<WishPreview>() };
            WishAccess wAccess = new WishAccess(new Guid(User.Identity.GetUserId()));
            foreach (var wish in wAccess.GetAllWishes())
            {
                model.Wishes.Add(new WishPreview { Name = wish.Name, Cost = wish.Cost, Currency = wish.Currency, ImageUrl = wish.ImageURL });
            }
            return View(model);
        }
    }
}