namespace ChessTournament.Application.Abstractions.Paging
{
    public sealed class Metadata
    {
        private const Int32 defaultPageSize = 10;
        private const Int32 defaultCurrentPage = 1;

        private Int32 pageSize = 0;
        private Int32 currentPage = 0;

        public int TotalCount { get; init; }
        public int TotalPages { get; set; }
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value == 0
                    ? defaultCurrentPage
                    : value;
            }
        }
        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = value == 0
                    ? defaultPageSize
                    : value;
            }
        }

        public Boolean HasNextPage => CurrentPage < TotalPages;
        public Boolean HasPreviousPage => CurrentPage > 1;

    }
}
