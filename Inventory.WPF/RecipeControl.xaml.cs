using Inventory.Models;
using Inventory.Data.Interfaces;
using Inventory.Managers.Factory;
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
using System.Text.RegularExpressions;

namespace Inventory.WPF
{
    /// <summary>
    /// Interaction logic for RecipeControl.xaml
    /// </summary>
    public partial class RecipeControl : UserControl
    {

        public String instructions;
        private List<TempRecipeItemModel> recipeItems;
        private List<String> newtags;
        private List<IngredientModel> searchingredients;

        public RecipeControl()
        {
            InitializeComponent();
            recipeItems = new List<TempRecipeItemModel>();
            newtags = new List<String>();
            searchingredients = new List<IngredientModel>();
            initSources();
            this.DataContext = this;
        }

        private void initSources()
        {
            //create cards
            //item control
            //uniform grid
            recipeCardGrid.ItemsSource = getRecipes();
            dgIngredients.ItemsSource = recipeItems;
            dgIngredients.Items.Refresh();
            List<IngredientModel> ingredients = getIngredients();
            txtIngredient.ItemsSource = ingredients;
            txtIngredient.SelectedIndex = -1;
            txtIngredient.Text = "";
            cbSearchIngredients.ItemsSource = ingredients;
            cbSearchIngredients.SelectedIndex = -1;
            cbSearchIngredients.Text = "";
            cbMeasure.ItemsSource = getMeasures();
            cbMeasure.SelectedIndex = -1;
            cbMeasure.Text = "";
            txtQuantity.Text = "";
            cbTags.ItemsSource = getAllTags();
            cbTags.SelectedIndex = -1;
            cbTags.Text = "";
        }

        private void clearAddRecipe()
        {
            lblError.Content = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtDirections.Text = "";
            txtIngredient.Text = "";
            txtQuantity.Text = "";
            txtTags.Text = "";
            recipeItems.Clear();
            instructions = "";
            newtags.Clear();
        }

        private void btnSearchAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (cbSearchIngredients.SelectedIndex < 0)
            {
                return;
            }
            IngredientModel sel = (IngredientModel)cbSearchIngredients.SelectedItem;
            if (searchingredients.Count > 0)
                txtSearchIngredientsList.Text += ", ";
            txtSearchIngredientsList.Text += sel.Name;
            searchingredients.Add(sel);
            cbSearchIngredients.Text = "";
            cbSearchIngredients.SelectedIndex = -1;
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            cbSearchIngredients.Text = "";
            cbSearchIngredients.SelectedIndex = -1;
            txtSearchIngredientsList.Text = "";
            txtSearchDescription.Text = "";
            txtSearchName.Text = "";
            searchingredients.Clear();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            RecipeSearchCriteriaModel criteria = new RecipeSearchCriteriaModel();
            if (!String.IsNullOrEmpty(txtSearchName.Text.Trim()))
            {
                criteria.Name = txtSearchName.Text.Trim();
            }
            if (!String.IsNullOrEmpty(txtSearchDescription.Text.Trim()))
            {
                criteria.Description = txtSearchDescription.Text.Trim();
            }
            if (searchingredients.Count > 0)
            {
                criteria.Ingredients = searchingredients;
            }
            recipeCardGrid.ItemsSource = searchRecipes(criteria);
            recipeCardGrid.Items.Refresh();
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            clearAddRecipe();
            listPanel.Visibility = Visibility.Hidden;
            addPanel.Visibility = Visibility.Visible;
        }

