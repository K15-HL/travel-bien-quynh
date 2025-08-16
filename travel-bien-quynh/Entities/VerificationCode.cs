using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace travel_bien_quynh.Entities
{
    public class VerificationCode : IMongoEntity
    { 

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("ExpiryTime")]
        public DateTime ExpiryTime { get; set; }
    }
}
