using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Options;
using ApiToDatabase.Data;

namespace ApiToDatabase.Services
{
    public class PictureService : IPictureService
    {
        private readonly IMongoCollection<Picture> _context;
        public PictureService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _context = mongoDatabase.GetCollection<Picture>("pictures");
        }

        public async Task<List<Picture>> GetPictures(string folderId)
        {
                //Get navigated folder
                Folder folder = await _context.Database.GetCollection<Folder>("folders")
                    .Find(x => x.Id == folderId).FirstOrDefaultAsync();

                //Get specific collection and query for all pictures that where inside our navigated folder
                return await _context
                    .Find(Builders<Picture>.Filter.In("_id", folder.Pictures.Select(x => ObjectId.Parse(x.ToString()))))
                    .ToListAsync();
        }
        public async Task<DeleteResult> DeletePicture(string folderId, string pictureId)
        {
            var res = await _context.Database.GetCollection<Folder>("folders").FindOneAndUpdateAsync(
                Builders<Folder>.Filter.Where(fold => fold.Id == folderId),
                Builders<Folder>.Update.Pull(x => x.Pictures, ObjectId.Parse(pictureId)));

            return await _context.DeleteOneAsync(x => x.Id == pictureId);
        }

        public async Task<Folder> CreatePicture(PictureRequest pictureRequest)
        {
            byte[] imageBytes = Convert.FromBase64String(pictureRequest.Data);
            var picture = new Picture
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = pictureRequest.Name,
                BinaryData = imageBytes
            };

            await _context.InsertOneAsync(picture);
           return await _context.Database.GetCollection<Folder>("folders").FindOneAndUpdateAsync(
                Builders<Folder>.Filter.Where(fold => fold.Id == pictureRequest.FolderId),
                Builders<Folder>.Update.AddToSet(x => x.Pictures, ObjectId.Parse(picture.Id)));
        }
    }
}
