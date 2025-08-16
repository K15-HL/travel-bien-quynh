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
            return NotFound(new { msg = "No news found" });
        }

        var foodResponse = foodList.Select(food => new
        {
            food.Id,
            food.Title,
            food.Price,
            food.Category,
            food.Image,
            food.Content,
            food.PublishedDate,
            food.IsActive
        });

        return Ok(new { data = foodResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFoodById(string id)
    {
        var news = await _foodRepository.GetAsync(id);
        if (news == null)
        {
            return NotFound(new { msg = "Food not found" });
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
                Content = request.Content,
                Category = request.Category,
                Price = request.Price,
                PublishedDate = DateTime.UtcNow,
                Image = request.Image,
                IsActive = true
            };

            await _foodRepository.CreateAsync(newFood);

            return Ok(new { msg = "Food created successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while creating the news", error = ex.Message });
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
            existingFood.Content = request.Content;
            existingFood.Category = request.Content;
            existingFood.Price = request.Price;
            existingFood.IsActive = request.IsActive;
            existingFood.Image = request.Image;

            await _foodRepository.UpdateAsync(id, existingFood);

            return Ok(new { msg = "Food updated successfully" });
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
