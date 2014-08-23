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
    
    public partial class PantryItem
    {
        public int ID { get; set; }
        public int IngredientId { get; set; }
        public double Quantity { get; set; }
        public Nullable<int> MeasureId { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string Description { get; set; }
    
        public virtual Ingredient Ingredient { get; set; }
        public virtual Measure Measure { get; set; }
    }
}
