using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheShoeWebsite.Models;

namespace TheShoeWebsite.Controllers
{
    public class ProductController : Controller
    {
        private BanGiayEntities db = new BanGiayEntities();
        public int PageSize = 20;
        //
        // GET: /Product/
        //  [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ViewResult Index(int page = 1)
        {
            //var products = db.Products.Include("Category").OrderBy(p => p.ProductId).Skip((page - 1) * PageSize).Take(PageSize);
            var products = db.Products.Include("Category");
            return View(products.ToList());
        }

        //
        // GET: /Product/Details/5
        // [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ViewResult Details(int id)
        {
            Product product = db.Products.Single(p => p.ProductId == id);
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description");
            return View();
        }


        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.AddObject(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id)
        {
            Product product = db.Products.Single(p => p.ProductId == id);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Attach(product);
                db.ObjectStateManager.ChangeObjectState(product, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id)
        {
            Product product = db.Products.Single(p => p.ProductId == id);
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Single(p => p.ProductId == id);
            db.Products.DeleteObject(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}