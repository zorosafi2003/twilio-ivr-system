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
using TwilioIvr.Application.IvrFeatures.DetectMenuFeatures.Commands.ConnectPhone;
using TwilioIvr.Application.IvrFeatures.DetectMenuFeatures.Commands.Greeting;
using TwilioIvr.Application.IvrFeatures.DetectMenuFeatures.Enums;
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
    public class DetectMenuController : TwilioController
    {
        private readonly IMediator _mediator;

        public DetectMenuController(IMediator Mediator)
        {
            _mediator = Mediator;
        }

        [HttpGet("ForwardToMenu")]
        public async Task<IActionResult> ForwardToMenu([FromQuery] string phoneNumber , [FromQuery]string phoneEx)
        {
            await _mediator.Send(new ForwardCallToMenu_DetectMenuCommand(phoneNumber, phoneEx));
            return Ok();
        }

        [HttpGet("ForwardToPerson")]
        public async Task<IActionResult> ForwardToPerson([FromQuery] string phoneNumber, [FromQuery] string phoneEx)
        {
            await _mediator.Send(new ForwardCallToPerson_DetectMenuCommand(phoneNumber, phoneEx));
            return Ok();
        }

        #region ListenMenu


        [HttpGet(DetectMenu_IvrActionEnum.EnterListenMenu)]
        public async Task<TwiMLResult> EnterListenMenu()
        {
            var response = await _mediator.Send(new EnterListenMenu_DetectMenuCommand( new Persistence.Models.IvrBaseModel
            {
                NextActionUrl = new IvrBaseModel.UrlInfoChildOfIvrBaseModel
                {
                    ActionName = DetectMenu_IvrActionEnum.ConfirmListenMenu
                }
            }));
            return TwiML(response);
        }

        [HttpGet(DetectMenu_IvrActionEnum.ConfirmListenMenu)]
        public async Task<TwiMLResult> ConfirmListenMenu(string speechResult)
        {
            var response = await _mediator.Send(new ConfirmListenMenu_DetectMenuCommand(new Persistence.Models.IvrBaseModel
            {
                Speach = speechResult
            }));
            return TwiML(response);
        }

        #endregion


        #region Phone

        [HttpGet(DetectMenu_IvrActionEnum.EnterPhone)]
        public async Task<TwiMLResult> EnterPhone(string phoneEx)
        {
            var response = await _mediator.Send(new EnterPhone_DetectMenuCommand(phoneEx ,new Persistence.Models.IvrBaseModel
            {
                NextActionUrl = new IvrBaseModel.UrlInfoChildOfIvrBaseModel
                {
                    ActionName = DetectMenu_IvrActionEnum.ConfirmPhone
                }
            }));
            return TwiML(response);
        }

        [HttpGet(DetectMenu_IvrActionEnum.ConfirmPhone)]
        public async Task<TwiMLResult> ConfirmPhone(string speechResult)
        {
            var response = await _mediator.Send(new ConfirmPhone_DetectMenuCommand(new Persistence.Models.IvrBaseModel
            {
                Speach = speechResult,
                NextActionUrl = new IvrBaseModel.UrlInfoChildOfIvrBaseModel
                {
                    ActionName = DetectMenu_IvrActionEnum.Greeting
                }
            }));
            return TwiML(response);
        }

        #endregion

        [HttpGet(DetectMenu_IvrActionEnum.Greeting)]
        public async Task<TwiMLResult> Greeting(string digits)
        {
            var response = await _mediator.Send(new GreetingMessage_DetectMenuCommand(new Persistence.Models.IvrBaseModel
            {
                Digits = digits,
                BackActionUrl = new IvrBaseModel.UrlInfoChildOfIvrBaseModel
                {
                    ActionName = DetectMenu_IvrActionEnum.ConfirmPhone
                }
            }));
            return TwiML(response);
        }
    }
}
