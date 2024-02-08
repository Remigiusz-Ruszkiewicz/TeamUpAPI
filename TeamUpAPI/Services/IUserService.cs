using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;

namespace TeamUpAPI.Services
{
    public interface IUserService
    {
        public User AddUser(AddUserRequest userRequest);

        public Task<Enums.OperationResult> EditUser(User user);

        public Enums.OperationResult DeleteUser(Guid id);

        public Task<ICollection<UserResponse>> GetUsersAsync();

        public Task<UserResponse?> GetUserByIdAsync(Guid id);
        public Task<UserResponse?> GeturrentUserAsync();

        public Task<ICollection<Friend>> GetUserFriendsAsync();
        public Task<ICollection<UserResponse>> GetRecomendedUsersAsync();
        public Task<ICollection<UserResponse>> GetRecomendedUsersByGameAsync(Guid gameId);
        public Task<Enums.OperationResult> AddToUserFriendsAsync(List<Guid> friendsIds);
        public Task<Enums.OperationResult> DeleteFromUserFriendsAsync(Guid id);
    }
}
