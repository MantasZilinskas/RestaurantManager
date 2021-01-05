using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.Interfaces;

namespace RestaurantManager.Services
{
    public class FileWrapper : IFileWrapper
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
