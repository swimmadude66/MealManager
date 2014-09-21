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

        private List<PlannerItemModel> allPlannerItems;
        private List<PlannerItemModel> selectedPlannerItems;
        private DateTime tempDateTime;

        public PlannerControl()
        {
            InitializeComponent();
            this.DataContext = this;
            allPlannerItems = new List<PlannerItemModel>();
            selectedPlannerItems = new List<PlannerItemModel>();
            plannerCalendar.SelectedDate = DateTime.Today;
            //plannerCalendar.CalendarDayButtonStyle;
            initSources();
            
        }

        private void initSources()
        {
            RecipeCombo.ItemsSource = getRecipes();
            PlannerGrid.ItemsSource = getPlannerItems();
            PlannerGrid.Items.Refresh();
        }

        private void plannerCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            updateRecipesByDate();
            //Console.WriteLine("Something at all");
            //Console.WriteLine(plannerCalendar.DisplayDate);
        }

        private void recipeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.Write(e.ToString());
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            saveRecipe();
        }

        private void Insert_Recipe_Click(object sender, RoutedEventArgs e)
        {
            tempDateTime = plannerCalendar.SelectedDate ?? System.DateTime.Today;
            tempDateTime = tempDateTime.Date;
            //Console.WriteLine("Before change " + tempDateTime);
            MainPlannerGrid.Visibility = Visibility.Collapsed;
            PlannerItemGrid.Visibility = Visibility.Visible;
            //Console.WriteLine("After change " + tempDateTime);
        }

        private void Edit_Recipe_Click(object sender, RoutedEventArgs e)
        {

        }


        private void updateRecipesByDate()
        {
            if (allPlannerItems != null && allPlannerItems.Count > 0)
            {
                tempDateTime = plannerCalendar.SelectedDate ?? default(DateTime);
                selectedPlannerItems = allPlannerItems.FindAll(x => x.Date.CompareTo(plannerCalendar.SelectedDate) == 0);
                initSources();
            }
        }

        private void saveRecipe()
        {
            String plannerItemName = MealName.Text;
            DateTime plannerItemDate = tempDateTime;
            RecipeModel plannerItemRecipe = (RecipeModel)RecipeCombo.SelectedItem;
            //TempPlannerItemModel tempPlannerItemModel = new TempPlannerItemModel(plannerItemName, plannerItemDate, plannerItemRecipes);
            PlannerItemModel plannerItemModel = new PlannerItemModel();
            plannerItemModel.Name = plannerItemName;
            plannerItemModel.Date = plannerItemDate;
            plannerItemModel.Recipe = plannerItemRecipe;
            allPlannerItems.Add(plannerItemModel);

            //Console.WriteLine("Here's our date" + plannerItemDate);

            MainPlannerGrid.Visibility = Visibility.Visible;
            PlannerItemGrid.Visibility = Visibility.Collapsed;

            initSources();
            updateRecipesByDate();
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
            return selectedPlannerItems;
        }

        
        
    }
}
