using System;
using CeluGamaSystem.Dtos;
using CeluGamaSystem.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CeluGamaSystem.Controllers
{
    [Route("api/error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public IActionResult LogError()
        {
            IExceptionHandlerPathFeature exFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exFeature != null)
            {
                if (exFeature.Error is OrderFieldsException Error)
                {
                    ApiError ApiError = new ApiError()
                    {
                        StatusCode = Error.StatusCode,
                        Message = Error.Message
                    };
                    switch (Error.StatusCode)
                    {
                        case 404:
                            return NotFound(ApiError);
                        case 400:
                            return BadRequest(ApiError);
                        case 422:
                            return UnprocessableEntity(ApiError);
                        case 500:
                            return StatusCode(500, ApiError);
                    }
                } 
                else if (exFeature.Error is LoginException loginErr)
                {
                    ApiError ApiError = new ApiError()
                    {
                        StatusCode = loginErr.StatusCode,
                        Message = loginErr.Message
                    };

                    return BadRequest(ApiError);
                }
                else if (exFeature.Error is Exception Err)
                {
                    ApiError ApiError = new ApiError()
                    {
                        StatusCode = 500,
                        Message = Err.Message
                    };
                    return StatusCode(500, ApiError);
                }
                else if (exFeature.Error is OrdersNotFoundException notFoundErr)
                {
                    ApiError ApiError = new ApiError()
                    {
                        StatusCode = notFoundErr.StatusCode,
                        Message = notFoundErr.Message
                    };
                    return NotFound(ApiError);
                }
                else if (exFeature.Error is ShippmentLabelUnauthorized)
                {
                    return Forbid();
                }
                else if (exFeature.Error is ChangePasswordException changeError)
                {
                    ApiError ApiError = new ApiError()
                    {
                        StatusCode = changeError.StatusCode,
                        Message = changeError.Message
                    };
                    return StatusCode(500, ApiError);
                }
            }
            return StatusCode(500);
        }
    }
}