using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ApiToDatabase.Models
{
    public class Folder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("CreationDate")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreationDate { get; set; }

        [BsonElement("pictures")]
        public List<ObjectId> Pictures { get; set; }

        [BsonElement("userId")]
        public ObjectId UserId { get; set; }
    }
}

