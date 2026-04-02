using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiInAsp.netcoreMvcDemo.Controllers
{
    public class EmployeeUIController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrWhiteSpace(username))
            {
                context.Result = RedirectToAction("Login", "AuthenticationUI");
                return;
            }

            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Export()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
