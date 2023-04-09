using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;
using RestaurantAggregator.DAL.Data;

namespace RestaurantAggregator.BL;

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

    public async Task CreateDishAsync(DishCreation dish, Guid menuId)
    {
        var menu = await _context.Menus.FirstOrDefaultAsync(m => m.Id == menuId);
        if (menu == null)
        {
            throw new NotFoundInDbException($"Menu with id {menuId} not found");
        }

        var dishEntity = new Dish
        {
            Name = dish.Name,
            Description = dish.Description,
            Price = dish.Price,
            IsVegeterian = dish.IsVegeterian,
            Photo = dish.Photo,
            Category = dish.Category,
            MenuId = menuId,
            Reviews = new List<Review>()
        };
        _context.Dishes.Add(dishEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDishAsync(Guid id)
    {
        var result = await _context.Dishes.Where(d => d.Id == id).ExecuteDeleteAsync();
        if (result == 0)
        {
            throw new NotFoundInDbException($"Dish with id {id} not found");
        }
        await _context.SaveChangesAsync();
    }

    public async Task<DishDTO> GetDishInfoByIdAsync(Guid id)
    {
        var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        if (dish == null)
        {
            throw new NotFoundInDbException($"Dish with id {id} not found");
        }
        return new DishDTO
        {
            Id = dish.Id,
            Name = dish.Name,
            Description = dish.Description,
            Price = dish.Price,
            IsVegeterian = dish.IsVegeterian,
            Photo = dish.Photo,
            Category = dish.Category,
            MenuId = dish.MenuId
        };
    }

    public async Task<double> GetDishRatingAsync(Guid dishId)
    {
        var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish == null)
        {
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        }
        var ratings = await _context.Reviews.Where(r => r.DishId == dishId).Select(r => r.Value).ToListAsync();
        if (ratings.Any())
        {
            return ratings.Average(r => r);
        }
        return 0;
    }

    public async Task UpdateDishAsync(DishCreation dish, Guid id)
    {
        var dishEntity = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        if (dishEntity == null)
        {
            throw new NotFoundInDbException($"Dish with id {id} not found");
        }
        dishEntity.Name = dish.Name;
        dishEntity.Description = dish.Description;
        dishEntity.Price = dish.Price;
        dishEntity.IsVegeterian = dish.IsVegeterian;
        dishEntity.Photo = dish.Photo;
        dishEntity.Category = dish.Category;
        await _context.SaveChangesAsync();
    }
}
