using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace travel_bien_quynh.Entities
{
    public class LogWallet: IMongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonElement("UserName")]
        public string UserName { get; set; }

        [Required]
        [BsonElement("Amount")]
        public double Amount { get; set; }
        [Required]
        [BsonElement("Money")]
        public double Money { get; set; }

        [Required]
        [BsonElement("BalanceBefore")]
        public double BalanceBefore { get; set; }

        [Required]
        [BsonElement("BalanceAfter")]
        public double BalanceAfter { get; set; }

        [Required]
        [BsonElement("TransactionType")]
        public TransactionType TransactionType { get; set; }

        [StringLength(500)]
        [BsonElement("Description")]
        public string Description { get; set; }

        [Required]
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [BsonElement("status")]
        public string Status { get; set; }
    }

    public enum TransactionType
    {
        Credit,
        Debit 
    }
}
