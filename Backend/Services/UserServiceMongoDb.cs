using ApiToDatabase.Controllers;
using ApiToDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations.ElementNameValidators;
using System;
using ZstdSharp.Unsafe;
using static System.Net.Mime.MediaTypeNames;

namespace ApiToDatabase.Services;

public class MongoDbContext : IMongoDbServices
{
    private readonly IMongoCollection<User> _userCollection;

    public MongoDbContext(IOptions<UserDatabaseSettings> userDatabaseSettings)
    {
        var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient
        .GetDatabase(userDatabaseSettings.Value.DatabaseName);

        _userCollection = mongoDatabase
        .GetCollection<User>(userDatabaseSettings.Value.UsersCollectionName);
    }

    #region UserApi
    public async Task<List<User>> GetUsersAsync()
    => await _userCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetUserAsync(string id)
    => await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    public async Task<User?> GetUserByNameAsync(string username)
    => await _userCollection.Find(x => x.UserName == username).FirstOrDefaultAsync();
    public async Task<bool> ValidateUserAsync(User user)
    => await _userCollection.CountDocumentsAsync(x => x.UserName == user.UserName && x.Password == user.Password) >= 1 ? true : false;

    public async Task<bool> UserExist(string username)
        => await _userCollection.CountDocumentsAsync(x => x.UserName == username) >= 1 ? true : false;
    public async Task CreateUserAsync(User newUser)
    => await _userCollection.InsertOneAsync(newUser);

    public async Task UpdateUserAsync(string id, User updatedUser)
    => await _userCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

    public async Task RemoveUserAsync(string id)
    => await _userCollection.DeleteOneAsync(x => x.Id == id);
    #endregion

    #region Folders Api
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
 
    public async Task<OkResult> CreateFolder(FolderRequest folderRequest)
    {
        Folder folder = new()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = folderRequest.Name,
            Pictures = new List<ObjectId> { }
        };

        await _userCollection.Database.GetCollection<Folder>("folders").InsertOneAsync(folder);
        return new OkResult();
    }

    public async Task<UpdateResult> UpdateFolderName(FolderRequest folderRequest)
    {
       var updateResult = await _userCollection.Database.GetCollection<Folder>("folders").UpdateOneAsync(
            Builders<Folder>.Filter.Where(x => x.Id == folderRequest.FolderId),
            Builders<Folder>.Update.Set("name", folderRequest.Name)
            );
        return updateResult;
    }
    public async Task<DeleteResult> DeleteFolder(string folderId)
    {
        // EJ testad!!!
        var folderCollection = _userCollection.Database.GetCollection<Folder>("folders");
        
        var folder = await folderCollection.Find(x => x.Id == folderId).FirstOrDefaultAsync();

        var MaybePicturesDeleted = await _userCollection.Database.GetCollection<Picture>("pictures").DeleteManyAsync(Builders<Picture>.Filter.In("_id", folder.Pictures.Select(x => ObjectId.Parse(x.ToString()))));

        var deletedResult = await folderCollection.DeleteOneAsync(
            Builders<Folder>.Filter.Where(x => x.Id == folderId)
            );

        return deletedResult;
    }

    #endregion

    #region PicturesAPI
    public async Task<List<Picture>> GetPictures(string folderId)
    {
        ////Get navigated folder
        //var foldersCollection = _userCollection.Database.GetCollection<Folder>("folders");
        //Folder folder = await foldersCollection.Find(x => x.Id == folderId/*== ObjectId.Parse(folderId)*/).FirstOrDefaultAsync();

        ////Get specific collection and query for all pictures that where inside our navigated folder
        //var picturesCollection = _userCollection.Database.GetCollection<Picture>("pictures");
        //var filter = Builders<Picture>.Filter.In("_id", folder.Pictures.Select(x => ObjectId.Parse(x.ToString())));
        //var pictures = await picturesCollection.Find(filter).ToListAsync();
        //return pictures;
        try
        {

            byte[] imageBytes;
            using (var ms = new MemoryStream())
            {
                var image = File.ReadAllBytes(@"C:\Users\ac.se.jonathanb\OneDrive - Origo hf\Pictures\Picture1.png");
                ms.Write(image, 0, image.Length);
                imageBytes = ms.ToArray();
            }
            byte[] imageBytes2;
            using (var ms = new MemoryStream())
            {
                var image = File.ReadAllBytes(@"C:\Users\ac.se.jonathanb\OneDrive - Origo hf\Pictures\DSCF0332.jpg");
                ms.Write(image, 0, image.Length);
                imageBytes2 = ms.ToArray();
            }

            return new List<Picture>
        {
            new Picture
            {
                Id = "63bca20c1107a8fb0d435e69",
                Name= "picture 11",
                BinaryData = imageBytes
            },
            new Picture
            {
                 Id = "63bca2111107a8fb0d435e6a",
                Name= "picture 32",
                BinaryData = imageBytes2
            }
        };
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task<DeleteResult> DeletePicture(string pictureId)
    {
        var deletedResult = await _userCollection.Database.GetCollection<Picture>("pictures").DeleteOneAsync(x => x.Id == pictureId);
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

        await _userCollection.Database.GetCollection<Picture>("pictures").InsertOneAsync(picture);


            //Folder folderInsert = new() { Id = pictureRequest.FolderId, Pictures = new List<ObjectId> { ObjectId.Parse(picture.Id) } };

            var folder = await _userCollection.Database.GetCollection<Folder>("folders").FindOneAndUpdateAsync(
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


    #endregion
}