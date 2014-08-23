using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Inventory.Models;

namespace Inventory.Data.Interfaces
{
    public interface IIngredientDAO
    {
        int SaveIngredient(string name, string Description);
        IngredientModel GetIngredient(int id);
        MeasureModel GetMeasure(int id);
        int getIngredientID(string name);
        List<IngredientModel> getIngredients();
    }
}
