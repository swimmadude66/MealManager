using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory.Data.Interfaces;
using Inventory.Managers.Factory;
using Inventory.Models;
using System.Collections.Generic;

namespace TestHarness
{
    [TestClass]
    public class TestSearch
    {
        public RecipeSearchCriteriaModel criteria;


        [TestMethod]
        public void TestEmptySearch()
        {
            criteria = new RecipeSearchCriteriaModel();
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            int results = manager.SearchRecipes(-1, 0, criteria).Count;
            Assert.AreEqual(manager.getRecipes(-1, 0, false).Count, results);

        }

        [TestMethod]
        public void TestNameSearch()
        {
            criteria = new RecipeSearchCriteriaModel();
            criteria.Name = "Chicken";
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            List<RecipeModel> models = manager.SearchRecipes(-1, 0, criteria);
            foreach(RecipeModel model in models){
                Assert.IsTrue(model.Name.Contains("Chicken"));
            }
        }

        [TestMethod]
        public void TestTagsSearch()
        {
            criteria = new RecipeSearchCriteriaModel();
            criteria.Tags = new List<String> {"chicken", "sandwich"};
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            List<RecipeModel> models = manager.SearchRecipes(-1, 0, criteria);
            foreach (RecipeModel model in models)
            {
                Assert.IsTrue(model.Tags.Contains("chicken") && model.Tags.Contains("sandwich"));
            }
        }

        [TestMethod]
        public void TestIDSearch()
        {
            criteria = new RecipeSearchCriteriaModel();
            criteria.ID = 7;
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            List<RecipeModel> models = manager.SearchRecipes(-1, 0, criteria);
            Assert.IsTrue(models.Count == 1);
        }

        [TestMethod]
        public void TestOneIngredientSearch()
        {
            criteria = new RecipeSearchCriteriaModel();
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            List<IngredientModel> ingredients = manager.getIngredients();
            IngredientModel ing = new IngredientModel();
            foreach(IngredientModel i in ingredients){
                if(i.Name.ToLower() == "chicken breast"){
                    ing = i;
                    break;
                }
            }
            criteria.Ingredients = new List<IngredientModel> { ing };
            List<RecipeModel> models = manager.SearchRecipes(-1, 0, criteria);
            foreach (RecipeModel model in models)
            {
                List<int> ingredientIDs = new List<int>();
                foreach (RecipeItemModel item in model.Items)
                {
                    ingredientIDs.Add(item.Ingredient.ID);
                }
                Assert.IsTrue(ingredientIDs.Contains(ing.ID));
            }
        }

        [TestMethod]
        public void TestMultiIngredientSearch()
        {
            criteria = new RecipeSearchCriteriaModel();
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            List<IngredientModel> ingredients = manager.getIngredients();
            List<IngredientModel> ing = new List<IngredientModel>();
            foreach(IngredientModel i in ingredients){
                if(i.Name.ToLower().Contains("chicken")){
                    ing.Add(i);
                }
            }
            criteria.Ingredients = ing;
            List<RecipeModel> models = manager.SearchRecipes(-1, 0, criteria);
            foreach (RecipeModel model in models)
            {
                List<int> ingredientIDs = new List<int>();
                foreach (RecipeItemModel item in model.Items)
                {
                    ingredientIDs.Add(item.Ingredient.ID);
                }
                foreach(IngredientModel imodel in ing){
                    Assert.IsTrue(ingredientIDs.Contains(imodel.ID));
                }
            }
        }

        [TestMethod]
        public void TestMixedSearch()
        {
            criteria = new RecipeSearchCriteriaModel();
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            List<IngredientModel> ingredients = manager.getIngredients();
            IngredientModel ing = new IngredientModel();
            foreach (IngredientModel i in ingredients)
            {
                if (i.Name.ToLower() == "chicken breast")
                {
                    ing = i;
                    break;
                }
            }
            criteria.Ingredients = new List<IngredientModel> { ing };
            criteria.Name = "Sandwich";
            List<RecipeModel> models = manager.SearchRecipes(-1, 0, criteria);
            foreach (RecipeModel model in models)
            {
                List<int> ingredientIDs = new List<int>();
                foreach (RecipeItemModel item in model.Items)
                {
                    ingredientIDs.Add(item.Ingredient.ID);
                }
                Assert.IsTrue(ingredientIDs.Contains(ing.ID) && model.Name.Contains("Sandwich"));
            }
        }

    }
}
