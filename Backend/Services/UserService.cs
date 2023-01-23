using ApiToDatabase.Models.RequestModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using ApiToDatabase.Data;
using ApiToDatabase.Models.DTOModels;

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

        public async Task<User> GetUserById(string id)
            => await _context.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<User> GetUserByName(string username)
            => await _context.Find(x => x.UserName == username).FirstOrDefaultAsync();
        public async Task<bool> ValidateUser(UserRequest userRequest)
        {
            var userInDatabase = await _context.Find(x => x.UserName == userRequest.Username).FirstOrDefaultAsync();
            var isValid = _passwordHasher.VerifyHashedPassword(userRequest, userInDatabase.Password, userRequest.Password);
            return isValid == PasswordVerificationResult.Success;
        }

        public async Task<bool> UserExist(string username)
            => await _context.CountDocumentsAsync(x => x.UserName == username) >= 1 ? true : false;

        public async Task<User> CreateUser(UserRequest userRequest)
        {
            User newUser = new()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserName = userRequest.Username,
                Password = _passwordHasher.HashPassword(userRequest, userRequest.Password),
            };
            await _context.InsertOneAsync(newUser);
            return newUser;
        }

        public async Task DeleteUser(string userId)
        {
            var folders = await _context.Database.GetCollection<Folder>("folders")
                .Find(Builders<Folder>.Filter.Where(x => x.UserId == ObjectId.Parse(userId)))
                .ToListAsync();

            foreach (var folder in folders)
            {
                await _context.Database.GetCollection<Picture>("pictures")
                    .DeleteManyAsync(Builders<Picture>.Filter.In("_id", folder.Pictures.Select(x => x)));

            }
            await _context.Database.GetCollection<Folder>("folders")
                .DeleteManyAsync(Builders<Folder>.Filter.In(f => f.Id, folders.Select(x => x.Id)));

            await _context.DeleteOneAsync(x => x.Id == userId);
        }
    }
}
