using WeRaven.Api.Models;

namespace WeRaven.Api.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
