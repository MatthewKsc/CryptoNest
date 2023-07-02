using System;
using System.Collections.Concurrent;
using System.Net;
using CryptoNest.Shared.Abstractions.Exceptions;
using CryptoNest.Shared.Infrastructure.Exceptions.Models;
using Humanizer;
using ErrorsResponse = CryptoNest.Shared.Abstractions.Exceptions.ErrorsResponse;

namespace CryptoNest.Shared.Infrastructure.Exceptions;

internal class ExceptionToResponseMapper : IExceptionToResponseMapper
{
    private const string DefaultErrorCode = "error";
    private const string DefaultErrorMessage = "There was an unknown error.";
    
    private static readonly ConcurrentDictionary<Type, string> Codes = new();

    public ErrorsResponse Map(Exception exception)
    {
        ErrorsResponse errorsResponse = exception switch
        {
            CryptoNestBaseException ex => new ErrorsResponse(
                new Models.ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), HttpStatusCode.BadRequest),
            _ => new ErrorsResponse(
                new Models.ErrorsResponse(new Error(DefaultErrorCode, DefaultErrorMessage)), HttpStatusCode.InternalServerError)
        };

        return errorsResponse;
    }

    private static string GetErrorCode(CryptoNestBaseException exception)
    {
        Type exceptionType = exception.GetType();

        return Codes.GetOrAdd(
            exceptionType,
            exceptionType.Name.Underscore().Replace("_exception", string.Empty));
    }
}