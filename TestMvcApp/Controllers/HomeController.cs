using System.Web.Mvc;
using Localization.Core;
using TestMvcApp.Models;

namespace TestMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly StringProvider<HomeController> _localizer;

        public HomeController(StringProvider<HomeController> localizer)
        {
            _localizer = localizer;
        }
        
        public ActionResult Index()
        {
            return View(new TestViewModel() 
            {
                Name = "John", 
                Lastname = "Doe",
                Description = _localizer.Translate("Identité utilisée pour désigner une hypothétique personne de profil moyen représentant la société dans laquelle elle vit (wikipedia).")
            });
        }

    }
}
