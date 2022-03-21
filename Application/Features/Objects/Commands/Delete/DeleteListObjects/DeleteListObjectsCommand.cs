using System.Collections.Generic;
using Application.Commons;
using Domain.Entities;
using MediatR;

namespace Application.Features.Objects.Commands.Delete.DeleteListObjects
{
    public class DeleteListObjectsCommand<T> : Command, IRequest<ResponseApi<Unit>> where T : 
        IdentifiableObject
    {
        public List<string> Ids { get; init; }
    }
}