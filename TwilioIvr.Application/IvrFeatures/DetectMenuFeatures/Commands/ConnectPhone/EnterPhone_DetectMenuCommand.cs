using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Http;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using TwilioIvr.Persistence.Abstruct;
using TwilioIvr.Persistence.Models;

namespace TwilioIvr.Application.IvrFeatures.DetectMenuFeatures.Commands.ConnectPhone
{
    public class EnterPhone_DetectMenuCommand : IRequest<VoiceResponse>
    {
        public string PhoneEx { get; set; }
        public IvrBaseModel Model { get; set; }

        public EnterPhone_DetectMenuCommand(string phoneEx, IvrBaseModel model)
        {
            PhoneEx = phoneEx;
            Model = model;
        }

        public class EnterPhone_DetectMenuCommandHandler : IRequestHandler<EnterPhone_DetectMenuCommand, VoiceResponse>
        {
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public EnterPhone_DetectMenuCommandHandler(ITwillioUrlProvider TwillioUrlProvider)
            {
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<VoiceResponse> Handle(EnterPhone_DetectMenuCommand command, CancellationToken cancellationToken)
            {
                var response = new VoiceResponse();

                var url = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                {
                    FunctionName = command.Model.NextActionUrl.ActionName,
                });

                response.Play(digits: "www202#");

                response.Redirect(url,method:HttpMethod.Get);
                     
                return response;
            }
        }
    }
}
