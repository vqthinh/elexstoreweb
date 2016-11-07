using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.DAO;

namespace ELEXStore.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        //
        // GET: /Admin/Orders/
        OrdersDao dao = new OrdersDao();
        public ActionResult Index(int pageSize = 10, int page = 1)
        {
            var orders = dao.ListAllPaging(pageSize, page);
            return View(orders);
        }

        public ActionResult ChangeStatus(int id, int pageSize = 10, int page = 1)
        {
            var order = dao.GetByID(id);
            if (order.Status == 0)
            {
                order.Status = 1;  
            }
            else
            {
                order.Status = 0;
            }
            
            dao.UpdateStatus(order);
            return RedirectToAction("Index");
        }

        public ActionResult OrdersDetails(int Id)
        {
            var order = dao.GetByID(Id);
            ViewBag.OrderDetails = dao.getOrderDetails(Id);
            ViewBag.Total = dao.getTotal(Id);
            return View(order);
        }

        public ActionResult Delete(int id)
        {
            if (dao.Delete(id))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
