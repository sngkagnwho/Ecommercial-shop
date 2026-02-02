using mtkpm.Application.Common.DTOs.Cart;

namespace mtkpm.Application.Common.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartDto> GetUserCartAsync(int userId);
        Task<CartItemDto> AddToCartAsync(int userId, AddToCartDto dto);
        Task<CartItemDto> UpdateCartItemAsync(int userId, int cartItemId, UpdateCartItemDto dto);
        Task<bool> RemoveFromCartAsync(int userId, int cartItemId);
        Task<bool> ClearCartAsync(int userId);
        Task<int> GetCartItemCountAsync(int userId);
    }
}
