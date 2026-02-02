using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Auth;
using mtkpm.Application.Features.Auth.Commands.ChangePassword;
using mtkpm.Application.Features.Auth.Commands.Login;
using mtkpm.Application.Features.Auth.Commands.Logout;
using mtkpm.Application.Features.Auth.Commands.RefreshToken;
using mtkpm.Application.Features.Auth.Commands.Register;
using mtkpm.Infrastructure.Services;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        /// <remarks>
        /// Tạo tài khoản người dùng mới trong hệ thống
        /// </remarks>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var command = new RegisterCommand
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = dto.Password,
                ConfirmPassword = dto.ConfirmPassword,
                PhoneNumber = dto.PhoneNumber,
                IpAddress = GetIpAddress(),
                DeviceInfo = GetDeviceInfo()
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <remarks>
        /// Xác thực người dùng và trả về access token + refresh token
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var command = new LoginCommand
            {
                UserNameOrEmail = dto.UserNameOrEmail,
                Password = dto.Password,
                RememberMe = dto.RememberMe,
                IpAddress = GetIpAddress(),
                DeviceInfo = GetDeviceInfo()
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Làm mới access token
        /// </summary>
        /// <remarks>
        /// Sử dụng refresh token để lấy access token mới
        /// </remarks>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var command = new RefreshTokenCommand
            {
                AccessToken = dto.AccessToken,
                RefreshToken = dto.RefreshToken,
                IpAddress = GetIpAddress(),
                DeviceInfo = GetDeviceInfo()
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <remarks>
        /// Hủy refresh token hiện tại
        /// </remarks>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new LogoutCommand(userId, dto.RefreshToken);

            await _mediator.Send(command);

            return Ok(new { message = "Đăng xuất thành công" });
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <remarks>
        /// Thay đổi mật khẩu của người dùng hiện tại
        /// </remarks>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new ChangePasswordCommand
            {
                UserId = userId,
                CurrentPassword = dto.CurrentPassword,
                NewPassword = dto.NewPassword,
                ConfirmNewPassword = dto.ConfirmNewPassword
            };

            var result = await _mediator.Send(command);

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }

        private string? GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"].ToString();
            }
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        private string? GetDeviceInfo()
        {
            return Request.Headers["User-Agent"].ToString();
        }
    }
}
