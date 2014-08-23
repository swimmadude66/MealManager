﻿using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Interfaces
{
    public interface IRecipeDAO
    {
        List<RecipeModel> getRecipes();
        List<String> getAllTags();
        void SaveTag(String tag);
        int SaveRecipe(string name, string description, string directions, string tagstring);
        int SaveRecipeItem(int recipeid, TempRecipeItemModel model);
    }
}
