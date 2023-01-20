using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ApiToDatabase.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _context;
        private readonly IPasswordHasher<UserRequest> _passwordHasher;
        public UserService(IMongoCollection<User> context, IPasswordHasher<UserRequest> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<User>> GetUsersAsync()
        => await _context.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserAsync(string id)
        => await _context.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<User?> GetUserByNameAsync(string username)
        => await _context.Find(x => x.UserName == username).FirstOrDefaultAsync();
        public async Task<bool> ValidateUserAsync(UserRequest userRequest)
        {
            //EJ testadd!!!!
            var res = await _context
                .CountDocumentsAsync(x => x.UserName == userRequest.UserName && 
            _passwordHasher.VerifyHashedPassword(userRequest, x.Password, userRequest.Password) == PasswordVerificationResult.Success) >= 1 ? true : false;

            return res;
        }

        //public async Task<bool> ValidateUserAsync(User user)
        //{
        //    await _userCollection.CountDocumentsAsync(x => x.UserName == user.UserName && x.Password == user.Password) >= 1 ? true : false;

        //}


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

        public async Task UpdateUserAsync(string id, User updatedUser)
        => await _context.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveUserAsync(string id)
        => await _context.DeleteOneAsync(x => x.Id == id);
    }
}
