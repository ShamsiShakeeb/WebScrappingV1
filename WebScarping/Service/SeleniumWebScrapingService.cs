using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebScarping.Model;

namespace WebScarping.Service
{
    public class SeleniumWebScrapingService : ISeleniumWebScrapingService
    {
        public (bool success, string message, List<IWebElement> webElements, ChromeDriver driver) Scrapping(string uniqueSiteTitle, Dictionary<string, string> customTags, List<string> navigateRules = null)
        {
            try
            {
                StreamReader r = new StreamReader("../ScarpingSiteInformation.json");
                string jsonString = r.ReadToEnd();
                var webScrapingModel = JsonConvert.DeserializeObject<List<WebScrapingModel>>(jsonString);
                var model = webScrapingModel.Where(x => x.UniqueTitle == uniqueSiteTitle)
                    .Select(x => x).FirstOrDefault();

                new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());

                ChromeOptions options = new ChromeOptions();
                //options.AddArguments("-headless");

                var driver = new ChromeDriver(options);

                driver.Navigate().GoToUrl(model.LoginLink);

                PerfromAction(new ModelMapper().HtmlMapper(model.LoginInfo), driver);

                driver.Navigate().GoToUrl(model.NavigateGetRequest ? string.Format(model.NavigationLink,
                     string.Join(",", navigateRules.ToArray()).Split(','))
                    : model.NavigationLink);

                PerfromAction(new ModelMapper().HtmlMapper(DynamicField(model.CustomProperties, customTags)), driver);

                PerfromAction(new ModelMapper().HtmlMapper(model.Elements), driver);

                PerfromAction(new ModelMapper().HtmlMapper(model.ActionButtons), driver);

                var htmlTableElements = driver.FindElements(By.CssSelector(model.QueryCssSelector))
                    .ToList();

                return (true, "data fatch successful", htmlTableElements, driver);
            }
            catch (Exception e)
            {
                return (false, e.Message.ToString(), null, null);
            }
        }

        private List<CustomProperties> DynamicField(List<CustomProperties> customProperties,
           Dictionary<string, string> customTags)
        {
            List<CustomProperties> dynamicProperties = new List<CustomProperties>();

            foreach (var tag in customTags)
            {
                var cutomModel = customProperties.Where(x => x.VariableName == tag.Key)
                    .Select(x => x).FirstOrDefault();

                if (cutomModel != null)
                {
                    cutomModel.Value = cutomModel.HtmlTag.Equals("datepicker") ?
                        Convert.ToDateTime(tag.Value).ToString(cutomModel.DateFormat) : tag.Value;

                    dynamicProperties.Add(cutomModel);
                }
            }

            return dynamicProperties;
        }

        private void PerfromAction(List<HtmlJsonPropsModel> model, ChromeDriver driver)
        {
            foreach (var props in model)
            {
                if (props.Key.ToLower().Equals("executescript"))
                {
                    driver.ExecuteScript(props.Value);
                }

                else if(props.HtmlTag.ToLower().Equals("button") && props.GetElementBy.ToLower().Equals("queryselector"))
                {
                    driver.FindElements(By.CssSelector(props.Key))[Convert.ToInt32(props.Value)].Click();
                }

                else if (props.HtmlTag.ToLower().Equals("button") || props.HtmlTag.ToLower().Equals("checkbox"))
                {
                    var by = props.GetElementBy.ToLower().Equals("name") ?
                        By.Name(props.Key) : By.Id(props.Key);
                    if (by != null)
                    {
                        var findEle = driver.FindElement(by);
                        findEle.Click();
                    }
                }

                else if (props.HtmlTag.ToLower().Equals("select"))
                {

                    var by = props.GetElementBy.ToLower().Equals("name") ?
                        By.Name(props.Key) : By.Id(props.Key);

                    if (by != null)
                    {

                        var selectBox = driver.FindElement(by);
                        var selectEle = new SelectElement(selectBox);
                        selectEle.SelectByValue(props.Value);
                    }
                }

                else
                {
                    var by = props.GetElementBy.ToLower().Equals("name") ?
                        By.Name(props.Key) : By.Id(props.Key);

                    if (by != null)
                    {
                        var findEle = driver.FindElement(by);
                        findEle.SendKeys(props.Value);
                    }
                }

            }
        }
    }
}
