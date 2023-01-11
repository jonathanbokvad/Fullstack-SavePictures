using ApiToDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations.ElementNameValidators;

namespace ApiToDatabase.Services;

public class UserServiceMongoDb : IUserService
{
    private readonly IMongoCollection<User> _userCollection;

    public UserServiceMongoDb(IOptions<UserDatabaseSettings> userDatabaseSettings)
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

    #region FolderAndPicturesApi
    public async Task<List<Folder>> GetFolders()
    {
        var folderss = _userCollection.Database.GetCollection<Folder>("folders");

        List<Folder> folderslist = await folderss.Find(_ => true).ToListAsync();

        //foreach (var doc in folderslist)
        //{
        //    var pictureArray = doc.Pictures;
        //}


        //var filter = Builders<Folder>.Filter.AnyEq("pictures", true);

        //var filterList= await _userCollection.Database.GetCollection<Folder>("folders").Find(filter).ToListAsync();


        //_userCollection.Database.GetCollection<Pictures>("pictures");
        //var folders = await folderslist;
        return folderslist;
        //var listfolders = folderss.Find(_ => true).ToListAsync();
        //return listfolders;

        //Task<ActionResult<List<Folder>>>
    //    try
    //    {

        //    var _folders = _userCollection.Database.GetCollection<Folder>("folders");
        //        //var _folders = database.GetCollection<Folder>("folders");
        //        var pipeline = new[] {
        //  new BsonDocument("$lookup", new BsonDocument
        //  {
        //    { "from", "pictures" },
        //    { "localField", "pictures" },
        //    { "foreignField", "_id" },
        //    { "as", "pictures" }
        //  })
        //};

        //        var result = await _folders.AggregateAsync<Folder>(pipeline);
        //        return result.ToList();
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        throw;
        //    }
        //    return null;
    }

    #endregion

}