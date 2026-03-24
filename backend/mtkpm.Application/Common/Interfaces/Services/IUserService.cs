using mtkpm.Application.Common.DTOs.User;

namespace mtkpm.Application.Common.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<UserDto?> GetByUserNameAsync(string userName);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }
}
