﻿using ApiToDatabase.Models.RequestModels;
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
            try
            {
                //Get navigated folder
                var foldersCollection = _context.Database.GetCollection<Folder>("folders");
                Folder folder = await foldersCollection.Find(x => x.Id == folderId/*== ObjectId.Parse(folderId)*/).FirstOrDefaultAsync();

                //Get specific collection and query for all pictures that where inside our navigated folder
                //var filter = Builders<Picture>.Filter.In("_id", folder.Pictures.Select(x => ObjectId.Parse(x.ToString())));
                var pictures = await _context
                    .Find(Builders<Picture>.Filter.In("_id", folder.Pictures.Select(x => ObjectId.Parse(x.ToString()))))
                    .ToListAsync();
                return pictures;

                //        byte[] imageBytes;
                //        using (var ms = new MemoryStream())
                //        {
                //            var image = File.ReadAllBytes(@"C:\Users\ac.se.jonathanb\OneDrive - Origo hf\Pictures\Picture1.png");
                //            ms.Write(image, 0, image.Length);
                //            imageBytes = ms.ToArray();
                //        }
                //        byte[] imageBytes2;
                //        using (var ms = new MemoryStream())
                //        {
                //            var image = File.ReadAllBytes(@"C:\Users\ac.se.jonathanb\OneDrive - Origo hf\Pictures\DSCF0332.jpg");
                //            ms.Write(image, 0, image.Length);
                //            imageBytes2 = ms.ToArray();
                //        }

                //        return new List<Picture>
                //{
                //    new Picture
                //    {
                //        Id = "63bca20c1107a8fb0d435e69",
                //        Name= "picture 11",
                //        BinaryData = imageBytes
                //    },
                //    new Picture
                //    {
                //         Id = "63bca2111107a8fb0d435e6a",
                //        Name= "picture 32",
                //        BinaryData = imageBytes2
                //    }
                //};
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<DeleteResult> DeletePicture(string pictureId)
        {
            var deletedResult = await _context.DeleteOneAsync(x => x.Id == pictureId);
            return deletedResult;
        }
        //Get the pictures Id to query for in mongoDb
        //List<string> picturesId = new();
        //folder.Pictures.Select(s => ObjectId.Parse(s.ToString()));

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


            //Folder folderInsert = new() { Id = pictureRequest.FolderId, Pictures = new List<ObjectId> { ObjectId.Parse(picture.Id) } };

            var folder = await _context.Database.GetCollection<Folder>("folders").FindOneAndUpdateAsync(
                Builders<Folder>.Filter.Where(fold => fold.Id == pictureRequest.FolderId),
                Builders<Folder>.Update.AddToSet("pictures", ObjectId.Parse(picture.Id))
                );
            var f = folder;

            return folder;
        }
        public Picture CreatePictureTest(byte[] binaryData, string name)
        {
            //var image = File.ReadAllBytes("image.jpg");
            var picture = new Picture
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = name,
                BinaryData = binaryData
            };

            return picture;
        }
    }
}
