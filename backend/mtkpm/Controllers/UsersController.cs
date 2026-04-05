using MediatR;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Features.Users.Commands.UpdateUser;
using mtkpm.Domain.Entities.Identity_Auth;
using mtkpm.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;

        public UsersController(IMediator mediator, ICurrentUserService currentUserService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        #region Profile Management

        /// <summary>
        /// Lấy thông tin profile của user hiện tại
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = _currentUserService.UserId!.Value;
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return NotFound(ApiResponse<UserDto>.FailureResponse("Người dùng không tìm thấy"));

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                CreatedAt = user.CreateAt,
                LastLoginAt = user.LastLoginAt
            };

            return Ok(ApiResponse<UserDto>.SuccessResponse(userDto));
        }

        /// <summary>
        /// Cập nhật profile của user hiện tại
        /// </summary>
        [HttpPut("me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new UpdateUserCommand
            {
                Id = userId,
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<UserDto>.SuccessResponse(result, "Cập nhật thông tin thành công"));
        }

        /// <summary>
        /// Lấy thông tin user khác (public profile)
        /// </summary>
        [HttpGet("{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null || user.IsDeleted)
                return NotFound(ApiResponse<UserDto>.FailureResponse("Người dùng không tìm thấy"));

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                CreatedAt = user.CreateAt
            };

            return Ok(ApiResponse<UserDto>.SuccessResponse(userDto));
        }

        #endregion

        #region User Management (Admin)

        /// <summary>
        /// Lấy danh sách tất cả người dùng (Admin only) - Hỗ trợ pagination
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PaginatedListDto<UserWithRolesDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] int pageIndex = 1, 
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "createdAt",
            [FromQuery] bool descending = true)
        {
            var query = _userManager.Users.Where(u => !u.IsDeleted);

            // Sắp xếp
            query = sortBy?.ToLower() switch
            {
                "username" => descending ? query.OrderByDescending(u => u.UserName) : query.OrderBy(u => u.UserName),
                "email" => descending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "createdat" => descending ? query.OrderByDescending(u => u.CreateAt) : query.OrderBy(u => u.CreateAt),
                _ => descending ? query.OrderByDescending(u => u.CreateAt) : query.OrderBy(u => u.CreateAt)
            };

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pageUsers = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserWithRolesDto>();
            foreach (var user in pageUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserWithRolesDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    CreatedAt = user.CreateAt,
                    LastLoginAt = user.LastLoginAt,
                    IsLocked = user.LockoutEnd > DateTimeOffset.UtcNow,
                    Roles = roles.ToList()
                });
            }

            var paginatedResult = new PaginatedListDto<UserWithRolesDto>
            {
                Items = userDtos,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(ApiResponse<PaginatedListDto<UserWithRolesDto>>.SuccessResponse(paginatedResult));
        }

        /// <summary>
        /// Lấy chi tiết user cụ thể (Admin only)
        /// </summary>
        [HttpGet("admin/{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserWithRolesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserDetailForAdmin(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                return NotFound(ApiResponse<UserWithRolesDto>.FailureResponse("Người dùng không tìm thấy"));

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserWithRolesDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                CreatedAt = user.CreateAt,
                LastLoginAt = user.LastLoginAt,
                IsLocked = user.LockoutEnd > DateTimeOffset.UtcNow,
                Roles = roles.ToList()
            };

            return Ok(ApiResponse<UserWithRolesDto>.SuccessResponse(userDto));
        }

        /// <summary>
        /// Tìm kiếm người dùng (Admin only) - Hỗ trợ tìm theo username, email, phone
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<UserWithRolesDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchUsers([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest(ApiResponse<IEnumerable<UserWithRolesDto>>.FailureResponse("Vui lòng nhập từ khóa tìm kiếm"));

            var searchLower = searchTerm.ToLower();
            var users = await _userManager.Users
                .Where(u => !u.IsDeleted && (
                    u.UserName!.ToLower().Contains(searchLower) ||
                    u.Email!.ToLower().Contains(searchLower) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(searchLower))
                ))
                .OrderByDescending(u => u.CreateAt)
                .Take(20)
                .ToListAsync();

            var userDtos = new List<UserWithRolesDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserWithRolesDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    CreatedAt = user.CreateAt,
                    LastLoginAt = user.LastLoginAt,
                    IsLocked = user.LockoutEnd > DateTimeOffset.UtcNow,
                    Roles = roles.ToList()
                });
            }

            return Ok(ApiResponse<IEnumerable<UserWithRolesDto>>.SuccessResponse(userDtos));
        }

        /// <summary>
        /// Admin cập nhật thông tin của bất kỳ user nào
        /// </summary>
        [HttpPut("{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateUserByAdmin(int userId, [FromBody] UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                return NotFound(ApiResponse<UserDto>.FailureResponse("Người dùng không tìm thấy"));

            var command = new UpdateUserCommand
            {
                Id = userId,
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<UserDto>.SuccessResponse(result, "Cập nhật thông tin user thành công"));
        }

        /// <summary>
        /// Cập nhật role của user - Ví dụ: biến user thường thành Admin
        /// </summary>
        [HttpPost("{userId}/roles")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleDto dto)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(dto.RoleName))
                return BadRequest(ApiResponse<bool>.FailureResponse("Tên role không được để trống"));

            var validRoles = new[] { "User", "Admin", "Moderator" };
            if (!validRoles.Contains(dto.RoleName))
                return BadRequest(ApiResponse<bool>.FailureResponse($"Role không hợp lệ. Hỗ trợ: {string.Join(", ", validRoles)}"));

            // Lấy user
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                return NotFound(ApiResponse<bool>.FailureResponse("Người dùng không tìm thấy"));

            // Không cho phép thay đổi role của user hiện tại
            if (user.Id == _currentUserService.UserId)
                return BadRequest(ApiResponse<bool>.FailureResponse("Không thể thay đổi role của chính mình"));

            try
            {
                // Xóa tất cả role cũ
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Count > 0)
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);

                // Thêm role mới
                var result = await _userManager.AddToRoleAsync(user, dto.RoleName);
                if (!result.Succeeded)
                    return BadRequest(ApiResponse<bool>.FailureResponse($"Lỗi: {string.Join(", ", result.Errors.Select(e => e.Description))}"));

                return Ok(ApiResponse<bool>.SuccessResponse(true, $"Đã cập nhật role thành: {dto.RoleName}"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.FailureResponse($"Lỗi hệ thống: {ex.Message}"));
            }
        }

        /// <summary>
        /// Khóa/Mở khóa tài khoản user
        /// </summary>
        [HttpPost("{userId}/lock")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LockUnlockUser(int userId, [FromBody] LockUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                return NotFound(ApiResponse<bool>.FailureResponse("Người dùng không tìm thấy"));

            if (user.Id == _currentUserService.UserId)
                return BadRequest(ApiResponse<bool>.FailureResponse("Không thể khóa tài khoản của chính mình"));

            try
            {
                if (dto.IsLocked)
                {
                    // Khóa tài khoản
                    var lockResult = await _userManager.SetLockoutEnabledAsync(user, true);
                    if (lockResult.Succeeded)
                    {
                        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                        return Ok(ApiResponse<bool>.SuccessResponse(true, "Đã khóa tài khoản người dùng"));
                    }
                }
                else
                {
                    // Mở khóa tài khoản
                    var unlockResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                    if (unlockResult.Succeeded)
                        return Ok(ApiResponse<bool>.SuccessResponse(true, "Đã mở khóa tài khoản người dùng"));
                }

                return BadRequest(ApiResponse<bool>.FailureResponse("Thao tác thất bại"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.FailureResponse($"Lỗi hệ thống: {ex.Message}"));
            }
        }

        /// <summary>
        /// Xóa mềm user (Admin only) - Không xóa hoàn toàn, chỉ đánh dấu xóa
        /// </summary>
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound(ApiResponse<bool>.FailureResponse("Người dùng không tìm thấy"));

            if (user.IsDeleted)
                return BadRequest(ApiResponse<bool>.FailureResponse("Người dùng đã bị xóa"));

            if (user.Id == _currentUserService.UserId)
                return BadRequest(ApiResponse<bool>.FailureResponse("Không thể xóa tài khoản của chính mình"));

            try
            {
                user.SetDeleted(_currentUserService.UserId ?? userId);
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    return BadRequest(ApiResponse<bool>.FailureResponse($"Lỗi: {string.Join(", ", result.Errors.Select(e => e.Description))}"));

                return Ok(ApiResponse<bool>.SuccessResponse(true, $"Đã xóa tài khoản người dùng: {user.UserName}"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.FailureResponse($"Lỗi hệ thống: {ex.Message}"));
            }
        }

        /// <summary>
        /// Lấy thống kê người dùng (Admin only)
        /// </summary>
        [HttpGet("statistics/dashboard")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserStatisticsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserStatistics()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var totalUsers = allUsers.Count(u => !u.IsDeleted);
            var activeUsers = allUsers.Count(u => !u.IsDeleted && u.LastLoginAt.HasValue && 
                u.LastLoginAt > DateTime.UtcNow.AddDays(-30));
            var lockedUsers = allUsers.Count(u => !u.IsDeleted && u.LockoutEnd > DateTimeOffset.UtcNow);
            var newUsersThisMonth = allUsers.Count(u => !u.IsDeleted && 
                u.CreateAt > DateTime.UtcNow.AddDays(-30));

            var statistics = new UserStatisticsDto
            {
                TotalUsers = totalUsers,
                ActiveUsersThisMonth = activeUsers,
                LockedUsers = lockedUsers,
                NewUsersThisMonth = newUsersThisMonth
            };

            return Ok(ApiResponse<UserStatisticsDto>.SuccessResponse(statistics));
        }

        #endregion
    }

    /// <summary>
    /// DTO để cập nhật role cho user
    /// </summary>
    public class UpdateUserRoleDto
    {
        [Required(ErrorMessage = "Tên role là bắt buộc")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên role phải từ 3-50 ký tự")]
        public string RoleName { get; set; } = null!;
    }

    /// <summary>
    /// DTO để khóa/mở khóa user
    /// </summary>
    public class LockUserDto
    {
        [Required]
        public bool IsLocked { get; set; }
    }

    /// <summary>
    /// DTO thống kê người dùng
    /// </summary>
    public class UserStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsersThisMonth { get; set; }
        public int LockedUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
    }
}
