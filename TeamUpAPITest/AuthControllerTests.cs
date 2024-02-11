using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Controllers;
using TeamUpAPI.Models;
using TeamUpAPI.Services;

namespace TeamUpAPITest
{
    public class AuthControllerTests
    {
        private Mock<IAuthService>? _mockAuthService;
        private AuthController? _controller;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Test]
        public async Task Login_WithCorrectCredentials_ReturnsOkObjectResult()
        {
            // Arrange
            var authRequest = new AuthRequest { Email = "testuser", Password = "testpassword" };
            var authResponse = new AuthOperationResponse { Result = true };
            _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<AuthRequest>()))
                .ReturnsAsync(authResponse);

            // Act
            var actionResult = await _controller.Login(authRequest);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okResult = actionResult.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(authResponse);
        }

        [Test]
        public async Task Login_WithIncorrectCredentials_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var authRequest = new AuthRequest { Email = "wronguser", Password = "wrongpassword" };
            var authResponse = new AuthOperationResponse { Result = false, Errors = new List<string> { "Invalid credentials" } };
            _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<AuthRequest>()))
                            .ReturnsAsync(authResponse);
        
            // Act
            var actionResult = await _controller.Login(authRequest);
        
            // Assert
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = actionResult.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo(authResponse);
        }
        
        [Test]
        public async Task Register_WithValidData_ReturnsOkObjectResult()
        {
            // Arrange
            var userRequest = new AddUserRequest { Username = "newuser", Password = "newpassword", Email = "newuser@example.com",GamesList = new List<Game>(),FriendsList = new List<Friend>()};
            var authResponse = new AuthOperationResponse { Result = true };
            _mockAuthService.Setup(x => x.RegisterAsync(It.IsAny<AddUserRequest>()))
                .ReturnsAsync(authResponse);

            // Act
            var actionResult = await _controller.Register(userRequest);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okResult = actionResult.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(authResponse);
        }
        [Test]
        public async Task Register_WithExistingUsername_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var userRequest = new AddUserRequest { Username = "existinguser", Password = "password", Email = "user@example.com" ,GamesList = new List<Game>(),FriendsList = new List<Friend>()};
            var authResponse = new AuthOperationResponse { Result = false, Errors = new List<string> { "Username already exists" } };
            _mockAuthService.Setup(x => x.RegisterAsync(It.IsAny<AddUserRequest>()))
                .ReturnsAsync(authResponse);

            // Act
            var actionResult = await _controller.Register(userRequest);

            // Assert
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = actionResult.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo(authResponse);
        }
        [Test]
        public async Task LoginWithToken_WithValidCredentials_ReturnsOkAndSetsCookies()
        {
            // Arrange
            var authRequest = new AuthRequest
            {
                Password = "testpassword",
                Email = "asd@asd.pl"
            };
            var authResponse = new AuthResponse { Token = "fake-token", Username = "testuser", UserId = Guid.NewGuid().ToString(),Email = "asd@asd.pl"};
            _mockAuthService.Setup(x => x.LoginWithTokenAsync(authRequest)).ReturnsAsync(authResponse);

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.ControllerContext = controllerContext;

            // Act
            var result = await _controller.LoginWithToken(authRequest);

            // Assert
            result.Should().BeOfType<ActionResult<AuthResponse>>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(authResponse);
            

            _controller.HttpContext.Response.Headers.Should().ContainKey("Set-Cookie");
        }
        [Test]
        public async Task LoginWithToken_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var authRequest = new AuthRequest
            {
                Password = "wrongpassword",
                Email = "abc@abc.com"
            };
            _mockAuthService.Setup(x => x.LoginWithTokenAsync(authRequest)).ReturnsAsync(value: null);

            // Act
            var result = await _controller.LoginWithToken(authRequest);

            // Assert
            result.Result.Should().BeOfType<UnauthorizedResult>();
        }
        [Test]
        public async Task RegisterWithToken_WithValidData_ReturnsOkAndSetsCookies()
        {
            // Arrange
            var addUserRequest = new AddUserRequest
            {
                Username = "newuser",
                Password = "newpassword",
                Email = "newuser@example.com",
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>()
            };
            var authResponse = new AuthResponse { Token = "fake-token", Username = "newuser", UserId = Guid.NewGuid().ToString() };
            _mockAuthService.Setup(x => x.RegisterWithTokenAsync(addUserRequest)).ReturnsAsync(authResponse);

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.ControllerContext = controllerContext;

            // Act
            var result = await _controller.RegisterWithToken(addUserRequest);

            // Assert
            result.Should().BeOfType<ActionResult<AuthResponse>>();
                var okResult = result.Result as OkObjectResult;
                okResult.Value.Should().BeEquivalentTo(authResponse);

            _controller.HttpContext.Response.Headers.Should().ContainKey("Set-Cookie");
        }
        [Test]
        public async Task RegisterWithToken_WithInvalidData_ReturnsUnauthorized()
        {
            // Arrange
            var addUserRequest = new AddUserRequest
            {
                Username = "existinguser",
                Password = "password",
                Email = "user@example.com",
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>()
            };
            _mockAuthService.Setup(x => x.RegisterWithTokenAsync(addUserRequest)).ReturnsAsync(value: null);

            // Act
            var result = await _controller.RegisterWithToken(addUserRequest);

            // Assert
            result.Result.Should().BeOfType<UnauthorizedResult>();
        }

    }
}
