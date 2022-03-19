﻿using Application.Commons;
using Application.Features.Objects.Queries.GetOneObject;
using Application.Features.Users.Vms;
using MediatR;

namespace Application.Features.Users.Queries.GetOneUserById
{
    public class GetOneUserByIdQuery : GetOneObjectByIdQuery, IRequest<ResponseApi<UserVm>>
    {

    }
}