using Domain.Entities;

namespace Application.Features.Objects.Commands.Delete.DeleteOneObject
{
    public class DeleteOneObjectCommand<T> : CommandUsingID where T : IdentifiableObject
    {

    }
}