using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using TeamUpAPI.Models;
using TeamUpAPI.Services;
using TeamUpAPI.Contracts.Requests;

namespace TeamUpAPITest.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<UserManager<User>>? _mockUserManager;
        private Mock<SignInManager<User>>? _mockSignInManager;
        private AuthService? _authService;

        [SetUp]
        public void SetUp()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
            _mockSignInManager = new Mock<SignInManager<User>>(_mockUserManager.Object, contextAccessorMock.Object, userPrincipalFactoryMock.Object, null, null, null, null);

            _authService = new AuthService(_mockUserManager.Object, _mockSignInManager.Object);
        }
        
        [Test]
        public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
        {
            // Arrange
            var authRequest = new AuthRequest { Email = "user@example.com", Password = "Password123" };
            var user = new User
            {
                Email = authRequest.Email,
                UserName = "user",
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>()
            };
            _mockUserManager.Setup(x => x.FindByEmailAsync(authRequest.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, authRequest.Password)).ReturnsAsync(true);
            _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, authRequest.Password, false, false))
                .ReturnsAsync(SignInResult.Success);
        
            // Act
            var result = await _authService.LoginAsync(authRequest);
        
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeTrue();
        }
        [Test]
        public async Task LoginAsync_WithInvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var authRequest = new AuthRequest { Email = "user@example.com", Password = "WrongPassword" };
            _mockUserManager.Setup(x => x.FindByEmailAsync(authRequest.Email)).ReturnsAsync((User)null);

            // Act
            var result = await _authService.LoginAsync(authRequest);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeFalse();
            result.Errors.Should().Contain("Can not find user with this mail");
        }
        [Test]
        public async Task RegisterAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var userRequest = new AddUserRequest { Email = "newuser@example.com", Password = "Password123", Username = "newuser",
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>() };
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), userRequest.Password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterAsync(userRequest);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeTrue();
        }
        [Test]
        public async Task RegisterAsync_WithExistingEmail_ReturnsFailure()
        {
            // Arrange
            var userRequest = new AddUserRequest { Email = "existing@example.com", Password = "Password123", Username = "existinguser",
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>() };
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), userRequest.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Email already exists." }));

            // Act
            var result = await _authService.RegisterAsync(userRequest);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeFalse();
            result.Errors.Should().Contain("Email already exists.");
        }
        [Test]
        public async Task LoginWithTokenAsync_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var authRequest = new AuthRequest { Email = "user@example.com", Password = "InvalidPassword" };
            var user = new User { Email = authRequest.Email, UserName = "user",
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>() };
            _mockUserManager.Setup(x => x.FindByEmailAsync(authRequest.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, authRequest.Password)).ReturnsAsync(false);

            // Act
            var result = await _authService.LoginWithTokenAsync(authRequest);

            // Assert
            result.Should().BeNull();
        }
        [Test]
        public async Task RegisterWithTokenAsync_ValidData_ReturnsAuthResponseWithToken()
        {
            // Arrange
            var userRequest = new AddUserRequest
            {
                Email = "newuser@example.com",
                Password = "ValidPassword",
                Username = "newuser",
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>()
            };
            var createUserResult = IdentityResult.Success;
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), userRequest.Password)).ReturnsAsync(createUserResult);

            // Act
            var result = await _authService.RegisterWithTokenAsync(userRequest);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().BeEmpty();
        }
        [Test]
        public async Task LoginWithTokenAsync_ValidCredentials_ReturnsAuthResponse()
        {
            // Arrange
            var authRequest = new AuthRequest { Email = "valid@example.com", Password = "ValidPassword" };
            var user = new User { UserName = "ValidUser", Email = "valid@example.com", Id = Guid.NewGuid().ToString() ,
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>()};
            var signInResult = Microsoft.AspNetCore.Identity.SignInResult.Success;

            _mockUserManager.Setup(x => x.FindByEmailAsync(authRequest.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, authRequest.Password)).ReturnsAsync(true);
            _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, authRequest.Password, false, false)).ReturnsAsync(signInResult);

            // Act
            var result = await _authService.LoginWithTokenAsync(authRequest);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(user.UserName);
            result.Token.Should().NotBeNullOrEmpty(); // Zakładając, że CreateToken działa poprawnie
        }

        [Test]
        public async Task LoginWithTokenAsync_LoginFailure_ReturnsNull()
        {
            // Arrange
            var authRequest = new AuthRequest { Email = "user@example.com", Password = "CorrectPassword" };
            var user = new User { UserName = "user", Email = "user@example.com", Id = Guid.NewGuid().ToString(),
                FriendsList = new List<Friend>(),
                GamesList = new List<Game>() };
            var signInResult = Microsoft.AspNetCore.Identity.SignInResult.Failed;

            _mockUserManager.Setup(x => x.FindByEmailAsync(authRequest.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, authRequest.Password)).ReturnsAsync(true);
            _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, authRequest.Password, false, false)).ReturnsAsync(signInResult);

            // Act
            var result = await _authService.LoginWithTokenAsync(authRequest);

            // Assert
            result.Should().BeNull();
        }

    }
}
