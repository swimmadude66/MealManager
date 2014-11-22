using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Interfaces
{
    public interface IRecipeDAO
    {
        List<RecipeModel> getRecipes(int Limit, int start, bool have);
        List<String> getAllTags();
        void SaveTag(String tag);
        int SaveRecipe(RecipeModel recipeItem, bool isEdit);
        RecipeModel getRecipeItems(int rid);
        int SaveRecipeItem(int recipeid, TempRecipeItemModel model);
        List<RecipeModel> SearchRecipes(int Limit, int start, RecipeSearchCriteriaModel criteria);
    }
}
