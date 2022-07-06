using GB.AspNetMvc.Models.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;
using Polly.Retry;

namespace GB.AspNetMvc.Models.Services
{
    public class MailSenderByMailKitService : IMailSenderService, IDisposable
    {
        private readonly SmtpClient _smtpClient;
        private readonly ILogger _logger;
        private MailSettings MailSettings { get; }

        public MailSenderByMailKitService(ILogger<MailSenderByMailKitService> logger,
                                          IOptionsSnapshot<MailSettings> options, 
                                          SmtpClient smtpClient)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _logger = logger;
            _smtpClient = smtpClient;
            MailSettings = options.Value;
        }

        public async Task SendMail(Product product)
        {
            var policy = Policy
                         .Handle<Exception>()
                         .RetryAsync(3, (exception, retryAttempt) =>
                         {
                             _logger.LogWarning(exception, "Ошибка во время отправки письма. Попытка: {Attempt}", retryAttempt);
                         });

            var result = await policy.ExecuteAndCaptureAsync(() => Send(product));

            if (result.Outcome == OutcomeType.Failure)
            {
                _logger.LogError(result.FinalException, "Не удалось отправить письмо");
            }
        }

        private Task Send(Product product)
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

                ConnectAndAuthenticate();

                _smtpClient.Send(message);

                _smtpClient.Disconnect(true);

                _logger.LogInformation("Сообщение о добалении товара отправлено успешно!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Не удалось отправить сообщение о добавлении товара!");
            }

            return Task.CompletedTask;
        }

        private void ConnectAndAuthenticate()
        {
            if (!_smtpClient.IsConnected)
                _smtpClient.Connect(MailSettings.Host, 465, true);

            if(!_smtpClient.IsAuthenticated)
                _smtpClient.Authenticate(MailSettings.Login, MailSettings.Password);
        }

        public void Dispose()
        {
            if (_smtpClient.IsConnected)
                _smtpClient.Disconnect(true);

            _smtpClient.Dispose();
        }
    }
}
