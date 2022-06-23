using GB.AspNetMvc.Models.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GB.AspNetMvc.Models.Services
{
    public class MailSenderByMailKitService : IMailSenderService
    {
        private readonly ILogger _logger;
        private MailSettings MailSettings { get; }

        public MailSenderByMailKitService(ILogger<MailSenderByMailKitService> logger, IOptions<MailSettings> options)
        {
            _logger = logger;
            MailSettings = options.Value;

        }

        public void SendMail(Product product)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("GbMvcProgect", MailSettings.From));
                message.To.Add(new MailboxAddress("Alexander", MailSettings.To));
                message.Subject = "Добавлен новый товар!";
                message.Body = new BodyBuilder()
                {
                    HtmlBody = "<div style=\"color: green;\">Сообщение от GbMvcProgect</div>" +
                               "<div><p>Добавлен новый товар</p>" +
                               $"<p>Наименование товара: {product.Name}</p>" +
                               $"<p>Категория товара: {product.Category}</p></div>"
                }.ToMessageBody();

                using var client = new SmtpClient();
                client.Connect(MailSettings.Host, 465, true);
                client.Authenticate(MailSettings.Login, MailSettings.Password);
                client.Send(message);

                client.Disconnect(true);

                _logger.LogInformation("Отправлено успешно!");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }
        }
    }
}
