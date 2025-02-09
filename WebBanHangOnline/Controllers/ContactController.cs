﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Contact
        public ActionResult Index(string id)
        {
            var items = db.Posts.FirstOrDefault(x => x.Alias == id);
            return View(items);
        }
    }
}