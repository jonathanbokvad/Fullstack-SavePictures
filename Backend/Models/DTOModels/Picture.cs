using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ApiToDatabase.Models.DTOModels
{
    public class Picture
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("binary")]
        public byte[] BinaryData { get; set; }
    }
}

