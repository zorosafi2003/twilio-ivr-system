using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using TwilioIvr.Persistence.Models;

namespace TwilioIvr.Persistence.Abstruct
{
    public interface ITwillioUrlProvider
    {
        Uri ReturnFunctionUrl(ReturnFunctionUrlModel model);
        void ReturnAudioFile(Gather gather, string message, string audioFileName, string dir);
        void ReturnAudioFile(VoiceResponse response, string message, string audioFileName, string dir);
    }
}
