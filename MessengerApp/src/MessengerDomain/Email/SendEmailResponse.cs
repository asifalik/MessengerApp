using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerDomain.Email
{
    public class SendEmailResponse
    {
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public bool Status { get; set; }
    }
}
