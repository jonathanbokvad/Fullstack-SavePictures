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

        //[BsonRepresentation(BsonType.Timestamp)]
        //public string? creationTime { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("pictures")]
        public List<ObjectId> Pictures { get; set; }
    }
    public class Picture
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("data")]
        public byte[] BinaryData { get; set; }
    }
}

