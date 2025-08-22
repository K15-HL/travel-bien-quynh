using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace travel_bien_quynh.Entities
{
    public class AtmHistory: IMongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("reference_number")]
        public string referenceNumber { get; set; }

        [BsonElement("transaction_date")]
        public DateTime transactionDate { get; set; }

        [BsonElement("amount_in")]
        public double amountIn { get; set; }

        [BsonElement("account_number")]
        public string accountNumber { get; set; }

        [BsonElement("bank_brand_name")]
        public string bankBrandName { get; set; }

        [BsonElement("balance_before")]
        public double BalanceBefore { get; set; }

        [BsonElement("balance_after")]
        public double BalanceAfter { get; set; }
    }
}
