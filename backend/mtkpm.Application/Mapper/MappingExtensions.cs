using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using mtkpm.Application.Common.DTOs.Common;

namespace mtkpm.Application.Mapper
{
    public static class MappingExtensions
    {
        public static async Task<PaginatedListDto<TDestination>> ToPaginatedListAsync<TSource, TDestination>(
            this IQueryable<TSource> source,
            int pageIndex,
            int pageSize,
            IMapper mapper) where TSource : class
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedItems = mapper.Map<List<TDestination>>(items);

            return new PaginatedListDto<TDestination>(mappedItems, count, pageIndex, pageSize);
        }

        public static async Task<List<TDestination>> ProjectToListAsync<TDestination>(
            this IQueryable source,
            IConfigurationProvider configuration)
        {
            return await source.ProjectTo<TDestination>(configuration).ToListAsync();
        }
    }
}
