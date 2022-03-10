using System.Collections.Generic;

namespace Domain.Paging
{
    public class PagingModel<T> where T : class
    {
        public long AllTotalCount { get; set; }
        public int ItemsCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}