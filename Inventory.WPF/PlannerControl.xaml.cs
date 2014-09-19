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
    /// Interaction logic for PlannerControl.xaml
    /// </summary>
    public partial class PlannerControl : UserControl
    {

        private List<PlannerItemModel> plannerItems;

        public PlannerControl()
        {
            InitializeComponent();
            this.DataContext = this;
            plannerItems = new List<PlannerItemModel>();
            initSources();
        }

        private void initSources()
        {
            RecipeCombo.ItemsSource = getRecipes();
            PlannerGrid.ItemsSource = getPlannerItems();
            PlannerGrid.Items.Refresh();
        }

        private void plannerCalendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            Console.Write(e.ToString());
        }

        private void recipeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.Write(e.ToString());
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            String plannerItemName = MealName.Text;
            //DateTime plannerItemDate = plannerCalendar.DisplayDate;
            RecipeModel plannerItemRecipe = (RecipeModel) RecipeCombo.SelectedItem;
            //TempPlannerItemModel tempPlannerItemModel = new TempPlannerItemModel(plannerItemName, plannerItemDate, plannerItemRecipes);
            PlannerItemModel plannerItemModel = new PlannerItemModel();
            plannerItemModel.Name = plannerItemName;
            //plannerItemModel.Date = plannerItemDate;
            plannerItemModel.Recipe = plannerItemRecipe;
            plannerItems.Add(plannerItemModel);
            //initSources();
        }

        //Domain Calls

        private List<RecipeModel> getRecipes()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.getRecipes();
        }

        private List<PlannerItemModel> getPlannerItems()
        {
            //IRecipeManager manager = ManagerFactory.GetRecipeManager();
            //return manager.getRecipes();
            return plannerItems;
        }

        
    }
}
