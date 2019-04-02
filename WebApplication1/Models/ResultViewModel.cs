using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ResultViewModel
    {

            //Charts model  
            public List<Chart> _charts { get; set; }

            //Financials  
            public List<Financial> _financials { get; set; }

            //Quote
            public Quote _quote { get; set; }

            //Earnings
            public List<Earning> _earnings { get; set; }
    }
}