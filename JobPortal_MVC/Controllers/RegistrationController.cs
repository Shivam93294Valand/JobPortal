using Microsoft.AspNetCore.Mvc;

namespace JobPortalMVC.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult RegistrationForm()
        {
            return View();
        }
    }
}
