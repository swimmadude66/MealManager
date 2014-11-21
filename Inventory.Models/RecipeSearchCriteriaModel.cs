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
        public bool have { get; set; }

        
        public override String ToString()
        {
            String val = "";
            if(ID != null)
                val += ID +" ";
            if (!String.IsNullOrWhiteSpace(Name))
                val += Name + " ";
            if (!String.IsNullOrWhiteSpace(Description))
                val += Description + " ";
            if (Time != null)
                val += Time + " ";
            if (!String.IsNullOrWhiteSpace(Directions))
                val += Directions + " ";
            if (Tags != null && Tags.Count > 0)
            {
                foreach (String tag in Tags)
                {
                    val += tag + " ";
                }
            }
            if (Ingredients != null && Ingredients.Count > 0)
            {
                foreach (IngredientModel ing in Ingredients)
                {
                    val += ing.Name + " ";
                }
            }
            return val;
        }
    }
}
