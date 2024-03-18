
namespace Model.Domain.Common
{
    public class PaginatedResult<T> where T : class
    {
        public required int TotalCount { get; set; }
        public required IEnumerable<T> Items { get; set; }
    }
}
