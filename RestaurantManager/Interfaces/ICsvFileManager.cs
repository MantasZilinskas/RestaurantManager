using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManager.Interfaces
{
    public interface ICsvFileManager<T>
    {
        public List<T> ReadFromFile(string filePath);
        public void WriteToFile(string filePath, List<T> list);
        public void AppendToFile(string filePath, T value);
    }
}
