using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    [Serializable]
    public class RecipeSearchCriteriaModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Time { get; set; }
        public String Directions { get; set; }
        public List<String> Tags { get; set; }
        public List<IngredientModel> Ingredients { get; set; }
    }
}
