using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioIvr.Persistence.Models
{
   public class IvrBaseModel
    {
        public string Digits { get; set; }
        public string Speach { get; set; }
        public string Dto { get; set; }
        public UrlInfoChildOfIvrBaseModel NextActionUrl { get; set; }
        public UrlInfoChildOfIvrBaseModel BackActionUrl { get; set; }
        public IDictionary<string, UrlInfoChildOfIvrBaseModel> CasesUrl { get; set; }

        public T GetDtoModel<T>()
        {
            if (!string.IsNullOrEmpty(Dto))
            {
                return JsonConvert.DeserializeObject<T>(Dto);
            }
            return default;
        }

        public class UrlInfoChildOfIvrBaseModel
        {
            public string Digits { get; set; }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }
            public string AreaName { get; set; }
            public int? TenantId { get; set; }
        }
    }
}
