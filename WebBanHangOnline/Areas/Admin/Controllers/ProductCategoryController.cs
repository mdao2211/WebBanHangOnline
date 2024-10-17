using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var items = db.ProductCategories;
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.ProductCategories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            var item = db.ProductCategories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.ProductCategories.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var category = db.ProductCategories.Find(id);
            if (category != null)
            {
                // Lấy danh sách các sản phẩm thuộc danh mục
                var products = db.Products.Where(p => p.ProductCategoryId == id).ToList();

                // Xóa tất cả hình ảnh của từng sản phẩm
                foreach (var product in products)
                {
                    // Lấy và xóa các hình ảnh của sản phẩm
                    var productImages = db.ProductImages.Where(x => x.ProductId == product.Id).ToList();
                    foreach (var image in productImages)
                    {
                        db.ProductImages.Remove(image);
                    }

                    // Xóa sản phẩm
                    db.Products.Remove(product);
                }

                // Xóa danh mục sản phẩm
                db.ProductCategories.Remove(category);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}