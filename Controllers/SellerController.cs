using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eticaret.DAL;
using eticaret.Models;

namespace eticaret.Controllers
{
    [Authorize]
    public class SellerController : Controller
    {
        private Context db = new Context();

        // GET: Seller
        public ActionResult Index()
        {
            return View(db.Seller.ToList());
        }

        // GET: Seller/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Seller.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // GET: Seller/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Seller/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Password")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                db.Seller.Add(seller);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(seller);
        }

        // GET: Seller/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Seller.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Seller/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Password")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seller).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seller);
        }

        // GET: Seller/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Seller.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Seller/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seller seller = db.Seller.Find(id);
            db.Seller.Remove(seller);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult SellerLogin()
        {
            return View();
        }
        public ActionResult AddProduct()
        {
            ViewBag.Categories = db.Category.ToList();
            ViewBag.SubCategories = db.SubCategory.ToList();
            ViewBag.Brands = db.Brand.ToList();

            return View();

        }

        [HttpPost]
        public ActionResult AddProduct(FormCollection form)
        {
            Product product = new Product();
            product.BrandID = Convert.ToInt32(form["brand"]);
            product.Name = form["name"];
            product.Price = Convert.ToInt32(form["price"]);
            product.Rating = 0;
            product.Detail = form["description"];
            product.isApproved = false;
            product.Stock = Convert.ToInt32(form["stock"]);
            product.CategoryID = Convert.ToInt32(form["categoryId"]);
            product.SubCategoryID = Convert.ToInt32(form["subcategoryId"]);
            product.SellerID = Convert.ToInt32(form["SellerId"]);

            db.Product.Add(product);
            db.SaveChanges();

            string imagesPath = Server.MapPath("~/Content/Images/ProductImages/") + product.Id; 
            Directory.CreateDirectory(imagesPath);

            for (int i = 1; i <= 3; i++)
            {
                HttpPostedFileBase image = Request.Files["image"+i];
                image.SaveAs(imagesPath + "/" +i+ ".jpg");
            }

            return RedirectToAction("AddProduct");


        }
    }
}
