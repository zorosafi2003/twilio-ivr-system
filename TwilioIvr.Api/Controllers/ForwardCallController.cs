using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.AspNet.Core;
using TwilioIvr.Application.Features.ForwardCallFeatures;
using TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Commands;
using TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Commands.MainMenu;
using TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Enums;
using TwilioIvr.Persistence.Models;
using static TwilioIvr.Persistence.Models.IvrBaseModel;

namespace TwilioIvr.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class ForwardCallController : TwilioController
    {
        private readonly IMediator _mediator;

        public ForwardCallController(IMediator Mediator)
        {
            _mediator = Mediator;
        }

        [HttpGet("ForwardTo")]
        public async Task<IActionResult> ForwardTo([FromQuery] string phoneNumber)
        {
            await _mediator.Send(new ForwardCallCommand(phoneNumber));
            return Ok();
        }

        [HttpGet(ForwardCall_IvrActionEnum.Call)]
        public async Task<TwiMLResult> Call(string from, string to , string accountSid, string callSid ,string direction)
        {
            var customerPhone = direction.Contains("inbound") == true ? from : to;
            var response = await _mediator.Send(new Call_IvrCommand(customerPhone, accountSid, callSid, new Persistence.Models.IvrBaseModel
            {
                NextActionUrl = new IvrBaseModel.UrlInfoChildOfIvrBaseModel
                {
                    ActionName = ForwardCall_IvrActionEnum.EnterMainMenu
                }
            }));
            return TwiML(response);
        }

        [HttpGet(ForwardCall_IvrActionEnum.EnterMainMenu)]
        public async Task<TwiMLResult> EnterMainMenu(string dto)
        {
            var response = await _mediator.Send(new EnterMainMenu_IvrCommand(new Persistence.Models.IvrBaseModel
            {
                Dto = dto,
                NextActionUrl = new IvrBaseModel.UrlInfoChildOfIvrBaseModel
                {
                    ActionName = ForwardCall_IvrActionEnum.VerifyMainMenu
                }
            }));
            return TwiML(response);
        }

        [HttpGet(ForwardCall_IvrActionEnum.VerifyMainMenu)]
        public async Task<TwiMLResult> VerifyMainMenu(string digits,string speechResult, string dto)
        {
            var response = await _mediator.Send(new VerifyMainMenu_IvrCommand(new Persistence.Models.IvrBaseModel
            {
                Digits = digits,
                Speach = speechResult ,
                Dto = dto,
                BackActionUrl = new IvrBaseModel.UrlInfoChildOfIvrBaseModel
                {
                    ActionName = ForwardCall_IvrActionEnum.EnterMainMenu
                },
                NextActionUrl = new IvrBaseModel.UrlInfoChildOfIvrBaseModel
                {
                    ActionName = ForwardCall_IvrActionEnum.ConfirmMainMenu
                }
            }));
            return TwiML(response);
        }

        [HttpGet(ForwardCall_IvrActionEnum.ConfirmMainMenu)]
        public async Task<TwiMLResult> ConfirmMainMenu(string digits, string dto)
        {
            var response = await _mediator.Send(new ConfirmMainMenu_IvrCommand(new Persistence.Models.IvrBaseModel
            {
                Digits = digits,
                Dto = dto,
                BackActionUrl = new IvrBaseModel.UrlInfoChildOfIvrBaseModel
                {
                    ActionName = ForwardCall_IvrActionEnum.VerifyMainMenu
                },
                CasesUrl = new Dictionary<string, UrlInfoChildOfIvrBaseModel>
                 {
                     {"*", new UrlInfoChildOfIvrBaseModel {ActionName = ForwardCall_IvrActionEnum.EnterMainMenu} } ,
                     {"2", new UrlInfoChildOfIvrBaseModel {ActionName = ForwardCall_IvrActionEnum.EnterMainMenu} } ,
                 }
            }));
            return TwiML(response);
        }
    }
}
