using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    [Serializable]
    public class RecipeItemModel
    {
        public int ID { get; set; }
        public int RecipeID { get; set; }
        public double Quantity { get; set; }
        public string Description { get; set; }
        
        public IngredientModel Ingredient { get; set; }
        public string Measure { get; set; }

        
    }
}
