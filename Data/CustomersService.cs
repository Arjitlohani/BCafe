
using System.Text.Json;

namespace BCafe.Data
{
    public static class CustomersService
    {
        private static string customersFilePath = Path.Combine(Utils.GetAppDirectoryPath(), "customers.json");

        public static List<Customer> GetAll()
        {
            if (!File.Exists(customersFilePath))
            {
                return new List<Customer>();
            }

            var json = File.ReadAllText(customersFilePath);
            return JsonSerializer.Deserialize<List<Customer>>(json) ?? new List<Customer>();
        }

        private static void SaveAll(List<Customer> customers)
        {
            var json = JsonSerializer.Serialize(customers);
            File.WriteAllText(customersFilePath, json);
        }
        public static Customer GetCustomerByName(string name)
        {
            var customers = GetAll();
            return customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public static int GetRegularOrderCount(string customerName, DateTime startDate, DateTime endDate)
        {
            var customer = GetCustomerByName(customerName);
            if (customer == null) return 0;

            var orders = OrderService.GetAllOrdersByCustomerId(customer.Id);
            return orders.Count(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && o.CreatedAt.DayOfWeek != DayOfWeek.Saturday && o.CreatedAt.DayOfWeek != DayOfWeek.Sunday);
        }
        public static void AddCustomer(Customer customer)
        {
            var customers = GetAll();
            customer.Id = customers.Any() ? customers.Max(c => c.Id) + 1 : 1;
            customer.AddDate = DateTime.Now;
            customers.Add(customer);
            SaveAll(customers);
        }

        public static void UpdateCustomer(Customer customer)
        {
            var customers = GetAll();
            var existing = customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existing != null)
            {
                existing.Name = customer.Name;
                existing.PhoneNumber = customer.PhoneNumber;
                // Update other fields if necessary
            }
            SaveAll(customers);
        }

        public static void DeleteCustomer(int id)
        {
            var customers = GetAll();
            var customer = customers.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                customers.Remove(customer);
            }
            SaveAll(customers);
        }
    }
    
}
