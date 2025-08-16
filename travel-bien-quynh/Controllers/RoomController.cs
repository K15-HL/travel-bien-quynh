using Microsoft.AspNetCore.Mvc;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Entities;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]/[action]")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;

    public RoomController(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoom()
    {
        var roomList = await _roomRepository.GetAsync();
        if (roomList == null)
        {
            return NotFound(new { msg = "No room found" });
        }

        var roomResponse = roomList.Select(room => new
        {
            room.Id,
            room.Title,
            room.Rating,
            room.Acreage,
            room.Adults,
            room.Children,
            room.Image,
            room.Description,
            room.Price,
            room.Amenities,
            room.PublishedDate,
            room.Status,
        });

        return Ok(new { data = roomResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoomById(string id)
    {
        var room = await _roomRepository.GetAsync(id);
        if (room == null)
        {
            return NotFound(new { msg = "Room not found" });
        }

        return Ok(new { data = room });
    }

    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        try
        {
            var newRoom = new Room
            {
                Title = request.Title,
                Description = request.Description,
                Image = request.Image,
                Acreage = request.Acreage,
                Price = request.Price,
                Rating = request.Rating,
                Amenities = request.Amenities,
                Adults = request.Adults,
                Children = request.Children,
                PublishedDate = DateTime.UtcNow,
                Status = true
            };

            await _roomRepository.CreateAsync(newRoom);

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
    public async Task<IActionResult> UpdateRoom(string id, [FromBody] UpdateRoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        var existingRoom = await _roomRepository.GetAsync(id);
        if (existingRoom == null)
        {
            return NotFound(new { msg = "Room not found" });
        }

        try
        {
            existingRoom.Title = request.Title;
            existingRoom.Image = request.Image;
            existingRoom.Description = request.Description;
            existingRoom.Amenities = request.Amenities;
            existingRoom.Price = request.Price;
            existingRoom.Status = request.Status;
            existingRoom.Rating = request.Rating;
            existingRoom.Children = request.Children;
            existingRoom.Adults = request.Adults;
            existingRoom.Acreage = request.Acreage;

            await _roomRepository.UpdateAsync(id, existingRoom);

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
    public async Task<IActionResult> DeleteRoom(string id)
    {
        var room = await _roomRepository.GetAsync(id);
        if (room == null)
        {
            return NotFound(new { msg = "Room not found" });
        }

        try
        {
            await _roomRepository.DeleteAsync(id);
            return Ok(new { msg = "Room deleted successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while deleting the news", error = ex.Message });
        }
    }
}
