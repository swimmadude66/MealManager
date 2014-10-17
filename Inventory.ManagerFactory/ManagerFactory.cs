using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Interfaces;
using Inventory.Data.Factory;
using Inventory.Managers;

namespace Inventory.Managers.Factory
{
    [Serializable]
    public static class ManagerFactory
    {
        public static RecipeManager _recipeManager;

        public static RecipeManager GetRecipeManager()
        {
            IRecipeDAO _recipeDAO = DAOFactory.GetRecipeDAO();
            IIngredientDAO _ingredientDAO = DAOFactory.GetIngredientDAO();
            IMeasureDAO _measureDAO = DAOFactory.GetMeasureDAO();
            IPlannerDAO _plannerDAO = DAOFactory.GetPlannerDAO();
            return new RecipeManager(_recipeDAO, _ingredientDAO, _measureDAO, _plannerDAO);
        }
        public static PantryManager GetPantryManager()
        {
            IIngredientDAO _ingredientDAO = DAOFactory.GetIngredientDAO();
            IPantryDAO _pantryDAO = DAOFactory.GetPantryDAO();
            return new PantryManager(_ingredientDAO, _pantryDAO);
        }

    }
}
