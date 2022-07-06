namespace GB.AspNetMvc.Models.Services.Interfaces
{
    public interface IMailSenderService
    {
        Task SendMail(Product product);
    }
}
