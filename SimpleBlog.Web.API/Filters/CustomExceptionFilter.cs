using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleBlog.Domain.Exceptions;
using System.Net;

namespace SimpleBlog.Web.API.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private static readonly Dictionary<Type, Func<Exception, ObjectResult>> _exceptionMappings = new()
    {
        { typeof(UserNotFoundException), ex => new NotFoundObjectResult(new { message = ex.Message }) },
        { typeof(UserNotAuthorizedException), ex => new UnauthorizedObjectResult(new { message = ex.Message }) },
        { typeof(UserAlreadyExistsException), ex => new ConflictObjectResult(new { message = ex.Message }) },
        { typeof(InvalidPasswordException), ex => new BadRequestObjectResult(new { message = ex.Message }) },
        { typeof(ConfigurationException), ex => new ObjectResult(new { message = ex.Message }) { StatusCode = StatusCodes.Status500InternalServerError } },
        { typeof(PostNotFoundException), ex => new NotFoundObjectResult(new { message = ex.Message }) },
        { typeof(PostConflictException), ex => new ConflictObjectResult(new { message = ex.Message }) },
    };

        public void OnException(ExceptionContext context)
        {
            if (_exceptionMappings.TryGetValue(context.Exception.GetType(), out var resultFunc))
            {
                context.Result = resultFunc(context.Exception);
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new ObjectResult(new { message = context.Exception.Message })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                context.ExceptionHandled = true;
            }
        }
    }

}