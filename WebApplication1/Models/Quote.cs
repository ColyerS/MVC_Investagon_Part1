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
    
    public partial class Quote
    {
        public int QuoteId { get; set; }
        public Nullable<decimal> LatestPrice { get; set; }
        public Nullable<System.DateTime> ReportDate { get; set; }
        public string Sector { get; set; }
        public string Symbol { get; set; }
        public Nullable<int> SymbolRef { get; set; }
    
        public virtual Symbol Symbol1 { get; set; }
    }
}
