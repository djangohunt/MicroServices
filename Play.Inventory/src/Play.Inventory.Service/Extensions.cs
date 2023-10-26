using Play.Inventory.Service.Entities;
using static Play.Inventory.Services.Dtos;

namespace Play.Inventory.Service;

public static class Extensions
{
	public static InventoryItemDto AsDto(this InventoryItem item)
	{
		return new InventoryItemDto(item.CatalogItemId, item.Quantity, item.AcquiredDate);
	}
}