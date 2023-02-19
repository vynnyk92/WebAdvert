using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using WebAdvert.Api.DTOs;
using WebAdvert.Models;

namespace WebAdvert.Api.Services
{
    public class DynamoAdvertStorageService : IAdvertStorageService
    {
        private readonly IMapper _mapper;
        private readonly IAmazonDynamoDB _amazonDynamoDb;

        public DynamoAdvertStorageService(IMapper mapper, IAmazonDynamoDB amazonDynamoDb)
        {
            _mapper = mapper;
            _amazonDynamoDb = amazonDynamoDb;
        }

        public async Task<string> Add(AdvertModel advertModel)
        {
            var dto = _mapper.Map<AdvertDto>(advertModel);
            dto.Id = Guid.NewGuid().ToString();
            dto.AdvertStatus = AdvertStatus.Pending;
            dto.CreatedAt = DateTime.UtcNow;

            using var context = new DynamoDBContext(_amazonDynamoDb);
            await context.SaveAsync(dto);

            return dto.Id;
        }

        public async Task Confirm(ConfirmAdvertModel confirmAdvertModel)
        {
            using var context = new DynamoDBContext(_amazonDynamoDb);
            var record = await context.LoadAsync<AdvertDto>(confirmAdvertModel.Id);
            if (record is null)
                throw new KeyNotFoundException($"Record doesn't exist {confirmAdvertModel.Id}");

            if (confirmAdvertModel.Status is AdvertStatus.Active)
            {
                record.AdvertStatus = AdvertStatus.Active;
                await context.SaveAsync(record);
            }
            else
            {
                await context.DeleteAsync(record);
            }
                
        }
    }
}
