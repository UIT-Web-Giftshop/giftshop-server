namespace Application.Features.Objects.Queries.GetOneObject
{
    public abstract class GetOneObjectByIdQuery : GetOneObjectQuery
    {
        public string Id { get; set; }
    }
}