using System.Collections.Generic;
using System.Web;
using ELEXStore.Models;
using Microsoft.Ajax.Utilities;

namespace ELEXStore.Common
{
    public static class CommonConstants
    {
        public static string USER_SESSION = "USER_SESSION";

        public static List<CartItem> listCart
        {
            get
            {
                var cart = HttpContext.Current.Session["CartSession"] as List<CartItem>;
                if (cart == null)
                {
                    cart = new List<CartItem>();
                    HttpContext.Current.Session["CartSession"] = cart;
                }
                return cart;
            }
            set
            {
                
            }
        }
    }
}