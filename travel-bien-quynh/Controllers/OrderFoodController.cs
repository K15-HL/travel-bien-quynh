using Microsoft.AspNetCore.Mvc;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Entities;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrderFoodController : ControllerBase
{
    private readonly IOrderFoodRepository _orderFoodRepository;

    public OrderFoodController(IOrderFoodRepository orderFoodRepository)
    {
        _orderFoodRepository = orderFoodRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrderFood()
    {
        var orderFoodList = await _orderFoodRepository.GetAsync();
        if (orderFoodList == null)
        {
            return NotFound(new { msg = "No orderFood found" });
        }

        var orderFoodResponse = orderFoodList.Select(orderFood => new
        {
            orderFood.Id,
            orderFood.Email,
            orderFood.Phone,
            orderFood.Adults,
            orderFood.CheckIn,
            orderFood.Checkout,
            orderFood.Nationality,
            orderFood.DepartureDate,
            orderFood.Children
        });

        return Ok(new { data = orderFoodResponse });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderFoodById(string id)
    {
        var e = await _orderFoodRepository.GetAsync(id);
        if (e == null)
        {
            return NotFound(new { msg = "orderFood not found" });
        }

        return Ok(new { data = e });
    }

    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> OrderFood([FromBody] OrderFoodRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        try
        {
            var newOrderFood = new OrderFood
            {
                Email = request.Email,
                Phone = request.Phone,
                DepartureDate = request.DepartureDate,
            };

            await _orderFoodRepository.CreateAsync(newOrderFood);

            return Ok(new { msg = "OrderFood successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error ", error = ex.Message });
        }
    }

    //[Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderFood(string id)
    {
        var orderFood = await _orderFoodRepository.GetAsync(id);
        if (orderFood == null)
        {
            return NotFound(new { msg = "orderFood not found" });
        }

        try
        {
            await _orderFoodRepository.DeleteAsync(id);
            return Ok(new { msg = "orderFood deleted successfully" });
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, new { msg = "An error occurred while deleting the orderFood", error = ex.Message });
        }
    }
}
