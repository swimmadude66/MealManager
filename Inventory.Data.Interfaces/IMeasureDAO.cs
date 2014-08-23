using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Interfaces
{
    public interface IMeasureDAO
    {
        List<MeasureModel> getMeasures();
        MeasureModel getMeasure(int id);
        int SaveMeasure(string name);
        int getMeasureID(string name);
    }
}
