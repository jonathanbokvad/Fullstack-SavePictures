using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace ApiToDatabase.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _context;

        public async Task<List<User>> GetUsersAsync()
        => await _context.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserAsync(string id)
        => await _context.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<User?> GetUserByNameAsync(string username)
        => await _context.Find(x => x.UserName == username).FirstOrDefaultAsync();
        public async Task<bool> ValidateUserAsync(UserRequest user)
        {
            IPasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            var UserInDatabase = await _context.Find(x => x.UserName == user.UserName).FirstOrDefaultAsync();

            //await _userCollection.CountDocumentsAsync(x => x.UserName == user.UserName && x.PasswordHash == user.PasswordHash) >= 1 ? true : false;
            return true;
        }
        //public async Task<bool> ValidateUserAsync(User user)
        //{
        //    await _userCollection.CountDocumentsAsync(x => x.UserName == user.UserName && x.Password == user.Password) >= 1 ? true : false;

        //}


        public async Task<bool> UserExist(string username)
            => await _context.CountDocumentsAsync(x => x.UserName == username) >= 1 ? true : false;
        public async Task CreateUserAsync(User newUser)
        => await _context.InsertOneAsync(newUser);

        public async Task UpdateUserAsync(string id, User updatedUser)
        => await _context.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveUserAsync(string id)
        => await _context.DeleteOneAsync(x => x.Id == id);
    }
}
