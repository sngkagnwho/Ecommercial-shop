using mtkpm.Application.Common.DTOs.User;

namespace mtkpm.Application.Common.Interfaces.Services
{
    public interface IFavouriteProductService
    {
        Task<IEnumerable<FavouriteProductDto>> GetUserFavouritesAsync(int userId);
        Task<FavouriteProductDto> AddFavouriteAsync(int userId, AddFavouriteProductDto dto);
        Task<bool> RemoveFavouriteAsync(int userId, int productId);
        Task<bool> IsFavouriteAsync(int userId, int productId);
    }
}
