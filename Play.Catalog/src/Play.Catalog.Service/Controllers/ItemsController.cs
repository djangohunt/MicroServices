using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers;

// Controllers group the set of actions that can handle API requests.

[ApiController]
// Handles routes starting with the template e.g.
// https://localhost:5001/items
[Route("items")]
public class ItemsController : ControllerBase
{
	// ********************  All the API methods for Items  **********************

	private readonly IItemsRepository _itemsRepository;

	public ItemsController(IItemsRepository itemsRepository)
	{
		_itemsRepository = itemsRepository;
	}

	// API method:
	// Mark the method with the type of HTTP verb that it'll be called by.
	[HttpGet]
	public async Task<IEnumerable<ItemDto>> GetAsync()
	{
		IEnumerable<ItemDto> items = (await _itemsRepository.GetAllAsync())
			.Select(item => item.AsDto());
		
		return items;
	}

	// Provide the template to pull the details we want from the API call.
	// GET /items/{id} -> 
	// GET /items/a407f470-e82b-4209-b080-b9e22f65fffe
	[HttpGet("{id}")]
	public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
	{
		Item? item = await _itemsRepository.GetAsync(id);

		if (item is null)
		{
			return NotFound();
		}

		return item.AsDto();
	}

	[HttpPost]
	// Return ActionResult<ItemDto> so we can return the item and how to find it.
	// Specify the contract for the newly created item (CreateItemDto)
	public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto itemDto)
	{
		//var item = new ItemDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price, DateTimeOffset.UtcNow);
		//Items.Add(item);

		Item item = itemDto.AsItem();
		await _itemsRepository.CreateAsync(item);

		// Tells the caller the item has been created and you can find it at the following route.
		// This is a standard example, giving the get by id method, the anonymous object with the ID and the new object itself.
		return CreatedAtAction(nameof(GetByIdAsync), new {id = item.Id}, item);
	}

	// PUT /items/{id}
	[HttpPut("{id}")]
	// No return type need this time.
	// Specify the contract required to update the item (UpdateItemDto)
	public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
	{
		Item? existingItem = await _itemsRepository.GetAsync(id);

		if (existingItem is null) { return NotFound(); }

		existingItem.Name = updateItemDto.Name;
		existingItem.Description = updateItemDto.Description;
		existingItem.Price = updateItemDto.Price;

		await _itemsRepository.UpdateAsync(existingItem);

		// Nothing to return.
		return NoContent();
	}

	// DELETE /items/{id}
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAsync(Guid id)
	{
		Item? existingItem = await _itemsRepository.GetAsync(id);

		if (existingItem is null) { return NotFound(); }

		await _itemsRepository.RemoveAsync(id);

		return NoContent();
	}
}