using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;

namespace RestaurantManager.Services
{
    public class CsvFileManager<T>: ICsvFileManager<T>
    {
        public List<T> ReadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var values = csv.GetRecords<T>();
                    return values.ToList();
                }
            }
            else
            {
                return new List<T>();
            }
        }

        public void WriteToFile(string filePath, List<T> list)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }
        }

        public void AppendToFile(string filePath, T value)
        {
            using (var stream = File.Open(filePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = false;
                csv.WriteRecord(value);
                csv.NextRecord();
            }
        }
    }
}
