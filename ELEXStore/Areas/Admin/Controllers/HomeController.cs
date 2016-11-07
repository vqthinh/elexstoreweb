using System.Web.Mvc;
using Data.DAO;

namespace ELEXStore.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Admin/Home/
        StatisticDao dao = new StatisticDao();
        public ActionResult Index()
        {
            var st = dao.Statistic();
            ViewBag.Views = st.Views;
            ViewBag.Orders = st.Orders;
            ViewBag.Solds = st.Solds;
            ViewBag.Profit = st.Profit;
            return View();
        }

    }
}
