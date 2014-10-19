using Inventory.Data.Interfaces;
using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Managers
{
    public class PantryManager : IPantryManager
    {
        private IIngredientDAO _ingredientDao;
        private IPantryDAO _pantryDao;

        public PantryManager(IIngredientDAO ingredientDao, IPantryDAO pantryDao)
        {
            _ingredientDao = ingredientDao;
            _pantryDao = pantryDao;
        }

        public List<PantryItemModel> GetPantryContents()
        {
            return _pantryDao.GetPantryContents();
        }

        public PantryItemModel GetPantryItemById(int ID)
        {
            return _pantryDao.GetPantryItemById(ID);
        }

        public void SavePantryItem(PantryItemModel pantryItem, bool isEdit)
        {
            _pantryDao.SavePantryItem(pantryItem, isEdit);
        }
    }
}
