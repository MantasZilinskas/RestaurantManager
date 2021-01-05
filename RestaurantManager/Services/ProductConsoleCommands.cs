using System;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Repositories;

namespace RestaurantManager.Services
{
    public class ProductConsoleCommands : IProductConsoleCommands
    {
        private readonly IProductRepository _productRepo;
        public ProductConsoleCommands()
        {
            _productRepo = new ProductRepository(new CsvFileManager<Product>(), new FileWrapper());
        }
        public void AddProduct()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();
            Console.Write("Portion count: ");
            var portionCount = Console.ReadLine();
            Console.Write("Unit: ");
            var unit = Console.ReadLine();
            Console.Write("Portion size: ");
            var portionSize = Console.ReadLine();
            if (ValidateProductInput(name, portionCount, unit, portionSize))
            {
                var product = new Product
                {
                    Name = name,
                    PortionCount = int.Parse(portionCount),
                    PortionSize = double.Parse(portionSize),
                    Unit = unit,
                };
                _productRepo.Add(product);
                Console.WriteLine("The product has been added successfully!");
            }
            else
            {
                Console.WriteLine("The product has not been added!");
            }
        }

        public void DisplayAllProducts()
        {
            var products = _productRepo.GetAll();
            var temp = new Product();
            foreach (var prop in temp.GetType().GetProperties())
            {

                Console.Write($"{prop.Name}|");
            }
            Console.WriteLine();
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id},{product.Name},{product.PortionCount},{product.Unit},{product.PortionSize}");
            }
        }

        public void RemoveProduct()
        {
            Console.Write("Specify the id of product you want to remove: ");
            var inputId = Console.ReadLine();
            Console.WriteLine();
            var isValidId = int.TryParse(inputId, out var id);
            if (isValidId && id > 0)
            {
                _productRepo.Remove(id);
            }
            else
            {
                Console.WriteLine("ERROR:Id must be a positive integer");
            }
        }

        public void UpdateProduct()
        {
            Console.Write("Specify the id of product you want to update: ");
            var inputId = Console.ReadLine();
            Console.WriteLine();
            var isValidId = int.TryParse(inputId, out var id);
            if (isValidId && id > 0)
            {
                Console.Write("Name: ");
                var name = Console.ReadLine();
                Console.Write("Portion count: ");
                var portionCount = Console.ReadLine();
                Console.Write("Unit: ");
                var unit = Console.ReadLine();
                Console.Write("Portion size: ");
                var portionSize = Console.ReadLine();
                if (ValidateProductInput(name, portionCount, unit, portionSize))
                {
                    var product = new Product
                    {
                        Name = name,
                        PortionCount = int.Parse(portionCount),
                        PortionSize = double.Parse(portionSize),
                        Unit = unit,
                    };
                    _productRepo.Update(id,product);
                }
                else
                {
                    Console.WriteLine("The product has not been updated!");
                }
            }
            else
            {
                Console.WriteLine("ERROR:Id must be a positive integer");
            }
        }

        private bool ValidateProductInput(string name, string portionCount, string unit, string portionSize)
        {
            var isInt = int.TryParse(portionCount, out var count);
            var isDouble = double.TryParse(portionSize, out var size);
            var isPositiveCount = count >= 0;
            var isPositiveSize = size >= 0;
            if (!isInt)
            {
                Console.WriteLine("ERROR:Portion count must be an integer");
            }
            if (!isDouble)
            {
                Console.WriteLine("ERROR:Portion size must be a real number");
            }
            if (!isPositiveCount)
            {
                Console.WriteLine("ERROR:Portion count must be positive");
            }
            if (!isPositiveSize)
            {
                Console.WriteLine("ERROR:Portion size must be positive");
            }

            return isInt && isDouble && isPositiveCount && isPositiveSize;
        }
    }
}
