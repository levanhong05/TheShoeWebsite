using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using TheShoeWebsite.Models;


namespace TheShoeWebsite.Controllers
{
    public class AccountController : Controller
    {

        BanGiayEntities db = new BanGiayEntities();
        BanGiayRepository repo = new BanGiayRepository();
        

        private void UpdateAccountIntoCart(int cusId)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.UpdateAccountIntoCart(cusId);
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
         
            var cus = repo.ValidateAccount(model.LoginName, model.Password);
            if (ModelState.IsValid)
            {
                if (cus != null)
                {
                    UpdateAccountIntoCart(cus.AccountId);
                    Session["AccountID"] = cus.AccountId;
                    FormsAuthentication.SetAuthCookie(cus.AccountId.ToString(), model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Product");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hay mật khẩu không đúng.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff
        [Authorize]
        public ActionResult LogOff()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.EmptyCart();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Product");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                Account cus = new Account();
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.LoginName, model.Password, model.Email, "câu hỏi", "trả lời", true, null, out createStatus);

                cus.LoginName = model.LoginName;
                cus.Password = model.Password;
                cus.Email = model.Email;
                db.AddToAccounts(cus);
                db.SaveChanges();

                UpdateAccountIntoCart(cus.AccountId);
                Session["AccountID"] = cus.AccountId;
                FormsAuthentication.SetAuthCookie(model.LoginName, false /* createPersistentCookie */);
                return RedirectToAction("Index", "Product");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Mật khẩu sai.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Tên đăng nhập đã tồn tại.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Email này đã được sử dụng.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Mật khẩu không hợp lệ.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Email không đúng.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Câu trả lời không hợp lý.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Sai mật khẩu.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Tên đăng nhập không hợp lệ.";

                case MembershipCreateStatus.ProviderError:
                    return "Có lỗi xảy ra trong quá trình đăng ký.Vui lòng liên hệ người quản trị.";

                case MembershipCreateStatus.UserRejected:
                    return "Qúa trình đăng ký bị lỗi.Vui lòng liên hệ quản trị viên.";

                default:
                    return "Có lỗi xảy ra trong quá trình truy xuất.Vui lòng thử lại .Nếu lỗi còn xuất hiện vui lòng liên hệ quản trị.";
            }
        }
        #endregion
    }

}
