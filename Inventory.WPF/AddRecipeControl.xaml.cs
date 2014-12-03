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
    /// Interaction logic for AddRecipeControl.xaml
    /// </summary>
    public partial class AddRecipeControl : UserControl
    {
        public AddRecipeControl()
        {
            InitializeComponent();
        }

        //private void btnSaveRecipe_Click(object sender, RoutedEventArgs e)
        //{
        //    //Validate
        //    if ((String.IsNullOrEmpty(txtName.Text.Trim())) || (String.IsNullOrEmpty(txtDirections.Text.Trim())) || (recipeItems.Count < 1))
        //    {
        //        lblError.Content = "Invalid Recipe, Unable to save";
        //        return;
        //    }
        //    //--------------------------------------
        //    SaveRecipe();
        //    clearAddRecipe();
        //}

        //private void SaveRecipe()
        //{
        //    RecipeModel recipeItem = new RecipeModel();

        //    recipeItem.Name = txtName.Text.Trim();
        //    recipeItem.Description = txtDescription.Text.Trim();
        //    recipeItem.Directions = txtDirections.Text.Trim();
        //    recipeItem.Tags = txtTags.Text.Trim();
        //    IRecipeManager manager = ManagerFactory.GetRecipeManager();
        //    int rid = manager.SaveRecipe(recipeItem, false);
        //    foreach (String s in newtags)
        //    {
        //        manager.SaveTag(s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower());
        //    }
        //    foreach (TempRecipeItemModel m in recipeItems)
        //    {
        //        m.IngredientID = getIngredientId(m.IngredientName);
        //        m.MeasureID = getMeasureID(m.MeasureName);
        //        manager.SaveRecipeItem(rid, m);
        //    }


        //}

        //private void clearAddRecipe()
        //{
        //    lblError.Content = "";
        //    txtName.Text = "";
        //    txtDescription.Text = "";
        //    txtDirections.Text = "";
        //    txtIngredient.Text = "";
        //    txtQuantity.Text = "";
        //    txtTags.Text = "";
        //    recipeItems.Clear();
        //    instructions = "";
        //    newtags.Clear();
        //}
    }
}
