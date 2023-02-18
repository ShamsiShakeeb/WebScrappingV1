using Microsoft.AspNetCore.Http;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebScarping.Model;
using WebScarping.Service;

namespace WebScarping.Factory
{
    public class SeleniumWebScrapingFactory : ISeleniumWebScrapingFactory
    {
        private ISeleniumWebScrapingService _seleniumWebScrapingService;
        private static string PAX = "";

        public SeleniumWebScrapingFactory(ISeleniumWebScrapingService seleniumWebScrapingService)
        {
            _seleniumWebScrapingService = seleniumWebScrapingService;
        }

        public ResponseModel GetHtmlElementFromFlyNovoAirSite(string uniqueTitle, FlyNovoAirRequestModel airModel)
        {
            var form = new Dictionary<string, string>();

            form["StartDate"] = airModel.StartDate;
            form["EndDate"] = airModel.EndDate;

            var htmlElement = _seleniumWebScrapingService.Scrapping(uniqueSiteTitle: uniqueTitle,
                customTags: IFromToDictonary(form));

            if (!htmlElement.success)
            {
                return (new ResponseModel
                {
                    success = htmlElement.success,
                    message = htmlElement.message,
                    data = null
                });
            }

            try
            {

                var baseSelector = htmlElement.webElements[6].FindElements(By.CssSelector("tr"))
                    .ToList();

                List<FlyNovoAirFetchModel> flyNovoAirFetchModels = new List<FlyNovoAirFetchModel>();

                for (int i = 1; i < baseSelector.Count; i++)
                {
                    var tableRow = baseSelector[i].FindElements(By.CssSelector("td"));

                    var model = new FlyNovoAirFetchModel();
                    bool outerTable = false;
                    if (tableRow.Count == 14)
                    {
                        outerTable = true;
                        if (tableRow[5].Text.ToString().Contains("Base Fare"))
                        {
                            break;
                        }

                        model.IssuingDate = tableRow[0].Text;
                        model.StafCode = tableRow[1].Text;
                        model.StafName = tableRow[2].Text;
                        model.TransactionStatus = tableRow[3].Text;
                        model.PNR = tableRow[4].Text;
                        model.BaseFair = tableRow[5].Text;
                        model.Surcharges = tableRow[6].Text;
                        model.Tax = tableRow[7].Text;
                        model.Fees = tableRow[8].Text;
                        model.TotalPrice = tableRow[9].Text;
                        model.Commission = tableRow[10].Text;
                        model.CommissionTax = tableRow[11].Text;
                        model.DebitToAccount = tableRow[12].Text;
                        model.DepositePayment = tableRow[13].Text;
                    }

                    var innerTable = new InnerTable();
                    List<InnerTable> innerTables = new List<InnerTable>();

                    if (outerTable)
                    {

                        for (int j = i + 1; j < baseSelector.Count; j++)
                        {

                            var InnerTableRow = baseSelector[j].FindElements(By.CssSelector("td"));

                            if (InnerTableRow.Count == 14)
                                break;

                            if (InnerTableRow.Count == 12)
                            {
                                if (string.IsNullOrEmpty(innerTable.Pax))
                                {
                                    innerTable.Pax = PAX;
                                }
                                innerTable.AirlinceCode = InnerTableRow[1].Text;
                                innerTable.TransactionNumber = InnerTableRow[2].Text;
                                innerTable.EmployeeCodeNo = InnerTableRow[3].Text;
                                innerTable.FlyForm = InnerTableRow[4].Text;
                                innerTable.FlyTo = InnerTableRow[5].Text;
                                innerTable.IssuingDate = InnerTableRow[6].Text;
                                innerTable.BaseFair = InnerTableRow[7].Text;
                                innerTable.Surcharges = InnerTableRow[8].Text;
                                innerTable.Tax = InnerTableRow[9].Text;
                                innerTable.Fees = InnerTableRow[10].Text;
                                innerTable.TotalPrice = InnerTableRow[11].Text;

                                innerTables.Add(innerTable);
                                innerTable = new InnerTable();
                            }
                            else if (InnerTableRow.Count == 6 && InnerTableRow[5].Text.ToString().Equals(" "))
                            {
                                innerTable.Pax = InnerTableRow[0].Text;
                                PAX = InnerTableRow[0].Text;
                            }

                        }
                        model.InnerTables = innerTables;
                        flyNovoAirFetchModels.Add(model);
                    }
                }

                htmlElement.driver.Close();

                var flyNovoModel = flyNovoAirFetchModels;
                List<DataPassingModel> dataPassingModels = new List<DataPassingModel>();

                for (int i = 0; i < flyNovoModel.Count; i++)
                {
                    var innerTable = flyNovoModel[i].InnerTables;

                    if (innerTable.Count == 0)
                    {
                        var model = new DataPassingModel();
                        model.PNR = flyNovoModel[i].PNR;
                        model.IssuingDate = flyNovoModel[i].IssuingDate;
                        model.StaffCode = flyNovoModel[i].StafCode;
                        model.StafeName = flyNovoModel[i].StafName;
                        model.amountDetails.BaseFair = Convert.ToDouble(flyNovoModel[i].BaseFair.Split(" ")[1].Replace(",", ""));
                        model.amountDetails.TotalTax = Convert.ToDouble(flyNovoModel[i].Tax.Split(" ")[1]);
                        model.amountDetails.SupplierStdCommission = Convert.ToDouble(flyNovoModel[i].BaseFair.Split(" ")[1].Replace(",", ""))
                            * (model.amountDetails.SupplierStdCommissionPercentage / 100);
                    }

                    for (int k = 0; k < innerTable.Count; k++)
                    {
                        var model = new DataPassingModel();
                        model.PNR = flyNovoModel[i].PNR;
                        model.IssuingDate = flyNovoModel[i].IssuingDate;
                        model.StaffCode = flyNovoModel[i].StafCode;
                        model.StafeName = flyNovoModel[i].StafName;
                        model.amountDetails.BaseFair = Convert.ToDouble(flyNovoModel[i].BaseFair.Split(" ")[1].Replace(",", ""));
                        model.amountDetails.TotalTax = Convert.ToDouble(flyNovoModel[i].Tax.Split(" ")[1]);
                        model.amountDetails.SupplierStdCommission = Convert.ToDouble(flyNovoModel[i].BaseFair.Split(" ")[1].Replace(",", ""))
                            * (model.amountDetails.SupplierStdCommissionPercentage / 100);

                        model.TicketNumber = innerTable[k].TransactionNumber.Split("/")[0];
                        model.AirlinceCode = innerTable[k].AirlinceCode;
                        model.Pax = innerTable[k].Pax;
                        model.Sector = innerTable[k].TransactionNumber.Split("/")[1].Equals("1") ? innerTable[k].FlyForm + "/" + innerTable[k].FlyTo
                            : innerTable[k].FlyForm + "/" + innerTable[k].FlyTo + "/" + innerTable[k].FlyForm;
                        model.Tax = Convert.ToDouble(innerTable[k].Tax);
                        model.BaseFair = Convert.ToDouble(innerTable[k].BaseFair.Replace(",", ""));
                        model.TicketStatus = innerTable[k].TransactionNumber.Split("/")[1].Equals("1") ? "One Way" : "Return Way";
                        model.amountDetails.SupplierAit = (model.BaseFair + model.Tax) * (model.amountDetails.SupplierAitPercentage / 100);
                        dataPassingModels.Add(model);

                    }

                }

                return (new ResponseModel
                {
                    success = dataPassingModels.Count == 0 ? false : true,
                    message = dataPassingModels.Count == 0 ? "No Data Found" : "Data Serialization Successfull",
                    data = dataPassingModels.Count == 0 ? null : dataPassingModels
                });

            }
            catch (Exception e)
            {
                return (new ResponseModel
                {
                    success = false,
                    message = "Data Serialization Not Successfull " + e.Message,
                    data = null
                });
            }
        }

