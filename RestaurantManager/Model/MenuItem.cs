using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;
using RestaurantManager.CsvTypeConverter;

namespace RestaurantManager.Model
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [TypeConverter(typeof(ToIntArrayConverter))]
        public List<int> Products { get; set; }
    }
}
