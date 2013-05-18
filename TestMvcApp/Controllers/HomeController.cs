using System.Web.Mvc;
using Localization.Core;
using TestMvcApp.Models;
using Localization;

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
                Description = _localizer.Translate("Identité utilisée pour désigner une hypothétique personne de profil moyen représentant la société dans laquelle elle vit ({0}).", "wikipedia")
            });
        }

        /// <summary>
        /// Illustrate another way...
        /// </summary>
        /// <returns></returns>
        public ActionResult Index2()
        {
            return View("Index", new TestViewModel()
            {
                Name = "John",
                Lastname = "Doe",
                // this.Translate requires Localization namespace (extension method)
                // StringProvider<HomeController> _localizer is no more needed!
                Description = this.Translate("Identité utilisée pour désigner une hypothétique personne de profil moyen représentant la société dans laquelle elle vit ({0}).", "wikipedia")
            });
        }

        /// <summary>
        /// Illustrate another way...
        /// </summary>
        /// <returns></returns>
        public ActionResult Index3()
        {
            return View("Index", new TestViewModel()
            {
                Name = "John",
                Lastname = "Doe",
                // "".Translate requires Localization namespace (extension method)
                // StringProvider<HomeController> _localizer is no more needed!
                // Available from anywhere (not only a Controller)...
                Description = "Identité utilisée pour désigner une hypothétique personne de profil moyen représentant la société dans laquelle elle vit ({0}).".Translate(this, "wikipedia")
            });
        }

    }
}
