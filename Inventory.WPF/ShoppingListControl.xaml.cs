using Inventory.Data.Interfaces;
using Inventory.Managers.Factory;
using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory.WPF
{
    /// <summary>
    /// Interaction logic for ShoppingListControl.xaml
    /// </summary>
    public partial class ShoppingListControl : UserControl
    {
        List<PantryItemModel> pantry;
        List<RecipeModel> planned;

        public ShoppingListControl()
        {
            InitializeComponent();
        }

        public List<IngredientModel> Compare()
        {
            //foreach pantryiem in pantry{
            //foreach Recipe in planned{
            //recipe.IngredientIDs.contains(pantryitem.Ingredient.ID);    
            //}
            //}
            return new List<IngredientModel>();
        }

        //getPlannedRecipes();
        //foreach(plannedrecipe r : plannedrecipes){
        //planned.Add(r.recipe);
        //}

        //Domain Calls

        public List<PantryItemModel> getPantryItems()
        {
            IPantryManager manager = ManagerFactory.GetPantryManager();
            return manager.GetPantryContents();
        }

        //Get Planned recipes

    }
}