        public ResponseModel GetHtmlElementFromZenithSite(string uniqueTitle, ZenithRequestModel airModel)
        {
            List<string> getPropertyParms = new List<string>();

            getPropertyParms.Add(Convert.ToDateTime(airModel.StartDate).ToString("dd/MM/yyyy"));
            getPropertyParms.Add(Convert.ToDateTime(airModel.EndDate).ToString("dd/MM/yyyy"));

            Dictionary<string, string> form = new Dictionary<string, string>();
            form["StartDate"] = airModel.StartDate;
            form["EndDate"] = airModel.EndDate;

            var fetch = _seleniumWebScrapingService.Scrapping(uniqueSiteTitle: uniqueTitle, customTags: IFromToDictonary(form)
                , navigateRules: getPropertyParms);

            if (!fetch.success)
            {
                return (new ResponseModel
                {
                    success = fetch.success,
                    message = fetch.message,
                    data = null
                });
            }

            try
            {

                var tableRow = fetch.webElements;

                List<ZenithFetchModel> zenithFetchModels = new List<ZenithFetchModel>();

                for (int i = 0; i < tableRow.Count; i++)
                {
                    var tableData = tableRow[i].FindElements(By.CssSelector("td")).ToList();

                    if (!tableData.Any())
                    {
                        break;
                    }
                    var model = new ZenithFetchModel();

                    for (int j = 0; j < tableData.Count; j++)
                    {
                        model = new ZenithFetchModel()
                        {
                            Report = tableData[0].Text,
                            PNR = tableData[1].Text,
                            TicketNumber = tableData[2].Text,
                            IssuingDate = tableData[3].Text,
                            Description = tableData[4].Text,
                            Statement = tableData[5].Text,
                            SaleWithoutTax = tableData[6].Text,
                            Tax = tableData[7].Text,
                            CreditNote = tableData[8].Text,
                            AmountIncomeTax = tableData[9].Text,
                            ReportStatus = tableData[10].Text,
                            IssuerLoginName = tableData[11].Text,
                        };
                    }
                    zenithFetchModels.Add(model);
                }
                fetch.driver.Close();
                List<DataPassingModel> dataPassingModels = new List<DataPassingModel>();
                for (int i = 0; i < zenithFetchModels.Count; i++)
                {
                    var model = new DataPassingModel();
                    model.PNR = zenithFetchModels[i].PNR.Split("\r\n")[0];
                    model.AirlinceCode = zenithFetchModels[i].TicketNumber.Substring(0, 3);
                    model.TicketNumber = zenithFetchModels[i].TicketNumber.Substring(4);
                    model.IssuingDate = zenithFetchModels[i].IssuingDate;
                    model.TicketStatus = zenithFetchModels[i].Statement;
                    model.BaseFair = string.IsNullOrEmpty(zenithFetchModels[i].SaleWithoutTax) ? 0 :
                        Convert.ToDouble(zenithFetchModels[i].SaleWithoutTax.Replace(",", ""));
                    model.Tax = string.IsNullOrEmpty(zenithFetchModels[i].Tax) ? 0 :
                        Convert.ToDouble(zenithFetchModels[i].SaleWithoutTax.Replace(",", ""));
                    model.amountDetails.SupplierStdCommission = string.IsNullOrEmpty(zenithFetchModels[i].AmountIncomeTax) ? 0 :
                        Convert.ToDouble(zenithFetchModels[i].AmountIncomeTax.Replace(" BDT", ""));
                    model.StaffCode = PreProcess('(', zenithFetchModels[i].IssuerLoginName, ')');
                    var staffName = zenithFetchModels[i].IssuerLoginName.Split('/')[1].Split(' ');
                    model.StafeName = staffName[0] + " " + staffName[1];
                    var sectorValidation = zenithFetchModels[i].Description.Split('>');
                    model.Sector = zenithFetchModels[i].Description.Contains("[OW]") ?
                                      sectorValidation[0].Substring(sectorValidation[0].Length - 4) + "/"
                                    + sectorValidation[1].Substring(0, 3) : sectorValidation[0].Substring(sectorValidation[0].Length - 4) + "/"
                                    + sectorValidation[1].Substring(0, 3) + "/" + sectorValidation[0].Substring(sectorValidation[0].Length - 4);
                    model.Pax = zenithFetchModels[i].Description.Contains("[OW]") ?
                        PreProcess('[', zenithFetchModels[i].Description.Split(" [OW] ")[1], ']')
                        : PreProcess('[', zenithFetchModels[i].Description.Split(" [RT] ")[1], ']');

                    model.amountDetails.SupplierAit = model.amountDetails.SupplierAit = (model.BaseFair + model.Tax) * (model.amountDetails.SupplierAitPercentage / 100);
                    model.amountDetails.SupplierStdCommission = model.BaseFair * (model.amountDetails.SupplierAitPercentage / 100);

                    dataPassingModels.Add(model);
                }

                return (new ResponseModel
                {
                    success = zenithFetchModels.Count == 0 ? false : true,
                    message = zenithFetchModels.Count == 0 ? "No Data Found" : "Data Serialization Successfull",
                    data = dataPassingModels
                });
            }
            catch (Exception e)
            {
                return (new ResponseModel
                {
                    success = false,
                    message = "Data Serialization Not Successfull " + e.Message,
                    data = null
                });
            }
        }

