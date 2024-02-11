using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamUpAPI.Controllers;
using TeamUpAPI.Services;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;

namespace TeamUpAPITest
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService>? _mockUserService;
        private UserController? _controller;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Test]
        public async Task AllUsers_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<UserResponse> { new UserResponse { Id = Guid.NewGuid().ToString(),Email = "abc@abc.com", Username = "User1",FriendsList = new List<Friend>(),GamesList = new List<Game>()}, new UserResponse { Id = Guid.NewGuid().ToString(),Email = "abc@abc.com", Username = "User2",FriendsList = new List<Friend>(),GamesList = new List<Game>() } };
            _mockUserService.Setup(s => s.UsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.AllUsers();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(users);
        }

        [Test]
        public async Task UserById_WithExistingId_ReturnsUser()
        {
            // Arrange
            var user = new UserResponse { Id = Guid.NewGuid().ToString(),Email = "abc@abc.com",Username = "User1" ,FriendsList = new List<Friend>(),GamesList = new List<Game>()};
            _mockUserService.Setup(s => s.UserByIdAsync(Guid.Parse(user.Id))).ReturnsAsync(user);

            // Act
            var result = await _controller.UserById(Guid.Parse(user.Id));

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(user);
        }

        [Test]
        public async Task UserById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockUserService.Setup(s => s.UserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(value: null);

            // Act
            var result = await _controller.UserById(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public Task AddUser_WithNewUser_ReturnsOkResult()
        {
            // Arrange
            var user = new User { FriendsList = new List<Friend>(), GamesList = new List<Game>() };
            var addUserRequest = new AddUserRequest { Username = "NewUser", Password = "password", Email = "user@example.com",FriendsList = new List<Friend>(),GamesList = new List<Game>() };
            _mockUserService.Setup(s => s.AddUser(It.IsAny<AddUserRequest>())).Returns(user);

            // Act
            var result = _controller.AddUser(addUserRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(user);
            return Task.CompletedTask;
        }
        [Test]
        public async Task UpdateUser_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid()
                    .ToString(),
                UserName = "UpdatedUser",
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>()
            };
            _mockUserService.Setup(s => s.EditUser(It.IsAny<User>())).ReturnsAsync(Enums.OperationResult.Ok);

            // Act
            var result = await _controller.UpdateUser(user);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeEquivalentTo(Enums.OperationResult.Ok);
        }
        [Test]
        public void DeleteUser_WithExistingUserIdInCookie_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() // Tworzenie nowego kontekstu HTTP
            };
            // Symulowanie ciasteczka
            controllerContext.HttpContext.Request.Headers["Cookie"] = $"X-Id={userId}";

            _controller.ControllerContext = controllerContext;

            _mockUserService.Setup(s => s.DeleteUser(Guid.Parse(userId)))
                .Returns(Enums.OperationResult.Ok);

            // Act
            var result = _controller.DeleteUser();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task UserFriends_ReturnsListOfFriends()
        {
            // Arrange
            var friends = new List<Friend> { new Friend
                {
                    Id = Guid.NewGuid()
                        .ToString(),
                    Email = "asd@asd.com",
                    FriendsList = new List<Friend>(),
                    GamesList = new List<Game>(),
                    UserName = "asd"
                }
            };
            _mockUserService.Setup(s => s.UserFriendsAsync()).ReturnsAsync(friends);

            // Act
            var result = await _controller.UserFriends();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeEquivalentTo(friends);
        }
        [Test]
        public async Task AddToUserFriends_WithValidFriendsIds_ReturnsOkResult()
        {
            // Arrange
            var friendsIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockUserService.Setup(s => s.AddToUserFriendsAsync(It.IsAny<List<Guid>>())).ReturnsAsync(Enums.OperationResult.Ok);

            // Act
            var result = await _controller.AddToUserFriends(friendsIds);

            // Assert
            result.Should().Be(Enums.OperationResult.Ok);
        }
        [Test]
        public async Task DeleteFromUserFriends_WithValidFriendId_ReturnsOkResult()
        {
            // Arrange
            var friendId = Guid.NewGuid();
            _mockUserService.Setup(s => s.DeleteFromUserFriendsAsync(friendId)).ReturnsAsync(Enums.OperationResult.Ok);

            // Act
            var result = await _controller.DeleteFromUserFriends(friendId);

            // Assert
            result.Should().Be(Enums.OperationResult.Ok);
        }
        [Test]
        public async Task RecommendedUsers_WithoutGameId_ReturnsListOfRecommendedUsers()
        {
            // Arrange
            var recommendedUsers = new List<UserResponse> { new UserResponse
                {
                    Id = Guid.NewGuid()
                        .ToString(),
                    Username = "RecommendedUser1",
                    Email = "asd@asd.com",
                    FriendsList = new List<Friend>(),
                    GamesList = new List<Game>(),
                }
            };
            _mockUserService.Setup(s => s.RecommendedUsersAsync()).ReturnsAsync(recommendedUsers);

            // Act
            var result = await _controller.RecommendedUsers();

            // Assert
            result.Should().BeEquivalentTo(recommendedUsers);
        }
        [Test]
        public async Task CurrentUserInfo_ReturnsCurrentUserInfo()
        {
            // Arrange
            var currentUserInfo = new UserResponse
            {
                Id = Guid.NewGuid()
                    .ToString(),
                Username = "CurrentUser",
                Email = "asd@asd.com",
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>(),
            };
            _mockUserService.Setup(s => s.CurrentUserAsync()).ReturnsAsync(currentUserInfo);

            // Act
            var result = await _controller.CurrentUserInfo();

            // Assert
            result.Should().BeEquivalentTo(currentUserInfo);
        }
        [Test]
        public void DeleteUser_WithoutUserIdInCookie_ReturnsNotFound()
        {
            // Arrange
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            
            _controller.ControllerContext = controllerContext;

            // Act
            var result = _controller.DeleteUser();

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Value.Should().Be("Bad Token try with valid one");
        }

    }
}
