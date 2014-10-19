using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Models
{
    [Serializable]
    public class MeasureModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            MeasureModel measure = obj as MeasureModel;
            if ((System.Object)measure == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (ID == measure.ID);
        }

        public bool Equals(MeasureModel measure)
        {
            // If parameter is null return false:
            if ((object)measure == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (ID == measure.ID);
        }

        public override int GetHashCode()
        {
            return ID * ID;
        }
    }
}
