using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;
using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.DAL.Data;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.BL.Mappers;

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
        var dishInCart = await _context.DishesInCarts.FirstOrDefaultAsync(c => c.ClientId == clientId && c.DishId == dishId && !c.InOrder);
        if (dishInCart != null)
        {
            dishInCart.Count += quantity;
            await _context.SaveChangesAsync();
            return;
        }
        await _context.DishesInCarts.AddAsync(new DishInCart
        {
            ClientId = clientId,
            Dish = dish,
            DishId = dishId,
            Count = quantity
        });
        await _context.SaveChangesAsync();
    }

    public async Task ClearCartAsync(Guid clientId)
    {
        await _context.DishesInCarts.Where(c => c.ClientId == clientId && !c.InOrder).ExecuteDeleteAsync();
    }

    public async Task<CartDTO> GetCartAsync(Guid clientId)
    {
        var dishes = _context.DishesInCarts.Include(c => c.Dish).Where(c => c.ClientId == clientId && !c.InOrder).ToListAsync();
        return new CartDTO
        {
            ClientId = clientId,
            Dishes = (await dishes).ConvertAll(x => x.ToDTO()),
        };
    }

    public async Task RemoveDishFromCartAsync(Guid dishId, Guid clientId, uint quantity)
    {
        var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish == null)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        }
        var dishInCart = await _context.DishesInCarts.FirstOrDefaultAsync(c => c.ClientId == clientId && c.DishId == dishId && !c.InOrder);
        if (dishInCart == null)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found in cart");
        }
        if (dishInCart.Count <= quantity)
        {
            _context.DishesInCarts.Remove(dishInCart);
        }
        else
        {
            dishInCart.Count -= quantity;
        }
    }
}
