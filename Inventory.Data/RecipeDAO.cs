﻿using Inventory.Data.Interfaces;
using Inventory.Data.Mappers;
using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data
{
    public class RecipeDAO : IRecipeDAO
    {
        public List<RecipeModel> getRecipes()
        {
            try
            {
                using(var context = new InventoryEntities()){
                    List<Recipe> recipes = (from r in context.Recipe
                                            select r).OrderBy(v=>v.Name).ToList();
                    return RecipeMapper.BindItems(recipes);
                }
            }
            catch{throw;}
        }

        public List<String> getAllTags()
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    List<Tags> tags = (from t in context.Tags
                                      select t).OrderBy(v=>v.Tag).ToList();
                    List<String> tagnames = new List<string>();
                    foreach (Tags t in tags)
                    {
                        tagnames.Add(t.Tag);
                    }
                    return tagnames;
                }
            }
            catch
            {
                throw;
            }
        }

        public void SaveTag(String tag)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    Tags e = (from t in context.Tags
                              where t.Tag == tag.Trim()
                              select t).FirstOrDefault();
                    if (e == null)
                    {
                        Tags t = new Tags();
                        t.Tag = tag;
                        context.Tags.Add(t);
                        context.SaveChanges();
                    }
                }
            }
            catch { throw; }
        }

        public int SaveRecipe(string name, string description, string directions, string tagstring)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    Recipe r = new Recipe();
                    r.Name = name;
                    r.Description = description;
                    r.Directions = directions;
                    r.TagString = tagstring;

                    context.Recipe.Add(r);
                    context.SaveChanges();

                    return r.ID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int SaveRecipeItem(int recipeid, TempRecipeItemModel model)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    RecipeItem r = new RecipeItem();
                    r.IngredientID = model.IngredientID;
                    r.MeasureID = model.MeasureID;
                    r.Quantity = model.Quantity;
                    r.RecipeID = recipeid;
                    r.Description = model.Description;

                    context.RecipeItem.Add(r);
                    context.SaveChanges();

                    return r.ID;
                }
            }
            catch { throw; }
        }

    }
}
