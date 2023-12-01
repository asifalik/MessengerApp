using MessengerDomain.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerService
{
    public interface IEmailService
    {
        Task<SendEmailResponse> SendEmailAsync(SendEmailRequest sendEmailRequest);
    }
}
