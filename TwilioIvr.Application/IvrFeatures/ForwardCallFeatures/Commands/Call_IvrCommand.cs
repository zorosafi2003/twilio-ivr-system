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

namespace TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Commands
{
    public class Call_IvrCommand : IRequest<VoiceResponse>
    {
        public string CustomerPhone { get; set; }
        public string AccountSid { get; set; }
        public string CallSid { get; set; }
        public IvrBaseModel Model { get; set; }
        public Call_IvrCommand(string customerPhone, string accountSid, string callSid, IvrBaseModel model)
        {
            CustomerPhone = customerPhone;
            AccountSid = accountSid;
            CallSid = callSid;
            Model = model;
        }
        public class Call_IvrCommandHandler : IRequestHandler<Call_IvrCommand, VoiceResponse>
        {
            private readonly IPhoneNumberProvider _phoneNumberProvider;
            private readonly ITwillioUrlProvider _twillioUrlProvider;

            public Call_IvrCommandHandler(IPhoneNumberProvider PhoneNumberProvider , ITwillioUrlProvider TwillioUrlProvider)
            {
                _phoneNumberProvider = PhoneNumberProvider;
                _twillioUrlProvider = TwillioUrlProvider;
            }
            public Task<VoiceResponse> Handle(Call_IvrCommand command, CancellationToken cancellationToken)
            {
                var response = new VoiceResponse();

                if (command.CustomerPhone.Contains("sip"))
                {
                    command.CustomerPhone = _phoneNumberProvider.ExtractPhoneNumberFromSipUrl(command.CustomerPhone);
                }

                if (!command.CustomerPhone.Contains("+"))
                {
                    command.CustomerPhone = _phoneNumberProvider.FormatPhoneNumber(command.CustomerPhone);
                }

                var dtoModel = new ForwardCall_IvrModel
                {
                    CustomerPhone = command.CustomerPhone,
                    AccountSid = command.AccountSid,
                    CallSid = command.CallSid
                };

                var url = _twillioUrlProvider.ReturnFunctionUrl(new ReturnFunctionUrlModel
                {
                    FunctionName = command.Model.NextActionUrl.ActionName,
                    DtoModel = dtoModel
                });

                response.Say("Thanks for calling out company");

                response.Redirect(url, method: HttpMethod.Get);

                return Task.FromResult(response);
            }
        }
    }
}
