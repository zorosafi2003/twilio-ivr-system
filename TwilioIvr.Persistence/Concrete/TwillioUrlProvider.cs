using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using TwilioIvr.Persistence.Abstruct;
using TwilioIvr.Persistence.Models;

namespace TwilioIvr.Persistence.Concrete
{
    public class TwillioUrlProvider : ITwillioUrlProvider
    {
        private string ngRokUrl;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly LinkGenerator _linkGenerator;
        private readonly IConfiguration _configuration;

        public TwillioUrlProvider(IHttpContextAccessor HttpContextAccessor, IHostingEnvironment HostingEnvironment,
           LinkGenerator LinkGenerator, IConfiguration Configuration)
        {
            _httpContextAccessor = HttpContextAccessor;
            _hostingEnvironment = HostingEnvironment;
            _linkGenerator = LinkGenerator;
            _configuration = Configuration;

            ngRokUrl = _configuration["NgRokUrl"].ToString();
        }
        public Uri ReturnFunctionUrl(ReturnFunctionUrlModel model)
        {
            var controller = model.ControllerName == null ? _httpContextAccessor.HttpContext.GetRouteValue("controller").ToString() : model.ControllerName;
            var area = model.AreaName == null ? _httpContextAccessor.HttpContext.GetRouteValue("area")?.ToString() : model.AreaName;

            var values = new
            {
                area,
                digits = model.Digits,
                dto = model.DtoModel == null ? null : JsonConvert.SerializeObject(model.DtoModel)
            };

            var urlByAction = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, model.FunctionName, controller, values);

            if (_hostingEnvironment.IsDevelopment())
            {
                urlByAction = urlByAction.Replace("localhost", ngRokUrl.Replace("http://", ""));

                if (_httpContextAccessor.HttpContext.Request.Host.Port != null)
                {
                    urlByAction = urlByAction.Replace($":{_httpContextAccessor.HttpContext.Request.Host.Port}", "");
                }

            }

            var url = new Uri(urlByAction);

            if (model.QueryParams?.Count > 0)
            {

                url = this.AddCustomHeaders(url, model.QueryParams);
            }

            return url;
        }
        public void ReturnAudioFile(Gather gather, string message, string audioFileName, string dir)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                gather.Say(message);
            }
            else
            {
                var fileUrl = new Url(Url.Combine(this.GetHostUrl().url, dir, audioFileName)).SetQueryParam("Guid", Guid.NewGuid().ToString());
                gather.Play(new Uri(fileUrl));
            }
        }
        public void ReturnAudioFile(VoiceResponse response, string message, string audioFileName, string dir)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                response.Say(message);
            }
            else
            {
                var fileUrl = new Url(Url.Combine(this.GetHostUrl().url, dir, audioFileName)).SetQueryParam("Guid", Guid.NewGuid().ToString());
                response.Play(new Uri(fileUrl));
            }
        }

        private Uri AddCustomHeaders(Uri uri, List<KeyValuePair<string, string>> queryStringList)
        {
            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var queryString in queryStringList)
            {
                query[queryString.Key] = queryString.Value;
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }
        private dynamic GetHostUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var scheme = request.Scheme;
            var host = string.Empty; ;

            if (_hostingEnvironment.IsEnvironment("Test"))
            {
                host = request.Host.Value;
            }
            else
            {
                host = (request.Host.Value.Contains("localhost") ? ngRokUrl : request.Host.Value);
            }

            return new { url = $"{scheme}://{host}", host };
        }
    }
}
