using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Mappers
{
    public static class MeasureMapper
    {
        public static List<MeasureModel> BindItems(List<Measure> items)
        {
            List<MeasureModel> models = new List<MeasureModel>();
            foreach (Measure item in items)
            {
                models.Add(BindItem(item));
            }
            return models;
        }

        public static MeasureModel BindItem(Measure item)
        {
            MeasureModel model = new MeasureModel();
            model.ID = item.ID;
            model.Name = item.Measurement;
            return model;
        }
    }
}
