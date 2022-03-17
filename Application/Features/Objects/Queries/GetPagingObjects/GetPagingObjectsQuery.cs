using System.ComponentModel;

namespace Application.Features.Objects.Queries.GetPagingObjects
{
    public abstract class GetPagingObjectsQuery : Query
    {
        [DefaultValue(1)]
        public int PageIndex { get; set; }

        [DefaultValue(20)]
        public int PageSize { get; set; }

        public string Search { get; set; }

        public string SortBy { get; set; }

        [DefaultValue(false)]
        public bool IsSortAscending { get; set; }
    }
}