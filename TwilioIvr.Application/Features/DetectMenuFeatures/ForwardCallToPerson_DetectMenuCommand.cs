using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwilioIvr.Application.IvrFeatures.DetectMenuFeatures.Enums;
using TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Enums;
using TwilioIvr.Persistence.Abstruct;

namespace TwilioIvr.Application.Features.ForwardCallFeatures
{
    public class ForwardCallToPerson_DetectMenuCommand : IRequest<Unit>
    {
        public string PhoneNumber { get; set; }
        public string PhoneEx { get; set; }
        public ForwardCallToPerson_DetectMenuCommand(string phoneNumber , string phoneEx)
        {
            PhoneNumber = phoneNumber;
            PhoneEx = phoneEx;
        }
        public class ForwardCallToPerson_DetectMenuCommandHandler : IRequestHandler<ForwardCallToPerson_DetectMenuCommand, Unit>
        {
            private readonly ITwilioProvider _twilioProvider;
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public ForwardCallToPerson_DetectMenuCommandHandler( ITwilioProvider TwilioProvider , ITwillioUrlProvider TwillioUrlProvider)
            {
                _twilioProvider = TwilioProvider;
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<Unit> Handle(ForwardCallToPerson_DetectMenuCommand request, CancellationToken cancellationToken)
            {
                var queryParams = new List<KeyValuePair<string, string>> { };

                if (!string.IsNullOrEmpty(request.PhoneEx))
                {
                    queryParams.Add(new KeyValuePair<string, string>("PhoneEx", request.PhoneEx));
                }

                var forwardCallUrl = _twillioUrlProvider.ReturnFunctionUrl(new Persistence.Models.ReturnFunctionUrlModel
                {
                    ControllerName = DetectMenu_IvrActionEnum.ControllerName,
                    FunctionName = DetectMenu_IvrActionEnum.EnterPhone ,
                    QueryParams = queryParams
                });

                await _twilioProvider.CreateCall(forwardCallUrl, Twilio.Http.HttpMethod.Get, request.PhoneNumber , null ,request.PhoneEx );

                return Unit.Value;
            }
        }
    }
}
