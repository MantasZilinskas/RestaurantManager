using System;
using System.Collections.Generic;
using System.Text;
using RestaurantManager.Model;

namespace RestaurantManager.Interfaces
{
    public interface IProductRepository
    {
        public void Add(Product product);
        public void Update(int id, Product product);
        public void Remove(int id);
        public List<Product> GetAll();
        public List<int> ProductsExist(List<int> productIdList);
    }
}
