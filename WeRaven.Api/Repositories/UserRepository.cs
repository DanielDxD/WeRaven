using Microsoft.EntityFrameworkCore;
using WeRaven.Api.Data;
using WeRaven.Api.Models;
using WeRaven.Api.Repositories.Interfaces;

namespace WeRaven.Api.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task CreateAuthAsync(Auth auth)
        {
            await _context.Auths.AddAsync(auth);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);

            var friendFolder = new FriendFolder
            {
                Name = "Melhores amigos",
                UserId = user.Id
            };

            await _context.FriendFolders.AddAsync(friendFolder);
        }

        public async Task<bool> ExistEmail(string email)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Select(x => x.Email)
                .FirstOrDefaultAsync(x => x == email);

            return user != null;
        }

        public async Task<bool> ExistUsername(string username)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Select(x => x.Username)
                .FirstOrDefaultAsync(x => x == username);

            return user != null;
        }

        public async Task<Auth?> GetAuthAsync(Guid userId) => await _context.Auths
                .FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<User?> GetUserAsync(Guid id, bool asNoTracking = true)
        {
            User? user = null;
            if(asNoTracking)
            {
                user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id.Equals(id));
            }
            else
            {
                user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Id.Equals(id));
            }

            return user;
        }

        public async Task<User?> GetUserAsync(string emailOrUsername, bool asNoTracking = true)
        {
            User? user = null;
            if (asNoTracking)
            {
                var userSelected = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == emailOrUsername);
                userSelected ??= await _context.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Username == emailOrUsername);

                user = userSelected;
            }
            else
            {
                var userSelected = await _context.Users
                    .FirstOrDefaultAsync(x => x.Email == emailOrUsername);
                userSelected ??= await _context.Users
                        .FirstOrDefaultAsync(x => x.Username == emailOrUsername);

                user = userSelected;
            }

            return user;
        }

        public void RemoveAuth(Auth auth)
        {
            _context.Auths.Remove(auth);
        }

        public void UpdateAuth(Auth auth)
        {
            _context.Auths.Update(auth);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }
    }
}
