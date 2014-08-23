using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    [Serializable]
    public class PantryItemModel
    {
        public int ID { get; set;}
        public double Quantity { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public String ExpirationDateString { get; set; }
        public string Description { get; set; }

        public int IngredientId { get; set; }
        public int MeasureId { get; set; }

        public IngredientModel Ingredient { get; set; }
        public MeasureModel Measure { get; set; }

        public String toString()
        {
            return Ingredient.Name;
        }
    }
}
