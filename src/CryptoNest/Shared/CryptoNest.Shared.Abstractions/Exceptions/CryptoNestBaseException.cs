using System;

namespace CryptoNest.Shared.Abstractions.Exceptions;

public abstract class CryptoNestBaseException : Exception
{
    protected CryptoNestBaseException(string message) : base(message) { }
}