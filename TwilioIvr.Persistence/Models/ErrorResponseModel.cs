using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwilioIvr.Persistence.Models
{
    public class ErrorResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StackTrace { get; set; }
        public dynamic Message { get; set; }
    }
}
