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
            var programRunning = true;
            var line = new string('-', 25);
            Console.WriteLine("Restaurant manager");
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
                    default:
                        Console.WriteLine("There is no such command");
                        break;
                }
            }
            Console.WriteLine("Restaurant manager is closed");
        }

    }
}
