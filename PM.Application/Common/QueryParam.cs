namespace PM.Application.Common
{
    public class QueryParam
    {
        public string? Search { get; set; }
        public string? OrderBy { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? CreatedAt { get; set; }

    }
}
