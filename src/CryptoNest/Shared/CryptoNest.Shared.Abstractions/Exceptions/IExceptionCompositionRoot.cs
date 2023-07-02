using System;

namespace CryptoNest.Shared.Abstractions.Exceptions;

public interface IExceptionCompositionRoot
{
    ErrorsResponse Map(Exception exception);
}