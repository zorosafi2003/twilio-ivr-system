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
    public class ConfirmPhone_DetectMenuCommand : IRequest<VoiceResponse>
    {
        public IvrBaseModel Model { get; set; }

        public ConfirmPhone_DetectMenuCommand( IvrBaseModel model)
        {
            Model = model;
        }

        public class EnterPhone_DetectMenuCommandHandler : IRequestHandler<ConfirmPhone_DetectMenuCommand, VoiceResponse>
        {
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public EnterPhone_DetectMenuCommandHandler(ITwillioUrlProvider TwillioUrlProvider)
            {
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<VoiceResponse> Handle(ConfirmPhone_DetectMenuCommand command, CancellationToken cancellationToken)
            {
                var response = new VoiceResponse();

                var url = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                {
                    FunctionName = command.Model.NextActionUrl.ActionName,
                });

                var gather = new Gather(action: url, method: HttpMethod.Get, numDigits:1 ,timeout:5 , input: new List<Gather.InputEnum> { Gather.InputEnum.Dtmf });

                gather.Say("to listen to message press 1");

                response.Append(gather).Redirect(url ,method:HttpMethod.Get);

                return response;
            }
        }
    }
}
