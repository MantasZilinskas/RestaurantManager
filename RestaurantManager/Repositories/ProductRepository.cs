using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Services;

namespace RestaurantManager.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private const string FilePath = @"Products.csv";
        private readonly ICsvFileManager<Product> csvManager;

        public ProductRepository()
        {
            csvManager = new CsvFileManager<Product>();
        }
        public void Add(Product product)
        {
            if (File.Exists(FilePath))
            {
                var products = csvManager.ReadFromFile(FilePath);
                product.Id = FindUniqueId(products);
                csvManager.AppendToFile(FilePath, product);
            }
            else
            {
                product.Id = 1;
                csvManager.WriteToFile(FilePath, new List<Product>() { product });
            }
        }

        private int FindUniqueId(List<Product> products)
        {
            var sorted = products.OrderBy(value => value.Id).ToList();
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

        public List<Product> GetAll()
        {
            var list = new List<Product>();
            if (File.Exists(FilePath))
            {
                list = csvManager.ReadFromFile(FilePath);
            }

            return list;
        }

        public void Remove(int id)
        {
            var products = csvManager.ReadFromFile(FilePath);
            var existing = products.FirstOrDefault(value => value.Id == id);
            if (existing == null)
            {
                Console.WriteLine($"ERROR:Product with id {id} does not exist");
            }
            else
            {
                products.Remove(existing);
                csvManager.WriteToFile(FilePath, products);
            }
        }

        public void Update(int id, Product product)
        {
            var products = csvManager.ReadFromFile(FilePath);
            var existing = products.FirstOrDefault(value => value.Id == id);
            if (existing == null)
            {
                Console.WriteLine($"ERROR:Product with id {id} does not exist");
            }
            else
            {
                products.Remove(existing);
                product.Id = id;
                products.Add(product);
                csvManager.WriteToFile(FilePath, products);
            }
        }

        public List<int> ProductsExist(List<int> productIdList)
        {
            var products = csvManager.ReadFromFile(FilePath);
            List<int> nonExistentIds = new List<int>();
            foreach (var id in productIdList)
            {
                var contains = products.Any(value => value.Id == id);
                if (!contains)
                {
                    nonExistentIds.Add(id);
                }
            }
            return nonExistentIds;
        }
    }
}
