namespace Hairlytics.Application.DTOs.HelperDTOs
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
        public int StartIndex => TotalCount == 0 ? 0 : ((PageNumber - 1) * PageSize) + 1;
        public int EndIndex => Math.Min(PageNumber * PageSize, TotalCount);
    }
}
