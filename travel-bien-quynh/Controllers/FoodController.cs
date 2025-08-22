using Microsoft.AspNetCore.Mvc;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Entities;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]/[action]")]
public class FoodController : ControllerBase
{
    private readonly IFoodRepository _foodRepository;

    public FoodController(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFood()
    {
        var foodList = await _foodRepository.GetAsync();
        if (foodList == null)
        {
            return NotFound(new { msg = "Not found" });
        }

        var foodResponse = foodList.Select(food => new
        {
            food.Id,
            food.Title,
            food.Label,
            food.Price,
            food.Category,
            food.Image,
            food.Content,
            food.PublishedDate,
            food.Status
        });

        return Ok(new { data = foodResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFoodById(string id)
    {
        var news = await _foodRepository.GetAsync(id);
        if (news == null)
        {
            return NotFound(new { msg = "Not found" });
        }

        return Ok(new { data = news });
    }

    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateFood([FromBody] CreateFoodRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        try
        {
            var newFood = new Food
            {
                Title = request.Title,
                Label = request.Label,
                Content = request.Content,
                Category = request.Category,
                Price = request.Price,
                PublishedDate = DateTime.UtcNow,
                Image = request.Image,
                Status = true
            };

            await _foodRepository.CreateAsync(newFood);

            return Ok(new { msg = "Created successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while creating", error = ex.Message });
        }
    }

    //[Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFood(string id, [FromBody] UpdateFoodRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        var existingFood = await _foodRepository.GetAsync(id);
        if (existingFood == null)
        {
            return NotFound(new { msg = "News not found" });
        }

        try
        {
            existingFood.Title = request.Title;
            existingFood.Label = request.Label;
            existingFood.Content = request.Content;
            existingFood.Category = request.Content;
            existingFood.Price = request.Price;
            existingFood.Status = request.Status;
            existingFood.Image = request.Image;

            await _foodRepository.UpdateAsync(id, existingFood);

            return Ok(new { msg = "Updated successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while updating the news", error = ex.Message });
        }
    }

    //[Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFood(string id)
    {
        var food = await _foodRepository.GetAsync(id);
        if (food == null)
        {
            return NotFound(new { msg = "News not found" });
        }

        try
        {
            await _foodRepository.DeleteAsync(id);
            return Ok(new { msg = "News deleted successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while deleting the news", error = ex.Message });
        }
    }
}
