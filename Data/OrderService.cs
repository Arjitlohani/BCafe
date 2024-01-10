using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BCafe.Data
{
    public class OrderService
    {
        private static string ordersFilePath = Path.Combine(Utils.GetAppDirectoryPath(), "orders.json");

        public static List<Order> GetAllOrders()
        {
            if (!File.Exists(ordersFilePath))
            {
                return new List<Order>();
            }

            var json = File.ReadAllText(ordersFilePath);
            return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
        }

        private static void SaveAll(List<Order> orders)
        {
            var json = JsonSerializer.Serialize(orders);
            File.WriteAllText(ordersFilePath, json);
        }

        public static void AddOrder(Order order)
        {
            var orders = GetAllOrders();
            order.Id = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;
            order.OrderDate = DateTime.Now;
            CalculateTotalPrice(order);
            orders.Add(order);
            SaveAll(orders);
        }

        public static void UpdateOrder(Order order)
        {
            var orders = GetAllOrders();
            var existingOrder = orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder != null)
            {
                existingOrder.Coffee = order.Coffee;
                existingOrder.CoffeePrice = order.CoffeePrice;
                existingOrder.AddInsPrice = order.AddInsPrice;
                CalculateTotalPrice(existingOrder); // Update total price
            }
            SaveAll(orders);
        }

        public static void DeleteOrder(int id)
        {
            var orders = GetAllOrders();
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                orders.Remove(order);
            }
            SaveAll(orders);
        }

        // Helper method to calculate the total price of an order
        private static void CalculateTotalPrice(Order order)
        {
            // Assuming CoffeePrice and AddInsPrice are already set
            order.TotalPrice = order.CoffeePrice + order.AddInsPrice;
        }


        internal static void DeleteByUserId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
