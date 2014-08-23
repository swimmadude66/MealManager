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
    public class MeasureDAO : IMeasureDAO
    {
        public List<MeasureModel> getMeasures()
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    List<Measure> items = (from m in context.Measure
                                           select m).OrderBy(v=>v.Measurement).ToList();

                    return MeasureMapper.BindItems(items);
                }
            }
            catch
            {
                throw;
            }
        }

        public MeasureModel getMeasure(int id)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    Measure item = (from m in context.Measure
                                           where m.ID == id
                                           select m).FirstOrDefault();

                    return MeasureMapper.BindItem(item);
                }
            }
            catch
            {
                throw;
            }
        }

        public int getMeasureID(string name)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    Measure measure = (from m in context.Measure
                                       where m.Measurement.ToLower() == name.ToLower()
                                       select m).FirstOrDefault();
                    if (measure == null)
                    {
                        return -1;
                    }
                    else
                    {
                        return measure.ID;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public int SaveMeasure(string name)
        {
            try
            {
                using (var context = new InventoryEntities())
                {
                    Measure measure = new Measure();
                    measure.Measurement = name;
                    context.Measure.Add(measure);
                    context.SaveChanges();
                    return measure.ID;
                }
            }
            catch
            {
                throw;
            }

        }
    }
}
