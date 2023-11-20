using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioIvr.Persistence.Abstruct
{
    public interface IPhoneNumberProvider
    {
        string ExtractPhoneNumberFromSipUrl(string fromNumber);
        string FormatPhoneNumber(string phoneNumber, string region = "US");
        bool IsValid(string phoneNumber, string region = "US");
    }
}
