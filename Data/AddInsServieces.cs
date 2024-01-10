using BCafe.Components.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BCafe.Data
{
    public static class AddInsService
    {
        private static string addinsFilePath = Path.Combine(Utils.GetAppDirectoryPath(), "addins.json");

        public static List<AddIn> GetAll()
        {
            if (!File.Exists(addinsFilePath))
            {
                return new List<AddIn>();
            }

            var json = File.ReadAllText(addinsFilePath);
            return JsonSerializer.Deserialize<List<AddIn>>(json) ?? new List<AddIn>();
        }

        private static void SaveAll(List<AddIn> addins)
        {
            var json = JsonSerializer.Serialize(addins);
            File.WriteAllText(addinsFilePath, json);
        }

        public static void AddAddin(AddIn addin)
        {
            var addins = GetAll();
            addin.Id = addins.Any() ? addins.Max(a => a.Id) + 1 : 1;
            addins.Add(addin);
            SaveAll(addins);
        }

        public static void UpdateAddin(AddIn addin)
        {
            var addins = GetAll();
            var existing = addins.FirstOrDefault(a => a.Id == addin.Id);
            if (existing != null)
            {
                existing.Name = addin.Name;
                existing.Price = addin.Price;
            }
            SaveAll(addins);
        }

        public static void DeleteAddin(int id)
        {
            var addins = GetAll();
            var addin = addins.FirstOrDefault(a => a.Id == id);
            if (addin != null)
            {
                addins.Remove(addin);
            }
            SaveAll(addins);
        }
    }
}
