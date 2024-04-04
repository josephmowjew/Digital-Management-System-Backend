using Microsoft.AspNetCore.Mvc;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
