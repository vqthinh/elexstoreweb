using System.Web.Mvc;
using Data.DAO;
using ELEXStore.Areas.Admin.Models;
using ELEXStore.Common;
namespace ELEXStore.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Admin/Login/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UsersDao();
                var result = dao.Login(model.UserName, Md5Encryptor.Md5Hash(model.Password));
                if (result==0)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession =  new UserLogin();
                    userSession.UserName = user.UserName;
                    Session.Add(CommonConstants.USER_SESSION,userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 1)
                {
                    ModelState.AddModelError("", "Username is not exist");
                    TempData["AlertMessage"] = "Username is not exist";
                    TempData["AlertType"] = "alert-danger";
                }
                else
                {
                    ModelState.AddModelError("", "Wrong Password");
                    TempData["AlertMessage"] = "Wrong password";
                    TempData["AlertType"] = "alert-danger";
                }
            }
            return View("Index");
        }

        public ActionResult LogOut()
        {
            Session.Remove(CommonConstants.USER_SESSION);
            return RedirectToAction("Index", "Login");
        }
    }
}
