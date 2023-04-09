using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.DAL.Data;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.BL.Services;

public class CartService : ICartService
{
    private readonly RestaurantDbContext _context;

    public CartService(RestaurantDbContext context)
    {
        _context = context;
    }
    public async Task AddDishToCartAsync(Guid dishId, Guid clientId, uint quantity)
    {
        var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish == null)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        }
        var dishInCart = await _context.Carts.FirstOrDefaultAsync(c => c.ClientId == clientId && c.DishId == dishId);
        if (dishInCart != null)
        {
            dishInCart.Count += quantity;
            await _context.SaveChangesAsync();
            return;
        }
        await _context.Carts.AddAsync(new DishInCart
        {
            ClientId = clientId,
            Dish = dish,
            DishId = dishId,
            Count = quantity,
        });
        await _context.SaveChangesAsync();
    }

    public async Task ClearCartAsync(Guid clientId)
    {
        await _context.Carts.Where(c => c.ClientId == clientId).ExecuteDeleteAsync();
    }

    public async Task<CartDTO> GetCartAsync(Guid clientId)
    {
        var dishes = _context.Carts.Include(c => c.Dish).Where(c => c.ClientId == clientId).Select(x => new { x.Dish, x.Count }).ToListAsync();
        return new CartDTO
        {
            ClientId = clientId,
            Dishes = (await dishes).ConvertAll(x => new DishInCartDTO
            {
                Dish = new DishDTO
                {
                    Id = x.Dish.Id,
                    Name = x.Dish.Name,
                    Description = x.Dish.Description,
                    Price = x.Dish.Price,
                    MenuId = x.Dish.MenuId,
                    Photo = x.Dish.Photo,
                    IsVegeterian = x.Dish.IsVegeterian,
                    Category = x.Dish.Category,
                },
                Count = x.Count,
            }),
        };
    }

    public async Task RemoveDishFromCartAsync(Guid dishId, Guid clientId, uint quantity)
    {
        var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish == null)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        }
        var dishInCart = await _context.Carts.FirstOrDefaultAsync(c => c.ClientId == clientId && c.DishId == dishId);
        if (dishInCart == null)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found in cart");
        }
        if (dishInCart.Count <= quantity)
        {
            _context.Carts.Remove(dishInCart);
        }
        else
        {
            dishInCart.Count -= quantity;
        }
    }
}
