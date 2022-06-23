namespace GB.AspNetMvc.Models.Services.Interfaces
{
    public interface IMailSenderService
    {
        void SendMail(Product product);
    }
}
