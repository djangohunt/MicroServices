using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Entities;
using static Play.Inventory.Services.Dtos;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
	private readonly IRepository<InventoryItem> _itemsRepository;

	public ItemsController(IRepository<InventoryItem> itemsRepository)
	{
		_itemsRepository = itemsRepository;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
	{
		if (userId == Guid.Empty)
		{
			return BadRequest();
		}

		IEnumerable<InventoryItemDto> items =
			(await _itemsRepository.GetAllAsync(item => item.UserId == userId))
			.Select(item => item.AsDto());

		return Ok(items);
	}

	[HttpPost]
	public async Task<ActionResult> PostAsync(GrantItemsDto grantItemDto)
	{
		InventoryItem? inventoryItem = await _itemsRepository.GetAsync(item => item.Id == grantItemDto.CatalogItemId);

		if (inventoryItem is null)
		{
			inventoryItem = new InventoryItem
			{
				CatalogItemId = grantItemDto.CatalogItemId,
				UserId = grantItemDto.UserId,
				Quantity = grantItemDto.Quantity,
				AcquiredDate = DateTimeOffset.UtcNow
			};

			await _itemsRepository.CreateAsync(inventoryItem);
		}
		else
		{
			inventoryItem.Quantity = grantItemDto.Quantity;
			await _itemsRepository.UpdateAsync(inventoryItem);
		}

		return Ok(inventoryItem);
	}
}