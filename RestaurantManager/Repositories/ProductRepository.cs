﻿using System;
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
        private readonly ICsvFileManager<Product> _csvManager;
        private readonly IFileWrapper _fileWrapper;
        public ProductRepository(ICsvFileManager<Product> csvManager, IFileWrapper fileWrapper)
        {
            _csvManager = csvManager;
            _fileWrapper = fileWrapper;
        }
        public void Add(Product product)
        {
            if (_fileWrapper.Exists(FilePath))
            {
                var products = _csvManager.ReadFromFile(FilePath);
                product.Id = FindUniqueId(products);
                _csvManager.AppendToFile(FilePath, product);
            }
            else
            {
                product.Id = 1;
                _csvManager.WriteToFile(FilePath, new List<Product>() { product });
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
            if (_fileWrapper.Exists(FilePath))
            {
                list = _csvManager.ReadFromFile(FilePath);
            }

            return list;
        }

        public void Remove(int id)
        {
            var products = _csvManager.ReadFromFile(FilePath);
            var existing = products.FirstOrDefault(value => value.Id == id);
            if (existing == null)
            {
                Console.WriteLine($"ERROR:Product with id {id} does not exist");
            }
            else
            {
                products.Remove(existing);
                _csvManager.WriteToFile(FilePath, products);
            }
        }

        public void Update(int id, Product product)
        {
            var products = _csvManager.ReadFromFile(FilePath);
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
                _csvManager.WriteToFile(FilePath, products);
            }
        }

        public List<int> ProductsExist(List<int> productIdList)
        {
            var products = _csvManager.ReadFromFile(FilePath);
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

        public List<int> DeductProducts(List<int> productIdList)
        {
            var products = _csvManager.ReadFromFile(FilePath);
            var emptyProducts = CheckAreProductsAvailable(productIdList, products);
            if (emptyProducts.Count == 0)
            {
                foreach (var productId in productIdList)
                {
                    var product = products.FirstOrDefault(value => value.Id == productId);
                    product.PortionCount--;
                }
                _csvManager.WriteToFile(FilePath,products);
            }
            return emptyProducts;
        }

        private List<int> CheckAreProductsAvailable(List<int> productIdList, List<Product> products)
        {
            var emptyProducts = new List<int>();
            foreach (var productId in productIdList)
            {
                var product = products.FirstOrDefault(value => value.Id == productId);
                if (product.PortionCount - 1 < 0)
                {
                    emptyProducts.Add(product.Id);
                }
            }
            return emptyProducts;
        }
    }
}
