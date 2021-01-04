using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManager.Model
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Products { get; set; }
    }
}
