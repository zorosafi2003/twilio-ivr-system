using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Enums;
using TwilioIvr.Persistence.Abstruct;

namespace TwilioIvr.Application.Features.MainFeatures
{
    public class ForwardCallCommand : IRequest<Unit>
    {
        public string PhoneNumber { get; set; }
        public ForwardCallCommand(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
        public class ForwardCallCommandHandler : IRequestHandler<ForwardCallCommand, Unit>
        {
            private readonly ITwilioProvider _twilioProvider;
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public ForwardCallCommandHandler( ITwilioProvider TwilioProvider , ITwillioUrlProvider TwillioUrlProvider)
            {
                _twilioProvider = TwilioProvider;
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<Unit> Handle(ForwardCallCommand request, CancellationToken cancellationToken)
            {
                var forwardCallUrl = _twillioUrlProvider.ReturnFunctionUrl(new Persistence.Models.ReturnFunctionUrlModel
                {
                    ControllerName = ForwardCall_IvrActionEnum.ControllerName,
                    FunctionName = ForwardCall_IvrActionEnum.Call
                });

                await _twilioProvider.CreateCall(forwardCallUrl, Twilio.Http.HttpMethod.Get, request.PhoneNumber);

                return Unit.Value;
            }
        }
    }
}
