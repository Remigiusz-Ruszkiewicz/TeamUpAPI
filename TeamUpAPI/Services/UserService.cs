using Microsoft.EntityFrameworkCore;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Data;
using TeamUpAPI.Helpers;
using UserToUserResponseMapper = TeamUpAPI.Helpers.Mappers.UserToUserResponseMapper;
using UserToFriendResponseMapper = TeamUpAPI.Helpers.Mappers.UserToFriendResponseMapper;
using TeamUpAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace TeamUpAPI.Services
{
    public class UserService : IUserService
    {
        public UserService(DataContext dbcontext, IGameService gameService)
        {
            Dbcontext = dbcontext;
            GameService = gameService;
        }
        public DataContext Dbcontext { get; }
        private IGameService GameService { get; }

        public async Task<Enums.OperationResult> AddToUserFriendsAsync(Guid userId, List<string> friendsIds)
        {
            User? user = await Dbcontext.Users.FirstOrDefaultAsync((x) => x.Id == userId.ToString());
            if (user != null)
            {
                foreach (string friendId in friendsIds)
                {
                    if (user.FriendsList != null)
                    {
                        if (!user.FriendsList.Contains(friendId))
                        {
                            user.FriendsList += $"{friendId};";
                        }
                    }
                    else
                    {
                        user.FriendsList = $"{friendId};";
                    }
                }
                await Dbcontext.SaveChangesAsync();
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.Error;
        }

        public async Task<Enums.OperationResult> DeleteFromUserFriendsAsync(Guid userId, List<string> friendsIds)
        {
            User? user = await Dbcontext.Users.FirstOrDefaultAsync((x) => x.Id == userId.ToString());
            if (user != null)
            {
                if (user.FriendsList != null)
                {
                    foreach (string friendId in friendsIds)
                    {
                        if (user.FriendsList.Contains(friendId))
                        {
                            user.FriendsList = user.FriendsList.Replace($"{friendId};", "");
                        }
                    }
                }
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
                //string? friends = null;
                string? games = null;
                //if (userRequest.FriendsList != null)
                //{
                //    foreach (string friendId in userRequest.FriendsList)
                //    {
                //        if (friends != null)
                //        {
                //            if (!friends.Contains(friendId))
                //            {
                //                friends += $"{friendId};";
                //            }
                //        }
                //        else
                //        {
                //            friends = $"{friendId};";
                //        }
                //    }
                //}
                if (userRequest.GamesList != null)
                {
                    foreach (string gameId in userRequest.GamesList)
                    {
                        if (games != null)
                        {
                            if (!games.Contains(gameId))
                            {
                                games += $"{gameId};";
                            }
                        }
                        else
                        {
                            games = $"{gameId};";
                        }
                    }
                }
                User user = new(
                    )
                { Id = Guid.NewGuid().ToString(), Username = userRequest.Username, StartHour = userRequest.StartHour, EndHour = userRequest.EndHour, Age = userRequest.Age, Email = userRequest.Email, Password = userRequest.Password, FriendsList = null, GamesList = games };
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
                List<Game>? games = new();
                List<FriendResponse>? friends = null;
                if (user != null)
                {
                    if (user.FriendsList != null)
                    {
                        friends = new();
                        string[] friendsIds = user.FriendsList.Split(';');
                        foreach (string friendId in friendsIds)
                        {
                            if (Guid.TryParse(friendId, out Guid guid))
                            {
                                ICollection<FriendResponse> friendsList = await GetUserFriendsAsync(guid);
                                if (!friendsList.IsNullOrEmpty())
                                {
                                    friends.AddRange(friendsList);
                                }
                            }

                        }

                    }
                    if (user.GamesList != null)
                    {

                        string[] gamesIds = user.GamesList.Split(';');
                        foreach (string gameId in gamesIds)
                        {
                            if (Guid.TryParse(gameId, out Guid guid))
                            {
                                Game? game = await GameService.GetGameByIdAsync(Guid.Parse(gameId));
                                if (game != null)
                                {
                                    games.Add(game);
                                }
                            }

                        }
                    }
                    return UserToUserResponseMapper.UserToUserResponse(user, games, friends);
                }
            }
            catch (Exception ee)
            {

                throw;
            }

            return null;
        }

        public async Task<ICollection<FriendResponse>> GetUserFriendsAsync(Guid id)
        {
#pragma warning disable U2U1201 // Local collections should be initialized with capacity
            List<FriendResponse> friends = new();
#pragma warning restore U2U1201 // Local collections should be initialized with capacity
            User? user = await Dbcontext.Users.SingleOrDefaultAsync((x) => x.Id == id.ToString());
            if (user != null)
            {
                List<string>? friendsIds = user.FriendsList?.Split(';').ToList();
                for (int i = 0; i < friendsIds?.Count; i++)
                {
                    string item = friendsIds[i];
                    User? friend = await Dbcontext.Users.SingleOrDefaultAsync((x) => x.Id == item);
                    if (friend != null)
                    {
                        List<Game>? games = new();
                        if (friend.GamesList != null)
                        {
                            string[] gamesIds = friend.GamesList.Split(';');
                            foreach (string gameId in gamesIds)
                            {
                                if (Guid.TryParse(gameId, out Guid guid))
                                {
                                    Game? game = await GameService.GetGameByIdAsync(Guid.Parse(gameId));
                                    if (game != null)
                                    {
                                        games.Add(game);
                                    }
                                }

                            }
                        }
                        friends.Add(UserToFriendResponseMapper.UserToFriendResponse(friend, games));
                    }
                }
            }

            return friends;
        }

        public async Task<ICollection<UserResponse>> GetUsersAsync(Guid id)
        {
            var users = await Dbcontext.Users.ToListAsync();
            List<UserResponse> userResponses = new();
            foreach (User user in users)
            {

                if (user.Id == id.ToString())
                {
                    continue;
                }
                UserResponse? response = await GetUserByIdAsync(Guid.Parse(user.Id));
                if (response != null)
                    userResponses.Add(response);
            }
            return userResponses;
        }

        public async Task<ICollection<UserResponse>> GetRecomendedUsersAsync(Guid id)
        {
            User? currentUser = await Dbcontext.Users.SingleOrDefaultAsync((x) => x.Id == id.ToString());
            List<string>? friendsIds = new List<string>();
            if (currentUser != null)
            {
                friendsIds = currentUser.FriendsList?.Split(';').ToList();
            }
            List<User> users = await Dbcontext.Users.ToListAsync();
            List<UserResponse> userResponses = new();
            foreach (User user in users)
            {
                if (friendsIds != null && friendsIds.Contains(user.Id))
                {
                    continue;
                }
                if (user.Id == id.ToString())
                {
                    continue;
                }
                UserResponse? response = await GetUserByIdAsync(Guid.Parse(user.Id));
                if (response != null)
                    userResponses.Add(response);
            }
            return userResponses;
        }
    }
}
