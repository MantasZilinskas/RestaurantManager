using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManager.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public  List<int> MenuItems { get; set; }
    }
}
