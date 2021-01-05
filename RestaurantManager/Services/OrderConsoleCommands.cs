using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Repositories;

namespace RestaurantManager.Services
{
    public class OrderConsoleCommands : IOrderConsoleCommands
    {
        private readonly IMenuItemRepository _menuItemRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public OrderConsoleCommands()
        {
            _menuItemRepo = new MenuItemRepository();
            _orderRepo = new OrderRepository();
            _productRepo = new ProductRepository();
        }
        public void CreateOrder()
        {
            Console.Write("Provide menu item ids that should be added to the order: ");
            var menuItemsLine = Console.ReadLine();
            var values = menuItemsLine.Split(' ');
            if (ValidateMenuItemsId(values))
            {
                var menuItemIdList = values.Select(int.Parse).ToList();
                var nonExistentIds = _menuItemRepo.MenuItemsExist(menuItemIdList);
                if (nonExistentIds.Count == 0)
                {
                    var orderProducts = new List<int>();
                    foreach (var menuItemId in menuItemIdList)
                    {
                        var products = _menuItemRepo.GetMenuItemProducts(menuItemId);
                        orderProducts.AddRange(products);
                    }

                    var emptyProducts = _productRepo.DeductProducts(orderProducts);
                    if (emptyProducts.Count == 0)
                    {
                        var order = new Order()
                        {
                            MenuItems = menuItemIdList,

                        };
                        _orderRepo.Add(order);
                    }
                    else
                    {
                        Console.Write("ERROR:Order does not have enough products. Missing products: ");
                        foreach (var id in emptyProducts)
                        {
                            Console.Write($"{id} ");
                        }
                        Console.Write("\n");
                        Console.WriteLine("Order has been declined!");
                    }

                }
                else
                {
                    Console.Write("ERROR:Menu items ");
                    foreach (var id in nonExistentIds)
                    {
                        Console.Write($"{id} ");
                    }
                    Console.Write("does not exist\n");
                }
            }
            else
            {
                Console.WriteLine("ERROR:Menu item ids must be positive integers and separated by space");
            }
        }
        public void DisplayAllOrders()
        {
            var orders = _orderRepo.GetAll();
            var temp = new Order();
            foreach (var prop in temp.GetType().GetProperties())
            {

                Console.Write($"{prop.Name}|");
            }
            Console.WriteLine();
            foreach (var order in orders)
            {
                Console.Write($"{order.Id},{order.DateTime},");
                foreach (var menuItemId in order.MenuItems)
                {
                    Console.Write($"{menuItemId} ");
                }
                Console.WriteLine();
            }
        }
        private bool ValidateMenuItemsId(string[] menuItemIdList)
        {
            var isValid = true;
            foreach (var value in menuItemIdList)
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
