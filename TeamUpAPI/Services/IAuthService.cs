using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;

namespace TeamUpAPI.Services
{
    public interface IAuthService
    {
        public Task<AuthOperationResponse> LoginAsync(AuthRequest authRequest);
        public Task<AuthOperationResponse> RegisterAsync(AddUserRequest userRequest);
        public Task<AuthResponse?> LoginWithTokenAsync(AuthRequest authRequest);
        public Task<AuthResponse?> RegisterWithTokenAsync(AddUserRequest userRequest);
    }
}
