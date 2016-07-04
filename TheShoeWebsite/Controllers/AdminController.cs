using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheShoeWebsite.Models;
using TheShoeWebsite.ViewModels;
using System.Data;
namespace TheShoeWebsite.Controllers
{         
    [Authorize]
    public class AdminController : Controller
    {
        private BanGiayEntities db = new BanGiayEntities();

        #region  Product
        //
        // GET: /Product/

        public ViewResult ProductList()
        {
            var products = db.Products.Include("Category");
            return View(products.ToList());
        }

        //
        // GET: /Product/Details/5

        public ViewResult ProductDetails(int id)
        {
            Product product = db.Products.Single(p => p.ProductId == id);
            return View(product);
        }


        //
        // GET: /Product/Create

        public ActionResult ProductCreate()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description");
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCatId", "Description");
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult ProductCreate(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.AddObject(product);
                db.SaveChanges();
                return RedirectToAction("ProductList");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult ProductEdit(int id)
        {
            Product product = db.Products.Single(p => p.ProductId == id);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult ProductEdit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Attach(product);
                db.ObjectStateManager.ChangeObjectState(product, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("ProductEdit");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult ProductDelete(int id)
        {
            Product product = db.Products.Single(p => p.ProductId == id);
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("ProductDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Single(p => p.ProductId == id);
            db.Products.DeleteObject(product);
            db.SaveChanges();
            return RedirectToAction("ProductList");
        }


        #endregion

        #region Category
        //
        // GET: /Category/

        public ViewResult CategoryList()
        {
            return View(db.Categories.ToList());
        }

        //
        // GET: /Category/Details/5

        public ViewResult CategoryDetails(string id)
        {
            Category category = db.Categories.Single(c => c.CategoryId == id);
            return View(category);
        }

        //
        // GET: /Category/Create

        public ActionResult CategoryCreate()
        {
            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult CategoryCreate(Category category)
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

        public ActionResult CategoryEdit(string id)
        {
            Category category = db.Categories.Single(c => c.CategoryId == id);
            return View(category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult CategoryEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Attach(category);
                db.ObjectStateManager.ChangeObjectState(category, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("CategoryEdit");
            }
            return View(category);
        }

        //
        // GET: /Category/Delete/5

        public ActionResult CategoryDelete(string id)
        {
            Category category = db.Categories.Single(c => c.CategoryId == id);
            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("CategoryDelete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Category category = db.Categories.Single(c => c.CategoryId == id);
            db.Categories.DeleteObject(category);
            db.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        #endregion

        #region  Account
        //
        // GET: /Account/

        public ViewResult AccountList()
        {
            return View(db.Accounts.ToList());
        }

        //
        // GET: /Account/Details/5

        public ViewResult AccountDetails(int id)
        {
            Account Account = db.Accounts.Single(c => c.AccountId == id);
            return View(Account);
        }

        //
        // GET: /Account/Create

        public ActionResult AccountCreate()
        {
            return View();
        }

        //
        // POST: /Account/Create

        [HttpPost]
        public ActionResult AccountCreate(Account Account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.AddObject(Account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Account);
        }

        //
        // GET: /Account/Edit/5

        public ActionResult AccountEdit(int id)
        {
            Account Account = db.Accounts.Single(c => c.AccountId == id);
            return View(Account);
        }

        //
        // POST: /Account/Edit/5

        [HttpPost]
        public ActionResult AccountEdit(Account Account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Attach(Account);
                db.ObjectStateManager.ChangeObjectState(Account, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("AccountEdit");
            }
            return View(Account);
        }

        //
        // GET: /Account/Delete/5

        public ActionResult AccountDelete(int id)
        {
            Account Account = db.Accounts.Single(c => c.AccountId == id);
            return View(Account);
        }

        //
        // POST: /Account/Delete/5

        [HttpPost, ActionName("AccountDelete")]
        public ActionResult AccountDeleteConfirmed(int id)
        {
            Account Account = db.Accounts.Single(c => c.AccountId == id);
            db.Accounts.DeleteObject(Account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region  Order
        //
        // GET: /Order/

        public ViewResult OrderList()
        {
            var orders = db.Orders.Include("Account");
            return View(orders.ToList());
        }

        //
        // GET: /Order/Details/5

        public ViewResult OrderDetails(int id)
        {
            Order order = db.Orders.Single(o => o.OrderId == id);
            return View(order);
        }

        //
        // GET: /Order/Create

        public ActionResult OrderCreate()
        {
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "LoginName");
            return View();
        }

        //
        // POST: /Order/Create

        [HttpPost]
        public ActionResult OrderCreate(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.AddObject(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "LoginName", order.AccountId);
            return View(order);
        }

        //
        // GET: /Order/Edit/5

        public ActionResult OrderEdit(int id)
        {
            Order order = db.Orders.Single(o => o.OrderId == id);
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "LoginName", order.AccountId);
            return View(order);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        public ActionResult OrderEdit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Attach(order);
                db.ObjectStateManager.ChangeObjectState(order, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "LoginName", order.AccountId);
            return View(order);
        }

        //
        // GET: /Order/Delete/5

        public ActionResult OrderDelete(int id)
        {
            Order order = db.Orders.Single(o => o.OrderId == id);
            return View(order);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("OrderDelete")]
        public ActionResult OrderDeleteConfirmed(int id)
        {
            Order order = db.Orders.Single(o => o.OrderId == id);
            db.Orders.DeleteObject(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Size
        //
        // GET: /Size/

        public ViewResult SizeList()
        {
            return View(db.Sizes.ToList());
        }

        //
        // GET: /Size/Details/5

        public ViewResult SizeDetails(int id)
        {
            Size size = db.Sizes.Single(s => s.SizeId == id);
            return View(size);
        }

        //
        // GET: /Size/Create

        public ActionResult SizeCreate()
        {
            return View();
        }

        //
        // POST: /Size/Create

        [HttpPost]
        public ActionResult SizeCreate(Size size)
        {
            if (ModelState.IsValid)
            {
                db.Sizes.AddObject(size);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(size);
        }

        //
        // GET: /Size/Edit/5

        public ActionResult SizeEdit(int id)
        {
            Size size = db.Sizes.Single(s => s.SizeId == id);
            return View(size);
        }

        //
        // POST: /Size/Edit/5

        [HttpPost]
        public ActionResult SizeEdit(Size size)
        {
            if (ModelState.IsValid)
            {
                db.Sizes.Attach(size);
                db.ObjectStateManager.ChangeObjectState(size, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(size);
        }

        //
        // GET: /Size/Delete/5

        public ActionResult SizeDelete(int id)
        {
            Size size = db.Sizes.Single(s => s.SizeId == id);
            return View(size);
        }

        //
        // POST: /Size/Delete/5

        [HttpPost, ActionName("SizeDelete")]
        public ActionResult SizeDeleteConfirmed(int id)
        {
            Size size = db.Sizes.Single(s => s.SizeId == id);
            db.Sizes.DeleteObject(size);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
