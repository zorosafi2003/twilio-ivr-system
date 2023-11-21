using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twilio.TwiML;
using TwilioIvr.Persistence.Abstruct;
using TwilioIvr.Persistence.Models;

namespace TwilioIvr.Application.IvrFeatures.DetectMenuFeatures.Commands.Greeting
{
    public class GreetingMessage_DetectMenuCommand : IRequest<VoiceResponse>
    {
        public IvrBaseModel Model { get; set; }

        public GreetingMessage_DetectMenuCommand(IvrBaseModel model)
        {
            Model = model;
        }
        public class GreetingMessage_DetectMenuCommandHandler : IRequestHandler<GreetingMessage_DetectMenuCommand, VoiceResponse>
        {
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public GreetingMessage_DetectMenuCommandHandler(ITwillioUrlProvider TwillioUrlProvider)
            {
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<VoiceResponse> Handle(GreetingMessage_DetectMenuCommand command, CancellationToken cancellationToken)
            {
                var response = new VoiceResponse();

                var backUrl = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                {
                    FunctionName = command.Model.BackActionUrl.ActionName,
                });

                if (!string.IsNullOrEmpty(command.Model.Digits))
                {
                    response.Say("Thanks for calling our company we need to get some information");

                    response.Hangup();
                }
                else
                {
                    response.Redirect(url: backUrl,method:Twilio.Http.HttpMethod.Get);
                }

                return response;
            }
        }
    }
}
