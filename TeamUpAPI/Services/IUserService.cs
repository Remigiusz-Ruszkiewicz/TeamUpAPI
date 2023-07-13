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

        public Task<ICollection<UserResponse>> GetUsersAsync(Guid id);

        public Task<UserResponse?> GetUserByIdAsync(Guid id);

        public Task<ICollection<FriendResponse>> GetUserFriendsAsync(Guid id);
        public Task<ICollection<UserResponse>> GetRecomendedUsersAsync(Guid id);
        public Task<ICollection<UserResponse>> GetRecomendedUsersByGameAsync(Guid id, string gameId);
        public Task<Enums.OperationResult> AddToUserFriendsAsync(Guid userId, List<string> friendsIds);
        public Task<Enums.OperationResult> DeleteFromUserFriendsAsync(Guid userId, List<string> friendsIds);
    }
}
