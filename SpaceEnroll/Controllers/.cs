using Microsoft.AspNetCore.Mvc;

namespace SpaceEnroll.Controllers
{
    public class CoderController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
