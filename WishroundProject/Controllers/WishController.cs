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
                if (!Request.Browser.IsMobileDevice)
                {
                    var html = new HtmlDocument();
                    html.LoadHtml(new WebClient().DownloadString(model.URL));
                    var root = html.DocumentNode;
                    var nodes = root.Descendants();
                    var totalNodes = nodes.Count();
                    ViewData[uniqueKey] = new ConfirmWish { Currency = "UAH", ImageUrl = "/ff", Cost = 23.4f, Code = "dsds", Name = "dsfsd", URL = model.URL };
                }
                else
                {
                    ViewData[uniqueKey] = model.URL;
                }
                return RedirectToAction("Confirm", new { k = uniqueKey, m = Request.Browser.IsMobileDevice });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Confirm(string k, bool m)
        {
            //if (string.IsNullOrEmpty(k) || k.Length != 6)
            //{
            //    return RedirectToAction("Create");
            //}

            ConfirmWishModel model = new ConfirmWishModel(); ;

            //if (m)
            //{
            //    ConfirmWish data = ViewData[k] as ConfirmWish;
            //    model = (ConfirmWishModel)data;
            //    model.IsClientParsing = false;
            //}
            //else
            //{
            //    string url = ViewData[k].ToString();
            //    model = new ConfirmWishModel { URL = url, IsClientParsing = true };
            //}

            return View(model);
        }

        [HttpPost]
        public ActionResult Confirm(ConfirmWishModel model)
        {
            if (ModelState.IsValid)
            {
                WishAccess wAccess = new WishAccess(new Guid(User.Identity.GetUserId()));
                Wish wish = wAccess.InsertWish(model.Name, model.Code, model.Cost, model.Currency, model.ImageUrl);
                if (wish != null)
                {
                    return RedirectToAction("All");
                }
            }
            return View(model);
        }

        public ActionResult All()
        {
            AllWishesModel model = new AllWishesModel{ Wishes = new List<WishPreview>()};
            WishAccess wAccess = new WishAccess(new Guid(User.Identity.GetUserId()));
            foreach (var wish in wAccess.GetAllWishes())
            {
                model.Wishes.Add(new WishPreview { Name = wish.Name, Cost = wish.Cost, Currency = wish.Currency, ImageUrl = wish.ImageURL });
            }
            return View(model);
        }
	}
}