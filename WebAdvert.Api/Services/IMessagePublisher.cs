using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using WebAdvert.Models;
using WebAdvert.Models.Messages;

namespace WebAdvert.Api.Services
{
    public interface IMessagePublisher
    {
        Task PublishAsync(ConfirmAdvertModel advertModel);
    }

    public class MessagePublisher : IMessagePublisher
    {
        private readonly IAdvertStorageService _advertStorageService;
        private readonly IConfiguration _configuration;
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;

        public MessagePublisher(IAdvertStorageService advertStorageService, IConfiguration configuration, IAmazonSimpleNotificationService amazonSimpleNotificationService)
        {
            _advertStorageService = advertStorageService;
            _configuration = configuration;
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
        }
        public async Task PublishAsync(ConfirmAdvertModel advertModel)
        {
            var advertTitle =  await _advertStorageService.GetAdvertInfo(advertModel.Id);
            var message = new AdvertConfirmedMessage
            {
                Id = advertModel.Id,
                Title = advertTitle
            };
            var topicArn = _configuration["Topic"];
            var messageString = JsonSerializer.Serialize(message);
            await _amazonSimpleNotificationService.PublishAsync(topicArn, messageString);
        }
    }
}
