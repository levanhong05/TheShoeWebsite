using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheShoeWebsite.Models;
using System.Data.Objects;

namespace TheShoeWebsite.Controllers
{
    public class CheckoutController : Controller
    {
        BanGiayEntities db = new BanGiayEntities();

        //
        // GET: /Checkout/AddressAndPayment
        [Authorize]
        public ActionResult SendOrder()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [Authorize]
        [HttpPost]
        public ActionResult SendOrder(SendOrderModel sendOrderModel)
        {

            Order order = new Order();
            int cusID = int.Parse(Session["AccountID"].ToString());
            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (ModelState.IsValid)
            {
                TryUpdateModel(order);
                if (cusID != 0)
                    order.AccountId = cusID;
                order.FullName = sendOrderModel.FullName;
                order.OrderDate = DateTime.Now;
                order.PhoneNumber = sendOrderModel.PhoneNumber;
                order.Total = cart.GetTotal();
                
                db.Orders.AddObject(order);
                db.SaveChanges();
                cart.CreateOrder(order);
                return
                    RedirectToAction("Complete",
                             new { id = order.OrderId });
            }
            return View(order);
        }

        //
        // GET: /Checkout/Complete

        public ActionResult Complete(int id)
        {
            // Validate Account owns this order
            int cusID = int.Parse(Session["AccountID"].ToString());
            bool isValid = db.Orders.Any(
                o => o.OrderId == id &&
                o.Account.AccountId == cusID);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

    }
}
