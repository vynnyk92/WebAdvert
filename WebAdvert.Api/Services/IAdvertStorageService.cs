using WebAdvert.Models;

namespace WebAdvert.Api.Services
{
    public interface IAdvertStorageService
    {
        Task<string> Add(AdvertModel advertModel);
        Task<bool> Confirm(ConfirmAdvertModel confirmAdvertModel);
    }
}
