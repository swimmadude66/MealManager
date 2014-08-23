//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class RecipeItem
    {
        public int ID { get; set; }
        public double Quantity { get; set; }
        public int MeasureID { get; set; }
        public int IngredientID { get; set; }
        public string Description { get; set; }
        public int RecipeID { get; set; }
    
        public virtual Ingredient Ingredient { get; set; }
        public virtual Measure Measure { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
