using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos;

// *********************  DTO's for Item API methods  **********************
public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
// Range sets the validation for the price.
public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
// Required enforces that the field is entered.
public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
public record UpdateItemPriceDto([Range(0, 5000)] decimal Price);