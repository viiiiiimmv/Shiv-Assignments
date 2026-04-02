using Microsoft.AspNetCore.Mvc;

namespace WebApiInAsp.netcoreMvcDemo.Controllers;

public class AuthenticationUIController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult SaveSession(string username)
    {
        HttpContext.Session.SetString("username", username);
        return Ok();
    }
}