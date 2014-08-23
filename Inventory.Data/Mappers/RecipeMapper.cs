using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Mappers
{
    public static class RecipeMapper
    {
        public static RecipeModel BindItem(Recipe r)
        {
            RecipeModel model = new RecipeModel();
            model.Calories = r.Calories;
            model.Description = r.Description;
            model.ID = r.ID;
            model.ImagePath = r.ImagePath;
            model.Name = r.Name;
            model.Rating = r.Rating;
            model.Directions = r.Directions;
            model.Tags = r.TagString;
            //foreach(RecipeItem i in r.RecipeItem){
            //    RecipeItemModel rmodel = new RecipeItemModel();
            //    rmodel.Description = i.Description;
            //    rmodel.ID = i.ID;
            //    rmodel.Ingredient = IngredientMapper.BindItem(i.Ingredient);
            //    rmodel.Measure = i.Measure.Measurement;
            //    rmodel.Quantity = i.Quantity;
            //    rmodel.RecipeID = i.RecipeID;
            //    model.Ingredients.Add(rmodel);
            //}
            return model;
        }

        public static List<RecipeModel> BindItems(List<Recipe> rs)
        {
            List<RecipeModel> recipes = new List<RecipeModel>();
            foreach (Recipe r in rs)
            {
                recipes.Add(BindItem(r));
            }
            return recipes;
        }
    }
}
