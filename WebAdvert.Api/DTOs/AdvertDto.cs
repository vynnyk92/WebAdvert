using WebAdvert.Models;
using Amazon.DynamoDBv2.DataModel;

namespace WebAdvert.Api.DTOs
{
    [DynamoDBTable("adverts")]
    public class AdvertDto
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        [DynamoDBProperty]
        public string Title { get; set; }
        [DynamoDBProperty]
        public string Description { get; set; }
        [DynamoDBProperty]
        public double Price { get; set; }
        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }
        [DynamoDBProperty]
        public AdvertStatus AdvertStatus { get; set; }
    }
}
