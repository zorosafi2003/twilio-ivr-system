using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Http;
using Twilio.TwiML;
using TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Models;
using TwilioIvr.Persistence.Abstruct;
using TwilioIvr.Persistence.Models;

namespace TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Commands.MainMenu
{
    public class ConfirmMainMenu_IvrCommand : IRequest<VoiceResponse>
    {
        public IvrBaseModel Model { get; set; }
        public ConfirmMainMenu_IvrCommand(IvrBaseModel model)
        {
            Model = model;
        }
        public class ConfirmMainMenu_IvrCommandHandler : IRequestHandler<ConfirmMainMenu_IvrCommand, VoiceResponse>
        {
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public ConfirmMainMenu_IvrCommandHandler(ITwillioUrlProvider TwillioUrlProvider)
            {
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public async Task<VoiceResponse> Handle(ConfirmMainMenu_IvrCommand command, CancellationToken cancellationToken)
            {
                Uri url = null;

                var response = new VoiceResponse();

                var dtoModel = command.Model.GetDtoModel<ForwardCall_IvrModel>();

                var UrlInfo_Case_star = command.Model.CasesUrl.Where(x => x.Key == "*").FirstOrDefault().Value;
                var UrlInfo_Case_2 = command.Model.CasesUrl.Where(x => x.Key == "2").FirstOrDefault().Value;

                var backUrl = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                {
                    FunctionName = command.Model.BackActionUrl.ActionName,
                    DtoModel = dtoModel
                });

                if (string.IsNullOrEmpty(command.Model.Digits))
                {
                    url = backUrl;
                }
                else if (command.Model.Digits == "*")
                {
                    url = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                    {
                        FunctionName = UrlInfo_Case_star.ActionName,
                        DtoModel = dtoModel
                    });
                }
                else if (command.Model.Digits == "1")
                {
                    _twillioUrlProvider.ReturnAudioFile(response, "we save your order thank you", "Thank_You.wav", null);

                    response.Hangup();
                }
                else if (command.Model.Digits == "2")
                {
                    url = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                    {
                        FunctionName = UrlInfo_Case_2.ActionName,
                        DtoModel = dtoModel
                    });
                }
                else
                {
                    url = backUrl;

                    _twillioUrlProvider.ReturnAudioFile(response, "sorry your entery is incorrect", "Incorrect_Entry.wav", null);
                }

                response.Redirect(url, method: HttpMethod.Get);

                return response;
            }
        }
    }
}
