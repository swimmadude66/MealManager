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
    public class PantryDAO : IPantryDAO
    {
        public List<PantryItemModel> GetPantryContents()
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    List<PantryItem> items = (from p in context.PantryItem
                                              where p.Quantity > 0
                                              select p).OrderBy(v=>v.Ingredient.Name).ToList();

                    return PantryItemMapper.BindItems(items);
                }
            }
            catch
            {
                throw;
            }
        }

        public void SavePantryItem(int ingredient, double quantity, int measure, string description, DateTime? expires)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    PantryItem dup = (from p in context.PantryItem
                                          where p.IngredientId == ingredient
                                          && p.Description == description
                                          && p.ExpirationDate == expires
                                          && p.MeasureId == measure
                                          select p).FirstOrDefault();
                    if(dup == null)
                    {
                        PantryItem item = new PantryItem();
                        item.Description = description;
                        item.IngredientId = ingredient;
                        item.MeasureId = measure;
                        item.Quantity = quantity;
                        if(expires != null)
                            item.ExpirationDate = expires;
                        context.PantryItem.Add(item);
                        context.SaveChanges();
                    }
                    else
                    {
                        dup.Quantity += quantity;
                        context.SaveChanges();
                    }
                }
            }
            catch
            {
                throw;
            }
        }           
    }
}
