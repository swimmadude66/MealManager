using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Interfaces;

namespace Inventory.Data.Factory
{
    [Serializable]
    public static class DAOFactory
    {
        public static IRecipeDAO GetRecipeDAO()
        {
            return new RecipeDAO();
        }
     
        public static IIngredientDAO GetIngredientDAO()
        {
               return new IngredientDao();
        }

        public static IPantryDAO GetPantryDAO()
        {
            return new PantryDAO();
        }

        public static IMeasureDAO GetMeasureDAO()
        {
            return new MeasureDAO();
        }
    }
}
