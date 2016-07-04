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
    public class OrderDetailController : Controller
    {
        private BanGiayEntities db = new BanGiayEntities();

        //
        // GET: /OrderDetail/

        public ViewResult Index()
        {
            var orderdetails = db.OrderDetails.Include("Order").Include("Product");
            return View(orderdetails.ToList());
        }

        //
        // GET: /OrderDetail/Details/5

        public ViewResult Details(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Single(o => o.OrderId == id);
            return View(orderdetail);
        }

        //
        // GET: /OrderDetail/Create

        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "FullName");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Color");
            return View();
        } 

        //
        // POST: /OrderDetail/Create

        [HttpPost]
        public ActionResult Create(OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.AddObject(orderdetail);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "FullName", orderdetail.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Color", orderdetail.ProductId);
            return View(orderdetail);
        }
        
        //
        // GET: /OrderDetail/Edit/5
 
        public ActionResult Edit(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Single(o => o.OrderId == id);
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "FullName", orderdetail.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Color", orderdetail.ProductId);
            return View(orderdetail);
        }

        //
        // POST: /OrderDetail/Edit/5

        [HttpPost]
        public ActionResult Edit(OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Attach(orderdetail);
                db.ObjectStateManager.ChangeObjectState(orderdetail, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "FullName", orderdetail.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Color", orderdetail.ProductId);
            return View(orderdetail);
        }

        //
        // GET: /OrderDetail/Delete/5
 
        public ActionResult Delete(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Single(o => o.OrderId == id);
            return View(orderdetail);
        }

        //
        // POST: /OrderDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            OrderDetail orderdetail = db.OrderDetails.Single(o => o.OrderId == id);
            db.OrderDetails.DeleteObject(orderdetail);
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