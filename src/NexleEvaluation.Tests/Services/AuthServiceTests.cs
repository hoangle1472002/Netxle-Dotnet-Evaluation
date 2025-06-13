using Moq;
using NexleEvaluation.Application.Helpers;
using NexleEvaluation.Application.Models.Requests.Auths;
using NexleEvaluation.Application.Models.Responses.Auth;
using NexleEvaluation.Application.Services.Implementations.Auths;
using NexleEvaluation.Application.Services.Interfaces.Auths;
using NexleEvaluation.Domain.Entities;
using NexleEvaluation.Domain.Entities.Identity;
using NexleEvaluation.Domain.Interfaces;
using Shouldly;
using System.Linq.Expressions;
using Xunit;

namespace NexleEvaluation.Tests.Services;
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenRepository> _tokenRepositoryMock;
        private readonly Mock<IJwtTokenManager> _jwtTokenManagerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenRepositoryMock = new Mock<ITokenRepository>();
            _jwtTokenManagerMock = new Mock<IJwtTokenManager>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _authService = new AuthService(
                                            _userRepositoryMock.Object,
                                            _tokenRepositoryMock.Object,
                                            _jwtTokenManagerMock.Object,
                                            _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task SignUpAsync_EmailAlreadyExists_ReturnsError()
        {
            var request = new SignUpRequest
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "Password123"
            };

            _userRepositoryMock.Setup(repo => repo.ExistsByEmailAsync(It.IsAny<string>()))
                               .ReturnsAsync(true);

            var result = await _authService.SignUpAsync(request);

            result.Ok.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
        }

        [Fact]
        public async Task SignUpAsync_EmailNotExists_CreatesUserAndReturnsResult()
        {
            var request = new SignUpRequest
            {
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "Password123"
            };

            _userRepositoryMock.Setup(repo => repo.ExistsByEmailAsync(It.IsAny<string>()))
                               .ReturnsAsync(false);

            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                          .ReturnsAsync((User user) => user);


            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _authService.SignUpAsync(request);

            result.Ok.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.User.Email.ShouldBe(request.Email);
        }

        [Fact]
        public async Task SignInAsync_InvalidCredentials_ReturnsError()
        {
            var request = new SignInRequest
            {
                Email = "wrong@example.com",
                Password = "wrongpass"
            };

            _userRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
                               .ReturnsAsync((User)null);

            var result = await _authService.SignInAsync(request);

            result.Ok.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
        }

        [Fact]
        public async Task SignInAsync_ValidCredentials_ReturnsTokens()
        {
            var request = new SignInRequest
            {
                Email = "john@example.com",
                Password = "Password123"
            };

            var user = new User
            {
                Id = 1,
                Email = request.Email,
                FirstName = "John",
                LastName = "Doe",
                Hash = PasswordHelpers.Hash(request.Password)
            };

            _userRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                               .ReturnsAsync(user);

            var tokenResponse = new TokenResponse
            {
                AccessToken = "access-token",
                RefreshToken = "refresh-token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1)
            };

            _jwtTokenManagerMock.Setup(t => t.GenerateClaimsToken(user))
                                .Returns(tokenResponse);

            var result = await _authService.SignInAsync(request);

            result.Ok.ShouldBeTrue();
            result.Data.Token.ShouldBe("access-token");
            result.Data.RefreshToken.ShouldBe("refresh-token");
        }

        [Fact]
        public async Task SignOutAsync_RemovesTokens()
        {
            var userId = 1;
            var tokens = new List<Token>
            {
                new Token { Id = 1, UserId = userId, RefreshToken = "abc" }
            };

            _tokenRepositoryMock.Setup(repo => repo.GetByUserIdAsync(userId))
                                .ReturnsAsync(tokens);

            await _authService.SignOutAsync(userId);

            _tokenRepositoryMock.Verify(r => r.RemoveRange(tokens), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RefreshTokenAsync_TokenNotFound_ReturnsError()
        {
            _tokenRepositoryMock.Setup(r => r.GetWithUserByRefreshTokenAsync("invalid"))
                                .ReturnsAsync((Token)null);

            var result = await _authService.RefreshTokenAsync(new RefreshTokenRequest { RefreshToken = "invalid" });

            result.Ok.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.Description.Contains("not found"));
        }

        [Fact]
        public async Task RefreshTokenAsync_ValidToken_ReturnsNewTokens()
        {
            var user = new User
            {
                Id = 1,
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                Hash = "hashed"
            };

            var existingToken = new Token
            {
                RefreshToken = "old-refresh-token",
                UserId = user.Id,
                User = user
            };

            var newTokenResponse = new TokenResponse
            {
                AccessToken = "new-access-token",
                RefreshToken = "new-refresh-token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1)
            };

            _tokenRepositoryMock.Setup(r => r.GetWithUserByRefreshTokenAsync("old-refresh-token"))
                                .ReturnsAsync(existingToken);
            _jwtTokenManagerMock.Setup(m => m.GenerateClaimsToken(user))
                                .Returns(newTokenResponse);

            var result = await _authService.RefreshTokenAsync(new RefreshTokenRequest
            {
                RefreshToken = "old-refresh-token"
            });

            result.Ok.ShouldBeTrue();
            result.Data.Token.ShouldBe("new-access-token");
            result.Data.RefreshToken.ShouldBe("new-refresh-token");
        }
}
