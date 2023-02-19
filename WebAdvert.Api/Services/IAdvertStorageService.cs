using WebAdvert.Models;

namespace WebAdvert.Api.Services
{
    public interface IAdvertStorageService
    {
        Task<string> Add(AdvertModel advertModel);
        Task Confirm(ConfirmAdvertModel confirmAdvertModel);
    }
}
