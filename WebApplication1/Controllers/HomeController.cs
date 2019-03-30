using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private FinanceSiteEntities db = new FinanceSiteEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult createProfile()
        {
            Profile profile = new Profile();
            profile.userName = "csigety";
            profile.password = "nothing";
            profile.gender = "f";
            profile.firstName = "Colyer";
            profile.lastName = "Sigety";
            profile.lifestyle = "binger";

            db.Profiles.Add(profile);
            db.SaveChanges();

            return View();
        }

        public ActionResult Login()
        {
            Profile profile = new Profile();

            return View(profile);
        }

        [HttpPost]
        public ActionResult Login( Profile profile)
        {

            return View(profile);
            //when user inputs the 
            //return View("Dashboard");
        }

        public void getQuote() {

            //IEXTradingDotNetCore.STOCK_QUOTE.Const_STOCK_QUOTE;

        }


        

    }
}