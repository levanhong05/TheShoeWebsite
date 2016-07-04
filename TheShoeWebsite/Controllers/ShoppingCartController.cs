using System.Linq;
using System.Web.Mvc;
using TheShoeWebsite.Models;
using TheShoeWebsite.ViewModels;

namespace TheShoeWebsite.Controllers
{
    public class ShoppingCartController : Controller
    {
        BanGiayEntities db = new BanGiayEntities();

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public ActionResult AddToCart(int id)
        {
            //Lay San Pham theo ID
            var addProduct = db.Products.Single(p => p.ProductId == id);


            // Lay gio hang hien tai
            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (Session["AccountID"] != null)
            {
                cart.AddToCart(addProduct);
                cart.UpdateAccountIntoCart(int.Parse(Session["AccountID"].ToString()));

            }
            else { cart.AddToCart(addProduct); };

            // Chay toi trang gio hang
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Xóa Sản phẩm khỏi giỏ hàng
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Lấy tên sản phẩm để xuất ra trong thông báo xóa
            string productId = db.Carts.Single(p => p.RecordId == id).Product.ProductId.ToString();


            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message =
                    " Sản phẩm vừa mới được xóa khỏi giỏ hàng.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }

        //Http Post
        //Tang so luong san pham 
        [HttpPost]
        public ActionResult UpdateQuantity(int Id = 1, int quantity = 5)
        {
            //Lay san pham
            var ProductIncrease = db.Products.Single(p => p.ProductId == Id);
            // Lay Gio Hang
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.UpdateQuantity(ProductIncrease, quantity);
            return RedirectToAction("Index");
        }

        //
        // GET: /ShoppingCart/CartSummary

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            ViewBag.CartSum = cart.GetTotal();

            return PartialView("CartSummary");
        }
    }
}