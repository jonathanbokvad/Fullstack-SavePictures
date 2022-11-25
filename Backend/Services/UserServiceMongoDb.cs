using ApiToDatabase.Models;
using Microsoft.Extensions.Options;
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

    public async Task<List<User>> GetUsersAsync() 
    => await _userCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetUserAsync(string id)
    => await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task CreateUserAsync(User newUser) 
    => await _userCollection.InsertOneAsync(newUser);

    public async Task UpdateUserAsync(string id, User updatedUser)
    => await _userCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

    public async Task RemoveUserAsync(string id)
    => await _userCollection.DeleteOneAsync(x => x.Id == id);
}