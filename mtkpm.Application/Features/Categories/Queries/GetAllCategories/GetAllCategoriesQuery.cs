using MediatR;
using mtkpm.Application.Common.DTOs.Category;

namespace mtkpm.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
    {
    }
}
