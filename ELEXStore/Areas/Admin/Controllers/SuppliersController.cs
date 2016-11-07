using System;
using System.Web.Mvc;
using Data.DAO;
using Data.Models;
using Microsoft.Ajax.Utilities;

namespace ELEXStore.Areas.Admin.Controllers
{
    public class SuppliersController : BaseController
    {
        SupplierDao dao = new SupplierDao();

        public ActionResult Index(int pageSize = 10 ,int page =1)
        {
            var model = dao.ListAllPaging(pageSize, page);
            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var model = dao.GetByID(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if (!supplier.Logo.IsNullOrWhiteSpace() && supplier.Logo.Contains("/"))
                {
                    supplier.Logo = splitImage(Request["Logo"]);
                }
                if (dao.Update(supplier))
                {
                    SetAlert("Edit Supplier success", "success");
                    return RedirectToAction("Index", "Suppliers");
                }
                else
                {
                    SetAlert("Edit Failed", "error");
                }
            }
            return View(supplier);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if (!supplier.Logo.IsNullOrWhiteSpace() && supplier.Logo.Contains("/"))
                {
                    supplier.Logo = splitImage(Request["Logo"]);
                }
           
                if (dao.Insert(supplier))
                {
                    SetAlert("Add Supplier success", "success");
                    return RedirectToAction("Index", "Suppliers");
                }
                else
                {
                    ModelState.AddModelError("", "Insert Failed");
                }
            }
            return View();
        }
        public string splitImage(string link)
        {
            var tmp = link.Substring(link.LastIndexOf("/", StringComparison.Ordinal));
            tmp = tmp.Replace("/", "");
            return tmp;
        }

        public ActionResult Delete(string id)
        {
            if (dao.Delete(id))
            {
                SetAlert("Supplier deleted", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Delete failed", "error");
            }
            return RedirectToAction("Index");
        }
    }
}
