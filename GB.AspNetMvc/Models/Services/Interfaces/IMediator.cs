namespace GB.AspNetMvc.Models.Services.Interfaces;

public interface IMediator
{
    Task Publish(Product product, bool isAdded);
}