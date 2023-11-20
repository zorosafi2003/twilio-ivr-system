using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwilioIvr.Persistence.Abstruct;

namespace TwilioIvr.Persistence.Concrete
{
    public class PhoneNumberProvider : IPhoneNumberProvider
    {
        public string ExtractPhoneNumberFromSipUrl(string fromNumber)
        {
            if (fromNumber.StartsWith("sip"))
            {
                fromNumber = fromNumber.Substring(4, fromNumber.IndexOf("@") - 4).Trim();
            }

            return fromNumber;
        }

        public string FormatPhoneNumber(string phoneNumber, string region = "US")
        {
            phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "");

            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

            var internationalPhone = phoneNumberUtil.Parse(phoneNumber, region);

            return phoneNumberUtil.Format(internationalPhone, PhoneNumberFormat.E164);
        }

        public bool IsValid(string phoneNumber, string region = "US")
        {
            try
            {
                phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "");

                var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

                var internationalPhone = phoneNumberUtil.Parse(phoneNumber, region);

                return phoneNumberUtil.IsValidNumber(internationalPhone);
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
