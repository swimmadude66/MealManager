﻿using Inventory.Models;
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

        public RecipeControl()
        {
            InitializeComponent();
            recipeItems = new List<TempRecipeItemModel>();
            newtags = new List<String>();
            initSources();
            this.DataContext = this;
        }

        private void initSources()
        {
            recipeGrid.ItemsSource = getRecipes();
            dgIngredients.ItemsSource = recipeItems;
            dgIngredients.Items.Refresh();
            txtIngredient.ItemsSource = getIngredients();
            txtIngredient.SelectedIndex = -1;
            txtIngredient.Text = "";
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

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            clearAddRecipe();
            listPanel.Visibility = Visibility.Hidden;
            addPanel.Visibility = Visibility.Visible;
        }

        private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            AddIngredient();
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

        private void AddIngredient()
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
            else if (Regex.IsMatch(quantstring, @"^([0-9]*\.?[0-9]+)/([0-9]+\.?[0-9]*)$"))
            {
                double a = double.Parse(quantstring.Substring(0, quantstring.IndexOf('/')));
                double b = double.Parse(quantstring.Substring(quantstring.IndexOf('/') + 1));
                quant = a / b;
            }

            if (string.IsNullOrEmpty(ingredient) || quant <= 0)
            {
                return;
            }
            TempRecipeItemModel model = new TempRecipeItemModel(ingredient, quant, measureName);
            recipeItems.Add(model);
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

       

       
       
    }
}
