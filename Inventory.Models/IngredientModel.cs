using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    [Serializable]
    public class IngredientModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public String toString()
        {
            return Name;
        }
    }
}
