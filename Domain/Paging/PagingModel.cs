using System.Collections.Generic;

namespace Domain.Paging
{
    public class PagingModel<T> where T : class
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}