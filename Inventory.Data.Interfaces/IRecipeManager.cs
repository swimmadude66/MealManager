using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Interfaces
{
    public interface IRecipeManager
    {
        List<RecipeModel> getRecipes();
        int SaveIngredient(string name, string description);
        int SaveMeasure(string name);
        int getMeasureID(string name);
        List<MeasureModel> getMeasures();
        int getIngredientID(String name);
        IngredientModel GetIngredient(int id);
        List<IngredientModel> getIngredients();
        List<String> getAllTags();
        void SaveTag(String tag);
        int SaveRecipe(string name, string description, string directions, string tagstring);
        int SaveRecipeItem(int recipeid, TempRecipeItemModel model);
        List<RecipeModel> SearchRecipes(RecipeSearchCriteriaModel criteria);
        int PlanRecipe(PlannerItemModel model, bool isEdit);
        void cancelPlan(int id);
        List<PlannerItemModel> GetPlannedRecipes(DateTime? start, DateTime? end);
    }
}
