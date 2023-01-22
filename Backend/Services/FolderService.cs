using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ApiToDatabase.Data;

namespace ApiToDatabase.Services
{
    public class FolderService: IFolderService
    {
        private readonly IMongoCollection<Folder> _context;
        public FolderService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _context = mongoDatabase.GetCollection<Folder>("folders");
        }

        public async Task<List<Folder>> GetFolders(string userId)
        {
            return await _context
                .Find(Builders<Folder>.Filter.Where(x => x.UserId == ObjectId.Parse(userId)))
                .ToListAsync();
        }

        public async Task<OkResult> CreateFolder(CreateFolderRequest createFolderRequest)
        {
            Folder folder = new()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = createFolderRequest.Name,
                CreationDate = DateTime.Now,
                Pictures = new List<ObjectId> { },
                UserId = ObjectId.Parse(createFolderRequest.UserId)
            };
            
            await _context.InsertOneAsync(folder);
            return new OkResult();
        }

        public async Task<UpdateResult> UpdateFolderName(FolderRequest folderRequest)
        {
            return await _context.UpdateOneAsync(
                 Builders<Folder>.Filter.Where(x => x.Id == folderRequest.FolderId),
                 Builders<Folder>.Update.Set("name", folderRequest.Name)
                 );
        }
        public async Task<DeleteResult> DeleteFolder(string folderId)
        {

            var folder = await _context.Find(x => x.Id == folderId).FirstOrDefaultAsync();

            var MaybePicturesDeleted = await _context.Database.GetCollection<Picture>("pictures")
                .DeleteManyAsync(Builders<Picture>.Filter.In("_id", folder.Pictures.Select(x => ObjectId.Parse(x.ToString()))));

            return await _context.DeleteOneAsync(
                Builders<Folder>.Filter.Where(x => x.Id == folderId)
                );
        }
    }
}
