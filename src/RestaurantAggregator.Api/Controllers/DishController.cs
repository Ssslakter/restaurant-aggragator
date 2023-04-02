using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("dish")]
public class DishController : ControllerBase
{
    [HttpGet("{dishId}")]
    public Task<ActionResult<DishDTO>> GetDishInfo(Guid dishId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("create")]
    public Task<IActionResult> CreateDish(DishCreation dish)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{dishId}/edit")]
    public Task<IActionResult> EditDish(Guid dishId, DishCreation dish)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{dishId}/delete")]
    public Task<IActionResult> DeleteDish(Guid dishId)
    {
        throw new NotImplementedException();
    }
    //TODO: add auth
    [HttpPost("{dishId}/review")]
    public Task<IActionResult> AddDishReview(Guid dishId, ReviewDTO reviewModel)
    {
        throw new NotImplementedException();
    }
}
