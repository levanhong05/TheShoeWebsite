using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheShoeWebsite.Models;
using System.Web.Mvc;

namespace TheShoeWebsite.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public decimal TotalByProduct { get; set; }

    }
}