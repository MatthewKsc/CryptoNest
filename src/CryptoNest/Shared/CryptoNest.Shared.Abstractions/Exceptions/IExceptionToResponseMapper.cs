using System;

namespace CryptoNest.Shared.Abstractions.Exceptions;

public interface IExceptionToResponseMapper
{
    ErrorsResponse Map(Exception exception);
}