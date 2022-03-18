using Application.Commons;
using MediatR;

namespace Application.Features.Objects.Commands.AddOneObject
{
    public class AddOneObjectCommand<T> : Command, IRequest<ResponseApi<string>>
    {
        public T Data { get; init; }
    }
}