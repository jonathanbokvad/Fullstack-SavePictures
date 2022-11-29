using ApiToDatabase.Data;
using ApiToDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace ApiToDatabase.Services
{
    public class UserServicePostgres
    {
        UserDbContext _context;
        public UserServicePostgres(UserDbContext context)
        {
            _context= context;
        }
        public async Task CreateUserAsync(User newUser)
        { 
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetUserAsync(string id)
            => await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<User?> GetUserByNameAsync(string username)
            => await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            
        public async Task<List<User>> GetUsersAsync()
            => await _context.Users.ToListAsync();

        public async Task RemoveUserAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public async Task UpdateUserAsync(string id, User updatedUser)
        {
            var user =  await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            _context.Users.Update(updatedUser);
           await _context.SaveChangesAsync();
        }
    }
}
