﻿using System;
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
        private IPlannerDAO _plannerDAO;

        public RecipeManager (IRecipeDAO recipeDAO, IIngredientDAO ingredientDAO, IMeasureDAO measureDAO, IPlannerDAO plannerDAO)
        {
            _recipeDAO = recipeDAO;
            _ingredientDAO = ingredientDAO;
            _measureDAO = measureDAO;
            _plannerDAO = plannerDAO;
        }

        public List<RecipeModel> getRecipes(int Limit, int start, bool have)
        {
            return _recipeDAO.getRecipes(Limit, start, have);
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

        public IngredientModel getIngredient(int ID)
        {
            return _ingredientDAO.GetIngredient(ID);
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

        public int SaveRecipe(RecipeModel recipeItem, bool isEdit)
        {
            return _recipeDAO.SaveRecipe(recipeItem, isEdit);
        }

        public int SaveRecipeItem(int recipeid, TempRecipeItemModel model)
        {
            return _recipeDAO.SaveRecipeItem(recipeid, model);
        }

        public int PlanRecipe(PlannerItemModel model, bool isEdit)
        {
            return _plannerDAO.PlanRecipe(model, isEdit);
        }

        public List<PlannerItemModel> GetPlannedRecipes(DateTime? start, DateTime? end)
        {
            return _plannerDAO.GetPlannedRecipes(start, end);
        }

        public void cancelPlan(int id)
        {
            _plannerDAO.cancelPlan(id);
        }

        public List<RecipeModel> SearchRecipes(int Limit, int start, RecipeSearchCriteriaModel criteria)
        {
            String critstring = criteria.ToString();
            if (criteria.ToString() == "")
                return getRecipes(Limit, start, criteria.have);
            return _recipeDAO.SearchRecipes(Limit, start, criteria);
            
        }

        public RecipeModel getRecipeItems(int rid)
        {
            return _recipeDAO.getRecipeItems(rid);
        }
       
    }
}
