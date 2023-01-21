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
        public async Task<List<Folder>> GetFolders()
        {
            try
            {
                //var foldersCollection = _userCollection.Database.GetCollection<Folder>("folders");

                //List<Folder> folderslist = await foldersCollection.Find(_ => true).ToListAsync();
                //return folderslist;

                return new List<Folder> {
                new Folder {
                Id = "63bca2071107a8fb0d435e68",
                Name = "Folder 1",
                Pictures = new List<ObjectId>{new ObjectId("63bca20c1107a8fb0d435e69"), new ObjectId("63bca2111107a8fb0d435e6a") }

            },
            new Folder {
                Id = "63bf0f7fe6f1167a5bbbf6cf",
                Name = "Folder 2",
             Pictures = new List<ObjectId>{new ObjectId("63bca20c1107a8fb0d435e69") }
            }};
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<OkResult> CreateFolder(CreateFolderRequest createFolderRequest)
        {
            Folder folder = new()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = createFolderRequest.Name,
                Pictures = new List<ObjectId> { },
                UserId = createFolderRequest.UserId
            };
            
            await _context.Database.GetCollection<Folder>("folders").InsertOneAsync(folder);
            return new OkResult();
        }

        public async Task<UpdateResult> UpdateFolderName(FolderRequest folderRequest)
        {
            var updateResult = await _context.Database.GetCollection<Folder>("folders").UpdateOneAsync(
                 Builders<Folder>.Filter.Where(x => x.Id == folderRequest.FolderId),
                 Builders<Folder>.Update.Set("name", folderRequest.Name)
                 );
            return updateResult;
        }
        public async Task<DeleteResult> DeleteFolder(string folderId)
        {
            // EJ testad!!!
            var folderCollection = _context.Database.GetCollection<Folder>("folders");

            var folder = await folderCollection.Find(x => x.Id == folderId).FirstOrDefaultAsync();

            var MaybePicturesDeleted = await _context.Database.GetCollection<Picture>("pictures").DeleteManyAsync(Builders<Picture>.Filter.In("_id", folder.Pictures.Select(x => ObjectId.Parse(x.ToString()))));

            var deletedResult = await folderCollection.DeleteOneAsync(
                Builders<Folder>.Filter.Where(x => x.Id == folderId)
                );

            return deletedResult;
        }
    }
}
