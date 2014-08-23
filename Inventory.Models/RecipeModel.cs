using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    [Serializable]
    public class RecipeModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Time { get; set; }
        public String Directions { get; set; }
        public String Tags { get; set; }
/*
 * ------------------------------------------------------------------------------------------------------------------------------- 
*/
        public int? Calories { get; set; }
        public string ImagePath { get; set; }
        public int? Rating { get; set; }
        //public List<GenreModel> Type { get; set; }

/*
 * -------------------------------------------------------------------------------------------------------------------------------
*/ 

        //public List<RecipeItemModel> Ingredients { get; set; }
    }
}
