//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PortfolioStock
    {
        public int portfolioStockId { get; set; }
        public string portfolioName { get; set; }
        public string stockSymbol { get; set; }
        public string stockName { get; set; }
        public decimal percent { get; set; }
        public Nullable<decimal> currentPrice { get; set; }
    }
}