using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using Twilio.Rest.Api.V2010.Account;

namespace TwilioIvr.Persistence.Abstruct
{
   public interface ITwilioProvider
    {
        Task CreateCall(Uri url, HttpMethod httpMethod, string to, string from = null, string digits = null);
    }
}
