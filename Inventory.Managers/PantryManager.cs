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

        public void SavePantryItem(int ingredient, double quantity, int measure, string description,DateTime? expires)
        {
            _pantryDao.SavePantryItem(ingredient, quantity, measure, description, expires);
        }
    }
}
