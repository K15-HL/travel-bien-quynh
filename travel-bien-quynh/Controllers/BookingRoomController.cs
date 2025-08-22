using Microsoft.AspNetCore.Mvc;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using travel_bien_quynh.Hubs;

[ApiController]
[Route("api/[controller]/[action]")]
public class BookingRoomController : ControllerBase
{
    private readonly IBookingRoomRepository _bookingRoomRepository;
    private readonly IHubContext<BookingHub> _hub;
    public BookingRoomController(IBookingRoomRepository bookingRoomRepository, IHubContext<BookingHub> hub)
    {
        _bookingRoomRepository = bookingRoomRepository;
        _hub = hub;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBookingRoom()
    {
        var bookingRoomList = await _bookingRoomRepository.GetAsync();
        if (bookingRoomList == null)
        {
            return NotFound(new { msg = "No bookingRoom found" });
        }

        var bookingRoomResponse = bookingRoomList.Select(bookingRoom => new
        {
            bookingRoom.Id,
            bookingRoom.Hotel,
            bookingRoom.FullName,
            bookingRoom.RoomType,
            bookingRoom.Nationality,
            bookingRoom.Email,
            bookingRoom.Phone,
            bookingRoom.Adults,
            bookingRoom.CheckIn,
            bookingRoom.Checkout,
            bookingRoom.Children,
            bookingRoom.TotalPrice,
        });

        return Ok(new { data = bookingRoomResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingRoomById(string id)
    {
        var e = await _bookingRoomRepository.GetAsync(id);
        if (e == null)
        {
            return NotFound(new { msg = "bookingRoom not found" });
        }

        return Ok(new { data = e });
    }

    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> BookingRoom([FromBody] BookingRoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        try
        {
            var newBookingRoom = new BookingRoom
            {
                Hotel = request.Hotel,
                RoomType = request.RoomType,
                CheckIn = request.CheckIn,
                Checkout = request.Checkout,
                Adults = request.Adults,
                Children = request.Children,
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                Nationality = request.Nationality,
                SpecialRequests = request.SpecialRequests,
                PaymentMethod = request.PaymentMethod,
                TotalPrice = request.TotalPrice,
                
            };

            await _bookingRoomRepository.CreateAsync(newBookingRoom);
            await _hub.Clients.All.SendAsync("ReceiveBooking", new
            {
                newBookingRoom.Hotel,
                newBookingRoom.RoomType,
                newBookingRoom.FullName,
                newBookingRoom.CheckIn,
                newBookingRoom.Checkout
            });
            return Ok(new { msg = "BookingRoom successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error ", error = ex.Message });
        }
    }

    //[Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(string id)
    {
        var bookingRoom = await _bookingRoomRepository.GetAsync(id);
        if (bookingRoom == null)
        {
            return NotFound(new { msg = "bookingRoom not found" });
        }

        try
        {
            await _bookingRoomRepository.DeleteAsync(id);
            return Ok(new { msg = "bookingRoom deleted successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while deleting the booking", error = ex.Message });
        }
    }
}
