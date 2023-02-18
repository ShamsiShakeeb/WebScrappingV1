using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScarping.Model
{
    public class DataPassingModel
    {
        public string PNR { set; get; } //
        public string AirlinceCode { set; get; } //
        public string TicketNumber { set; get; } //
        public string Sector { set; get; } //
        public string Pax { set; get; } //
        public string StaffCode { set; get; } //
        public string StafeName { set; get; } //
        public string IssuingDate { set; get; } //
        public double BaseFair { set; get; } //
        public double Tax { set; get; } //
        public string TicketStatus { set; get; } //
        public AmountDetails amountDetails { set; get; } = new AmountDetails();
    }

}
public class AmountDetails
{
    public double BaseFair { set; get; }
    public double TotalTax { set; get; }
    public string TaxDetails { set; get; } = "";
    public double SupplierStdCommission { set; get; } = 0;
    public double SupplierStdCommissionPercentage { set; get; } = 7;
    public double SupplierFee { set; get; } = 0;
    public double SupplierAit { set; get; } = 0;
    public double SupplierAitPercentage { set; get; } = 3;
    public double SupplierTotalAmount { set; get; } = 0;
    public double CustomerExtraEarning { set; get; } = 0;
    public double CustomerExtraEarningPercentage { set; get; } = 0;
    public double CustomerServiceFee { set; get; } = 0;
    public double CustomerServiceFeePercentage { set; get; } = 0;
    public double CustomerPaybackAmount { set; get; } = 0;
    public double CustomerOtherCharge { set; get; } = 0;
    public double CustomerOtherChargePercentage { set; get; } = 0;
    public double CustomerAit { set; get; } = 0;
}

