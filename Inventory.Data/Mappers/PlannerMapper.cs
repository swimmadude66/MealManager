using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Mappers
{
    public static class PlannerMapper
    {
        public static List<PlannerItemModel> BindItems(List<PlannedRecipe> items)
        {
            List<PlannerItemModel> result = new List<PlannerItemModel>();
            foreach (PlannedRecipe item in items)
            {
                result.Add(BindItem(item));
            }
            return result;
        }
        public static PlannerItemModel BindItem(PlannedRecipe item){
            PlannerItemModel model = new PlannerItemModel();
            model.ID = item.ID;
            model.Date = item.Date;
            model.Recipe = RecipeMapper.BindItem(item.Recipe);
            return model;
        }
    }
}
