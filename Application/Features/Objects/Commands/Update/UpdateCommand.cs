using Application.Commons;
using MediatR;

namespace Application.Features.Objects.Commands.Update
{
    public abstract class UpdateCommand : Command, IRequest<ResponseApi<Unit>>
    {
        public string Id { get; init; }
    }
}