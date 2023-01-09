using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ApiToDatabase.Models
{
    public class Folder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }
        public string Name { get; set; }
        public List<ObjectId> Pictures { get; set; }
    }
    public class Picture
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}

