using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScarping.Service
{
    public interface ISeleniumWebScrapingService
    {
        (bool success, string message, List<IWebElement> webElements, ChromeDriver driver) Scrapping(string uniqueSiteTitle, Dictionary<string, string> customTags,
            List<string> navigateRules = null);
    }
}
