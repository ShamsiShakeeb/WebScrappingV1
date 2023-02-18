using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebScarping.Model;

namespace WebScarping.Factory
{
    public interface ISeleniumWebScrapingFactory
    {
        ResponseModel GetHtmlElementFromFlyNovoAirSite(string uniqueTitle, FlyNovoAirRequestModel model);
        ResponseModel GetHtmlElementFromZenithSite(string uniqueTitle, ZenithRequestModel model);
        ResponseModel GetHtmlElementFromJazeeraAirSite(string uniqueTitle, ZenithRequestModel airModel);
    }
}
