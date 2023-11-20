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

namespace TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Commands.MainMenu
{
   public class EnterMainMenu_IvrCommand : IRequest<VoiceResponse>
    {
        public IvrBaseModel Model { get; set; }

        public EnterMainMenu_IvrCommand( IvrBaseModel model)
        {
            Model = model;
        }

        public class EnterMainMenu_IvrCommandHandler : IRequestHandler<EnterMainMenu_IvrCommand, VoiceResponse>
        {
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public EnterMainMenu_IvrCommandHandler(ITwillioUrlProvider TwillioUrlProvider)
            {
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<VoiceResponse> Handle(EnterMainMenu_IvrCommand command, CancellationToken cancellationToken)
            {
                var response = new VoiceResponse();

                var dtoModel = command.Model.GetDtoModel<ForwardCall_IvrModel>();

                var url = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                {
                    FunctionName = command.Model.NextActionUrl.ActionName,
                    DtoModel = dtoModel
                });

                var gather = new Gather(action: url, numDigits: 1, method: HttpMethod.Get , timeout:3 , finishOnKey:"#" , speechTimeout: "3",
                    input : new List<Gather.InputEnum> { Gather.InputEnum.Dtmf , Gather.InputEnum.Speech } , hints :"meat, bread" , speechModel: Gather.SpeechModelEnum.PhoneCall);

                _twillioUrlProvider.ReturnAudioFile(gather, "for meat press 1 or say meat for bread press 2 or say bread ", "Enter_Main_Menu.wav", null);

                response.Append(gather).Redirect(url, method: HttpMethod.Get);

                return response;
            }
        }
    }
}
