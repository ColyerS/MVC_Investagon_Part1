using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class APIController : Controller
    {

        public static string mostactiveUrl = "https://api.iextrading.com/1.0/stock/market/list/mostactive";
        public static string gainersUrl = "https://api.iextrading.com/1.0/stock/market/list/gainers";
        public static string losersUrl = "https://api.iextrading.com/1.0/stock/market/list/losers";
        public static string MostVolumeUrl = "https://api.iextrading.com/1.0/stock/market/list/iexvolume";
        public static string PercentChangeUrl = "https://api.iextrading.com/1.0/stock/market/list/iexpercent";
        public static string infocusUrl = "https://api.iextrading.com/1.0/stock/market/list/infocus";
        public static string OneDayListUrl = "https://api.iextrading.com/1.0/stock/{0}/chart/1d";
        public static string sectorPerfUrl = "https://api.iextrading.com/1.0/stock/market/sector-performance";
        public static string topsUrl = "https://api.iextrading.com/1.0/tops";

        public static void Main()
        {
            var symbol = "msft";
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/chart/1y";

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
                        Console.WriteLine("Open: " + historicalData.open);
                        Console.WriteLine("Close: " + historicalData.close);
                        Console.WriteLine("Low: " + historicalData.low);
                        Console.WriteLine("High: " + historicalData.high);
                        Console.WriteLine("Change: " + historicalData.change);
                        Console.WriteLine("Change Percentage: " + historicalData.changePercent);
                    }
                }
            }
        }
    }


    public class HistoricalDataResponse
    {
        public string date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public int volume { get; set; }
        public int unadjustedVolume { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double vwap { get; set; }
        public string label { get; set; }
        public double changeOverTime { get; set; }
    }

    public class MostActive
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public string sector { get; set; }
        public string calculationPrice { get; set; }
        public double open { get; set; }
        public object openTime { get; set; }
        public double close { get; set; }
        public object closeTime { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public object latestUpdate { get; set; }
        public int latestVolume { get; set; }
        public double iexRealtimePrice { get; set; }
        public int iexRealtimeSize { get; set; }
        public object iexLastUpdated { get; set; }
        public double delayedPrice { get; set; }
        public object delayedPriceTime { get; set; }
        public double extendedPrice { get; set; }
        public int extendedChange { get; set; }
        public int extendedChangePercent { get; set; }
        public object extendedPriceTime { get; set; }
        public double previousClose { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double iexMarketPercent { get; set; }
        public int iexVolume { get; set; }
        public int avgTotalVolume { get; set; }
        public double iexBidPrice { get; set; }
        public int iexBidSize { get; set; }
        public double iexAskPrice { get; set; }
        public int iexAskSize { get; set; }
        public object marketCap { get; set; }
        public double peRatio { get; set; }
        public double week52High { get; set; }
        public double week52Low { get; set; }
        public double ytdChange { get; set; }
    }


    public class Gainers
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public string sector { get; set; }
        public string calculationPrice { get; set; }
        public double open { get; set; }
        public object openTime { get; set; }
        public double close { get; set; }
        public object closeTime { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public object latestUpdate { get; set; }
        public int latestVolume { get; set; }
        public double iexRealtimePrice { get; set; }
        public int iexRealtimeSize { get; set; }
        public object iexLastUpdated { get; set; }
        public double delayedPrice { get; set; }
        public object delayedPriceTime { get; set; }
        public double extendedPrice { get; set; }
        public double extendedChange { get; set; }
        public double extendedChangePercent { get; set; }
        public object extendedPriceTime { get; set; }
        public double previousClose { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double iexMarketPercent { get; set; }
        public int iexVolume { get; set; }
        public int avgTotalVolume { get; set; }
        public double iexBidPrice { get; set; }
        public int iexBidSize { get; set; }
        public double iexAskPrice { get; set; }
        public int iexAskSize { get; set; }
        public object marketCap { get; set; }
        public double? peRatio { get; set; }
        public double week52High { get; set; }
        public double week52Low { get; set; }
        public double ytdChange { get; set; }
    }


    public class TodayPrice
    {
        public string date { get; set; }
        public string minute { get; set; }
        public string label { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double average { get; set; }
        public int volume { get; set; }
        public double notional { get; set; }
        public int numberOfTrades { get; set; }
        public double marketHigh { get; set; }
        public double marketLow { get; set; }
        public double marketAverage { get; set; }
        public int marketVolume { get; set; }
        public double marketNotional { get; set; }
        public int marketNumberOfTrades { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public double marketOpen { get; set; }
        public double marketClose { get; set; }
        public double? changeOverTime { get; set; }
        public double marketChangeOverTime { get; set; }
    }

    public class Losers
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public string sector { get; set; }
        public string calculationPrice { get; set; }
        public double open { get; set; }
        public object openTime { get; set; }
        public double close { get; set; }
        public object closeTime { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public object latestUpdate { get; set; }
        public int latestVolume { get; set; }
        public double? iexRealtimePrice { get; set; }
        public int? iexRealtimeSize { get; set; }
        public object iexLastUpdated { get; set; }
        public double delayedPrice { get; set; }
        public object delayedPriceTime { get; set; }
        public double extendedPrice { get; set; }
        public double extendedChange { get; set; }
        public double extendedChangePercent { get; set; }
        public object extendedPriceTime { get; set; }
        public double previousClose { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double? iexMarketPercent { get; set; }
        public int? iexVolume { get; set; }
        public int avgTotalVolume { get; set; }
        public double? iexBidPrice { get; set; }
        public int? iexBidSize { get; set; }
        public double? iexAskPrice { get; set; }
        public int? iexAskSize { get; set; }
        public int marketCap { get; set; }
        public double? peRatio { get; set; }
        public double week52High { get; set; }
        public double week52Low { get; set; }
        public double ytdChange { get; set; }
    }



    public class HighestVolume
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public string sector { get; set; }
        public string calculationPrice { get; set; }
        public double open { get; set; }
        public object openTime { get; set; }
        public double close { get; set; }
        public object closeTime { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public object latestUpdate { get; set; }
        public int latestVolume { get; set; }
        public double iexRealtimePrice { get; set; }
        public int iexRealtimeSize { get; set; }
        public object iexLastUpdated { get; set; }
        public double delayedPrice { get; set; }
        public object delayedPriceTime { get; set; }
        public double extendedPrice { get; set; }
        public int extendedChange { get; set; }
        public int extendedChangePercent { get; set; }
        public object extendedPriceTime { get; set; }
        public double previousClose { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double iexMarketPercent { get; set; }
        public int iexVolume { get; set; }
        public int avgTotalVolume { get; set; }
        public double iexBidPrice { get; set; }
        public int iexBidSize { get; set; }
        public double iexAskPrice { get; set; }
        public int iexAskSize { get; set; }
        public object marketCap { get; set; }
        public double? peRatio { get; set; }
        public double week52High { get; set; }
        public double week52Low { get; set; }
        public double ytdChange { get; set; }
    }

    public class PercentChange
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public string sector { get; set; }
        public string calculationPrice { get; set; }
        public double open { get; set; }
        public object openTime { get; set; }
        public double close { get; set; }
        public object closeTime { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public object latestUpdate { get; set; }
        public int latestVolume { get; set; }
        public double iexRealtimePrice { get; set; }
        public int iexRealtimeSize { get; set; }
        public object iexLastUpdated { get; set; }
        public double delayedPrice { get; set; }
        public object delayedPriceTime { get; set; }
        public double extendedPrice { get; set; }
        public double extendedChange { get; set; }
        public double extendedChangePercent { get; set; }
        public object extendedPriceTime { get; set; }
        public double previousClose { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double iexMarketPercent { get; set; }
        public int iexVolume { get; set; }
        public int avgTotalVolume { get; set; }
        public double iexBidPrice { get; set; }
        public int iexBidSize { get; set; }
        public double iexAskPrice { get; set; }
        public int iexAskSize { get; set; }
        public object marketCap { get; set; }
        public double? peRatio { get; set; }
        public double week52High { get; set; }
        public double week52Low { get; set; }
        public double ytdChange { get; set; }
    }



}