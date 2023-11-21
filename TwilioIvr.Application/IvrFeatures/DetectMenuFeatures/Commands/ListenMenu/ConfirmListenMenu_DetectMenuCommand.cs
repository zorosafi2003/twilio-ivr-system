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
    public class ConfirmListenMenu_DetectMenuCommand : IRequest<VoiceResponse>
    {
        public IvrBaseModel Model { get; set; }
        public ConfirmListenMenu_DetectMenuCommand(IvrBaseModel model)
        {
            Model = model;
        }
        public class ConfirmListenMenu_DetectMenuCommandHandler : IRequestHandler<ConfirmListenMenu_DetectMenuCommand, VoiceResponse>
        {
            private readonly IPhoneNumberProvider _phoneNumberProvider;
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public ConfirmListenMenu_DetectMenuCommandHandler(IPhoneNumberProvider PhoneNumberProvider , ITwillioUrlProvider TwillioUrlProvider)
            {
                _phoneNumberProvider = PhoneNumberProvider;
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<VoiceResponse> Handle(ConfirmListenMenu_DetectMenuCommand command, CancellationToken cancellationToken)
            {
                var response = new VoiceResponse();

                //here we got the menu
                var speach = command.Model.Speach;

                response.Hangup();

                return response;
            }
        }
    }
}
