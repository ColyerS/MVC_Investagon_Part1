using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Controllers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Dynamic;

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

        public ActionResult SignUp()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Lifestyle = db.Lifestyles.ToList();
            //mymodel.Students = GetStudents();
            return View(mymodel);
        }

        //public List<Lifestyle> GetLifestyles()
        //{
        //    List<Lifestyle> lifestylelist = new List<Lifestyle>();
        //    lifestylelist = db.Lifestyles.ToList();
        //    foreach (Lifestyle l in db.Lifestyles.ToList())
        //    {

        //    }
        //    return lifestylelist;
        //}

        //test create record in Profile table
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
        public ActionResult Login(Profile profile)
        {

            return View(profile);
            //when user inputs the 
            //return View("Dashboard");
        }

        //DASHBOARD _ GRAB NEW DATA FOR A SYMBOL, UPDATE DATA FOR SYMBOL.
        public ActionResult DashboardUpdate()
        {
            StockHistory model = new StockHistory();

            return View();
        }

        //test run program when submit button is entered on DashboardUpdate
        [HttpPost]
        public ActionResult DashboardUpdate(StockHistory model)
        {
            List<StockHistory> stockHistoryList = WriteChartSymbol(model.stockSymbol);
            foreach (StockHistory sh in stockHistoryList)
            {
                db.StockHistories.Add(sh);
                db.SaveChanges();
            }

            return View();
            //when user inputs the 
            //return View("Dashboard");
        }


        public static string mostactiveUrl = "https://api.iextrading.com/1.0/stock/market/list/mostactive";
        public static string gainersUrl = "https://api.iextrading.com/1.0/stock/market/list/gainers";
        public static string losersUrl = "https://api.iextrading.com/1.0/stock/market/list/losers";
        public static string MostVolumeUrl = "https://api.iextrading.com/1.0/stock/market/list/iexvolume";
        public static string PercentChangeUrl = "https://api.iextrading.com/1.0/stock/market/list/iexpercent";
        public static string infocusUrl = "https://api.iextrading.com/1.0/stock/market/list/infocus";
        public static string OneDayListUrl = "https://api.iextrading.com/1.0/stock/{0}/chart/1d";
        public static string sectorPerfUrl = "https://api.iextrading.com/1.0/stock/market/sector-performance";
        public static string topsUrl = "https://api.iextrading.com/1.0/tops";

        public static List<StockHistory> WriteChartSymbol(string symbol)
        {
            List<StockHistory> symbolHistory = new List<StockHistory>();
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/chart/1m";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    foreach (dynamic historicalData in JsonConvert.DeserializeObject<List<dynamic>>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult()))
                    {
                        //Console.WriteLine("Open: " + historicalData.open);
                        //Console.WriteLine("Close: " + historicalData.close);
                        //Console.WriteLine("Low: " + historicalData.low);
                        //Console.WriteLine("High: " + historicalData.high);
                        //Console.WriteLine("Change: " + historicalData.change);
                        //Console.WriteLine("Change Percentage: " + historicalData.changePercent);
                        StockHistory stockTest = new StockHistory();
                        stockTest.stockSymbol = symbol;
                        stockTest.stockDate = Convert.ToDateTime(historicalData.date);
                        stockTest.stockPrice = Convert.ToDecimal(historicalData.close);

                        symbolHistory.Add(stockTest);
                        //private FinanceSiteEntities dbAPIController = new FinanceSiteEntities();


                        // dbAPIController.StockHistories.Add(stockTest);
                        //dbAPIController.SaveChanges();

                    }
                    return symbolHistory;
                }
                else if (!(response.IsSuccessStatusCode))
                {
                    return symbolHistory;
                }
                return symbolHistory;
            }
        }

        public static string quoteUrl = "https://api.iextrading.com/1.0/stock/{0}/quote?displayPercent=true";
        public static List<PortfolioStock> QuoteSymbol(List<string> symbols)
        {
            List<PortfolioStock> stockQuote = new List<PortfolioStock>();
            var IEXTrading_API_PATH = quoteUrl;
            foreach (string symbol in symbols)
            {
                IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    //For IP-API
                    client.BaseAddress = new Uri(IEXTrading_API_PATH);
                    HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        foreach (dynamic quoteData in JsonConvert.DeserializeObject<List<dynamic>>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult()))
                        {
                            //Console.WriteLine("Open: " + historicalData.open);
                            //Console.WriteLine("Close: " + historicalData.close);
                            //Console.WriteLine("Low: " + historicalData.low);
                            //Console.WriteLine("High: " + historicalData.high);
                            //Console.WriteLine("Change: " + historicalData.change);
                            //Console.WriteLine("Change Percentage: " + historicalData.changePercent);
                            PortfolioStock stockTest = new PortfolioStock();
                            stockTest.stockSymbol = symbol;
                            stockTest.stockName = Convert.ToString(quoteData.companyName);
                            stockTest.currentPrice = Convert.ToDecimal(quoteData.latestPrice);

                            stockQuote.Add(stockTest);

                        }
                        return stockQuote;
                    }
                    else if (!(response.IsSuccessStatusCode))
                    {
                        return stockQuote;
                    }
                    
                }
               
            }
            return stockQuote;
        }

        ///////////////////////////
    }
}