        private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (AddIngredient())
                initSources();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            clearAddRecipe();
            listPanel.Visibility = Visibility.Visible;
            addPanel.Visibility = Visibility.Hidden;
        }

        private void btnSaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            //Validate
            if ((String.IsNullOrEmpty(txtName.Text.Trim())) || (String.IsNullOrEmpty(txtDirections.Text.Trim())) || (recipeItems.Count < 1))
            {
                lblError.Content = "Invalid Recipe, Unable to save";
                return;
            }
            //--------------------------------------
            SaveRecipe();
            clearAddRecipe();
            initSources();
            listPanel.Visibility = Visibility.Visible;
            addPanel.Visibility = Visibility.Hidden;
        }

        private void btnAdd_Tag_Click(object sender, RoutedEventArgs e)
        {
            string tagtext = "";
            if (cbTags.SelectedIndex < 0)
            {
                if (!newtags.Contains(cbTags.Text.Trim())){
                    newtags.Add(cbTags.Text.Trim());
                }
                tagtext = cbTags.Text.Trim();
            }
            else
            {
                tagtext = ((String)cbTags.SelectedItem).Trim();
            }
            if (!txtTags.Text.Contains(tagtext))
            {
                if (txtTags.Text.Length <= 0)
                {
                    txtTags.Text = tagtext;
                }
                else
                {
                    txtTags.Text += ", " + tagtext;
                }
            }
        }

        private void RecipeCardViewBtn_Click(object sender, RoutedEventArgs e)
        {
            RecipeCardControl recipeViewBtn = (RecipeCardControl)sender;
            RecipeModel recipeModel = (RecipeModel)recipeViewBtn.DataContext;
            RecipeDetailMenu.DataContext = recipeModel;
            RecipeDetailName.Text = recipeModel.Name;
            DescriptionTxt.Text = recipeModel.Description;
            DirectionsTxt.Text = recipeModel.Directions;

            //IRecipeManager recipeManager = ManagerFactory.GetRecipeManager();
            //recipeManager.getIngredients();
            //foreach(int id in recipeModel.IngredientIDs){

            //}
            //recipeModel.
            TagsTxt.Text = recipeModel.Tags;

            MainRecipesMenu.Visibility = Visibility.Collapsed;
            RecipeDetailMenu.Visibility = Visibility.Visible;
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            MainRecipesMenu.Visibility = Visibility.Visible;
            RecipeDetailMenu.Visibility = Visibility.Collapsed;

            RecipeDetailNameBox.Visibility = Visibility.Collapsed;
            RecipeDetailName.Visibility = Visibility.Visible;
            //RecipeDetailNameBox.Text = RecipeDetailName.Text;

            EditBtn.Visibility = Visibility.Visible;
            PlanRecipeBtn.Visibility = Visibility.Visible;
            DoneBtn.Visibility = Visibility.Collapsed;

            DescriptionBox.Visibility = Visibility.Collapsed;
            DescriptionTxt.Visibility = Visibility.Visible;

            DirectionsBox.Visibility = Visibility.Collapsed;
            DirectionsTxt.Visibility = Visibility.Visible;

            IngredientsBox.Visibility = Visibility.Collapsed;
            IngredientsTxt.Visibility = Visibility.Visible;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            RecipeDetailName.Visibility = Visibility.Collapsed;
            RecipeDetailNameBox.Visibility = Visibility.Visible;
            RecipeDetailNameBox.Text = RecipeDetailName.Text;

            EditBtn.Visibility = Visibility.Collapsed;
            PlanRecipeBtn.Visibility = Visibility.Collapsed;
            DoneBtn.Visibility = Visibility.Visible;

            DescriptionTxt.Visibility = Visibility.Collapsed;
            DescriptionBox.Visibility = Visibility.Visible;
            DescriptionBox.Text = DescriptionTxt.Text;

            DirectionsTxt.Visibility = Visibility.Collapsed;
            DirectionsBox.Visibility = Visibility.Visible;
            DirectionsBox.Text = DirectionsTxt.Text;

            //IngredientsTxt.Visibility = Visibility.Collapsed;
            //IngredientsBox.Visibility = Visibility.Visible;
            //IngredientsBox.Text = IngredientsTxt.Text;

            //TagsTxt.Visibility = Visibility.Collapsed;
            //TagsBox.Visibility = Visibility.Visible;
            //TagsBox.Text = TagsTxt.Text;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Button applyBtn = (Button)sender;
            RecipeModel recipeModel = (RecipeModel)RecipeDetailMenu.DataContext;
            TempRecipeItemModel tempRecipeModel = new TempRecipeItemModel("", 0, "");


            RecipeDetailNameBox.Visibility = Visibility.Collapsed;
            RecipeDetailName.Visibility = Visibility.Visible;
            //tempRecipeModel.Name = RecipeDetailNameBox.Text;

            EditBtn.Visibility = Visibility.Visible;
            PlanRecipeBtn.Visibility = Visibility.Visible;
            DoneBtn.Visibility = Visibility.Collapsed;

            DescriptionBox.Visibility = Visibility.Collapsed;
            DescriptionTxt.Visibility = Visibility.Visible;
            tempRecipeModel.Description = RecipeDetailNameBox.Text;

            DirectionsBox.Visibility = Visibility.Collapsed;
            DirectionsTxt.Visibility = Visibility.Visible;

            IngredientsBox.Visibility = Visibility.Collapsed;
            IngredientsTxt.Visibility = Visibility.Visible;

            IRecipeManager recipeManager = ManagerFactory.GetRecipeManager();
            //recipeManager.
            //recipeManager.

            //initSources;

            //TagsBox.Visibility = Visibility.Collapsed;
            //TagsTxt.Visibility = Visibility.Visible;
        }

        private bool AddIngredient()
        {
            string ingredient = "";

            if (txtIngredient.SelectedIndex >= 0)
            {
                ingredient = ((IngredientModel)txtIngredient.SelectedItem).Name;
            }
            else
            {
                ingredient = txtIngredient.Text.Trim();
            }

            String measureName = "";
            if (cbMeasure.SelectedIndex >= 0)
            {
                measureName = ((MeasureModel)cbMeasure.SelectedItem).Name;
            }
            else
            {
                measureName = cbMeasure.Text.Trim();
            }

            double quant = -1.0;
            string quantstring = txtQuantity.Text.Trim();
            if (Regex.IsMatch(quantstring, @"^[0-9\.]+$"))
            {
                quant = Double.Parse(quantstring);
            }
            else if (Regex.IsMatch(quantstring, @"^([0-9]+)/([0-9]+)$"))
            {
                double a = double.Parse(quantstring.Substring(0, quantstring.IndexOf('/')));
                double b = double.Parse(quantstring.Substring(quantstring.IndexOf('/') + 1));
                quant = a / b;
            }
            else if (Regex.IsMatch(quantstring, @"^[0-9]+\s+[0-9]+/[0-9]+$"))
            {
                string[] parts = Regex.Split(quantstring, @"\s+");
                double whole = double.Parse(parts[0]);
                double a = double.Parse(parts[1].Substring(0, parts[1].IndexOf('/')));
                double b = double.Parse(parts[1].Substring(parts[1].IndexOf('/') + 1));
                quant = whole + (a / b);
            }
            else
            {
                return false;
            }

            if (string.IsNullOrEmpty(ingredient) || quant < 1/64)
            {
                return false;
            }
            TempRecipeItemModel model = new TempRecipeItemModel(ingredient, quant, measureName);
            model.QuantityString = Tools.ToolBox.DecimalToFraction(model.Quantity);
            recipeItems.Add(model);
            return true;
        }

        private void SaveRecipe()
        {
            string name = txtName.Text.Trim();
            string descrip = txtDescription.Text.Trim();
            string directions = txtDirections.Text.Trim();
            string tagstring = txtTags.Text.Trim();
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            int rid = manager.SaveRecipe(name, descrip, directions, tagstring);
            foreach (String s in newtags)
            {
                manager.SaveTag(s.Substring(0,1).ToUpper() + s.Substring(1).ToLower());
            }
            foreach (TempRecipeItemModel m in recipeItems)
            {
                m.IngredientID = getIngredientId(m.IngredientName);
                m.MeasureID = getMeasureID(m.MeasureName);
                manager.SaveRecipeItem(rid, m);
            }


        }

        private int getIngredientId(String name)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            int id = manager.getIngredientID(name);
            if (id < 0)
            {
                return manager.SaveIngredient(name, "");
            }
            else return id;
        }

        private int getMeasureID(String name)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            int id = manager.getMeasureID(name);
            if (id < 0)
            {
                return manager.SaveMeasure(name);
            }
            else return id;
        }

        //Domain Calls
        private List<RecipeModel> getRecipes()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.getRecipes();
        }

        private List<IngredientModel> getIngredients()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.getIngredients();
        }

        private List<MeasureModel> getMeasures()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.getMeasures();
        }

        private List<String> getAllTags()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.getAllTags();
        }

        private List<RecipeModel> searchRecipes(RecipeSearchCriteriaModel criteria)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.SearchRecipes(criteria);
        }

        

        

    }
}
