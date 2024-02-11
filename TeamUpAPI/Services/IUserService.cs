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

        public Task<ICollection<UserResponse>> UsersAsync();

        public Task<UserResponse?> UserByIdAsync(Guid id);
        public Task<UserResponse?> CurrentUserAsync();

        public Task<ICollection<Friend>> UserFriendsAsync();
        public Task<ICollection<UserResponse>> RecommendedUsersAsync();
        public Task<ICollection<UserResponse>> RecommendedUsersByGameAsync(Guid gameId);
        public Task<Enums.OperationResult> AddToUserFriendsAsync(List<Guid> friendsIds);
        public Task<Enums.OperationResult> DeleteFromUserFriendsAsync(Guid id);
    }
}
