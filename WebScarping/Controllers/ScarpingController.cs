using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebScarping.Factory;
using WebScarping.Model;

namespace WebScarping.Controllers
{
    public class ScarpingController : ControllerBase
    {
        private readonly ISeleniumWebScrapingFactory _seleniumWebScrapingFactory;
        public ScarpingController(ISeleniumWebScrapingFactory seleniumWebScrapingFactory)
        {
            _seleniumWebScrapingFactory = seleniumWebScrapingFactory;
        }
        [Route("api/zenith")]
        [HttpPost]
        public IActionResult Zenith([FromBody] ZenithRequestModel model)
        {
            var result = new ResponseModel();

            if (model != null)
            {
                result = _seleniumWebScrapingFactory.GetHtmlElementFromZenithSite("Zenith", model);
            }
            
            return Ok(new { success = (model!=null) , data = result});
        }
        [Route("api/flyNovoAir")]
        [HttpPost]
        public IActionResult FlyNovoAir([FromBody] FlyNovoAirRequestModel model)
        {
            var result = new ResponseModel();
            if (model != null)
            {
                result = _seleniumWebScrapingFactory.GetHtmlElementFromFlyNovoAirSite("Fly Novo Air", model);
            }
            return Ok(new { success = (model != null), data = result });
        }
        [Route("api/JazeeraAir")]
        [HttpPost]
        public IActionResult JazeeraAir([FromBody] FlyNovoAirRequestModel model)
        {
            var result = _seleniumWebScrapingFactory.GetHtmlElementFromFlyNovoAirSite("JazeeraAir", model);
            return Ok();
        }
    }
}
