using Inventory.Data.Interfaces;
using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Mappers
{
    public static class PantryItemMapper
    {

        public static List<PantryItemModel> BindItems(List<PantryItem> items)
        {
            List<PantryItemModel> models = new List<PantryItemModel>();

            foreach (PantryItem item in items)
            {
                models.Add(BindItem(item));
            }
            return models;
        }

        public static PantryItemModel BindItem(PantryItem item)
        {
            PantryItemModel model = new PantryItemModel();
            model.ID = item.ID;
            model.Description  = item.Description;
            model.Quantity = item.Quantity;
            model.Ingredient = IngredientMapper.BindItem(item.Ingredient);
            model.IngredientId = item.IngredientId;
            model.ExpirationDate = item.ExpirationDate;
            if (item.ExpirationDate != null)
            {
                DateTime temp = (DateTime)item.ExpirationDate;
                model.ExpirationDateString = temp.Date.ToShortDateString();
            }
            else
            {
                model.ExpirationDateString = "-";
            }
   
            if(item.MeasureId!=null)
            {
                model.Measure = MeasureMapper.BindItem(item.Measure);
                model.MeasureId = (int) item.MeasureId;
            }
            return model;
        }

    }
}
