using GB.AspNetMvc.Models.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;

namespace GB.AspNetMvc.Models.Services
{
    public class MailSenderByMailKitService : IMailSenderService, IDisposable, IAsyncDisposable
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
                         .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryAttempt, 2)),
                             (exception, retryAttempt) =>
                         {
                             _logger.LogWarning(exception, "Ошибка во время отправки письма. Попытка: {Attempt}", retryAttempt);
                         });

            var result = await policy.ExecuteAndCaptureAsync(async () => await SendAsync(product));

            if (result.Outcome == OutcomeType.Failure)
            {
                _logger.LogError(result.FinalException, "Не удалось отправить письмо");
            }
        }

        private async Task SendAsync(Product product)
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

                await ConnectAndAuthenticateAsync();

                await _smtpClient.SendAsync(message);

                _logger.LogInformation("Сообщение о добалении товара отправлено успешно!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Не удалось отправить сообщение о добавлении товара!");
            }
        }

        private async Task ConnectAndAuthenticateAsync()
        {
            if (!_smtpClient.IsConnected)
                await _smtpClient.ConnectAsync(MailSettings.Host, 465, true);

            if(!_smtpClient.IsAuthenticated)
                await _smtpClient.AuthenticateAsync(MailSettings.Login, MailSettings.Password);
        }

        public void Dispose()
        {
            if (_smtpClient.IsConnected)
                _smtpClient.Disconnect(true);

            _smtpClient.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_smtpClient.IsConnected)
                await _smtpClient.DisconnectAsync(true);

            _smtpClient.Dispose();
        }
    }
}
