using Microsoft.AspNetCore.Mvc;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Entities;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]/[action]")]
public class NewsController : ControllerBase
{
    private readonly INewsRepository _newsRepository;

    public NewsController(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNews()
    {
        var newsList = await _newsRepository.GetAsync();
        if (newsList == null)
        {
            return NotFound(new { msg = "No news found" });
        }

        var newsResponse = newsList.Select(news => new
        {
            news.Id,
            news.Title,
            news.Link,
            news.Image,
            news.Category,
            news.Description,
            news.Author,
            news.Content,
            news.PublishedDate,
            news.Status
        });

        return Ok(new { data = newsResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsById(string id)
    {
        var news = await _newsRepository.GetAsync(id);
        if (news == null)
        {
            return NotFound(new { msg = "News not found" });
        }

        return Ok(new { data = news });
    }

    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateNews([FromBody] CreateNewsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        try
        {
            var newNews = new News
            {
                Title = request.Title,
                Image = request.Image,
                Link = request.Link,
                Content = request.Content,
                Description = request.Description,
                Author = request.Author,
                Category = request.Category,
                PublishedDate = DateTime.UtcNow,
                Status = true
            };

            await _newsRepository.CreateAsync(newNews);

            return Ok(new { msg = "News created successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while creating the news", error = ex.Message });
        }
    }

    //[Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNews(string id, [FromBody] UpdateNewsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        var existingNews = await _newsRepository.GetAsync(id);
        if (existingNews == null)
        {
            return NotFound(new { msg = "News not found" });
        }

        try
        {
            existingNews.Title = request.Title;
            existingNews.Image = request.Image;
            existingNews.Link = request.Link;
            existingNews.Content = request.Content;
            existingNews.Description = request.Description;
            existingNews.Author = request.Author;
            existingNews.Category = request.Category;
            existingNews.Status = request.Status;

            await _newsRepository.UpdateAsync(id, existingNews);

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
    public async Task<IActionResult> DeleteNews(string id)
    {
        var news = await _newsRepository.GetAsync(id);
        if (news == null)
        {
            return NotFound(new { msg = "News not found" });
        }

        try
        {
            await _newsRepository.DeleteAsync(id);
            return Ok(new { msg = "News deleted successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while deleting the news", error = ex.Message });
        }
    }
}
