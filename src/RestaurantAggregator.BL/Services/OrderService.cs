using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.Auth.Client.Services;
using RestaurantAggregator.BL.Mappers;
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
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;

    public OrderService(RestaurantDbContext context, IUserService userService, INotificationService notificationService)
    {
        _context = context;
        _userService = userService;
        _notificationService = notificationService;
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
        var courierProfile = await _userService.GetProfileAsync(courierId);
        order.CourierName = $"{courierProfile.Name} {courierProfile.MiddleName} {courierProfile.Surname}";
        await _context.SaveChangesAsync();
    }

    public async Task ChangeOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            throw new NotFoundInDbException($"Order with id {orderId} not found");
        }
        if ((status - order.Status != 1) && status != OrderStatus.Canceled)
        {
            throw new BusinessException("Order status can be changed only by 1 step");
        }
        order.Status = status;
        _notificationService.SendOrderStatusNotification(new OrderStatusNotification
        {
            ClientId = order.ClientId,
            OrderNumber = (int)order.OrderNumber,
            Status = status
        });
        await _context.SaveChangesAsync();
    }

    public async Task CreateOrderFromCartAsync(Guid clientId, string address)
    {
        var dishesInCart = await _context.DishesInCarts
        .Include(d => d.Dish).Where(d => d.ClientId == clientId && !d.InOrder).ToListAsync();
        if (dishesInCart.Count == 0)
        {
            throw new NotFoundInDbException("Cart for client is empty");
        }
        if (dishesInCart.Select(d => d.Dish.RestaurantId).Distinct().Count() > 1)
        {
            throw new InvalidDataException("All dishes in cart must be from the same restaurant");
        }
        var clientProfile = await _userService.GetProfileAsync(clientId);
        var totalPrice = 0.0M;
        foreach (var dish in dishesInCart)
        {
            dish.Price = dish.Dish.Price;
            dish.InOrder = true;
            totalPrice += dish.Dish.Price * dish.Count;
        }
        var order = new Order
        {
            RestaurantId = dishesInCart[0].Dish.RestaurantId,
            Status = OrderStatus.Created,
            OrderTime = DateTime.Now.ToUniversalTime(),
            DeliveryTime = DateTime.Now.AddHours(1).ToUniversalTime(),
            ClientId = clientId,
            Dishes = dishesInCart,
            Address = address,
            TotalPrice = totalPrice,
            ClientName = $"{clientProfile.Name} {clientProfile.MiddleName} {clientProfile.Surname}"
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
        return order.ToDetails();
    }

    public async Task<ICollection<OrderDTO>> GetOrdersByClientIdAsync(Guid clientId, OrderStatus? status, uint page)
    {
        var orders = await GetOrdersByFieldId(clientId, "client", status).ToListAsync();
        return orders.ConvertAll(o => o.ToDTO());
    }

    public async Task<ICollection<OrderWithDishes>> GetOrdersByCookIdAsync(Guid cookId, uint page)
    {
        var orders = await GetOrdersByFieldId(cookId, "cook", OrderStatus.Kitchen).Include(o => o.Dishes).ToListAsync();
        return orders.ConvertAll(o => o.ToOrderWithDishes());
    }

    public async Task<ICollection<OrderDTO>> GetOrdersByCourierIdAsync(Guid courierId, uint page)
    {
        var orders = await GetOrdersByFieldId(courierId, "courier", OrderStatus.Delivery).ToListAsync();
        return orders.ConvertAll(o => o.ToDTO());
    }

    public async Task<ICollection<OrderDTO>> GetOrdersByRestaurantIdAsync(Guid restaurantId, OrderStatus? status, uint page)
    {
        var orders = await GetOrdersByFieldId(restaurantId, "restaurant", status).ToListAsync();
        return orders.ConvertAll(o => o.ToDTO());
    }

    public async Task<ICollection<OrderDTO>> GetOrdersByStatusAsync(OrderStatus status, uint page)
    {
        var orders = await GetOrdersByFieldId(Guid.Empty, "any", status).ToListAsync();
        return orders.ConvertAll(o => o.ToDTO());
    }

    private IQueryable<Order> GetOrdersByFieldId(Guid fieldId, string fieldName, OrderStatus? status)
    {
        var query = fieldName switch
        {
            "any" => _context.Orders.AsQueryable(),
            "cook" => _context.Orders.Where(o => o.CookId == fieldId),
            "courier" => _context.Orders.Where(o => o.CourierId == fieldId),
            "client" => _context.Orders.Where(o => o.ClientId == fieldId),
            "restaurant" => _context.Orders.Where(o => o.RestaurantId == fieldId),
            _ => throw new ArgumentException($"Role {fieldName} is not supported")
        };
        return query.FilterByStatus(status);
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