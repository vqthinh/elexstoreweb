using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Data.DAO;
using Data.Models;
using Microsoft.Ajax.Utilities;

namespace ELEXStore.Controllers
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/
        ProductsDao dao = new ProductsDao();

        public ActionResult Details(int id)
        {
            var product = dao.GetByID(id);
            dao.IncreaseViews(id);
            ViewBag.RandomProducts = dao.ListRandomProducts();
            ViewBag.RelatedProducts = dao.ListRelatedProducts(product.CategoryID);
            return View(product);
        }

        [HttpPost]
        public ActionResult SearchResult(string SearchResult)
        {
            if (!SearchResult.IsNullOrWhiteSpace())
            {
                var id = Convert.ToInt32(SearchResult);
                return RedirectToAction("Details", new {Controller = "Products", id});
            }
            return RedirectToAction("Shop", "Products");
        }

        public ActionResult Category(int id, int page = 1, int pageSize = 10)
        {
            var product = dao.ListProductsByCategory(id, pageSize, page);
            return View(product);
        }

        public ActionResult Shop(int page = 1, int pageSize = 6)
        {
            var product = dao.ListAllPaging(pageSize, page);
            ViewBag.RandomProducts = dao.ListRandomProducts();
            return View(product);
        }

        [HttpGet]
        public JsonResult SearchProducts(string term = "")
        {
            var list = dao.SearchByName(term);
            //var ser = new JavaScriptSerializer(); 
            //var tests = ser.Serialize(list); 
            return Json(list, JsonRequestBehavior.AllowGet);
            ;
        }

        public PartialViewResult _SearchBox()
        {
            return PartialView();
        }

        public PartialViewResult _RandomProducts()
        {
            var list = dao.ListRandomProducts();
            return PartialView(list);
        }
    }
}
