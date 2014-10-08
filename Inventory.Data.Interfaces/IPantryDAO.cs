using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Interfaces
{
    public interface IPantryDAO
    {
        List<PantryItemModel> GetPantryContents();
        PantryItemModel GetPantryItemById(int ID);
        void SavePantryItem(PantryItemModel pantryItem, bool isEdit);
    }
}
