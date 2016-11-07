using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.DAO;

namespace ELEXStore.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        SupplierDao supplierDao = new SupplierDao();
        ProductsDao productsDao = new ProductsDao();
        CategoriesDao categoriesDao = new CategoriesDao();
        public ActionResult Index()
        {
            @ViewBag.Supplier = supplierDao.ListAll();
            @ViewBag.SpecialProducts = productsDao.ListSpecial();
            @ViewBag.TopViews = productsDao.ListTopViews().Take(3).Skip(0);
            @ViewBag.TopSells = productsDao.ListTopSells().Take(3).Skip(0);
            @ViewBag.News = productsDao.ListNews().Take(3).Skip(0);
            
            return View();
        }

        public PartialViewResult _Menu()
        {
            var list = categoriesDao.ListAll();
            return PartialView(list);
        }

        public PartialViewResult _CartLayout()
        {
            return PartialView();
        }

    }
}
