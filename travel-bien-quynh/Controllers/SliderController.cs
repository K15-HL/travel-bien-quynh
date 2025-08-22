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
        var sliderList = await _sliderRepository.GetAsync();
        if (sliderList == null)
        {
            return NotFound(new { msg = "No Slider found" });
        }

        var sliderResponse = sliderList.Select(slider => new
        {
            slider.Id,
            slider.Image,
            slider.Title,
            slider.Description,
            slider.Url,
            slider.PublishedDate,
            slider.Status,
        });

        return Ok(new { data = sliderResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSliderById(string id)
    {
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null)
        {
            return NotFound(new { msg = "Slider not found" });
        }

        return Ok(new { data = slider });
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
                Url = request.Url,
                Status = true
            };

            await _sliderRepository.CreateAsync(newSlider);

            return Ok(new { msg = "Slider created successfully" });
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
            return NotFound(new { msg = "Slider not found" });
        }

        try
        {
            existingSlider.Title = request.Title;
            existingSlider.Image = request.Image;
            existingSlider.Description = request.Description;
            existingSlider.Status = request.Status;

            await _sliderRepository.UpdateAsync(id, existingSlider);

            return Ok(new { msg = "Slider updated successfully" });
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
            return NotFound(new { msg = "Slider not found" });
        }

        try
        {
            await _sliderRepository.DeleteAsync(id);
            return Ok(new { msg = "Slider deleted successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while deleting the news", error = ex.Message });
        }
    }
}
