using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    [Serializable]
    public class TempRecipeItemModel
    {
        public double Quantity { get; set; }
        public string QuantityString { get; set; }
        public string Description { get; set; }
        public string IngredientName { get; set; }
        public string MeasureName { get; set; }
        public int IngredientID { get; set; }
        public int MeasureID { get; set; }

        public TempRecipeItemModel(string ing, double quantity, string measure)
        {
            IngredientName = ing;
            Quantity = quantity;
            MeasureName = measure;
        }
    }
}
