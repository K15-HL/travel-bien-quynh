using Microsoft.AspNetCore.Mvc;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Entities;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]/[action]")]
public class TourController : ControllerBase
{
    private readonly ITourRepository _tourRepository;

    public TourController(ITourRepository tourRepository)
    {
        _tourRepository = tourRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTour()
    {
        var tourList = await _tourRepository.GetAsync();
        if (tourList == null)
        {
            return NotFound(new { msg = "No news found" });
        }

        var tourResponse = tourList.Select(tour => new
        {
            tour.Id,
            tour.Title,
            tour.Label,
            tour.Image,
            tour.Rating,
            tour.OriginalPrice,
            tour.DiscountedPrice,
            tour.TotalReviews,
            tour.Schedule,
            tour.PeopleRange,
            tour.Description,
            tour.List,
            tour.PublishedDate,
            tour.Status
        });

        return Ok(new { data = tourResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTourById(string id)
    {
        var tour = await _tourRepository.GetAsync(id);
        if (tour == null)
        {
            return NotFound(new { msg = "News not found" });
        }

        return Ok(new { data = tour });
    }

    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTour([FromBody] CreateTourRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        try
        {
            var newTour = new Tour
            {
                Title = request.Title,
                Label = request.Label,
                DiscountedPrice = request.DiscountedPrice,
                OriginalPrice = request.OriginalPrice,
                Duration = request.Duration,
                TotalReviews = request.TotalReviews,
                PeopleRange = request.PeopleRange,
                Schedule = request.Schedule,
                Image = request.Image,
                Description = request.Description,
                List = request.List,
                PublishedDate = DateTime.UtcNow,
                Status = true
            };

            await _tourRepository.CreateAsync(newTour);

            return Ok(new { msg = "Thêm tour successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while creating the news", error = ex.Message });
        }
    }

    //[Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTour(string id, [FromBody] UpdateTourRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        var existingTour = await _tourRepository.GetAsync(id);
        if (existingTour == null)
        {
            return NotFound(new { msg = "Tore not found" });
        }

        try
        {
            existingTour.Title = request.Title;
            existingTour.Label = request.Label;
            existingTour.Image = request.Image;
            existingTour.Description = request.Description;
            existingTour.List = request.List;
            existingTour.Duration = request.Duration;
            existingTour.Rating = request.Rating;
            existingTour.DiscountedPrice = request.DiscountedPrice;
            existingTour.TotalReviews = request.TotalReviews;
            existingTour.Schedule = request.Schedule;
            existingTour.Status = request.Status;

            await _tourRepository.UpdateAsync(id, existingTour);

            return Ok(new { msg = "News updated successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while updating the news", error = ex.Message });
        }
    }

    //[Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTour(string id)
    {
        var tour = await _tourRepository.GetAsync(id);
        if (tour == null)
        {
            return NotFound(new { msg = "News not found" });
        }

        try
        {
            await _tourRepository.DeleteAsync(id);
            return Ok(new { msg = "News deleted successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while deleting the news", error = ex.Message });
        }
    }
}