        public ResponseModel GetHtmlElementFromJazeeraAirSite(string uniqueTitle, ZenithRequestModel airModel)
        {
            List<string> getPropertyParms = new List<string>();

            getPropertyParms.Add(Convert.ToDateTime(airModel.StartDate).ToString("dd/MM/yyyy"));
            getPropertyParms.Add(Convert.ToDateTime(airModel.EndDate).ToString("dd/MM/yyyy"));

            Dictionary<string, string> form = new Dictionary<string, string>();
            form["StartDate"] = airModel.StartDate;
            form["EndDate"] = airModel.EndDate;

            var fetch = _seleniumWebScrapingService.Scrapping(uniqueSiteTitle: uniqueTitle, customTags: IFromToDictonary(form)
                , navigateRules: getPropertyParms);

            return null;
        }

        private Dictionary<string, string> IFromToDictonary(Dictionary<string,string> form)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var frm in form)
            {
                dict[frm.Key] = frm.Value;
            }

            return dict;
        }
        private string PreProcess(char sunfix, string value, char prefix)
        {
            char[] a = value.ToCharArray();
            string x = "";
            bool svr = false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == prefix) break;
                if (a[i] == sunfix)
                {
                    svr = true;

                }
                if (svr)
                    x += a[i];
            }
            return x.Replace(sunfix + "", "");
        }
    }
}
