using Microsoft.AspNetCore.Mvc;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Entities;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]/[action]")]
public class SliderController : ControllerBase
{
    private readonly ISliderRepository _sliderRepository;

    public SliderController(ISliderRepository sliderRepository)
    {
        _sliderRepository = sliderRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSlider()
    {
        var roomList = await _sliderRepository.GetAsync();
        if (roomList == null)
        {
            return NotFound(new { msg = "No room found" });
        }

        var roomResponse = roomList.Select(room => new
        {
            room.Id,
            room.Image,
            room.Description,
            room.PublishedDate,
            room.IsActive
        });

        return Ok(new { data = roomResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSliderById(string id)
    {
        var room = await _sliderRepository.GetAsync(id);
        if (room == null)
        {
            return NotFound(new { msg = "Slider not found" });
        }

        return Ok(new { data = room });
    }

    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSlider([FromBody] CreateSliderRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        try
        {
            var newSlider = new Slider
            {
                Title = request.Title,
                Image = request.Image,
                Description = request.Description,
                PublishedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _sliderRepository.CreateAsync(newSlider);

            return Ok(new { msg = "Room created successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while creating the news", error = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSlider(string id, [FromBody] UpdateSliderRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        var existingSlider = await _sliderRepository.GetAsync(id);
        if (existingSlider == null)
        {
            return NotFound(new { msg = "Room not found" });
        }

        try
        {
            existingSlider.Title = request.Title;
            existingSlider.Image = request.Image;
            existingSlider.Description = request.Description;
            existingSlider.IsActive = request.IsActive;

            await _sliderRepository.UpdateAsync(id, existingSlider);

            return Ok(new { msg = "Room updated successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while updating the room", error = ex.Message });
        }
    }

    //[Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSlider(string id)
    {
        var room = await _sliderRepository.GetAsync(id);
        if (room == null)
        {
            return NotFound(new { msg = "Room not found" });
        }

        try
        {
            await _sliderRepository.DeleteAsync(id);
            return Ok(new { msg = "Room deleted successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while deleting the news", error = ex.Message });
        }
    }
}
