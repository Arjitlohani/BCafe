
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
        public static List<Order> GetAllOrdersByCustomerId(int customerId)
        {
            return GetAllOrders().Where(o => o.CustomerId == customerId).ToList();
        }
        public static void AddOrder(Order order)
        {
            var orders = GetAllOrders();
            // Apply discount logic here
            var customer = CustomersService.GetCustomerByName(order.CustomerName);
            if (customer != null)
            {
                var regularOrderCount = CustomersService.GetRegularOrderCount(order.CustomerName, DateTime.Now.AddMonths(-1), DateTime.Now);
                if (regularOrderCount >= 10) // Assuming 10 is the threshold for regular
                {
                    // Apply 10% discount
                    order.CoffeePrice *= 0.9m;
                }

                // Check if the order is the 11th order
                if ((regularOrderCount + 1) % 11 == 0)
                {
                    // Make the 11th order free
                    order.CoffeePrice = 0;
                }
            }
           
            order.Id = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;
            order.CreatedAt = DateTime.Now;
            CalculateTotalPrice(order);
            orders.Add(order);
            SaveAll(orders);
        }
        public static int GetOrderCountForCustomer(int customerId)
        {
            return GetAllOrders().Count(o => o.Id == customerId);
        }
        public static bool IsCustomerRegular(int customerId, DateTime startDate, DateTime endDate)
        {
            // Get all orders for the customer
            var orders = GetAllOrders().Where(o => o.Id == customerId).ToList();

            // Get all weekdays in the range
            var weekdaysInRange = GetWeekdaysInRange(startDate, endDate);

            // Check if there's at least one order for each weekday
            foreach (var date in weekdaysInRange)
            {
                if (!orders.Any(o => o.OrderDate.Date == date.Date))
                {
                    return false; // If any weekday has no order, return false
                }
            }

            return true; // All weekdays have at least one order
        }

        private static IEnumerable<DateTime> GetWeekdaysInRange(DateTime start, DateTime end)
        {
            var dates = new List<DateTime>();

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                {
                    dates.Add(dt);
                }
            }

            return dates;
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


      
    }
}
