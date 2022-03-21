using Application.Commons;
using MediatR;

namespace Application.Features.Objects.Commands
{
    public abstract class CommandUsingID : Command, IRequest<ResponseApi<Unit>>
    {
        public string Id { get; init; }
    }
}