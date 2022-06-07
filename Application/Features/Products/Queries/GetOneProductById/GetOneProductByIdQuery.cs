using Application.Commons;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetOneProductById
{
    public class GetOneProductByIdQuery : IRequest<ResponseApi<Product>>
    {
        public string Id { get; set; }
    }
}