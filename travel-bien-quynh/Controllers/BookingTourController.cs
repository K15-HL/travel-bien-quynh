using Microsoft.AspNetCore.Mvc;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Entities;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]/[action]")]
public class BookingTourController : ControllerBase
{
    private readonly IBookingTourRepository _bookingTourRepository;

    public BookingTourController(IBookingTourRepository bookingTourRepository)
    {
        _bookingTourRepository = bookingTourRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooking()
    {
        var bookingTourList = await _bookingTourRepository.GetAsync();
        if (bookingTourList == null)
        {
            return NotFound(new { msg = "No news found" });
        }

        var bookingTourResponse = bookingTourList.Select(bookingTour => new
        {
            bookingTour.Id,
            bookingTour.FullName,
            bookingTour.Email,
            bookingTour.Phone,
            bookingTour.Address,
            bookingTour.Adults,
            bookingTour.Infants,
            bookingTour.Children,
            bookingTour.TotalPrice,
            bookingTour.DepartureDate,
        });

        return Ok(new { data = bookingTourResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingTourById(string id)
    {
        var e = await _bookingTourRepository.GetAsync(id);
        if (e == null)
        {
            return NotFound(new { msg = "bookingTour not found" });
        }

        return Ok(new { data = e });
    }

    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> BookingTour([FromBody] BookingTourRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        try
        {
            var newBookingTour = new BookingTour
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                TourType = request.TourType,
                Children = request.Children,
                Infants = request.Infants,
                TotalPrice = request.TotalPrice,
                DepartureDate = request.DepartureDate,
                Address = request.Address,
                PaymentMethod = request.PaymentMethod,
            };

            await _bookingTourRepository.CreateAsync(newBookingTour);

            return Ok(new { msg = "BookingTour successfully" });
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
        var bookingTour = await _bookingTourRepository.GetAsync(id);
        if (bookingTour == null)
        {
            return NotFound(new { msg = "bookingTour not found" });
        }

        try
        {
            await _bookingTourRepository.DeleteAsync(id);
            return Ok(new { msg = "bookingTour deleted successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while deleting the booking", error = ex.Message });
        }
    }
}
