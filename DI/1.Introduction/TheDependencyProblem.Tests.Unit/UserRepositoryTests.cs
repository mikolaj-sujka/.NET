using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using TheDependencyProblem.Data;
using Xunit;

namespace TheDependencyProblem.Tests.Unit;

public class UserRepositoryTests
{
    private readonly UserService _userService;
    private readonly IUserRepository _mockUserRepository = Substitute.For<IUserRepository>();

    public UserRepositoryTests()
    {
        _userService = new UserService(_mockUserRepository);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        _mockUserRepository.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act

        // Assert

    }
}
