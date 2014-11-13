using Inventory.Data.Interfaces;
using Inventory.Models;
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
    /// Interaction logic for PantryControl.xaml
    /// </summary>
    public partial class PantryControl : UserControl
    {
        PantryItemModel itemToEdit = null;
        
        public PantryControl()
        {
            InitializeComponent();
            this.DataContext = this;
            initSources();
        }

        private void initSources()
        {
            //pantryGrid.ItemsSource = getPantry();
            pantryList.ItemsSource = getPantry();
            ddlMeasure.ItemsSource = getMeasures();
            txtIngredientName.ItemsSource = getIngredients();
            txtIngredientName.SelectedIndex = -1;
            ddlMeasure.SelectedIndex = 0;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool isEdit;
            if (itemToEdit == null)
            {
                isEdit = false;
            } 
            else
            {
                isEdit = true;
            }
            if (SavePantryItem(isEdit))
            {
                lblAddEditHeader.Content = "Add a new panty item";
                txtIngredientName.Text = "";
                txtIngredientDescription.Text = "";
                txtQuantity.Text = "";
                dpExpires.SelectedDate = null;
                ddlMeasure.SelectedIndex = 0;
                if (itemToEdit != null)
                {
                    lblSuccess.Content = "Ingredient Updated Succesfully";
                } 
                else
                {
                    lblSuccess.Content = "Ingredient Added Succesfully";
                }
                lblSuccess.Visibility = Visibility.Visible;
                itemToEdit = null;
                initSources();
            }       
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            lblAddEditHeader.Content = "Add a new panty item";
            txtIngredientName.Text = "";
            txtIngredientDescription.Text = "";
            txtQuantity.Text = "";
            ddlMeasure.SelectedIndex = 0;
            dpExpires.SelectedDate = null;
            itemToEdit = null;
            //pantryGrid.SelectedIndex = -1;
        }

        private void editRow(object sender, MouseButtonEventArgs e)
        {
            DataGrid pantryList = sender as DataGrid;
            itemToEdit = pantryList.SelectedItem as PantryItemModel;

            lblSuccess.Visibility = Visibility.Hidden;
            lblAddEditHeader.Content = "Edit the selected panty item";
            txtIngredientName.Text = itemToEdit.Ingredient.toString();
            txtIngredientDescription.Text = itemToEdit.Description;
            txtQuantity.Text = itemToEdit.Quantity.ToString();
            ddlMeasure.SelectedIndex = ddlMeasure.Items.IndexOf(itemToEdit.Measure);
            dpExpires.SelectedDate = itemToEdit.ExpirationDate;
        }

        //Domain Calls

        public List<PantryItemModel> getPantry()
        {
            IPantryManager manager = ManagerFactory.GetPantryManager();
            List<PantryItemModel> data = manager.GetPantryContents();
            foreach (PantryItemModel datum in data)
            {
                datum.StringQuantity = Tools.ToolBox.DecimalToFraction(datum.Quantity);
            }
            return data;
        }

        private bool SavePantryItem(bool isEdit)
        {   
            string description = "";
            if (!string.IsNullOrEmpty(txtIngredientDescription.Text))
            {
                description = txtIngredientDescription.Text.Trim();
            }
            DateTime? expires = dpExpires.SelectedDate;

            string name = "";

            if (txtIngredientName.SelectedIndex >= 0)
            {
                name = ((IngredientModel)txtIngredientName.SelectedItem).Name;
            }
            else
            {
                name = txtIngredientName.Text.Trim();
            }

            String measureName ="";
            if (ddlMeasure.SelectedIndex >= 0)
            {
                measureName = ((MeasureModel)ddlMeasure.SelectedItem).Name;
            }
            else
            {
                measureName = ddlMeasure.Text.Trim(); 
            }

            string quantstring = txtQuantity.Text.Trim();
            double quant = Tools.ToolBox.FractionToDecimal(quantstring);            

            if (name.Equals("") || string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) || quant <=(1.0/64.0))
                return false;
            //Instantiate pantry Item and send
            PantryItemModel item;
            if (itemToEdit == null)
            {
                item = new PantryItemModel();
            }
            else
            {
                item = itemToEdit;
            }
            item.IngredientId = getIngredientId(name);
            item.Quantity = quant;
            item.MeasureId = getMeasureID(measureName);
            item.Description = description;
            item.ExpirationDate = expires;

            IPantryManager manager = ManagerFactory.GetPantryManager();
            manager.SavePantryItem(item, isEdit);
            return true;
        }

        private List<IngredientModel> getIngredients()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.getIngredients();
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

        private List<MeasureModel> getMeasures()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.getMeasures();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button pantryItemBtn = (Button)sender;
            PantryItemModel pantryItemModel = (PantryItemModel)pantryItemBtn.DataContext;
            List<IngredientModel> ingredients = txtIngredientName.ItemsSource.Cast<IngredientModel>().ToList();
            Grid pantryItemGrid = (Grid)pantryItemBtn.Parent;
            StackPanel pantryItemPanel = (StackPanel)pantryItemGrid.Children[1];
            pantryItemPanel.Visibility = Visibility.Collapsed;
            StackPanel pantryEditItemPanel = (StackPanel)pantryItemGrid.Children[2];
            pantryEditItemPanel.Visibility = Visibility.Visible;
            TextBox quantityTextBox = (TextBox)pantryEditItemPanel.FindName("QuantityTextBox");
            quantityTextBox.Text = pantryItemModel.StringQuantity;
            ComboBox measureComboBox = (ComboBox)pantryEditItemPanel.FindName("MeasureComboBox");
            measureComboBox.ItemsSource = ddlMeasure.ItemsSource;
            measureComboBox.SelectedItem = pantryItemModel.Measure;
            ComboBox ingredientComboBox = (ComboBox)pantryEditItemPanel.FindName("IngredientComboBox");
            ingredientComboBox.ItemsSource = txtIngredientName.ItemsSource;
            ingredientComboBox.SelectedIndex = ingredients.FindIndex(x => x.ID == pantryItemModel.Ingredient.ID);
            TextBox descriptionTextBox = (TextBox)pantryEditItemPanel.FindName("DescriptionTextBox");
            descriptionTextBox.Text = pantryItemModel.Description;
            DatePicker expirationDatePicker = (DatePicker)pantryEditItemPanel.FindName("ExpirationDatePicker");
            expirationDatePicker.SelectedDate = pantryItemModel.ExpirationDate;
            //Console.Write(quantityTextBlock.Text);
        }

        private void FinishBtn_Click(object sender, RoutedEventArgs e)
        {
            Button finishEditBtn = (Button)sender;
            PantryItemModel pantryItemModel = (PantryItemModel)finishEditBtn.DataContext;
            StackPanel pantryEditItemPanel = (StackPanel)finishEditBtn.Parent;
            Grid pantryItemGrid = (Grid)pantryEditItemPanel.Parent;
            StackPanel pantryItemPanel = (StackPanel)pantryItemGrid.Children[1];

            TextBox quantityTextBox = (TextBox)pantryEditItemPanel.FindName("QuantityTextBox");
            pantryItemModel.StringQuantity = quantityTextBox.Text;
            ComboBox measureComboBox = (ComboBox)pantryEditItemPanel.FindName("MeasureComboBox");
            String measureName = ((MeasureModel)measureComboBox.SelectedItem).Name;
            pantryItemModel.MeasureId = getMeasureID(measureName);
            ComboBox ingredientComboBox = (ComboBox)pantryEditItemPanel.FindName("IngredientComboBox");
            String ingredientName = ((IngredientModel)ingredientComboBox.SelectedItem).Name;
            pantryItemModel.IngredientId = getIngredientId(ingredientName);
            TextBox descriptionTextBox = (TextBox)pantryEditItemPanel.FindName("DescriptionTextBox");
            pantryItemModel.Description = descriptionTextBox.Text;
            DatePicker expirationDatePicker = (DatePicker)pantryEditItemPanel.FindName("ExpirationDatePicker");
            pantryItemModel.ExpirationDate = expirationDatePicker.SelectedDate;
            IPantryManager manager = ManagerFactory.GetPantryManager();
            manager.SavePantryItem(pantryItemModel, true);

            initSources();

            pantryItemPanel.Visibility = Visibility.Visible;
            pantryEditItemPanel.Visibility = Visibility.Collapsed;
        }
    }
}
