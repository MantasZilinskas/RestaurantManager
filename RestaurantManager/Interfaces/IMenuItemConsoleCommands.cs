using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.Interfaces
{
    public interface IMenuItemConsoleCommands
    {
        public void AddMenuItem();
        public void RemoveMenuItem();
        public void DisplayAllMenuItems();
        public void UpdateMenuItem();
    }
}
