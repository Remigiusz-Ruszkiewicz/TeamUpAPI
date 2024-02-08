using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Models;

namespace TeamUpAPI.Helpers.Mappers
{
    public static class UserToFriendResponseMapper
    {
        public static FriendResponse UserToFriendResponse(User user)
        {
            return new FriendResponse()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Age = user.Age,
                StartHour = user.StartHour,
                EndHour = user.EndHour,
                GamesList = user.GamesList,
            };
        }
    }
}