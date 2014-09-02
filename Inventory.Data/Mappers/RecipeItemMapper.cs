using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Mappers
{
    class RecipeItemMapper
    {
        public static List<RecipeItemModel> BindItems(List<RecipeItem> items)
        {
            List<RecipeItemModel> models = new List<RecipeItemModel>();

            foreach(RecipeItem item in items)
            {
                models.Add(BindItem(item));
            }
            return models;
        }

        public static RecipeItemModel BindItem(RecipeItem item)
        {
            RecipeItemModel model = new RecipeItemModel();
            model.ID = item.ID;
            model.Description  = item.Description;
            model.Quantity = item.Quantity;
            model.RecipeID = item.RecipeID;
            model.Ingredient = IngredientMapper.BindItem(item.Ingredient);
            model.Measure = MeasureMapper.BindItem(item.Measure);
            return model;
        }

    }
}
