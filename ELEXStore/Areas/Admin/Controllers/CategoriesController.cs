using System;
using System.Web.Mvc;
using Data.DAO;
using Data.Models;
using Microsoft.Ajax.Utilities;

namespace ELEXStore.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        //
        // GET: /Admin/Categories/
        readonly CategoriesDao dao = new CategoriesDao();

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var listCategories = dao.ListAllPaging(page, pageSize);
            return View(listCategories);
        }

        public ActionResult Edit(int id)
        {
            var model = dao.GetById(id);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        public string splitImage(string link)
        {
            var tmp = link.Substring(link.LastIndexOf("/", StringComparison.Ordinal));
            tmp = tmp.Replace("/", "");
            return tmp;
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (!category.Image.IsNullOrWhiteSpace() && category.Image.Contains("/"))
                {
                    category.Image = splitImage(Request["Image"]);
                }
                if (!category.Icon.IsNullOrWhiteSpace() && category.Icon.Contains("/"))
                {
                    category.Icon = splitImage(Request["Icon"]);
                }
               
                if (dao.Insert(category))
                    {
                        SetAlert("Add Category success", "success");
                        return RedirectToAction("Index", "Categories");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Insert Failed");
                    }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (!category.Image.IsNullOrWhiteSpace() && category.Image.Contains("/"))
                {
                    category.Image = splitImage(Request["Image"]);
                }
                if (!category.Icon.IsNullOrWhiteSpace() && category.Icon.Contains("/"))
                {
                    category.Icon = splitImage(Request["Icon"]);
                }
                if (dao.Update(category))
                {
                    SetAlert("Edit Category success","success");
                    return RedirectToAction("Index", "Categories");
                }
                else
                {
                    SetAlert("Edit Failed","error");
                }
            }
            return View(category);
        }
        public ActionResult Delete(int id)
        {
            if (dao.Delete(id))
            {
                SetAlert("Category deleted", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Delete failed","error");
            }
            return RedirectToAction("Index");
        }
    }
}
