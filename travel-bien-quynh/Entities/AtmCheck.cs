using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace travel_bien_quynh.Entities
{
    public class AtmCheck : IMongoEntity
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("reference_number")]
        public string referenceNumber { get; set; }
    }
}
