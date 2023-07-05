using TeamUpAPI.Models;

namespace TeamUpAPI.Services
{
    public interface ITokenService
    {
        public string CreateToken(User user);
    }
}
