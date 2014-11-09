using Inventory.Data.Interfaces;
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
        public List<RecipeModel> getRecipes(int Limit)
        {
            try
            {
                List<RecipeModel> recipeInfo = new List<RecipeModel>();
                using(var context = new InventoryEntities()){
                    List<Recipe> recipes;
                    if (Limit >= 0)
                    {
                        recipes = (from r in context.Recipe
                                   where r.Name != ""
                                   select r).OrderBy(v => v.Name).Take(Limit).ToList();
                    }
                    else
                    {
                        recipes = (from r in context.Recipe
                                   where r.Name != ""
                                   select r).OrderBy(v => v.Name).ToList();
                    }
                    foreach (Recipe recipe in recipes)
                    {
                        RecipeModel model = new RecipeModel();
                        model.ID = recipe.ID;
                        model.Name = recipe.Name;
                        model.Description = recipe.Description;
                        //any other Recipe specific (not items) info here
                        recipeInfo.Add(model);
                    }
                    return recipeInfo;
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

        public List<RecipeItemModel> getRecipeItems(int rid)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    List<RecipeItem> items = (from i in context.RecipeItem
                                              where i.RecipeID == rid
                                              select i).ToList();
                    return RecipeItemMapper.BindItems(items);
                }
            }
            catch { throw; }
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

        public List<RecipeModel> SearchRecipes(RecipeSearchCriteriaModel criteria)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    String filtwrapper = "Select * From (";
                    String basequery = "Select * From Recipe";
                    String searchquery = " WHERE";
                    int numParams = 0;
                    //id
                    if (criteria.ID != null && criteria.ID >= 0)
                    {
                        searchquery += " Id = " + criteria.ID;
                        numParams++;
                    }
                    //name
                    if (!String.IsNullOrWhiteSpace(criteria.Name))
                    {
                        if (numParams > 0)
                        {
                            searchquery += " AND";
                        }
                        searchquery += " Name LIKE \"%" + criteria.Name + "%\"";
                        numParams++;
                    }
                    //description
                    if (!String.IsNullOrWhiteSpace(criteria.Description))
                    {
                        if (numParams > 0)
                        {
                            searchquery += " AND";
                        }
                        searchquery += " Description LIKE \"%" + criteria.Description + "%\"";
                        numParams++;
                    }
                    //tags
                    if (criteria.Tags != null)
                    {
                        foreach (String tag in criteria.Tags)
                        {
                            if (numParams > 0)
                            {
                                searchquery += " AND";
                            }
                            searchquery += " TagString LIKE \"%" + tag + "%\"";
                            numParams++;
                        }
                    }

                    if (numParams > 0)
                    {
                        searchquery = basequery + searchquery;
                    }
                    else
                    {
                        searchquery = basequery;
                    }
                    if (criteria.Ingredients != null && criteria.Ingredients.Count > 0)
                    {
                        searchquery = filtwrapper + searchquery + ") as init";

                        String ingredSearch = "(Select ingf.* From( ";
                        int ingredientcount = 0;
                        foreach (IngredientModel ing in criteria.Ingredients)
                        {
                            ingredSearch += "(Select RecipeID from RecipeItem where IngredientID = " + ing.ID + ") ";
                            if (ingredientcount + 1 < criteria.Ingredients.Count)
                            {
                                ingredSearch += "UNION ALL ";
                            }
                            ingredientcount++;
                        }
                        ingredSearch += ") as ingf Group By ingf.RecipeID Having Count(*)>=" + ingredientcount + ")";
                        ingredSearch += "as result on result.RecipeID=init.ID";
                        searchquery += " Join " + ingredSearch;
                    }

                    List<Recipe> result = context.Recipe.SqlQuery(searchquery.Trim()).ToList();
                    return RecipeMapper.BindItems(result);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
