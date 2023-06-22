using MariageApp.API.Models;

namespace MariageApp.API.Data
{
    public interface IMariageRepository
    {
          void Add <T>(T entity) where T:class;
         void Delete <T>(T entity) where T:class;
         Task<bool> SaveAll();
         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);
          Task<Photo> GetPhoto(int id);
          Task<Photo> GetMainPhotoForUser(int userId);
    }
}