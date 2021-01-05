using System;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;
using RestaurantManager.CsvHelperConfig;

namespace RestaurantManager.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string DateTime { get; set; }
        [TypeConverter(typeof(ToIntArrayConverter))]
        public  List<int> MenuItems { get; set; }
    }
}
