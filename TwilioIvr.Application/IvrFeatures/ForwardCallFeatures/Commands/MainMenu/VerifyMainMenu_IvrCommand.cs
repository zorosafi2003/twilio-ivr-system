using MediatR;
using Newtonsoft.Json;
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
    public class VerifyMainMenu_IvrCommand : IRequest<VoiceResponse>
    {
        public IvrBaseModel Model { get; set; }
        public VerifyMainMenu_IvrCommand(IvrBaseModel model)
        {
            Model = model;
        }
        public class VerifyMainMenu_IvrCommandHandler : IRequestHandler<VerifyMainMenu_IvrCommand, VoiceResponse>
        {
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public VerifyMainMenu_IvrCommandHandler(ITwillioUrlProvider TwillioUrlProvider)
            {
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<VoiceResponse> Handle(VerifyMainMenu_IvrCommand command, CancellationToken cancellationToken)
            {
                Uri url = null;

                var response = new VoiceResponse();

                var dtoModel = JsonConvert.DeserializeObject<ForwardCall_IvrModel>(command.Model.Dto);

                Uri backUrl = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                {
                    FunctionName = command.Model.BackActionUrl.ActionName,
                    DtoModel = dtoModel
                });

                var validInputArr = new List<string> { "1", "2", "meat", "bread" };

                var userAction = command.Model.Digits;
                if (string.IsNullOrEmpty(userAction))
                {
                    userAction = command.Model.Speach;
                }

                if (string.IsNullOrEmpty(userAction))
                {
                    url = backUrl;
                }
                else if (userAction == "*")
                {
                    url = backUrl;
                }
                else if (validInputArr.Contains(userAction) == true)
                {
                    if (int.TryParse(userAction, out int digit))
                    {
                        dtoModel.SelectedItemId = digit;
                        dtoModel.SelectedItemName = digit == 1 ? "meat" : "bread";
                    }
                    else
                    {
                        dtoModel.SelectedItemId = command.Model.Speach.ToLower() == "meat" ? 1 : 2;
                        dtoModel.SelectedItemName = command.Model.Speach.ToLower();
                    }

                    url = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                    {
                        FunctionName = command.Model.NextActionUrl.ActionName,
                        DtoModel = dtoModel
                    });

                    var gather = new Gather(action: url, numDigits: 1, method: HttpMethod.Get , timeout: 6, finishOnKey: "#");

                    _twillioUrlProvider.ReturnAudioFile(gather, "you entered", "You_Entered.wav", null);

                    gather.Pause().Say(dtoModel.SelectedItemName).Pause();

                    _twillioUrlProvider.ReturnAudioFile(gather, "if yes press 1 if no press 2 ", "If_Correct.wav", null);

                    response.Append(gather);
                }
                else
                {
                    url = backUrl;

                    _twillioUrlProvider.ReturnAudioFile(response, "sorry your entery is incorrect", "Incorrect_Code.wav", null);
                }

                response.Redirect(url, HttpMethod.Get);

                return response;
            }
        }
    }
}
