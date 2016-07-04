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
    public class SubCategoryController : Controller
    {
        private BanGiayEntities db = new BanGiayEntities();
        private BanGiayRepository dr = new BanGiayRepository();
        //
        // GET: /SubCat/

        public ViewResult Index()
        {
            var subcategories = db.SubCategories.Include("Category");
            return View(subcategories.ToList());
        }

      

        //
        // GET: /SubCat/Details/5

        public ViewResult Details(string id)
        {
           List< Product> products = dr.GetProductBySubCat(id).ToList();
            return View(products);
        }

        //
        // GET: /SubCat/Create

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description");
            return View();
        }

        //
        // POST: /SubCat/Create

        [HttpPost]
        public ActionResult Create(SubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.SubCategories.AddObject(subcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", subcategory.CategoryId);
            return View(subcategory);
        }

        //
        // GET: /SubCat/Edit/5

        public ActionResult Edit(string id)
        {
            SubCategory subcategory = db.SubCategories.Single(s => s.SubCatId == id);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", subcategory.CategoryId);
            return View(subcategory);
        }

        //
        // POST: /SubCat/Edit/5

        [HttpPost]
        public ActionResult Edit(SubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.SubCategories.Attach(subcategory);
                db.ObjectStateManager.ChangeObjectState(subcategory, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", subcategory.CategoryId);
            return View(subcategory);
        }

        //
        // GET: /SubCat/Delete/5

        public ActionResult Delete(string id)
        {
            SubCategory subcategory = db.SubCategories.Single(s => s.SubCatId == id);
            return View(subcategory);
        }

        //
        // POST: /SubCat/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            SubCategory subcategory = db.SubCategories.Single(s => s.SubCatId == id);
            db.SubCategories.DeleteObject(subcategory);
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