using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebScarping.Model;

namespace WebScarping.Service
{
    public class ModelMapper
    {
        public List<HtmlJsonPropsModel> HtmlMapper(List<LoginInfo> jsonProperty)
        {
            List<HtmlJsonPropsModel> htmlJsonPropsModels = new List<HtmlJsonPropsModel>();
            foreach (var props in jsonProperty)
            {
                var data = new HtmlJsonPropsModel()
                {
                    HtmlTag = props.HtmlTag,
                    GetElementBy = props.GetElementBy,
                    Key = props.Key,
                    Value = props.Value
                };
                htmlJsonPropsModels.Add(data);
            }
            return htmlJsonPropsModels;
        }
        public List<HtmlJsonPropsModel> HtmlMapper(List<ActionButtons> jsonProperty)
        {
            List<HtmlJsonPropsModel> htmlJsonPropsModels = new List<HtmlJsonPropsModel>();
            foreach (var props in jsonProperty)
            {
                var data = new HtmlJsonPropsModel()
                {
                    HtmlTag = props.HtmlTag,
                    GetElementBy = props.GetElementBy,
                    Key = props.Key,
                    Value = props.Value
                };
                htmlJsonPropsModels.Add(data);
            }
            return htmlJsonPropsModels;
        }

        public List<HtmlJsonPropsModel> HtmlMapper(List<Elements> jsonProperty)
        {
            List<HtmlJsonPropsModel> htmlJsonPropsModels = new List<HtmlJsonPropsModel>();
            foreach (var props in jsonProperty)
            {
                var data = new HtmlJsonPropsModel()
                {
                    HtmlTag = props.HtmlTag,
                    GetElementBy = props.GetElementBy,
                    Key = props.Key,
                    Value = props.Value
                };
                htmlJsonPropsModels.Add(data);
            }
            return htmlJsonPropsModels;
        }
        public List<HtmlJsonPropsModel> HtmlMapper(List<CustomProperties> jsonProperty)
        {
            List<HtmlJsonPropsModel> htmlJsonPropsModels = new List<HtmlJsonPropsModel>();
            foreach (var props in jsonProperty)
            {
                var data = new HtmlJsonPropsModel()
                {
                    HtmlTag = props.HtmlTag,
                    GetElementBy = props.GetElementBy,
                    Key = props.Key,
                    Value = props.Value
                };
                htmlJsonPropsModels.Add(data);
            }
            return htmlJsonPropsModels;
        }
    }
}
