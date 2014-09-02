using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Data.Interfaces;
using Inventory.Data.Factory;

using Inventory.Models;

namespace Inventory.Managers
{
    public class RecipeManager : IRecipeManager
    {
        private IRecipeDAO _recipeDAO;
        private IIngredientDAO _ingredientDAO;
        private IMeasureDAO _measureDAO;

        public RecipeManager (IRecipeDAO recipeDAO, IIngredientDAO ingredientDAO, IMeasureDAO measureDAO)
        {
            _recipeDAO = recipeDAO;
            _ingredientDAO = ingredientDAO;
            _measureDAO = measureDAO;
        }

        public List<RecipeModel> getRecipes(){
            List<RecipeModel> rawList = _recipeDAO.getRecipes();
            foreach (RecipeModel rec in rawList)
            {
                getRecipeData(rec);
            }
            return rawList;
        }

        private void getRecipeData(RecipeModel rec)
        {
            rec.Items = _recipeDAO.getRecipeItems(rec.ID);
            rec.IngredientIDs = new List<int>();
            foreach (RecipeItemModel item in rec.Items)
            {
                rec.IngredientIDs.Add(item.Ingredient.ID);
            }
        }
        
        public int SaveIngredient(string name, string description)
        {
            return _ingredientDAO.SaveIngredient(name, description);
        }

        public int SaveMeasure(string name)
        {
            return _measureDAO.SaveMeasure(name);
        }

        public int getMeasureID(string name)
        {
            return _measureDAO.getMeasureID(name);
        }

        public List<MeasureModel> getMeasures()
        {
            return _measureDAO.getMeasures();
        }

        public int getIngredientID(string name)
        {
            return _ingredientDAO.getIngredientID(name);
        }

        public List<IngredientModel> getIngredients()
        {
            return _ingredientDAO.getIngredients();
        }

        public List<String> getAllTags()
        {
            return _recipeDAO.getAllTags();
        }

        public void SaveTag(String tag)
        {
            _recipeDAO.SaveTag(tag);
        }

        public int SaveRecipe(string name, string description, string directions, string tagstring)
        {
            return _recipeDAO.SaveRecipe(name, description, directions, tagstring);
        }

        public int SaveRecipeItem(int recipeid, TempRecipeItemModel model)
        {
            return _recipeDAO.SaveRecipeItem(recipeid, model);
        }

        public List<RecipeModel> SearchRecipes(RecipeSearchCriteriaModel criteria){
            List<RecipeModel> recs = _recipeDAO.SearchRecipes(criteria);
            if (criteria.Ingredients != null && criteria.Ingredients.Count > 0)
            {
                return FilterRecipes(recs, criteria);
            }
            else
            {
                return recs;
            }
        }

        public List<RecipeModel> FilterRecipes(List<RecipeModel> recipes, RecipeSearchCriteriaModel criteria)
        {
            List<RecipeModel> filteredList = new List<RecipeModel>();
            foreach (RecipeModel rec in recipes)
            {
                bool contains = true;
                getRecipeData(rec);
                foreach(IngredientModel ing in criteria.Ingredients){
                    if (!rec.IngredientIDs.Contains(ing.ID))
                    {
                        contains = false;
                        break;
                    }
                }
                if (contains)
                {
                    filteredList.Add(rec);
                }
            }
            return filteredList;
        }
       
    }
}
