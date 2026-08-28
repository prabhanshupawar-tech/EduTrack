using Microsoft.EntityFrameworkCore;

namespace EduTrack.ViewModels
{
    /// <summary>
    /// Generic pagination wrapper used by Student, Course and Enrollment list pages.
    /// Wraps a page of items plus metadata needed to render Bootstrap pagination controls.
    /// </summary>
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Executes the query with Skip/Take and materializes a page of results.
        /// </summary>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 5 : pageSize;

            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
