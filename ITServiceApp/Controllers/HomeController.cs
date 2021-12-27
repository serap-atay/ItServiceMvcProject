using ITServiceApp.InjectOrnek;
using Microsoft.AspNetCore.Mvc;

namespace ITServiceApp.Controllers
{
    public class HomeController : Controller
    {
    
        public IActionResult Index()
        {
         
            return View();
        }
    }
}
