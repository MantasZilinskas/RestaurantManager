using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Interfaces
{
    public interface IProductConsoleCommands
    {
        public void AddProduct();
        public void RemoveProduct();
        public void DisplayAllProducts();
        public void UpdateProduct();
    }
}
