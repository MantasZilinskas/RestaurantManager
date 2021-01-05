using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Services;

namespace RestaurantManager.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private const string FilePath = @"Orders.csv";
        private readonly ICsvFileManager<Order> _csvManager;
        private readonly IFileWrapper _fileWrapper;
        public OrderRepository(ICsvFileManager<Order> csvManager, IFileWrapper fileWrapper)
        {
            _csvManager = csvManager;
            _fileWrapper = fileWrapper;
        }
        public void Add(Order order)
        {
            if (_fileWrapper.Exists(FilePath))
            {
                var orders = _csvManager.ReadFromFile(FilePath);
                order.Id = FindUniqueId(orders);
                order.DateTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");
                _csvManager.AppendToFile(FilePath, order);
            }
            else
            {
                order.Id = 1;
                order.DateTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");
                _csvManager.WriteToFile(FilePath, new List<Order>() { order });
            }
        }
        private int FindUniqueId(List<Order> orders)
        {
            var sorted = orders.OrderBy(value => value.Id).ToList();
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

        public List<Order> GetAll()
        {
            var list = new List<Order>();
            if (_fileWrapper.Exists(FilePath))
            {
                list = _csvManager.ReadFromFile(FilePath);
            }

            return list;
        }
    }
}
