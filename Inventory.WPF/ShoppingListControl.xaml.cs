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

        System.DateTime date1 = new System.DateTime(1996, 6, 3, 22, 15, 0);
        System.DateTime date2 = new System.DateTime(2016, 12, 6, 13, 2, 0);

        public ShoppingListControl()
        {
            InitializeComponent();            
        }

        public void DoComparison(object sender, RoutedEventArgs e)
        {
            listGrid.ItemsSource = Compare();
        }

        public List<IngredientModel> Compare()
        {
            List<PantryItemModel> pantryItems = getPantryItems();
            DateTime? fromDate = fromPicker.SelectedDate;
            DateTime? untilDate = untilPicker.SelectedDate;
            List<RecipeModel> recipes = getPlannedRecipes(fromDate, untilDate);
            
            List<IngredientModel> shoppingList = new List<IngredientModel>();

            foreach(RecipeModel recipe in recipes)
            {
                foreach(int ingredientID in recipe.IngredientIDs)
                {
                    int check = 0;
                    foreach (PantryItemModel pantryItem in pantryItems)
                    {
                        if (pantryItem.IngredientId == ingredientID) check = 1;
                    }
                    if (check != 1) shoppingList.Add(getIngredientByID(ingredientID));
                    
                }
            }
            return shoppingList;
        }

        public List<RecipeModel> getPlannedRecipes(DateTime? begin, DateTime? end)
        {
            List<RecipeModel> recipes = new List<RecipeModel>();
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            List<PlannerItemModel> plannedrecipes = manager.GetPlannedRecipes(begin, end);
            foreach (PlannerItemModel plannedrecipe in plannedrecipes)
            {
                recipes.Add(plannedrecipe.Recipe);
            }

            return recipes;
        }

        public List<PantryItemModel> getPantryItems()
        {
            IPantryManager manager = ManagerFactory.GetPantryManager();
            return manager.GetPantryContents();
        }

        public IngredientModel getIngredientByID(int ingredientID)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.GetIngredient(ingredientID);
        }
        
    }
}
