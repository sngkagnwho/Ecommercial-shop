using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using mtkpm.Application.Common.DTOs.Auth;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Identity_Auth;
using mtkpm.Infrastructure.Configuration;

namespace mtkpm.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService,
            IUnitOfWork unitOfWork,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDto request, string? ipAddress = null, string? deviceInfo = null)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return AuthResponse.FailureResult($"Email '{request.Email}' đã được đăng ký", 
                    new List<string> { "Email already registered" });
            }

            var existingUsername = await _userManager.FindByNameAsync(request.UserName);
            if (existingUsername != null)
            {
                return AuthResponse.FailureResult($"Tên người dùng '{request.UserName}' đã được sử dụng",
                    new List<string> { "Username already taken" });
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return AuthResponse.FailureResult("Đăng ký thất bại", errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateAccessToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(
                userId: user.Id,
                token: refreshToken,
                expiresAt: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                deviceInfo: deviceInfo,
                ipAddress: ipAddress
            );

            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            var userInfo = new UserInfo
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return AuthResponse.SuccessResult(
                accessToken, 
                refreshToken, 
                DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                userInfo,
                "Đăng ký thành công"
            );
        }

        public async Task<AuthResponse> LoginAsync(LoginDto request, string? ipAddress = null, string? deviceInfo = null)
        {
            User? user = null;

            if (request.UserNameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            }

            if (user == null)
            {
                return AuthResponse.FailureResult("Thông tin đăng nhập không chính xác",
                    new List<string> { "Invalid credentials" });
            }

            if (user.IsDeleted)
            {
                return AuthResponse.FailureResult("Tài khoản đã bị xóa",
                    new List<string> { "Account has been deleted" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                return AuthResponse.FailureResult("Tài khoản đã bị khóa",
                    new List<string> { "Account is locked out" });
            }

            if (!result.Succeeded)
            {
                return AuthResponse.FailureResult("Thông tin đăng nhập không chính xác",
                    new List<string> { "Invalid credentials" });
            }

            user.UpdateLastLogin();
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateAccessToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(
                userId: user.Id,
                token: refreshToken,
                expiresAt: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                deviceInfo: deviceInfo,
                ipAddress: ipAddress
            );

            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            var userInfo = new UserInfo
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return AuthResponse.SuccessResult(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                userInfo,
                "Đăng nhập thành công"
            );
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenDto request, string? ipAddress = null, string? deviceInfo = null)
        {
            var userId = _jwtService.GetUserIdFromExpiredToken(request.AccessToken);
            if (userId == null)
            {
                return AuthResponse.FailureResult("Token không hợp lệ",
                    new List<string> { "Invalid token" });
            }

            var user = await _userManager.FindByIdAsync(userId.ToString()!);
            if (user == null || user.IsDeleted)
            {
                return AuthResponse.FailureResult("Người dùng không tồn tại",
                    new List<string> { "User not found" });
            }

            var storedRefreshToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken);
            
            if (storedRefreshToken == null || !storedRefreshToken.IsActive || storedRefreshToken.UserId != userId)
            {
                return AuthResponse.FailureResult("Refresh token không hợp lệ",
                    new List<string> { "Invalid refresh token" });
            }

            storedRefreshToken.Revoke(ipAddress, "Replaced by new token");

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtService.GenerateAccessToken(user, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken(
                userId: user.Id,
                token: newRefreshToken,
                expiresAt: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                deviceInfo: deviceInfo,
                ipAddress: ipAddress
            );

            await _unitOfWork.RefreshTokens.AddAsync(newRefreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            var userInfo = new UserInfo
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return AuthResponse.SuccessResult(
                newAccessToken,
                newRefreshToken,
                DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                userInfo,
                "Làm mới token thành công"
            );
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken, string? ipAddress = null)
        {
            var token = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
            
            if (token == null || !token.IsActive)
            {
                return false;
            }

            token.Revoke(ipAddress, "Revoked by user");
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RevokeAllTokensAsync(int userId)
        {
            await _unitOfWork.RefreshTokens.RevokeAllUserTokensAsync(userId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LogoutAsync(int userId, string refreshToken)
        {
            var token = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
            
            if (token != null && token.UserId == userId && token.IsActive)
            {
                token.Revoke(null, "Logged out");
                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }
    }
}
