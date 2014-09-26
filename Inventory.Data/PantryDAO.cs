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

        public void SavePantryItem(PantryItemModel pantryItem, bool isEdit)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    if (isEdit)
                    {
                        PantryItem item = (from p in context.PantryItem
                                           where p.ID == pantryItem.ID
                                           select p).FirstOrDefault();
                        item.Description = pantryItem.Description;
                        item.IngredientId = pantryItem.IngredientId;
                        item.MeasureId = pantryItem.MeasureId;
                        item.Quantity = pantryItem.Quantity;
                        if (pantryItem.ExpirationDate != null)
                            item.ExpirationDate = pantryItem.ExpirationDate;
                        context.SaveChanges();
                    }
                    else
                    {
                        PantryItem dup = (from p in context.PantryItem
                                          where p.IngredientId == pantryItem.IngredientId
                                          && p.Description == pantryItem.Description
                                          && p.ExpirationDate == pantryItem.ExpirationDate
                                          && p.MeasureId == pantryItem.MeasureId
                                          select p).FirstOrDefault();

                        if (dup == null)
                        {
                            PantryItem item = new PantryItem();
                            item.Description = pantryItem.Description;
                            item.IngredientId = pantryItem.IngredientId;
                            item.MeasureId = pantryItem.MeasureId;
                            item.Quantity = pantryItem.Quantity;
                            if (pantryItem.ExpirationDate != null)
                                item.ExpirationDate = pantryItem.ExpirationDate;
                            context.PantryItem.Add(item);
                            context.SaveChanges();
                        }
                        else
                        {
                            dup.Quantity += pantryItem.Quantity;
                            context.SaveChanges();
                        }
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
