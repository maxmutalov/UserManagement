using Microsoft.EntityFrameworkCore;

namespace ChessTournament.Application.Abstractions.Paging
{
    public sealed class PagedList<TValue> : List<TValue>
    {
        public Metadata Metadata { get; set; }

        private PagedList(
            List<TValue> items,
            Int32 totalCount,
            Int32 currentPage,
            Int32 pageSize)
        {
            Metadata = new()
            {
                TotalCount = totalCount,
                TotalPages = (Int32)Math.Ceiling((Decimal)(totalCount / pageSize)),
                CurrentPage = currentPage,
                PageSize = pageSize
            };

            AddRange(items);
        }

        public PagedList()
        {
        }

        public static async Task<PagedList<TValue>> ToPagedList(
            IQueryable<TValue> items,
            Int32 currentPage,
            Int32 pageSize,
            CancellationToken cancellationToken = default)
        {
            Int32 totalCount = await items.CountAsync(cancellationToken);

            var pagedListItems = items
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var pagedList = new PagedList<TValue>(
                        pagedListItems.Result, totalCount, currentPage, pageSize);

            return pagedList;
        }
    }
}
