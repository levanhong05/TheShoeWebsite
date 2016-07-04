using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheShoeWebsite.Models
{
    public partial class ShoppingCart
    {
        BanGiayEntities db = new BanGiayEntities();
        string ShoppingCartId { get; set; }
        public const string CartSesstionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }


        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product)
        {
            // Get the matching cart and Product instances
            var cartItem = db.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.ProductId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductId = product.ProductId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.AddToCarts(cartItem);
            }
            else
            {
                //Tang so luong  len 1 
                //Neu da co trong gio hang
                cartItem.Count++;
            }
            // Save changes
            db.SaveChanges();
        }

        public void UpdateQuantity(Product product, int quantity)
        {
            // Get the matching cart and Product instances
            var cartItem = db.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.ProductId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductId = product.ProductId,
                    CartId = ShoppingCartId,
                    Count = quantity,
                    DateCreated = DateTime.Now
                };
                db.AddToCarts(cartItem);
            }
            else
            {
                cartItem.Count += quantity;
            };
            db.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = db.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.DeleteObject(cartItem);
                }
                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.DeleteObject(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal()
        {

            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Product.Price).Sum();


            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;


            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    CurrentPrice = item.Product.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                //db.OrderDetails.AddObject(orderDetail);
                db.AddToOrderDetails(orderDetail);
                orderTotal += (item.Count * item.Product.Price);

            }
           // order.Total = orderTotal;
            db.SaveChanges();
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
        // We're using HttpContextBase to allow access to cookies.

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                //if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                //{
                //    context.Session[CartSessionKey] =
                //        context.User.Identity.Name;
                //}
                //else
                //{
                // Generate a new random GUID using System.Guid class
                Guid tempCartId = Guid.NewGuid();
                // Send tempCartId back to client as a cookie
                context.Session[CartSessionKey] = tempCartId.ToString();
                //}
            }
            return context.Session[CartSessionKey].ToString();
        }

        //Khi khách hàng login vào
        //Tự động cập nhật Mã khách Hàng vào AccountId

        public void UpdateAccountIntoCart(int _AccountID)
        {
            var shoppingCart = db.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.AccountId = _AccountID;
            }
            db.SaveChanges();
        }

        public static string CartSessionKey { get; set; }
    }

}

