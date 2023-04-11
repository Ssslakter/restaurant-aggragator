using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;
using RestaurantAggregator.DAL.Data;

namespace RestaurantAggregator.BL.Services;

public class OrderService : IOrderService
{
    private readonly RestaurantDbContext _context;

    public OrderService(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task AssingCookToOrderAsync(Guid orderId, Guid cookId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            throw new NotFoundInDbException($"Order with id {orderId} not found");
        }
        order.CookId = cookId;
        await _context.SaveChangesAsync();
    }

    public async Task AssingCourierToOrderAsync(Guid orderId, Guid courierId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            throw new NotFoundInDbException($"Order with id {orderId} not found");
        }
        order.CourierId = courierId;
        await _context.SaveChangesAsync();
    }

    public async Task ChangeOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            throw new NotFoundInDbException($"Order with id {orderId} not found");
        }
        order.Status = status;
        await _context.SaveChangesAsync();
    }

    public async Task CreateOrderFromCartAsync(Guid clientId, string address)
    {
        var dishesInCart = await _context.DishesInCarts.Where(d => d.ClientId == clientId && !d.InOrder).ToListAsync();
        if (dishesInCart.Count == 0)
        {
            throw new NotFoundInDbException($"Cart for client with id {clientId} is empty");
        }
        //fixate dish prices
        var totalPrice = 0.0M;
        foreach (var dish in dishesInCart)
        {
            dish.Price = dish.Dish.Price;
            dish.InOrder = true;
            totalPrice += dish.Dish.Price * dish.Count;
        }
        var order = new Order
        {
            Status = OrderStatus.Created,
            OrderTime = DateTime.Now,
            ClientId = clientId,
            Dishes = dishesInCart,
            Address = address,
            TotalPrice = totalPrice
        };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<OrderDetails> GetOrderByIdAsync(Guid id)
    {
        var order = await _context.Orders.Include(o => o.Dishes).ThenInclude(d => d.Dish).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            throw new NotFoundInDbException($"Order with id {id} not found");
        }
        return new OrderDetails
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            OrderTime = order.OrderTime,
            DeliveryTime = order.DeliveryTime,
            Address = order.Address,
            TotalPrice = order.TotalPrice,
            Dishes = order.Dishes.ConvertAll(d => new DishInOrderDTO
            {
                Id = d.Dish.Id,
                Name = d.Dish.Name,
                Description = d.Dish.Description,
                Price = d.Dish.Price,
                IsVegeterian = d.Dish.IsVegeterian,
                Photo = d.Dish.Photo,
                Category = d.Dish.Category,
                Count = d.Count,
                MenuId = d.Dish.MenuId
            })
        };
    }

    public async Task<ICollection<OrderDTO>> GetOrdersByClientIdAsync(Guid clientId, OrderStatus? status)
    {
        return await GetOrdersByFieldIdAsync(clientId, "client", status);
    }

    public async Task<ICollection<OrderDTO>> GetOrdersByCookIdAsync(Guid cookId, OrderStatus? status)
    {
        return await GetOrdersByFieldIdAsync(cookId, "cook", status);
    }

    public async Task<ICollection<OrderDTO>> GetOrdersByCourierIdAsync(Guid courierId, OrderStatus? status)
    {
        return await GetOrdersByFieldIdAsync(courierId, "courier", status);
    }

    public async Task<ICollection<OrderDTO>> GetOrdersByRestaurantIdAsync(Guid restaurantId, OrderStatus? status)
    {
        return await GetOrdersByFieldIdAsync(restaurantId, "restaurant", status);
    }

    public async Task<OrderStatus> GetOrderStatusAsync(Guid orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            throw new NotFoundInDbException($"Order with id {orderId} not found");
        }
        return order.Status;
    }

    private async Task<ICollection<OrderDTO>> GetOrdersByFieldIdAsync(Guid fieldId, string fieldName, OrderStatus? status)
    {
        var query = fieldName switch
        {
            "cook" => _context.Orders.Where(o => o.CookId == fieldId),
            "courier" => _context.Orders.Where(o => o.CourierId == fieldId),
            "client" => _context.Orders.Where(o => o.ClientId == fieldId),
            "restaurant" => _context.Orders.Where(o => o.RestaurantId == fieldId),
            _ => throw new ArgumentException($"Role {fieldName} is not supported")
        };
        var orders = await query.FilterByStatus(status).ToListAsync();
        return orders.ConvertAll(o => new OrderDTO
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            Status = o.Status,
            OrderTime = o.OrderTime,
            DeliveryTime = o.DeliveryTime,
            Address = o.Address,
            TotalPrice = o.TotalPrice
        });
    }
}

internal static class IQueryableExtensions
{
    public static IQueryable<Order> FilterByStatus(this IQueryable<Order> orders, OrderStatus? status)
    {
        if (status == null)
        {
            return orders;
        }
        return orders.Where(o => o.Status == status);
    }
}
