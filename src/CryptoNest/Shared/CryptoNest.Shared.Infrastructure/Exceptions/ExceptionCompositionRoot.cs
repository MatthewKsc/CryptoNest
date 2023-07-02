using System;
using System.Collections.Generic;
using System.Linq;
using CryptoNest.Shared.Abstractions.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoNest.Shared.Infrastructure.Exceptions;

internal class ExceptionCompositionRoot : IExceptionCompositionRoot
{
    private readonly IServiceProvider serviceProvider;

    public ExceptionCompositionRoot(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    
    public ErrorsResponse Map(Exception exception)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        IExceptionToResponseMapper[] mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>()
            .ToArray();
        IEnumerable<IExceptionToResponseMapper> nonDefaultMappers = mappers
            .Where(x => x is not ExceptionToResponseMapper);
        ErrorsResponse errorsResponse = nonDefaultMappers
            .Select(x => x.Map(exception))
            .SingleOrDefault(x => x is not null);
        
        if (errorsResponse is not null)
        {
            return errorsResponse;
        }

        IExceptionToResponseMapper defaultMapper = mappers.SingleOrDefault(x => x is ExceptionToResponseMapper);

        return defaultMapper?.Map(exception);
    }
}