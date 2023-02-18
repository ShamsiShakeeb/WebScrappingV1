using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScarping.Model
{
    public class ZenithFetchModel
    {
        public string Report { set; get; }
        public string PNR { set; get; }
        public string TicketNumber { set; get; }
        public string IssuingDate { set; get; }
        public string Description { set; get; }
        public string Statement { set; get; }
        public string SaleWithoutTax { set; get; }
        public string Tax { set; get; }
        public string CreditNote { set; get; }
        public string AmountIncomeTax { set; get; }
        public string ReportStatus { set; get; }
        public string IssuerLoginName { set; get; }
    }
}
