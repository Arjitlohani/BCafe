
using System.Text.Json;

namespace BCafe.Data
{
    public static class CoffeeService
    {
        private static string coffeeFilePath = Path.Combine(Utils.GetAppDirectoryPath(), "coffees.json");

        public static List<Coffee> GetAll()
        {
            if (!File.Exists(coffeeFilePath))
            {
                return new List<Coffee>();
            }

            var json = File.ReadAllText(coffeeFilePath);
            return JsonSerializer.Deserialize<List<Coffee>>(json) ?? [];
        }

        private static void SaveAll(List<Coffee> coffees)
        {
            var json = JsonSerializer.Serialize(coffees);
            File.WriteAllText(coffeeFilePath, json);
        }

        public static void AddCoffee(Coffee coffee)
        {
            var coffees = GetAll();
            coffee.Id = coffees.Any() ? coffees.Max(c => c.Id) + 1 : 1;
            coffees.Add(coffee);
            SaveAll(coffees);
        }

        public static void UpdateCoffee(Coffee coffee)
        {
            var coffees = GetAll();
            var existing = coffees.FirstOrDefault(c => c.Id == coffee.Id);
            if (existing != null)
            {
                existing.ProductName = coffee.ProductName;
                existing.Price = coffee.Price;
            }
            SaveAll(coffees);
        }

        public static void DeleteCoffee(int id)
        {
            var coffees = GetAll();
            var coffee = coffees.FirstOrDefault(c => c.Id == id);
            if (coffee != null)
            {
                coffees.Remove(coffee);
            }
            SaveAll(coffees);
        }
    }
}
