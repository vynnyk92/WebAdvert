using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
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

        public async Task<bool> CheckTableExist()
        {
            try
            {
                var data = await _amazonDynamoDb.DescribeTableAsync("adverts");
                return string.Equals(data.Table.TableStatus, "Active", StringComparison.CurrentCultureIgnoreCase);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
           
        }

        public async Task<string> GetAdvertInfo(string id)
        {
            var qRequest = new QueryRequest
            {
                TableName = "adverts",
                KeyConditionExpression = "Id = :v_Id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> 
                {
                    {":v_Id", new AttributeValue { S =  id }}
                },
                ScanIndexForward = false,
                Limit = 1
            };

            var data = await _amazonDynamoDb.QueryAsync(qRequest);
            return data.Items.FirstOrDefault()?["Title"]?.S ?? string.Empty;
        }
    }
}
