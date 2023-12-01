using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerDomain.Email
{
    public class SendEmailRequest
    {
        public int ContentType { get; set; }
        public string RecipientEmailId { get; set; }
        

    }
}
