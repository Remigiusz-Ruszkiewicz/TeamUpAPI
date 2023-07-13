using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Models;

namespace TeamUpAPI.Services
{
    public interface ITokenService
    {
        public Task<AuthResponse?> LoginAsync(AuthRequest request);
        public Task<AuthResponse?> RegisterAsync(AddUserRequest userRequest);
    }
}
