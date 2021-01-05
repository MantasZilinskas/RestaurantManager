using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Services;

namespace RestaurantManager.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private const string FilePath = @"MenuItems.csv";
        private readonly ICsvFileManager<MenuItem> _csvManager;

        public MenuItemRepository()
        {
            _csvManager = new CsvFileManager<MenuItem>();
        }
        public void Add(MenuItem menuItem)
        {
            if (File.Exists(FilePath))
            {
                var menuItems = _csvManager.ReadFromFile(FilePath);
                menuItem.Id = FindUniqueId(menuItems);
                _csvManager.AppendToFile(FilePath, menuItem);
            }
            else
            {
                menuItem.Id = 1;
                _csvManager.WriteToFile(FilePath, new List<MenuItem>() { menuItem });
            }
        }
        private int FindUniqueId(List<MenuItem> menuItems)
        {
            var sorted = menuItems.OrderBy(value => value.Id).ToList();
            var id = 1;
            var unique = false;
            using var sequenceEnum = sorted.GetEnumerator();

            while (sequenceEnum.MoveNext() && !unique)
            {
                if (sequenceEnum.Current.Id != id)
                {
                    unique = true;
                }
                else
                {
                    id++;
                }
            }

            return id;
        }

        public List<MenuItem> GetAll()
        {
            var list = new List<MenuItem>();
            if (File.Exists(FilePath))
            {
                list = _csvManager.ReadFromFile(FilePath);
            }

            return list;
        }

        public void Remove(int id)
        {
            var menuItems = _csvManager.ReadFromFile(FilePath);
            var existing = menuItems.FirstOrDefault(value => value.Id == id);
            if (existing == null)
            {
                Console.WriteLine($"ERROR:Menu item with id {id} does not exist");
            }
            else
            {
                menuItems.Remove(existing);
                _csvManager.WriteToFile(FilePath, menuItems);
            }
        }

        public void Update(int id, MenuItem menuItem)
        {
            var menuItems = _csvManager.ReadFromFile(FilePath);
            var existing = menuItems.FirstOrDefault(value => value.Id == id);
            if (existing == null)
            {
                Console.WriteLine($"ERROR:Menu item with id {id} does not exist");
            }
            else
            {
                menuItems.Remove(existing);
                menuItem.Id = id;
                menuItems.Add(menuItem);
                _csvManager.WriteToFile(FilePath, menuItems);
            }
        }

        public List<int> MenuItemsExist(List<int> menuItemIdList)
        {
            var menuItems = _csvManager.ReadFromFile(FilePath);
            List<int> nonExistentIds = new List<int>();
            foreach (var id in menuItemIdList)
            {
                var contains = menuItems.Any(value => value.Id == id);
                if (!contains)
                {
                    nonExistentIds.Add(id);
                }
            }
            return nonExistentIds;
        }

        public List<int> GetMenuItemProducts(int menuItemId)
        {
            var menuItems = _csvManager.ReadFromFile(FilePath);
            var menuItem = menuItems.FirstOrDefault(value => value.Id == menuItemId);
            return menuItem.Products;
        }
    }
}
