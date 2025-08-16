using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace travel_bien_quynh.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetComment()
        {
            var commentList = await _commentRepository.GetAsync();
            if (commentList == null)
            {
                return NotFound(new { msg = "No news found" });
            }

            var commentResponse = commentList.Select(comment => new
            {
                comment.FullName,
            });

            return Ok(new { data = commentResponse });
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { msg = "Invalid request data" });
            }

            try
            {
                var newComment = new Comment
                {
                    FullName = request.FullName,
                };

                await _commentRepository.CreateAsync(newComment);

                return Ok(new { msg = "okay created successfully" });
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, new { msg = "An error occurred while creating the Information", error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(string id, [FromBody] UpdateCommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { msg = "Invalid request data" });
            }

            var existingComment = await _commentRepository.GetAsync(id);
            if (existingComment == null)
            {
                return NotFound(new { msg = "Information not found" });
            }

            try
            {
                existingComment.FullName = request.FullName;

                await _commentRepository.UpdateAsync(id, existingComment);

                return Ok(new { msg = "c updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, new { msg = "An error occurred while updating the news", error = ex.Message });
            }
        }
    }
}
