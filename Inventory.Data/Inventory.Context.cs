﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InventoryEntities : DbContext
    {
        public InventoryEntities()
            : base("name=InventoryEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Measure> Measure { get; set; }
        public virtual DbSet<PantryItem> PantryItem { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<RecipeItem> RecipeItem { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<PlannedRecipe> PlannedRecipe { get; set; }
    }
}
