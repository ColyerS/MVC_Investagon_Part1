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
    
    public partial class StockHistory
    {
        public int stockId { get; set; }
        public string stockSymbol { get; set; }
        public System.DateTime stockDate { get; set; }
        public decimal stockPrice { get; set; }
    }
}
