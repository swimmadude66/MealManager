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
    public class PlannerDAO : IPlannerDAO
    {
        public int PlanRecipe(PlannerItemModel model, bool isEdit)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    if (isEdit)
                    {
                        PlannedRecipe plan = (from p in context.PlannedRecipe
                                              where p.ID == model.ID
                                              select p).FirstOrDefault();
                        plan.RecipeID = model.Recipe.ID;
                        plan.Date = model.Date;
                        plan.Active = true;
                        context.PlannedRecipe.Attach(plan);
                        context.SaveChanges();
                        return plan.ID;
                    }
                    else
                    {
                        PlannedRecipe pr = new PlannedRecipe();
                        pr.RecipeID = model.Recipe.ID;
                        pr.Date = model.Date;
                        pr.Active = true;
                        context.PlannedRecipe.Add(pr);
                        context.SaveChanges();
                        return pr.ID;
                    }
                }
            }
            catch { throw; }
        }

        public void cancelPlan(int id)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    PlannedRecipe plan = (from p in context.PlannedRecipe
                                          where p.ID == id
                                          select p).FirstOrDefault();
                    plan.Active = false;
                    context.PlannedRecipe.Attach(plan);
                    var entry = context.Entry(plan);
                    entry.Property(a => a.Active).IsModified = true;
                    context.SaveChanges();
                }
            }
            catch { throw;}
        }

        public List<PlannerItemModel> GetPlannedRecipes(DateTime? start, DateTime? end)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    List<PlannedRecipe> recs = new List<PlannedRecipe>();
                    if (start != null)
                    {
                        if (end != null)
                        {
                            if (start.Equals(end))
                            {
                                recs = (from p in context.PlannedRecipe
                                        where p.Date == start && p.Active == true
                                        select p).ToList();
                            }
                            else
                            {
                                recs = (from p in context.PlannedRecipe
                                        where (p.Date >= start && p.Date <= end) && p.Active == true
                                        select p).ToList();
                            }
                        }
                        else
                        {
                            recs = (from p in context.PlannedRecipe
                                    where (p.Date >= start) && p.Active == true
                                    select p).ToList();
                        }
                    }
                    else
                    {
                        if (end != null)
                        {
                            recs = (from p in context.PlannedRecipe
                                    where (p.Date <= end) && p.Active == true
                                    select p).ToList();
                        }
                        else
                        {
                            recs = (from p in context.PlannedRecipe
                                    where p.Active == true
                                    select p).ToList();
                        }
                    }
                    return PlannerMapper.BindItems(recs);
                }
            }
            catch { throw; }
        }

        public List<IngredientModel> GenerateShoppingList(DateTime? start, DateTime? end)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    List<Ingredient> shopList = new List<Ingredient>();
                    String startQuery = "Select * From Ingredient Join (Select need.IngredientID From PantryItem Right Join (Select Distinct IngredientID From RecipeItem Join "+
                                    "(Select RecipeID From PlannedRecipe Where Active = 1 ";

                    String endquery = ") as planned On planned.RecipeID = RecipeItem.RecipeID) as need "+
                                    "On PantryItem.IngredientID = need.IngredientID Where PantryItem.IngredientID is NULL) as list on list.IngredientID = Ingredient.ID";
                    String startfilt = "";
                    String endfilt = "";
                    if(start != null){
                        startfilt = "And Date >= '" + start.Value.Year +"-" + start.Value.Month +"-" + start.Value.Day + "'";
                    }
                    if(end != null){
                        endfilt = "And Date < '" + end.Value.Year +"-" + end.Value.Month +"-" + end.Value.Day + "'";
                    }

                    String query = startQuery + startfilt + endfilt + endquery;
                    shopList = context.Ingredient.SqlQuery(query).ToList();

                    return Mappers.IngredientMapper.BindItems(shopList);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
