using System;
using System.Web.Mvc;
using Data.DAO;
using Data.Models;
using Microsoft.Ajax.Utilities;

namespace ELEXStore.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        ProductsDao dao = new ProductsDao();

        public ActionResult Index(int pageSize = 3, int page = 1)
        {
            var model = dao.ListAllPaging(pageSize, page);
            return View(model);
        }
        [ValidateInput(false)]
        public ActionResult Edit(int id)
        {   
            var model = dao.GetByID(id);
            SetViewBagCategory(model.CategoryID);
            SetViewBagSupplier(model.SupplierID);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (!product.Image.IsNullOrWhiteSpace() && product.Image.Contains("/"))
                {
                    product.Image = splitImage(Request["Image"]);
                }
                if (dao.Update(product))
                {
                    SetAlert("Edit Product success", "success");
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    SetAlert("Edit Failed", "error");
                }
            }
            SetViewBagSupplier(product.SupplierID);
            SetViewBagCategory(product.CategoryID);
            return View(product);
        }

        public string splitImage(string link)
        {
            var tmp = link.Substring(link.LastIndexOf("/", StringComparison.Ordinal));
            tmp = tmp.Replace("/", "");
            return tmp;
        }

        public ActionResult Create()
        {
            SetViewBagSupplier();
            SetViewBagCategory();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (!product.Image.IsNullOrWhiteSpace() && product.Image.Contains("/"))
                {
                    product.Image = splitImage(Request["Image"]);
                }

                if (dao.Insert(product))
                {
                    SetAlert("Add Product success", "success");
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    ModelState.AddModelError("", "Insert Failed");
                }
            }
            SetViewBagSupplier();
            SetViewBagCategory();
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (dao.Delete(id))
            {
                SetAlert("Product deleted", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Delete failed", "error");
            }
            return RedirectToAction("Index");
        }

        public void SetViewBagSupplier(string selected=null)
        {
            var daoS = new SupplierDao();
            ViewData["SupplierID"] = new SelectList(daoS.ListAll(),"ID","Name",selected);
         
        }

        public void SetViewBagCategory(int? selected = null)
        {
            var daoC = new CategoriesDao();
            ViewData["CategoryID"] = new SelectList(daoC.ListAll(), "ID", "Name", selected);
        }

   
    }
}
