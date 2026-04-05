using System;
using System;
using System.Collections.Generic;

namespace mtkpm.Application.Common.DTOs.User
{
    /// <summary>
    /// DTO cho User khi Admin xem - bao g?m roles và tr?ng thái khóa
    /// </summary>
    public class UserWithRolesDto
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Tr?ng thái khóa tài kho?n
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Danh sách roles c?a user
        /// </summary>
        public List<string> Roles { get; set; } = new();
    }
}
