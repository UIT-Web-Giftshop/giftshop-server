using System.ComponentModel;

namespace Application.Features.Objects.Queries.GetPagingObjects
{
    public abstract class GetPagingObjectsQuery : Query
    {
        [DefaultValue(1)] public int PageIndex { get; set; } = 1;

        [DefaultValue(20)] public int PageSize { get; set; } = 20;

        public string Search { get; set; }

        public string SortBy { get; set; }
        
        public string Filter { get; set; }

        [DefaultValue(false)]
        public bool IsSortAscending { get; set; }
    }
}