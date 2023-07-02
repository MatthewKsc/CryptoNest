using System;
using System.Net;
using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CryptoNest.Shared.Infrastructure.Exceptions;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly IExceptionCompositionRoot exceptionCompositionRoot;

    public ExceptionMiddleware(IExceptionCompositionRoot exceptionCompositionRoot)
    {
        this.exceptionCompositionRoot = exceptionCompositionRoot;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            //add logger in next Tasks
            await HandleErrorAsync(context, exception);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        ErrorsResponse errorsResponse = exceptionCompositionRoot.Map(exception);
        context.Response.StatusCode = (int) (errorsResponse?.StatusCode ?? HttpStatusCode.InternalServerError);

        object response = errorsResponse?.Response;

        if (response is null)
        {
            return;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}