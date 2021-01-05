using System;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Repositories;
using RestaurantManager.Services;

namespace RestaurantManager
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IProductConsoleCommands productCommands = new ProductConsoleCommands();
            IMenuItemConsoleCommands menuItemCommands = new MenuItemConsoleCommands();
            IOrderConsoleCommands orderCommands = new OrderConsoleCommands();
            var programRunning = true;
            var line = new string('-', 40);
            Console.WriteLine("Restaurant manager");
            Console.WriteLine(line);
            Console.WriteLine("Type 'help' to see available commands");
            Console.WriteLine(line);
            while (programRunning)
            {
                Console.Write(">> ");
                var command = Console.ReadLine();
                Console.WriteLine();
                switch (command)
                {
                    case "exit":
                        programRunning = false;
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "help":
                        DisplayAvailableCommands();
                        break;
                    case "AddProduct":
                        productCommands.AddProduct();
                        break;
                    case "DisplayAllProducts":
                        productCommands.DisplayAllProducts();
                        break;
                    case "RemoveProduct":
                        productCommands.RemoveProduct();
                        break;
                    case "UpdateProduct":
                        productCommands.UpdateProduct();
                        break;
                    case "AddMenuItem":
                        menuItemCommands.AddMenuItem();
                        break;
                    case "DisplayAllMenuItems":
                        menuItemCommands.DisplayAllMenuItems();
                        break;
                    case "RemoveMenuItem":
                        menuItemCommands.RemoveMenuItem();
                        break;
                    case "UpdateMenuItem":
                        menuItemCommands.UpdateMenuItem();
                        break;
                    case "CreateOrder":
                        orderCommands.CreateOrder();
                        break;
                    case "DisplayAllOrders":
                        orderCommands.DisplayAllOrders();
                        break;
                    default:
                        Console.WriteLine("There is no such command");
                        break;
                }
            }
            Console.WriteLine("Restaurant manager is closed");
        }
        private static void DisplayAvailableCommands()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("exit -> Closes restaurant manager");
            Console.WriteLine("help -> Displays all available commands");
            Console.WriteLine("clear -> Clears Console window");
            Console.WriteLine("AddProduct -> Creates new product");
            Console.WriteLine("RemoveProduct -> Removes product");
            Console.WriteLine("UpdateProduct -> Updates product information");
            Console.WriteLine("AddMenuItem -> Creates new menu item");
            Console.WriteLine("RemoveMenuItem -> Removes menu item");
            Console.WriteLine("UpdateMenuItem -> Updates menu item information");
            Console.WriteLine("CreateOrder -> Creates new order");
            Console.WriteLine("DisplayAllOrders -> Displays order information");
            Console.WriteLine("DisplayAllMenuItems -> Displays menu item information");
            Console.WriteLine("DisplayAllProducts -> Displays product information");
        }

    }
    
}
