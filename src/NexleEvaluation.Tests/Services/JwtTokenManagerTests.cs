using NexleEvaluation.Application.Models.Responses.Auth;
using NexleEvaluation.Application.Services.Implementations.Auths;
using NexleEvaluation.Application.Settings;
using NexleEvaluation.Domain.Entities.Identity;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace NexleEvaluation.Tests.Services
{
    public class JwtTokenManagerTests
    {
        private readonly JwtSettings _jwtSettings;
        private readonly JwtTokenManager _tokenManager;

        public JwtTokenManagerTests()
        {
            _jwtSettings = new JwtSettings
            {
                Secret = "ThisIsASecretKeyForTesting12345!",
                Expiration = TimeSpan.FromMinutes(15),
                RefreshTokenLifetime = 7
            };

            _tokenManager = new JwtTokenManager(_jwtSettings);
        }

        [Fact]
        public void GenerateClaimsToken_Should_ReturnValidTokenResponse()
        {
            var user = new User
            {
                Id = 1,
                Email = "test@example.com"
            };

            TokenResponse response = _tokenManager.GenerateClaimsToken(user);

            response.ShouldNotBeNull();
            response.AccessToken.ShouldNotBeNullOrEmpty();
            response.RefreshToken.ShouldNotBeNullOrEmpty();

            response.AccessTokenExpiryTime.ShouldBeGreaterThan(DateTime.UtcNow);
            response.RefreshTokenExpiryTime.ShouldBeGreaterThan(DateTime.UtcNow.AddDays(6));

            var handler = new JwtSecurityTokenHandler();
            handler.CanReadToken(response.AccessToken).ShouldBeTrue();

            var token = handler.ReadJwtToken(response.AccessToken);

            token.Claims.ShouldContain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Email);
            token.Claims.ShouldContain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
            token.Claims.ShouldContain(c => c.Type == ClaimTypes.Name && c.Value == user.Email);

            var expectedExpiry = DateTime.UtcNow.Add(_jwtSettings.Expiration);
            var actualExpiry = token.ValidTo;
            var difference = (expectedExpiry - actualExpiry).Duration();

            difference.ShouldBeLessThan(TimeSpan.FromSeconds(10));
        }
    }
}
