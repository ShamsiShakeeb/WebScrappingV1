using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScarping.Model
{
    public class FlyNovoAirFetchModel
    {
        public string IssuingDate { set; get; }

        public string StafCode { set; get; }
        public string StafName { set; get; }
        public string TransactionStatus { set; get; }
        public string PNR { set; get; }
        public string BaseFair { set; get; }
        public string Surcharges { set; get; }
        public string Tax { set; get; }
        public string Fees { set; get; }
        public string TotalPrice { set; get; }
        public string Commission { set; get; }
        public string CommissionTax { set; get; }
        public string DebitToAccount { set; get; }
        public string DepositePayment { set; get; }

        public List<InnerTable> InnerTables { set; get; }

    }
}
public class InnerTable
{
    public string Pax { set; get; }
    public string AirlinceCode { set; get; }
    public string TransactionNumber { set; get; }
    public string EmployeeCodeNo { set; get; }
    public string FlyForm { set; get; }
    public string FlyTo { set; get; }
    public string IssuingDate { set; get; }
    public string BaseFair { set; get; }
    public string Surcharges { set; get; }
    public string Tax { set; get; }
    public string Fees { set; get; }
    public string TotalPrice { set; get; }
}
