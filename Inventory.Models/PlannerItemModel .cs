using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    [Serializable]
    public class PlannerItemModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}
