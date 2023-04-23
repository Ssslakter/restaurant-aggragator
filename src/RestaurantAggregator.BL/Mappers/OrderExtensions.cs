using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.DAL.Data;

namespace RestaurantAggregator.BL.Mappers;

public static class OrderExtensions
{
    public static OrderDTO ToDTO(this Order order)
    {
        return new OrderDTO
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            OrderTime = order.OrderTime,
            DeliveryTime = order.DeliveryTime,
            Address = order.Address,
            TotalPrice = order.TotalPrice
        };
    }

    public static OrderWithDishes ToOrderWithDishes(this Order order)
    {
        return new OrderWithDishes
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            OrderTime = order.OrderTime,
            DeliveryTime = order.DeliveryTime,
            Address = order.Address,
            TotalPrice = order.TotalPrice,
            Dishes = order.Dishes.ConvertAll(d => d.ToOrderDTO(d.Price))
        };
    }

    public static OrderDetails ToDetails(this Order order)
    {
        return new OrderDetails
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            OrderTime = order.OrderTime,
            DeliveryTime = order.DeliveryTime,
            Address = order.Address,
            TotalPrice = order.TotalPrice,
            ClientName = order.ClientName,
            CourierName = order.CourierName,
            Dishes = order.Dishes.ConvertAll(d => d.ToOrderDTO(d.Price))
        };
    }
}