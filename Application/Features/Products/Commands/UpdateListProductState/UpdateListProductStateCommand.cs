using System.Collections.Generic;
using Application.Commons;
using Application.Features.Products.Commands.UpdateOneProductState;
using MediatR;

namespace Application.Features.Products.Commands.UpdateListProductState
{
    public class UpdateListProductStateCommand : IRequest<ResponseApi<Unit>>
    {
        public HashSet<string> Skus { get; set; }
        public ProductState State { get; set; }
    }
}