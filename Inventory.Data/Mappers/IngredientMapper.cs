using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Inventory.Models;

namespace Inventory.Data
{
    public static class IngredientMapper
    {
        public static List<IngredientModel> BindItems(List<Ingredient> items)
        {
            List<IngredientModel> models = new List<IngredientModel>();

            foreach (Ingredient item in items)
            {
                models.Add(BindItem(item));
            }
            return models;
        }

        public static IngredientModel BindItem(Ingredient item)
        {
            IngredientModel model = new IngredientModel();
            model.ID = item.ID;
            model.Name = item.Name;
            model.Description = item.Description;
            return model;
        }
    }
}
