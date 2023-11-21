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
using TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Models;
using TwilioIvr.Persistence.Abstruct;
using TwilioIvr.Persistence.Models;

namespace TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Commands
{
    public class EnterListenMenu_DetectMenuCommand : IRequest<VoiceResponse>
    {
        public IvrBaseModel Model { get; set; }
        public EnterListenMenu_DetectMenuCommand(IvrBaseModel model)
        {
            Model = model;
        }
        public class EnterListenMenu_DetectMenuCommandHandler : IRequestHandler<EnterListenMenu_DetectMenuCommand, VoiceResponse>
        {
            private readonly IPhoneNumberProvider _phoneNumberProvider;
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public EnterListenMenu_DetectMenuCommandHandler(IPhoneNumberProvider PhoneNumberProvider , ITwillioUrlProvider TwillioUrlProvider)
            {
                _phoneNumberProvider = PhoneNumberProvider;
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<VoiceResponse> Handle(EnterListenMenu_DetectMenuCommand command, CancellationToken cancellationToken)
            {
                var response = new VoiceResponse();

                var url = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                {
                    FunctionName = command.Model.NextActionUrl.ActionName,
                });

                var gather = new Gather(action: url, method: HttpMethod.Get, speechTimeout: "10",
                   input: new List<Gather.InputEnum> {Gather.InputEnum.Speech } ,speechModel: Gather.SpeechModelEnum.ExperimentalConversations);

                response.Append(gather);

                return response;
            }
        }
    }
}
