using ApplicationCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieShopAPI.Middlewares
{
    // this middleware will be used to catch all the exceptions
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MovieShopExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MovieShopExceptionMiddleware> _logger; // injection
        public MovieShopExceptionMiddleware(RequestDelegate next, ILogger<MovieShopExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation("Inside the Middleware");
                await _next(httpContext);//don't forget await/async
            }
            catch (Exception ex)
            {

                var exceptionDetails = new ErrorModel
                {
                    Message = ex.Message,
                    // StackTrace = ex.StackTrace, should not show stacktrace to users
                    ExceptionDateTime = DateTime.UtcNow,
                    // ExceptionType = ex.GetType().ToString(), avoid showing sensitive information
                    Path = httpContext.Request.Path,
                    HttpRequestType = httpContext.Request.Method,
                    User = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : null
                };

                _logger.LogError("Exception happened, log this to text or Json files using SeriLog");
                // need to return http status code 500
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var result = JsonSerializer.Serialize<ErrorModel>(exceptionDetails);
                await httpContext.Response.WriteAsync(result);

                // if it were MVC, we need to show error page
                // httpContext.Response.Redirect("/home/error");
            }

            // here we can add logic
            // first catch exception
            // then check exception type, message
            // check stacktrace
            // where/when exception happened
            // for which URL and which Http Method (controller, action method)
            // for which User, if user is logged in or not

            // we should save information so that analyze later
            // instead of System.IO, we can use ASP.NET Core's builtin logging mechanism (ILogger)
            // can be used by 3rd party log (e.g. SeriLog, NLog)
            // we can then send email to Dev team 

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MovieShopExceptionMiddlewareExtensions
    {
        // extension methods (static class, static method, this keyword)
        public static IApplicationBuilder UseMovieShopExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MovieShopExceptionMiddleware>();
        }
    }
}
