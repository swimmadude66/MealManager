﻿using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Interfaces
{
    public interface IPlannerDAO
    {
        int PlanRecipe(PlannerItemModel model, bool isEdit);
        List<PlannerItemModel> GetPlannedRecipes(DateTime? start, DateTime? end);
        void cancelPlan(int id);
        List<IngredientModel> GenerateShoppingList(DateTime? start, DateTime? end);
    }
}
