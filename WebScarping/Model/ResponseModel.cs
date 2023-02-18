using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScarping.Model
{
    public class ResponseModel
    {
        public bool success { set; get; }
        public string message { set; get; }
        public List<DataPassingModel> data { set; get; }
    }
}
