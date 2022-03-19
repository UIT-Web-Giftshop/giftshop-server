using Application.Commons;
using MediatR;

namespace Application.Features.Objects.Commands.Update.UpdateOneObject
{
    public class UpdateOneObjectCommand<T> : UpdateCommand
    {
        public T Data { get; init; }
    }
}