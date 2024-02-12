using FluentAssertions;
using TeamUpAPI.Helpers.Mappers;
using TeamUpAPI.Models;

namespace TeamUpAPITest.Helpers.Mappers
{
    public class UserToUserResponseMapperTests
    {
        [Test]
        public void UserToUserResponse_MapsPropertiesCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testUser",
                Email = "test@example.com",
                Age = 30,
                StartHour = 9,
                EndHour = 17,
                GamesList = new List<Game>(),
                FriendsList = new List<Friend>() 
            };

            // Act
            var result = UserToUserResponseMapper.UserToUserResponse(user);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
            result.Username.Should().Be(user.UserName);
            result.Email.Should().Be(user.Email);
            result.Age.Should().Be(user.Age);
            result.StartHour.Should().Be(user.StartHour);
            result.EndHour.Should().Be(user.EndHour);
            result.GamesList.Should().BeEquivalentTo(user.GamesList);
            result.FriendsList.Should().BeEquivalentTo(user.FriendsList);
        }
    }
}
