using Microsoft.AspNetCore.Mvc;

namespace JobPortalMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult LoginForm()
        {
            return View();
        }
    }
}
