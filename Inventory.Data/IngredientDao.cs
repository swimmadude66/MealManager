using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Inventory.Data.Interfaces;
using Inventory.Models;
using Inventory.Data.Mappers;

namespace Inventory.Data
{
    public class IngredientDao : IIngredientDAO
    {

        public int SaveIngredient(string name, string Description)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    Ingredient ingredient = new Ingredient();
                    ingredient.Name = name;
                    ingredient.Description = Description;
                    context.Ingredient.Add(ingredient);
                    context.SaveChanges();
                    return ingredient.ID;
                }
            }
            catch{
               
                throw;
            }
            
        }

        public List<IngredientModel> getIngredients()
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    List<Ingredient> ingredients = (from i in context.Ingredient
                                             select i).OrderBy(v=>v.Name).ToList();
                    return IngredientMapper.BindItems(ingredients);
                }
            }
            catch
            {
                throw;
            }
        }

        public IngredientModel GetIngredient(int id)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    Ingredient ingredient = (from i in context.Ingredient
                                             where i.ID == id
                                             select i).FirstOrDefault();
                    return IngredientMapper.BindItem(ingredient);
                }
            }
            catch
            {
                throw;
            }
        }

        public int getIngredientID(string name)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    Ingredient ingredient = (from i in context.Ingredient
                                             where i.Name.ToLower() == name.ToLower()
                                             select i).FirstOrDefault();
                    if (ingredient == null)
                    {
                        return -1;
                    }
                    else return ingredient.ID;
                }
            }
            catch
            {
                throw;
            }
        }
        public MeasureModel GetMeasure(int id)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    Measure measure = (from m in context.Measure
                                       where m.ID == id
                                       select m).FirstOrDefault();
                    return MeasureMapper.BindItem(measure);
                }
            }
            catch
            {
                throw;
            }
        }

        
    }

    
}
