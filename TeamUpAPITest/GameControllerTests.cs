using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TeamUpAPI.Controllers;
using TeamUpAPI.Helpers;
using TeamUpAPI.Services;
using TeamUpAPI.Models;

namespace TeamUpAPITest
{
    [TestFixture]
    public class GameControllerTests
    {
        private Mock<IGameService>? _mockGameService;
        private GameController? _controller;

        [SetUp]
        public void SetUp()
        {
            _mockGameService = new Mock<IGameService>();
            _controller = new GameController(_mockGameService.Object);
        }

        [Test]
        public async Task GameByIdAsync_WithExistingId_ReturnsOkObjectResult()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game { Id = gameId.ToString(), Name = "Test Game" };
            _mockGameService.Setup(s => s.GameByIdAsync(gameId))
                            .ReturnsAsync(game);

            // Act
            var result = await _controller.GameByIdAsync(gameId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeEquivalentTo(game);
        }

        [Test]
        public async Task AllGames_ReturnsOkObjectResultWithGamesList()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid().ToString(), Name = "Test Game 1" },
                new Game { Id = Guid.NewGuid().ToString(), Name = "Test Game 2" }
            };
            _mockGameService.Setup(s => s.GamesAsync())
                            .ReturnsAsync(games);

            // Act
            var result = await _controller.AllGames();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeEquivalentTo(games);
        }

        [Test]
        public void AllCategories_ReturnsOkObjectResultWithCategories()
        {
            // Arrange
            var categories = new List<Enums.GameCategories> { Enums.GameCategories.Platformer, Enums.GameCategories.Racing};
            _mockGameService.Setup(s => s.GameCategories())
                            .Returns(categories);

            // Act
            var result = _controller.AllCategories();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeEquivalentTo(categories);
        }

        [Test]
        public async Task AddToUserGames_WithValidGameIds_ReturnsOkResult()
        {
            // Arrange
            var gamesIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockGameService.Setup(s => s.AddToUserGamesAsync(It.IsAny<List<Guid>>()))
                            .ReturnsAsync(Enums.OperationResult.Ok);

            // Act
            var result = await _controller.AddToUserGames(gamesIds);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
        [Test]
        public async Task AllGames_WhenNoGames_ReturnsEmptyList()
        {
            // Arrange
            _mockGameService.Setup(x => x.GamesAsync()).ReturnsAsync(new List<Game>());

            // Act
            var result = await _controller.AllGames();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            ((List<Game>)okResult.Value!).Should().BeEmpty();
        }
        [Test]
        public void AllCategories_ReturnsListOfCategories()
        {
            // Arrange
            var categories = new List<Enums.GameCategories> { Enums.GameCategories.Platformer, Enums.GameCategories.Racing};
            _mockGameService.Setup(x => x.GameCategories()).Returns(categories);

            // Act
            var result = _controller.AllCategories();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(categories);
        }
        [Test]
        public async Task AddToUserGames_WithInvalidGamesIds_ReturnsBadRequest()
        {
            // Arrange
            var gamesIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockGameService.Setup(x => x.AddToUserGamesAsync(gamesIds)).ReturnsAsync(Enums.OperationResult.BadRequest);

            // Act
            var result = await _controller.AddToUserGames(gamesIds);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
        [Test]
        public async Task DeleteFromUserGames_WithValidGamesIds_ReturnsOkResult()
        {
            // Arrange
            var gamesIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _mockGameService.Setup(x => x.DeleteFromUserGamesAsync(gamesIds)).ReturnsAsync(Enums.OperationResult.Ok);
        
            // Act
            var result = await _controller.DeleteFromUserGames(gamesIds);
        
            // Assert
            //result.Should().BeOfType<Enums.OperationResult>();
            result.Should().Be(Enums.OperationResult.Ok);
        }
        [Test]
        public async Task GamesByCategoryAsync_WithValidCategory_ReturnsGamesList()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid().ToString(), Name = "Game 1", Category = Enums.GameCategories.Racing },
                new Game { Id = Guid.NewGuid().ToString(), Name = "Game 2", Category = Enums.GameCategories.Racing }
            };
            _mockGameService.Setup(s => s.GamesByCategoryAsync(It.IsAny<Enums.GameCategories>()))
                .ReturnsAsync(games);

            // Act
            var result = await _controller.GamesByCategoryAsync(Enums.GameCategories.Racing);

            // Assert
            result.Should().BeEquivalentTo(games);
        }
        [Test]
        public async Task CurrentUserGamesList_ReturnsCurrentUserGames()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid().ToString(), Name = "User Game 1" },
                new Game { Id = Guid.NewGuid().ToString(), Name = "User Game 2" }
            };
            _mockGameService.Setup(s => s.CurrentUserGamesListAsync())
                .ReturnsAsync(games);

            // Act
            var result = await _controller.CurrentUserGamesList();

            // Assert
            result.Should().BeEquivalentTo(games);
        }

    }
}
