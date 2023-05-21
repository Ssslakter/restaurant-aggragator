using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.BL.Mappers;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;
using RestaurantAggregator.DAL.Data;

namespace RestaurantAggregator.BL.Services;

public class DishService : IDishService
{
    private readonly RestaurantDbContext _context;

    public DishService(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task AddReviewToDishAsync(Guid dishId, Guid clientId, ReviewDTO reviewModel)
    {
        var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish == null)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        }
        if (!_context.DishesInCarts.Any(d => d.DishId == dishId && d.ClientId == clientId && d.InOrder))
        {
            throw new BusinessException($"User can't review dish with id {dishId}, since it wasn't ordered");
        }
        try
        {
            dish.Reviews.Add(new Review
            {
                ClientId = clientId,
                DishId = dishId,
                Value = reviewModel.Value
            });
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new DbViolationException($"Review for dish with id {dishId} already exists");
        }
    }

    public async Task<DishDTO> CreateDishAsync(DishCreation dish, Guid restaurantId)
    {
        var dishEntity = new Dish
        {
            Name = dish.Name,
            Description = dish.Description,
            Price = dish.Price,
            IsVegeterian = dish.IsVegeterian,
            Photo = dish.Photo,
            Category = dish.Category,
            RestaurantId = restaurantId,
            Reviews = new List<Review>()
        };
        await _context.Dishes.AddAsync(dishEntity);
        await _context.SaveChangesAsync();
        return dishEntity.ToDTO();
    }

    public async Task DeleteDishAsync(Guid dishId, Guid restaurantId)
    {
        var result = await _context.Dishes
        .Where(d => d.Id == dishId && d.RestaurantId == restaurantId)
        .ExecuteDeleteAsync();
        if (result == 0)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<DishDTO>> GetDishesAsync(Guid restaurantId)
    {
        return await _context.Dishes.Include(d => d.Reviews)
        .Where(d => d.RestaurantId == restaurantId)
        .Select(d => d.ToDTO())
        .ToListAsync();
    }

    public async Task<DishDTO> GetDishInfoByIdAsync(Guid dishId, Guid restaurantId)
    {
        var dishEntity = await _context.Dishes.Include(d => d.Reviews)
        .FirstOrDefaultAsync(d => d.Id == dishId && d.RestaurantId == restaurantId);
        if (dishEntity == null)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        }
        return dishEntity.ToDTO();
    }

    public async Task<DishDTO> UpdateDishAsync(DishCreation dish, Guid dishId, Guid restaurantId)
    {
        var dishEntity = await _context.Dishes
        .FirstOrDefaultAsync(d => d.Id == dishId && d.RestaurantId == restaurantId);
        if (dishEntity == null)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        }
        dishEntity.Name = dish.Name;
        dishEntity.Description = dish.Description;
        dishEntity.Price = dish.Price;
        dishEntity.IsVegeterian = dish.IsVegeterian;
        dishEntity.Photo = dish.Photo;
        dishEntity.Category = dish.Category;
        await _context.SaveChangesAsync();
        return dishEntity.ToDTO();
    }
}
