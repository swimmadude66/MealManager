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
        private List<PantryItemModel> pantry;
        private List<RecipeModel> planned;

        public ShoppingListControl()
        {
            InitializeComponent();
            pantry = new List<PantryItemModel>();
            planned = new List<RecipeModel>();
            pantry = getPantryItems();
        }

        public void DoComparison(object sender, RoutedEventArgs e)
        {
            listGrid.ItemsSource = Compare();
        }

        public List<IngredientModel> Compare()
        {
            DateTime? fromDate = fromPicker.SelectedDate;
            DateTime? untilDate = untilPicker.SelectedDate;
            planned = getPlannedRecipes(fromDate, untilDate);
            List<IngredientModel> shoppingList = new List<IngredientModel>();
            List<int> pantrying = new List<int>();
            foreach (PantryItemModel item in pantry)
            {
                if (!pantrying.Contains(item.IngredientId))
                {
                    pantrying.Add(item.IngredientId);
                }
            }
            List<int> ShoppingListIDs = new List<int>();
            foreach(RecipeModel recipe in planned)
            {
                foreach (RecipeItemModel recipeitem in recipe.Items)
                {
                    if (!pantrying.Contains(recipeitem.Ingredient.ID) && !ShoppingListIDs.Contains(recipeitem.Ingredient.ID))
                    {
                        shoppingList.Add(recipeitem.Ingredient);
                        ShoppingListIDs.Add(recipeitem.Ingredient.ID);
                    }
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
