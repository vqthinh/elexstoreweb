using System.Linq;
using System.Web.Mvc;
using Data.DAO;
using ELEXStore.Models;
using ELEXStore.Common;

namespace ELEXStore.Controllers
{
    public class CartController : Controller
    {
        ProductsDao dao = new ProductsDao();
        //
        // GET: /Cart/
        private const string CartSession = "CartSession";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
        public JsonResult DeleteAllCart()
        {
            CommonConstants.listCart.Clear();
            Session[CartSession] = CommonConstants.listCart;
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            CommonConstants.listCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = CommonConstants.listCart;
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddItem(int productID, int quantity)
        {
            var product = new ProductsDao().GetByID(productID);
            if (CommonConstants.listCart.Exists(x => x.Product.ID == productID))
            {
                foreach (var item in CommonConstants.listCart)
                {
                    if (item.Product.ID == productID)
                    {
                        item.Quantity += quantity;
                    }
                }
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem
                {
                    Product = product,
                    Quantity = quantity
                };
                CommonConstants.listCart.Add(item);
            }
            //Gán vào session
            Session[CartSession] = CommonConstants.listCart;
            var info =
                new
                {
                    Sum = CommonConstants.listCart.Sum(x => x.Quantity),
                    Total = CommonConstants.listCart.Sum(x => x.Quantity * (x.Product.Price - x.Product.Price * x.Product.Discount / 100))
                };

            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult checkQuantity(int productID, int quantity)
        {
            var countCart = 0;
            var itemCart = CommonConstants.listCart.FirstOrDefault(x => x.Product.ID == productID);
            countCart = itemCart != null ? itemCart.Quantity : 0;

            var countProduct = dao.countProduct(productID);
            return Json(countCart + quantity > countProduct, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _listOrder()
        {
            return PartialView(CommonConstants.listCart);
        }
        public PartialViewResult _listCheckOut()
        {
            return PartialView(CommonConstants.listCart);
        }
        #region Checkout

        public ActionResult CheckOut()
        {
            return View();
        }

        #endregion

        
    }
}
