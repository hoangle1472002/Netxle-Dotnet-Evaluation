using NexleEvaluation.Application.Helpers;
using NexleEvaluation.Application.Models.Requests.Auths;
using NexleEvaluation.Application.Models.Responses;
using NexleEvaluation.Application.Models.Responses.Auth;
using NexleEvaluation.Application.Models.Responses.User;
using NexleEvaluation.Application.Services.Interfaces.Auths;
using NexleEvaluation.Application.Utils;
using NexleEvaluation.Domain.Entities;
using NexleEvaluation.Domain.Entities.Identity;
using NexleEvaluation.Domain.Interfaces;
using System.Threading.Tasks;

namespace NexleEvaluation.Application.Services.Implementations.Auths
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IJwtTokenManager _tokenManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUserRepository userRepository, ITokenRepository tokenRepository, IJwtTokenManager tokenManager, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _tokenManager = tokenManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SignUpResponse>> SignUpAsync(SignUpRequest request)
        {
            var result = new Result<SignUpResponse>();

            if (await _userRepository.ExistsByEmailAsync(request.Email.Trim()))
            {
                result.Errors.Add(ErrorResponse.FromResource("Email already exists."));
                return result;
            }

            var hash = PasswordHelpers.Hash(request.Password);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email.Trim(),
                Hash = hash,
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            result.Data = new SignUpResponse
            {
                User = new UserDetailResponse
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                },
            };

            return result;
        }

        public async Task<Result<SignInResponse>> SignInAsync(SignInRequest request)
        {
            var result = new Result<SignInResponse>();

            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null || !PasswordHelpers.Verify(request.Password, user.Hash))
            {
                result.Errors.Add(ErrorResponse.FromResource("Incorrect email or password."));
                return result;
            }

            var token = _tokenManager.GenerateClaimsToken(user);
            var tokenEntity = new Token
            {
                UserId = user.Id,
                RefreshToken = token.RefreshToken,
                ExpiresIn = token.RefreshTokenExpiryTime.ToString(DateTimeUtil.DateTimeFormat)
            };

            await _tokenRepository.AddAsync(tokenEntity);
            await _unitOfWork.SaveChangesAsync();

            result.Data = new SignInResponse
            {
                User = new UserDetailResponse
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                },
                Token = token.AccessToken,
                RefreshToken = token.RefreshToken
            };

            return result;
        }

        public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var result = new Result<RefreshTokenResponse>();

            var existingToken = await _tokenRepository.GetWithUserByRefreshTokenAsync(request.RefreshToken);

            if (existingToken is null)
            {
                result.Errors.Add(ErrorResponse.FromResource("Refresh token not found."));
                return result;
            }

            var user = existingToken.User;
            if (user is null)
            {
                result.Errors.Add(ErrorResponse.FromResource("User not found."));
                return result;
            }

            _tokenRepository.HardDelete(existingToken);
            var newToken = _tokenManager.GenerateClaimsToken(user);

            var tokenEntity = new Token
            {
                UserId = user.Id,
                RefreshToken = newToken.RefreshToken,
                ExpiresIn = newToken.RefreshTokenExpiryTime.ToString(DateTimeUtil.DateTimeFormat)
            };
            await _tokenRepository.AddAsync(tokenEntity);
            await _unitOfWork.SaveChangesAsync();

            result.Data = new RefreshTokenResponse
            {
                Token = newToken.AccessToken,
                RefreshToken = newToken.RefreshToken
            };

            return result;
        }

        public async Task SignOutAsync(int userId)
        {
            var tokens = await _tokenRepository.GetByUserIdAsync(userId);
            _tokenRepository.RemoveRange(tokens);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
