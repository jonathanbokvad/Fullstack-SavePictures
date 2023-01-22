using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using ApiToDatabase.Data;

namespace ApiToDatabase.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _context;
        private readonly IPasswordHasher<UserRequest> _passwordHasher;
        public UserService(IOptions<DatabaseSettings> userDatabaseSettings, IPasswordHasher<UserRequest> passwordHasher)
        {
            var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDatabaseSettings.Value.DatabaseName);
            _context = mongoDatabase.GetCollection<User>("Users");
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> GetUserAsync(string id)
            => await _context.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<User?> GetUserByNameAsync(string username)
            => await _context.Find(x => x.UserName == username).FirstOrDefaultAsync();
        public async Task<bool> ValidateUserAsync(UserRequest userRequest)
        {
            var userInDatabase = await _context.Find(x => x.UserName == userRequest.UserName).FirstOrDefaultAsync();
            var isValid = _passwordHasher.VerifyHashedPassword(userRequest, userInDatabase.Password, userRequest.Password);
            return isValid == PasswordVerificationResult.Success ? true : false;
        }

        public async Task<bool> UserExist(string username)
            => await _context.CountDocumentsAsync(x => x.UserName == username) >= 1 ? true : false;

        public async Task<User> CreateUserAsync(UserRequest userRequest)
        {
            User newUser = new()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserName = userRequest.UserName,
                Password = _passwordHasher.HashPassword(userRequest, userRequest.Password),
            };
            await _context.InsertOneAsync(newUser);
            return newUser;
        }


        //Ej använda ännu!!!!!!!!!!!!!!!!!
        public async Task UpdateUserAsync(string id, User updatedUser)
            => await _context.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveUserAsync(string id)
            => await _context.DeleteOneAsync(x => x.Id == id);
    }
}
