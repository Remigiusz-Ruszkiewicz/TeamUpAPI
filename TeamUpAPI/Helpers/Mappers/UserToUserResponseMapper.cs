using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Models;

namespace TeamUpAPI.Helpers.Mappers
{
    public static class UserToUserResponseMapper
    {
        public static UserResponse UserToUserResponse(User user, List<Game>? games, List<FriendResponse>? friends)
        {
            return new UserResponse()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Age = user.Age,
                StartHour = user.StartHour,
                EndHour = user.EndHour,
                GamesList = games,
                FriendsList = friends,
            };
        }
    }
}
