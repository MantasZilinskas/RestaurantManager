using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Repositories;

namespace RestaurantManager.Services
{
    public class MenuItemConsoleCommands : IMenuItemConsoleCommands
    {
        private readonly IMenuItemRepository _menuItemRepo;
        private readonly IProductRepository _productRepo;
        public MenuItemConsoleCommands()
        {
            _menuItemRepo = new MenuItemRepository();
            _productRepo = new ProductRepository();
        }
        public void AddMenuItem()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();
            Console.Write("Product ids: ");
            var productsLine = Console.ReadLine();
            var values = productsLine.Split(' ');
            if (ValidateProductsId(values))
            {

                var productIdList = values.Select(int.Parse).ToList();
                var nonExistentIds = _productRepo.ProductsExist(productIdList);
                if (nonExistentIds.Count == 0)
                {
                    var menuItem = new MenuItem
                    {
                        Name = name,
                        Products = productIdList,

                    };
                    _menuItemRepo.Add(menuItem);
                }
                else
                {
                    Console.Write("ERROR:Products ");
                    foreach (var id in nonExistentIds)
                    {
                        Console.Write($"{id} ");
                    }
                    Console.Write("does not exist\n");
                }

            }
            else
            {
                Console.WriteLine("ERROR:Product ids must be positive integers and separated by space");
            }
        }

        public void DisplayAllMenuItems()
        {
            var menuItems = _menuItemRepo.GetAll();
            var temp = new MenuItem();
            foreach (var prop in temp.GetType().GetProperties())
            {

                Console.Write($"{prop.Name}|");
            }
            Console.WriteLine();
            foreach (var menuItem in menuItems)
            {
                Console.Write($"{menuItem.Id},{menuItem.Name},");
                foreach (var productId in menuItem.Products)
                {
                    Console.Write($"{productId} ");
                }
                Console.WriteLine();
            }
        }

        public void RemoveMenuItem()
        {
            Console.Write("Specify the id of menu item you want to remove: ");
            var inputId = Console.ReadLine();
            Console.WriteLine();
            var isValidId = int.TryParse(inputId, out var id);
            if (isValidId && id > 0)
            {
                _menuItemRepo.Remove(id);
            }
            else
            {
                Console.WriteLine("ERROR:Id must be a positive integer");
            }
        }

        public void UpdateMenuItem()
        {
            Console.Write("Specify the id of menu item you want to update: ");
            var inputId = Console.ReadLine();
            Console.WriteLine();
            var isValidId = int.TryParse(inputId, out var id);
            if (isValidId && id > 0)
            {
                Console.Write("Name: ");
                var name = Console.ReadLine();
                Console.Write("Product ids: ");
                var productsLine = Console.ReadLine();
                var values = productsLine.Split(' ');
                if (ValidateProductsId(values))
                {

                    var productIdList = values.Select(int.Parse).ToList();
                    var nonExistentIds = _productRepo.ProductsExist(productIdList);
                    if (nonExistentIds.Count == 0)
                    {
                        var menuItem = new MenuItem
                        {
                            Name = name,
                            Products = productIdList,

                        };
                        _menuItemRepo.Update(id,menuItem);
                    }
                    else
                    {
                        Console.Write("ERROR:Products ");
                        foreach (var productId in nonExistentIds)
                        {
                            Console.Write($"{productId} ");
                        }
                        Console.Write("does not exist\n");
                    }

                }
                else
                {
                    Console.WriteLine("ERROR:Product ids must be positive integers and separated by space");
                }
            }
            else
            {
                Console.WriteLine("ERROR:Id must be a positive integer");
            }
        }

        private bool ValidateProductsId(string[] productIdList)
        {
            var isValid = true;
            foreach (var value in productIdList)
            {
                var isValidId = int.TryParse(value, out var id);
                if (!isValidId && id <= 0)
                {
                    isValid = false;
                }
            }
            return isValid;
        }
    }
}
