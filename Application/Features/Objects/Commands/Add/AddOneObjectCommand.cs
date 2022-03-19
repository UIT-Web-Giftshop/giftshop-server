using Application.Commons;
using Application.Features.Objects.Vms;
using MediatR;

namespace Application.Features.Objects.Commands.Add
{
    public class AddOneObjectCommand<T> : Command, IRequest<ResponseApi<string>> where T : ObjectVm
    {
        public T Data { get; init; }
    }
}