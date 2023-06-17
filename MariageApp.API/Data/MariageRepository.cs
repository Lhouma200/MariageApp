using MariageApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MariageApp.API.Data
{
    public class MariageRepository :IMariageRepository
    {
        private readonly DataContext _context;
        public MariageRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
           _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(u=>u.Photos).FirstOrDefaultAsync(u=>u.Id==id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
           var users = await _context.Users.Include(u=>u.Photos).ToListAsync();
           return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()>0;
        }
    }
    }
