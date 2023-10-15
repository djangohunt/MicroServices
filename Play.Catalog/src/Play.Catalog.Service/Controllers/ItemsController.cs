using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers;

// Controllers group the set of actions that can handle API requests.

[ApiController]
// Handles routes starting with the template e.g.
// https://localhost:5001/items
[Route("items")]
public class ItemsController : ControllerBase
{
	// ********************  All the API methods for Items  **********************

	// Made static so the list isn't recreated ever time an API method is invoked!!!
	// If you don't make it static then this list will be created every time this class is instantiated.
	private static readonly List<ItemDto> Items = new()
	{
		new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
		new ItemDto(Guid.NewGuid(), "Antidote", "Antidote to poison", 7, DateTimeOffset.Now),
		new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
	};

	// API method:
	// Mark the method with the type of HTTP verb that it'll be called by.
	[HttpGet]
	public IEnumerable<ItemDto> Get()
	{
		return Items;
	}

	// Provide the template to pull the details we want from the API call.
	// GET /items/{id} -> 
	// GET /items/a407f470-e82b-4209-b080-b9e22f65fffe
	[HttpGet("{id}")]
	public IActionResult GetById(Guid id)
	{
		ItemDto? item = Items.SingleOrDefault(item => item.Id == id);

		if (item is null)
		{
			return NotFound();
		}

		return Ok(item);
	}

	[HttpPost]
	// Return ActionResult<ItemDto> so we can return the item and how to find it.
	// Specify the contract for the newly created item (CreateItemDto)
	public ActionResult<ItemDto> Post(CreateItemDto itemDto)
	{
		var item = new ItemDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price, DateTimeOffset.UtcNow);
		Items.Add(item);

		// Tells the caller the item has been created and you can find it at the following route.
		// This is a standard example, giving the get by id method, the anonymous object with the ID and the new object itself.
		return CreatedAtAction(nameof(GetById), new {id = item.Id}, item);
	}

	// PUT /items/{id}
	[HttpPut("{id}")]
	// No return type need this time.
	// Specify the contract required to update the item (UpdateItemDto)
	public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
	{
		ItemDto? existingItem = Items.SingleOrDefault(item => item.Id == id);

		if (existingItem is null)
		{
			return NotFound();
		}

		// Create a clone as ItemDto is immutable.
		ItemDto updatedItem = existingItem with
		{
			Name = updateItemDto.Name, 
			Description = updateItemDto.Description, 
			Price = updateItemDto.Price
		};

		// Replace the item with the updated version.
		int index = Items.FindIndex(item => item.Id == id);
		Items[index] = updatedItem;

		// Nothing to return.
		return NoContent();
	}

	// DELETE /items/{id}
	[HttpDelete("{id}")]
	public IActionResult Delete(Guid id)
	{
		int index = Items.FindIndex(item => item.Id == id);

		if (index < 0)
		{
			return NotFound();
		}

		Items.RemoveAt(index);

		return NoContent();
	}
}