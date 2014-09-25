using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    [Serializable]
    public class TempPlannerItemModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public String MealName {get; set;}
        public List<RecipeModel> PlannedRecipes { get; set; }

        public TempPlannerItemModel(string name, DateTime date, List<RecipeModel> recipes )
        {
            MealName = name;
            Date = date;
            PlannedRecipes = recipes;
        }
    }
}
