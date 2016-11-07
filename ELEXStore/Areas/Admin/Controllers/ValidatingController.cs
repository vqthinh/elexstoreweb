using System.Linq;
using System.Web.Mvc;

namespace ELEXStore.Areas.Admin.Controllers
{
    public class ValidatingController : BaseController
    {
        public JsonResult IsSupplierExists(string Id)
        {
            var model = new { valid = !db.Suppliers.Any(x => x.ID == Id) };
            return Json(model, JsonRequestBehavior.AllowGet);
        }  
    }
}
