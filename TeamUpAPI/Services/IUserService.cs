using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;

namespace TeamUpAPI.Services
{
    public interface IUserService
    {
        public User AddUser(AddUserRequest userRequest);

        public Task<Enums.OperationResult> EditUser(User user);

        public Enums.OperationResult DeleteUser(Guid id);

        public Task<ICollection<User>> GetUsersAsync();

        public Task<User> GetUserByIdAsync(Guid id);

        public Task<ICollection<User>> GetUserFriendsAsync(Guid id);
    }
}
