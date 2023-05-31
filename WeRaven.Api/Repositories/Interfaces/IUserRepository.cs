using WeRaven.Api.Models;

namespace WeRaven.Api.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase
    {
        Task CreateUserAsync(User user);
        Task CreateAuthAsync(Auth auth);
        Task<User?> GetUserAsync(Guid id, bool asNoTracking = true);
        Task<User?> GetUserAsync(string emailOrUsername, bool asNoTracking = true);
        Task<Auth?> GetAuthAsync(Guid userId);
        Task<bool> ExistEmail(string email);
        Task<bool> ExistUsername(string username);
        void UpdateUser(User user);
        void UpdateAuth(Auth auth);
        void RemoveAuth(Auth auth);
    }
}
