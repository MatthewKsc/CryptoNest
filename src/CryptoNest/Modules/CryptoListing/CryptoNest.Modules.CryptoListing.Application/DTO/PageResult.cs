namespace CryptoNest.Modules.CryptoListing.Application.DTO;

public record PageResult<T>(T[] Items, long TotalNumberOfItems, int PageSize, int CurrentPage);