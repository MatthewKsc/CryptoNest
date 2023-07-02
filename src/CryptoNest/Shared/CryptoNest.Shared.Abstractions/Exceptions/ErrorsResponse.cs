using System.Net;

namespace CryptoNest.Shared.Abstractions.Exceptions;

public record ErrorsResponse(object Response, HttpStatusCode StatusCode);