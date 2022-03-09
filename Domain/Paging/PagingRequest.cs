using System.ComponentModel;

namespace Domain.Paging
{
    public class PagingRequest
    {
        [DefaultValue(1)]
        public int PageIndex { get; set; }
        [DefaultValue(20)]
        public int PageSize { get; set; }
    }
}