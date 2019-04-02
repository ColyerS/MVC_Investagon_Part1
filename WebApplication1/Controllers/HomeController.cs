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

        public FinanceSiteEntities1 db = new FinanceSiteEntities1();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

      

        //DASHBOARD _ GRAB NEW DATA FOR A SYMBOL, UPDATE DATA FOR SYMBOL.
        public ActionResult DashboardUpdate()
        {
            Symbol model = new Symbol();


            return View();
        }

        //public ActionResult Search(string term)
        //{
        //    var names = db.Symbol.Where(p => p.SymbolName.Contains(term)).Select(p => p.SymbolName).ToList();
        //    return Json(names, JsonRequestBehavior.AllowGet);
        //}

        //test run program when submit button is entered on DashboardUpdate
        [HttpPost]
        public ActionResult DashboardUpdate(Symbol model)
        {
            //Get symbol model for user lookup.
            var symbolid = db.Symbol
                    .Where(b => b.Symbol1 == model.Symbol1)
                    .FirstOrDefault();

            //Input Chart data into chart table from API based on symbol
            List<Chart> chartList = WriteChartSymbol(symbolid);
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[Chart]");
            foreach (Chart cL in chartList)
            {
                db.Charts.Add(cL);
                db.SaveChanges();
            }

            //Input Quote data into quote table from API based on symbol
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[Quote]");
            Quote quoteTest = WriteQuote(symbolid);
            db.Quotes.Add(quoteTest);
            db.SaveChanges();

            //Input Earnings data into earnings table from API based on symbol
            List<Earning> earningList = WriteEarnings(symbolid);
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[Earnings]");
            foreach (Earning eL in earningList)
            {
                db.Earnings.Add(eL);
                db.SaveChanges();
            }
            //Input Financials data into financials table from API based on symbol
            List<Financial> financialList = WriteFinancials(symbolid);
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[Financials]");
            foreach (Financial fL in financialList)
            {
                db.Financials.Add(fL);
                db.SaveChanges();
            }
            
            //persist data for next request
            TempData["SymbolInfo"] = symbolid;

            return RedirectToAction("Dashboard", symbolid.SymbolId);
            //when user inputs the 
            //return View("Dashboard");
        }

        [HttpGet]
        public ActionResult Dashboard(string symbolInt)
        {
            //check that symbol is not null
            if (TempData["SymbolInfo"] != null)
            {
                var symbolid = TempData["SymbolInfo"] as Symbol;
                //Get Quote Detail
                var quote1 = (from a in db.Quotes
                                  where a.SymbolRef == symbolid.SymbolId
                                  select a).FirstOrDefault();
                
                //Get Financial Detail
                var financialsL = (from a in db.Financials
                               where a.SymbolRef == symbolid.SymbolId
                               select a).ToList();

                //Get Result Exam marks detail as per student ID  
                var earningsL = (from a in db.Earnings
                              where a.SymbolRef == symbolid.SymbolId
                              select a).ToList();
                
                //Create a list of dates and points for the chart to consume.
                var chartL = (from a in db.Charts
                 where a.SymbolRef == symbolid.SymbolId
                 select a).ToList();

                List<int> chartPoints = new List<int>();
                foreach (Chart p in chartL)
                {
                    chartPoints.Add(Convert.ToInt32(p.ClosePrice));
                }

                List<string> chartDates = new List<string>();
                foreach (Chart p in chartL)
                {
                    DateTime dt = Convert.ToDateTime(p.ReportDate);
                    chartDates.Add(dt.ToString("MM-dd-yy"));
                }

                ViewBag.DataPoints = JsonConvert.SerializeObject(chartPoints);
                ViewBag.DataLabels = JsonConvert.SerializeObject(chartDates);

                //Output set to ViewModel  
                var model = new ResultViewModel { _financials = financialsL, _quote = quote1 , _earnings = earningsL};

                return View(model);
            }
            else //error if there is not symbol input.
            {
                return View("error");
            }
         
        }

        //Get quote for current symbol.
        public static Quote WriteQuote(Symbol symbol)
        {
            Quote quoteTest = new Quote();
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/quote?displayPercent=true";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol.Symbol1);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    dynamic historicalData  = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    
                        Quote quoteTest2 = new Quote();
                        quoteTest2.Sector = historicalData.sector;
                        quoteTest2.ReportDate = Convert.ToDateTime(historicalData.latestTime);
                        quoteTest2.LatestPrice = Convert.ToDecimal(historicalData.latestPrice);
                        quoteTest2.Symbol = Convert.ToString(historicalData.symbol);
                        quoteTest2.Symbol1 = symbol;
                        quoteTest2.SymbolRef = symbol.SymbolId;

                        quoteTest = quoteTest2;

                        return quoteTest;
                }
                
            }
            return quoteTest;
        }

 
        //Chart Table update.
        public static List<Chart> WriteChartSymbol(Symbol symbol)
        {
            List<Chart> symbolChart = new List<Chart>();
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/chart/1m";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol.Symbol1);

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
                        Chart stockTest = new Chart();
                        stockTest.Symbol1 = symbol;
                        stockTest.SymbolRef = symbol.SymbolId;
                        stockTest.Symbol = symbol.Symbol1;
                        stockTest.ReportDate = Convert.ToDateTime(historicalData.date);
                        stockTest.ClosePrice= Convert.ToDecimal(historicalData.close);

                        symbolChart.Add(stockTest);
                        //private FinanceSiteEntities dbAPIController = new FinanceSiteEntities();


                        // dbAPIController.StockHistories.Add(stockTest);
                        //dbAPIController.SaveChanges();

                    }
                    return symbolChart;
                }
                else if (!(response.IsSuccessStatusCode))
                {
                    return symbolChart;
                }
                return symbolChart;
            }
        }

        //Earnings table update.
        public static List<Earning> WriteEarnings(Symbol symbol)
        {
            List<Earning> symbolEarning = new List<Earning>();
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/earnings";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol.Symbol1);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    dynamic historicalData = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    
                    foreach (dynamic s in JsonConvert.DeserializeObject<List<dynamic>>(Convert.ToString(historicalData.earnings)))
                    {
                        Earning stockE = new Earning();
                        stockE.Symbol1 = symbol;
                        stockE.SymbolRef = symbol.SymbolId;
                        stockE.Symbol = symbol.Symbol1;
                        stockE.FiscalPeriod = Convert.ToString(s.fiscalPeriod);
                        stockE.ActualEPS = Convert.ToDecimal(s.actualEPS);

                        symbolEarning.Add(stockE);
                    }

                       
                    return symbolEarning;
                }
                else if (!(response.IsSuccessStatusCode))
                {
                    return symbolEarning;
                }
                return symbolEarning;
            }
        }

        //Financials table update.
        public static List<Financial> WriteFinancials(Symbol symbol)
        {
            List<Financial> symbolFinancial = new List<Financial>();
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/financials";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol.Symbol1);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    dynamic historicalData = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                    foreach (dynamic s in JsonConvert.DeserializeObject<List<dynamic>>(Convert.ToString(historicalData.financials)))
                    {
                        Financial stockF = new Financial();
                        stockF.Symbol1 = symbol;
                        stockF.SymbolRef = symbol.SymbolId;
                        stockF.Symbol = symbol.Symbol1;
                        stockF.ShareholderEquity = Convert.ToDecimal(s.shareholderEquity);
                        stockF.ReportDate = Convert.ToDateTime(s.reportDate);

                        symbolFinancial.Add(stockF);
                    }


                    return symbolFinancial;
                }
                else if (!(response.IsSuccessStatusCode))
                {
                    return symbolFinancial;
                }
                return symbolFinancial;
            }
        }




        public void AddSymbol()
        {
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/ref-data/symbols";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH);

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
                        
                        Symbol symbolTest = new Symbol();
                        symbolTest.Symbol1 = Convert.ToString(historicalData.symbol);
                        symbolTest.SymbolName = Convert.ToString(historicalData.name);

                        db.Symbol.Add(symbolTest);
                        db.SaveChanges();
                    

                    }

                }
            }
        }
        ///////////////////////////
        //public ActionResult SignUp()
        //{
        //    ViewBag.Message = "Sign up form.";

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult SignUp(Profile profile)
        //{
        //    return View(profile);
        //}

        ////public List<Lifestyle> GetLifestyles()
        ////{
        ////    List<Lifestyle> lifestylelist = new List<Lifestyle>();
        ////    lifestylelist = db.Lifestyles.ToList();
        ////    foreach (Lifestyle l in db.Lifestyles.ToList())
        ////    {

        ////    }
        ////    return lifestylelist;
        ////}

        ////test create record in Profile table
        ////public ActionResult createProfile()
        ////{
        ////    Profile profile = new Profile();
        ////    profile.userName = "csigety";
        ////    profile.password = "nothing";
        ////    profile.gender = "f";
        ////    profile.firstName = "Colyer";
        ////    profile.lastName = "Sigety";
        ////    profile.lifestyle = "binger";

        ////    db.Profiles.Add(profile);
        ////    db.SaveChanges();

        ////    return View();
        ////}

        //public ActionResult Login()
        //{
        //    Profile profile = new Profile();

        //    return View(profile);
        //}


        //[HttpPost]
        //public ActionResult Login(Profile profile)
        //{

        //    return View(profile);
        //    //when user inputs the 
        //    //return View("Dashboard");
        //}

    }
}

