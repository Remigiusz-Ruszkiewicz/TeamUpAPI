using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Models;

namespace TeamUpAPI.Helpers.Mappers
{
    public static class UserToUserResponseMapper
    {
        public static UserResponse UserToUserResponse(User user)
        {
            return new UserResponse()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Age = user.Age,
                StartHour = user.StartHour,
                EndHour = user.EndHour,
                GamesList = user.GamesList,
                FriendsList = user.FriendsList,
            };
        }
    }
}
