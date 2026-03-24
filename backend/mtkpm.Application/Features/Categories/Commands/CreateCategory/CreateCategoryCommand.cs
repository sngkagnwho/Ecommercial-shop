using MediatR;
using mtkpm.Application.Common.DTOs.Category;

namespace mtkpm.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
