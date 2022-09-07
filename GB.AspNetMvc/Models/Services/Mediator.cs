using GB.AspNetMvc.Models.Services.Interfaces;

namespace GB.AspNetMvc.Models.Services
{
    public class Mediator : IMediator
    {
        private readonly IMailSenderService _mailSenderService;

        public Mediator(IMailSenderService mailSenderService)
        {
            _mailSenderService = mailSenderService;
        }

        public async Task Publish(Product product, bool isAdded, CancellationToken cancellationToken)
        {
            if (isAdded) await _mailSenderService.SendMail(product, cancellationToken);
        }
    }
}
