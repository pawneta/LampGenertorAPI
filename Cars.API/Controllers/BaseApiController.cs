using MediatR;
using Microsoft.AspNetCore.Mvc;
//using MediaR
namespace Cars.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]//endpoint zawsze zaczyna sie "api/..."
    
    public class BaseApiController:ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
