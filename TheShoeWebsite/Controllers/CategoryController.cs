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
    public class CategoryController : Controller
    {
        private BanGiayEntities db = new BanGiayEntities();

        
        public  ActionResult LeftMenu()
        {

            var cat = db.Categories.ToList();
            return PartialView(cat);
        }

        //
        // GET: /Category/
     // [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ViewResult Index()
        {
            return View(db.Categories.ToList());
        }

        //
        // GET: /Category/Details/5
       // [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ViewResult Details(string id)
        {
            Category category = db.Categories.Include("Products").SingleOrDefault(c => c.CategoryId == id);
            return View(category);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.AddObject(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(string id)
        {
            Category category = db.Categories.Single(c => c.CategoryId == id);
            return View(category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Attach(category);
                db.ObjectStateManager.ChangeObjectState(category, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(string id)
        {
            Category category = db.Categories.Single(c => c.CategoryId == id);
            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Category category = db.Categories.Single(c => c.CategoryId == id);
            db.Categories.DeleteObject(category);
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