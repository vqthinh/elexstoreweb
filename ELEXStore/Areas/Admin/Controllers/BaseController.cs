using System.Web.Mvc;
using System.Web.Routing;
using Data.Models;
using ELEXStore.Common;

namespace ELEXStore.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public ELEXStoreModels db = new ELEXStoreModels();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new{ controller="Login",action="Index",Area ="Admin"}));
            }
            base.OnActionExecuting(filterContext);
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-error";
            }
        }

    }
}
