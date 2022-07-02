namespace GB.AspNetMvc.Models.Services.Interfaces;

public interface IMediator
{
    void Publish(Product product, bool isAdded);
}