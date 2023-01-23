using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiToDatabase.Models.DTOModels;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("UserName")]
    public string UserName { get; set; }

    [BsonElement("Password")]
    public string Password { get; set; }
}