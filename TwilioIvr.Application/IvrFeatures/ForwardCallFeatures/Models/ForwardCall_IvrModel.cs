using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioIvr.Application.IvrFeatures.ForwardCallFeatures.Models
{
   public class ForwardCall_IvrModel
    {
        public string CustomerPhone { get; set; }
        public string AccountSid { get; set; }
        public string CallSid { get; set; }
        public int? SelectedItemId { get; set; }
        public string SelectedItemName { get; set; }
    }
}
