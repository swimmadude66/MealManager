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

        public event EventHandler RecipePlanned;

        public String instructions;
        private List<TempRecipeItemModel> recipeItems;
        private List<String> newtags;
        private List<IngredientModel> searchingredients;
        public bool isEditting { get; set; }

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
            recipeCardGrid.ItemsSource = getRecipes(50, 0, (bool)rbViewHave.IsChecked);
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

        public bool isValidMealDate(DateTime? mealDate)
        {
            DateTime? testedDateTime = mealDate;

            if (testedDateTime != null && testedDateTime.HasValue && (testedDateTime.Value.CompareTo(System.DateTime.Today) >= 0))
            {
                return true;
            }
            return false;
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
            criteria.have = (bool)rbViewHave.IsChecked;
            recipeCardGrid.ItemsSource = searchRecipes(50, 0, criteria);
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
            RecipeDetailName.Text = recipeModel.Name.Trim();
            DescriptionTxt.Text = recipeModel.Description.Trim();
            DirectionsTxt.Text = recipeModel.Directions.Trim();

            IngredientsListCtl.ItemsSource = recipeModel.Items;
            
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

            if (PlannerGrid.Visibility == Visibility.Visible)
            {
                RecipeScrollView.Height += 200;
                PlannerGrid.Visibility = Visibility.Collapsed;
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            RecipeModel recipeModel = (RecipeModel)RecipeDetailMenu.DataContext;

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
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Button applyBtn = (Button)sender;
            RecipeModel recipeModel = (RecipeModel)RecipeDetailMenu.DataContext;

            if (string.IsNullOrWhiteSpace(DescriptionBox.Text.ToString()) || string.IsNullOrWhiteSpace(DirectionsBox.Text.ToString()))
                return;

            recipeModel.Description = DescriptionBox.Text.ToString();
            recipeModel.Directions = DirectionsBox.Text.ToString();

            IRecipeManager recipeManager = ManagerFactory.GetRecipeManager();

            recipeManager.SaveRecipe(recipeModel, true);
            initSources();

            RecipeDetailNameBox.Visibility = Visibility.Collapsed;
            RecipeDetailName.Visibility = Visibility.Visible;
            RecipeDetailName.Text = RecipeDetailNameBox.Text;

            EditBtn.Visibility = Visibility.Visible;
            PlanRecipeBtn.Visibility = Visibility.Visible;
            DoneBtn.Visibility = Visibility.Collapsed;

            DescriptionBox.Visibility = Visibility.Collapsed;
            DescriptionTxt.Visibility = Visibility.Visible;
            DescriptionTxt.Text =  DescriptionBox.Text;

            DirectionsBox.Visibility = Visibility.Collapsed;
            DirectionsTxt.Visibility = Visibility.Visible;
            DirectionsTxt.Text =  DirectionsBox.Text;
        }

        private void Plan_Click(object sender, RoutedEventArgs e)
        {
            if (PlannerGrid.Visibility == Visibility.Collapsed)
            {
                RecipeScrollView.Height -= 200;
                PlannerGrid.Visibility = Visibility.Visible;
            }
        }

        private void btnCancelPlannedRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (PlannerGrid.Visibility == Visibility.Visible)
            {
                RecipeScrollView.Height += 200;
                PlannerGrid.Visibility = Visibility.Collapsed;
            }
        }

        public void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (isValidMealDate(PlannerDatePicker.SelectedDate))
                planRecipe();
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
            RecipeModel recipeItem = new RecipeModel();

            recipeItem.Name = txtName.Text.Trim();
            recipeItem.Description = txtDescription.Text.Trim();
            recipeItem.Directions = txtDirections.Text.Trim();
            recipeItem.Tags = txtTags.Text.Trim();
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            int rid = manager.SaveRecipe(recipeItem, false);
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

        private void planRecipe()
        {
            DateTime plannerItemDate = (DateTime)PlannerDatePicker.SelectedDate;
            RecipeModel plannerItemRecipe = (RecipeModel)RecipeDetailMenu.DataContext;
            PlannerItemModel model = new PlannerItemModel();
            model.Date = plannerItemDate;
            model.Recipe = plannerItemRecipe;
            savePlan(model, false);
            if(RecipePlanned != null)
                RecipePlanned(this, EventArgs.Empty);
            initSources();
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
        private List<RecipeModel> getRecipes(int Limit, int size, bool have)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.getRecipes(Limit, size, have);
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

        private int savePlan(PlannerItemModel model, bool isEdit)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.PlanRecipe(model, isEdit);
        }

        private List<RecipeModel> searchRecipes(int Limit, int size, RecipeSearchCriteriaModel criteria)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.SearchRecipes(Limit, size, criteria);
        }
    }
}
