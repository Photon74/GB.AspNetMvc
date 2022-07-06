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

        public void Publish(Product product, bool isAdded)
        {
            if(isAdded) _mailSenderService.SendMail(product);
        }
    }
}
