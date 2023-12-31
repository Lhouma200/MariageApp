using MariageApp.API.Helpers;
using MariageApp.API.Models;

namespace MariageApp.API.Data
{
    public interface IMariageRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id, bool isCurrentUser);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<Like> GetLike(int userId, int recipientId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetConversation(int userId, int recipientId);
        Task<int> GetUnreadMessagesForUser(int userId);
        Task<Payment> GetPaymentForUser(int userId);
        Task<ICollection<User>> GetLikersOrLikees(int userId, string type);
         Task<ICollection<User>> GetAllUsersExceptAdmin();

    }
}