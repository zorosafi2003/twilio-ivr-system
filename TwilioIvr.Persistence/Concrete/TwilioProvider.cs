using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Http;
using Twilio.Rest.Api.V2010.Account;
using TwilioIvr.Persistence.Abstruct;

namespace TwilioIvr.Persistence.Concrete
{
    public class TwilioProvider : ITwilioProvider
    {
        private string accountSid;
        private string authToken;
        private string twilioPhone;

        private readonly IConfiguration _configuration;

        public TwilioProvider(IConfiguration Configuration)
        {
            _configuration = Configuration;

            accountSid = _configuration["TwilioConfig:AccountSid"].ToString();
            authToken = _configuration["TwilioConfig:AuthToken"].ToString();
            twilioPhone = _configuration["TwilioConfig:PhoneNumber"].ToString();
        }
        public async Task CreateCall(Uri url, HttpMethod httpMethod, string to, string from = null, string digits = null)
        {
            TwilioClient.Init(accountSid, authToken);

            _ = await CallResource.CreateAsync(
                         url: url,
                         to: new Twilio.Types.PhoneNumber(to),
                         from: string.IsNullOrEmpty( from ) ? new Twilio.Types.PhoneNumber(twilioPhone) : new Twilio.Types.PhoneNumber(from),
                         method: httpMethod,
                         sendDigits: string.IsNullOrEmpty(digits) ? null : digits
                     );
        }
    }
}
