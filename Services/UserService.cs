using ApiToDatabase.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace ApiToDatabase.Services;

public class UserService
{
    private readonly IMongoCollection<User> _userCollection;

    public UserService(IOptions<UserDatabaseSettings> userDatabaseSettings)
    {
        var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(userDatabaseSettings.Value.DatabaseName);
        _userCollection = mongoDatabase.GetCollection<User>(userDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task<List<User>> GetUsersAsync() 
    => await _userCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetUserAsync(string id)
    => await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    
    public async Task CreateUserAsync(User newUser) =>
        await _userCollection.InsertOneAsync(newUser);

}