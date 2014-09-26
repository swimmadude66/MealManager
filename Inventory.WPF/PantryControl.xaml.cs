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
        public PantryControl()
        {
            InitializeComponent();
            this.DataContext = this;
            initSources();
        }

        private void initSources()
        {
            pantryGrid.ItemsSource = getPantry();
            ddlMeasure.ItemsSource = getMeasures();
            txtIngredientName.ItemsSource = getIngredients();
            txtIngredientName.SelectedIndex = -1;
            ddlMeasure.SelectedIndex = 0;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtIngredientName.Text))
            {
                SavePantryItem();
                txtIngredientName.Text = "";
                txtIngredientDescription.Text = "";
                dpExpires.SelectedDate = null;
                lblSuccess.Visibility = Visibility.Visible;
                initSources();
            }          
        }

        private void editRow(object sender, MouseButtonEventArgs e)
        {
            Console.Write("You found a secret");

            DataGrid pantryList = sender as DataGrid;
            PantryItemModel itemToEdit = pantryList.SelectedItem as PantryItemModel;

            //Do things with this model.
            //Maybe populate lower list
            //Or highlight row and allow edit
            //Think on it

            //Change save method args for manager to model and isedit flag

            //Ask adam for a save runthrough
            //Also ask him why his code sucks

        }

        //Domain Calls

         public List<PantryItemModel> getPantry()
        {
            IPantryManager manager = ManagerFactory.GetPantryManager();
            return manager.GetPantryContents();
        }

        private void SavePantryItem()
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

            double quant = -1.0;
            string quantstring = txtQuantity.Text.Trim();
            if (Regex.IsMatch(quantstring,@"^[0-9\.]+$"))
            {
                quant = Double.Parse(quantstring);
            }
            else if (Regex.IsMatch(quantstring, @"^([0-9]*\.?[0-9]+)/([0-9]+\.?[0-9]*)$"))
            {
                double a = double.Parse(quantstring.Substring(0, quantstring.IndexOf('/')));
                double b = double.Parse(quantstring.Substring(quantstring.IndexOf('/') + 1));
                quant = a / b;
            }

            if (name.Equals("") || string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) || quant <=0)
                return;
            //Instantiate pantry Item and send
            PantryItemModel item = new PantryItemModel();

            item.IngredientId = getIngredientId(name);
            item.Quantity = quant;
            item.MeasureId = getMeasureID(measureName);
            item.Description = description;
            item.ExpirationDate = expires;

            IPantryManager manager = ManagerFactory.GetPantryManager();
            manager.SavePantryItem(item, false);
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
    }
}
