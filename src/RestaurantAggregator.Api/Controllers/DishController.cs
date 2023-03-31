using Microsoft.AspNetCore.Mvc;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("dish")]
public class DishController : ControllerBase
{
    [HttpGet("{dishId}")]
    public Task<IActionResult> GetDishInfo(string dishId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("create")]
    public Task<IActionResult> CreateDish(string dishModel)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{dishId}/edit")]
    public Task<IActionResult> EditDish(string dishId, string dishModel)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{dishId}/delete")]
    public Task<IActionResult> DeleteDish(string dishId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{dishId}/review")]
    public Task<IActionResult> AddDishReview(string dishId, string reviewModel)
    {
        throw new NotImplementedException();
    }
}
