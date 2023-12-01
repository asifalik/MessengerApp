using MessengerDomain.Email;
using MessengerService;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        #region PRIVATE VARIABLES
        private IEmailService _emailService;
        private readonly ILogger<EmailController> _logger; 
        #endregion

        #region CONSTRUCTOR
        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        } 
        #endregion

        #region ENDPOINTS
        [HttpPost(Name = "SendEmail")]
        public async Task<SendEmailResponse> SendEmail(SendEmailRequest sendEmailRequest)
        {
            try
            {
                _logger.LogInformation($"Sending email to {0}", sendEmailRequest.RecipientEmailId);
                var sendEmailResponse = await _emailService.SendEmailAsync(sendEmailRequest);
                _logger.LogInformation($"Sending email to {0} {1}", sendEmailRequest.RecipientEmailId, sendEmailResponse.Status ? "success" : "failed");
                return sendEmailResponse;
            }
            catch (Exception exception)
            {
                return new SendEmailResponse() { Status = false, Message = exception.Message };
            }

        } 
        #endregion
    }
}