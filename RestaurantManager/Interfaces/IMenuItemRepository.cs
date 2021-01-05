using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Model;

namespace RestaurantManager.Interfaces
{
    public interface IMenuItemRepository
    {
        public void Add(MenuItem menuItem);
        public void Update(int id, MenuItem menuItem);
        public void Remove(int id);
        public List<MenuItem> GetAll();
    }
}
