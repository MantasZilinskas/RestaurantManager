using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Model;

namespace RestaurantManager.Interfaces
{
    public interface IOrderRepository
    {
        public void Add(Order order);
        public List<Order> GetAll();
    }
}
