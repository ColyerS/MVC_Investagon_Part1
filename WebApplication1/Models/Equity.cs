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
    
    public partial class Equity
    {
        public int EquityId { get; set; }
        public float change { get; set; }
        public float changeOverTime { get; set; }
        public float changePercent { get; set; }
        public float close { get; set; }
        public string date { get; set; }
        public float high { get; set; }
        public string label { get; set; }
        public float low { get; set; }
        public float open { get; set; }
        public string symbol { get; set; }
        public int unadjustedVolume { get; set; }
        public int volume { get; set; }
        public float vwap { get; set; }
    
        public virtual Company Company { get; set; }
    }
}
