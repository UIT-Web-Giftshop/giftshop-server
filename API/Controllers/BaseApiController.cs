﻿using Application.Commons;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult HandleResponseStatus<T>(ResponseApi<T> responseApi)
        {
            if (responseApi.Success)
            {
                return StatusCode(responseApi.Status, responseApi);
            }
            
            //TODO: more handling, refactor status code response
            return responseApi.Status switch
            {
                StatusCodes.Status404NotFound => NotFound(responseApi),
                StatusCodes.Status400BadRequest => BadRequest(responseApi),
                _ => StatusCode(StatusCodes.Status500InternalServerError, responseApi)
            };
        }
    }
}