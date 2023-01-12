using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ApiToDatabase.Models
{
    public class Folder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("pictures")]
        public List<ObjectId> Pictures { get; set; }
    }
    public class Picture
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("data")]
        public byte[] Data { get; set; }
    }
}

