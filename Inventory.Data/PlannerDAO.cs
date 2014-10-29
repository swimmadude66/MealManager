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
    }
}
