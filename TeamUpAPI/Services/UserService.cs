using Microsoft.EntityFrameworkCore;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Data;
using TeamUpAPI.Helpers;
using UserToUserResponseMapper = TeamUpAPI.Helpers.Mappers.UserToUserResponseMapper;
using UserToFriendResponseMapper = TeamUpAPI.Helpers.Mappers.UserToFriendResponseMapper;
using TeamUpAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TeamUpAPI.Services
{
    public class UserService : IUserService
    {
        public UserService(DataContext dbcontext, UserManager<User> userManager)
        {
            Dbcontext = dbcontext;
            _userManager = userManager;
        }
        public DataContext Dbcontext { get; }
        private readonly UserManager<User> _userManager;

        public async Task<Enums.OperationResult> AddToUserFriendsAsync(List<Guid> friendsIds)
        {
            var user = await _userManager.GetUserAsync(ClaimsPrincipal.Current);
            if (user != null)
            {
                foreach (Guid friendId in friendsIds)
                {
                    if (!user.FriendsList.Any((x) => x.Id == friendId.ToString()))
                    {
                        User? friendProfile = await Dbcontext.Users.FirstOrDefaultAsync((x) => x.Id == friendId.ToString());
                        user.FriendsList.Add(new Friend() { Id = friendId.ToString(), Email = friendProfile.Email, UserName = friendProfile.Email, Age = friendProfile.Age, StartHour = friendProfile.StartHour, EndHour = friendProfile.EndHour, FriendsList = friendProfile.FriendsList, GamesList = friendProfile.GamesList });
                        Dbcontext.Users.Update(user);
                    }
                }
                await Dbcontext.SaveChangesAsync();
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.Error;
        }

        public async Task<Enums.OperationResult> DeleteFromUserFriendsAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(ClaimsPrincipal.Current);

            if (user == null) return Enums.OperationResult.Error;

            var friend = user.FriendsList.FirstOrDefault(f => f.Id == id.ToString());
            if (friend != null)
            {
                user.FriendsList.Remove(friend);
                await Dbcontext.SaveChangesAsync();
                return Enums.OperationResult.Ok;
            }

            return Enums.OperationResult.Error;
        }

        public User AddUser(AddUserRequest userRequest)
        {
            try
            {
                Dbcontext.Database.BeginTransaction();
                User user = new(
                    )
                { Id = Guid.NewGuid().ToString(), UserName = userRequest.Username, StartHour = userRequest.StartHour, EndHour = userRequest.EndHour, Age = userRequest.Age, Email = userRequest.Email, PasswordHash = userRequest.Password, FriendsList = userRequest.FriendsList, GamesList = userRequest.GamesList };
                Dbcontext.Set<User>().Add(user);
                Dbcontext.SaveChanges();
                Dbcontext.Database.CommitTransaction();
                return user;
            }
            catch (Exception)
            {
                Dbcontext.Database.RollbackTransaction();
                throw;
            }
        }

        public Enums.OperationResult DeleteUser(Guid id)
        {
            try
            {
                var user = Dbcontext.Set<User>()
                    .FirstOrDefault(x => x.Id == id.ToString());

                if (user == null)
                {
                    return Enums.OperationResult.NotFound;
                }

                Dbcontext.Database.BeginTransaction();

                Dbcontext.Set<User>().Remove(user);

                Dbcontext.SaveChanges();
                Dbcontext.Database.CommitTransaction();
                return Enums.OperationResult.Ok;
            }
            catch (Exception)
            {
                Dbcontext.Database?.RollbackTransaction();
                return Enums.OperationResult.Error;
            }
        }

        public async Task<Enums.OperationResult> EditUser(User user)
        {
            Dbcontext.Users.Update(user);
            await Dbcontext.SaveChangesAsync();
            return Enums.OperationResult.Ok;
        }

        public async Task<UserResponse?> GetUserByIdAsync(Guid id)
        {
            try
            {
                User? user = await Dbcontext.Users.SingleOrDefaultAsync((x) => x.Id == id.ToString());
                if (user != null) return UserToUserResponseMapper.UserToUserResponse(user);
            }
            catch (Exception ee)
            {
                return null;
            }

            return null;
        }

        public async Task<ICollection<Friend>> GetUserFriendsAsync()
        {
            var user = await _userManager.GetUserAsync(ClaimsPrincipal.Current);
            if (user != null)
            {
                return user.FriendsList;
            }
            return new List<Friend>();
        }

        public async Task<ICollection<UserResponse>> GetUsersAsync()
        {
            var users = await Dbcontext.Users.ToListAsync();
            List<UserResponse> userResponses = new();
            var userId = _userManager.GetUserId(ClaimsPrincipal.Current);
            foreach (User user in users)
            {
                if (user.Id == userId)
                {
                    continue;
                }
                UserResponse? response = await GetUserByIdAsync(Guid.Parse(user.Id));
                if (response != null)
                    userResponses.Add(response);
            }
            return userResponses;
        }

        public async Task<ICollection<UserResponse>> GetRecomendedUsersAsync()
        {
            var currentUser = await _userManager.GetUserAsync(ClaimsPrincipal.Current);
            if (currentUser != null)
            {
                List<User> users = await Dbcontext.Users.ToListAsync();
                List<UserResponse> userResponses = new();
                foreach (User user in users)
                {
                    if (currentUser.FriendsList.Any((x) => x.Id == user.Id))
                    {
                        continue;
                    }
                    if (user.Id == currentUser.Id)
                    {
                        continue;
                    }
                    UserResponse? response = await GetUserByIdAsync(Guid.Parse(user.Id));
                    if (response != null)
                        userResponses.Add(response);
                }
                return userResponses;
            }
            return new List<UserResponse>();
        }
        public async Task<ICollection<UserResponse>> GetRecomendedUsersByGameAsync(Guid gameId)
        {
            var currentUser = await _userManager.GetUserAsync(ClaimsPrincipal.Current);
            if (currentUser != null)
            {
                List<User> users = await Dbcontext.Users.ToListAsync();
                List<UserResponse> userResponses = new();
                foreach (User user in users)
                {
                    if (currentUser.FriendsList.Any((x) => x.Id == user.Id))
                    {
                        continue;
                    }
                    if (user.Id == currentUser.Id)
                    {
                        continue;
                    }
                    if (user.GamesList.IsNullOrEmpty())
                    {
                        continue;
                    }
                    UserResponse? response = await GetUserByIdAsync(Guid.Parse(user.Id));
                    if (response != null)
                        userResponses.Add(response);
                }
                return userResponses;
            }
            return new List<UserResponse>();
        }

        public async Task<UserResponse?> GeturrentUserAsync() => UserToUserResponseMapper.UserToUserResponse(await _userManager.GetUserAsync(ClaimsPrincipal.Current));
    }
}
