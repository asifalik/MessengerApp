using MessengerApi.Controllers;
using MessengerDomain.Config;
using MessengerDomain.Email;
using MessengerService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MessengerApiTest
{
    public class EmailControllerTest
    {
        private readonly Mock<ILogger<EmailController>> _mockLogger;

        public EmailControllerTest()
        {
            _mockLogger = new Mock<ILogger<EmailController>>();
        }

        [Fact]
        public void SendEmail_Within_Time_Window_Email_Has_Been_Sent()
        {
            //Arrage
            var sendEmailRequest = new SendEmailRequest() { RecipientEmailId = "", ContentType = 0 };
            var sendEmailResponse = new SendEmailResponse() { Status = true, Message = "Scuccessfully sent" };
            var mockSMTPConfig = new Mock<IOptions<SMTPConfig>>();
            mockSMTPConfig.Setup(x => x.Value).Returns(new SMTPConfig() { FromMailId = "mockemail@mock.com", Host = "mock.com", Password = "123", Port = 80, TimeOut = 2000, UserName = "user1" });

            var mockEmailTimeConfig = new Mock<IOptions<NotificationsTimeConfig>>();
            mockEmailTimeConfig.Setup(x => x.Value).Returns(new NotificationsTimeConfig() { From = DateTime.Now.TimeOfDay, To = DateTime.Now.AddMinutes(10).TimeOfDay });
            
            var _mockEmailService = new Mock<IEmailService>();
            _mockEmailService.Setup(x => x.SendEmailAsync(It.Is<SendEmailRequest>(r=> r == sendEmailRequest))).ReturnsAsync(sendEmailResponse);

            var _mockEmailController = new EmailController(_mockEmailService.Object, _mockLogger.Object);

            //Act
            var response = _mockEmailController.SendEmail(sendEmailRequest);

            //Assert
            Assert.Equal(sendEmailResponse.Status, response.Result.Status);
            Assert.Equal(sendEmailResponse.Message, response.Result.Message);
        }

        [Fact]
        public void SendEmail_Outside_Time_Window_Email_Has_Not_Been_Sent()
        {
            //Arrage
            var _mockSMTPConfig = new Mock<IOptions<SMTPConfig>>();
            var _mockEmailTimeConfig = new Mock<IOptions<NotificationsTimeConfig>>();
            var _mockEmailService = new Mock<IEmailService>();
            var emailTimeConfig = new NotificationsTimeConfig() { From = DateTime.Now.AddHours(1).TimeOfDay, To = DateTime.Now.AddHours(2).TimeOfDay };
            var sendEmailRequest = new SendEmailRequest() { RecipientEmailId = "", ContentType = 0 };
            var sendEmailResponse = new SendEmailResponse() { Status = false, Message = "Timespan Validation Failed." };
            
            _mockSMTPConfig.Setup(x => x.Value).Returns(new SMTPConfig() { FromMailId = "mockemail@mock.com", Host = "mock.com", Password = "123", Port = 80, TimeOut = 2000, UserName = "user1" });
            _mockEmailTimeConfig.Setup(x => x.Value).Returns(emailTimeConfig);
            _mockEmailService.Setup(x => x.SendEmailAsync(It.Is<SendEmailRequest>(r => r == sendEmailRequest))).ReturnsAsync(sendEmailResponse);

            var _mockEmailController = new EmailController(_mockEmailService.Object, _mockLogger.Object);

            //Act
            var response = _mockEmailController.SendEmail(sendEmailRequest);

            //Assert
            Assert.Equal(sendEmailResponse.Status, response.Result.Status);
            Assert.Equal(sendEmailResponse.Message, response.Result.Message);
        }
    }
}