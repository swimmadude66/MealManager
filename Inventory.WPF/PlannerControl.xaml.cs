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
using MonthCalendar;



namespace Inventory.WPF
{
    /// <summary>
    /// Interaction logic for PlannerControl.xaml
    /// </summary>
    public partial class PlannerControl : UserControl
    {
        public List<PlannerItemModel> allPlannerItems;
        public List<RecipeModel> availableRecipes;
        private Dictionary<DateTime, List<RecipeModel>> PlannedRecipes;
        public List<Day> Days { get; set; }
        public List<string> DayNames { get; set; }
        public DateTime CurrentDate { get; set; }

        public PlannerControl()
        {
            InitializeComponent();
            allPlannerItems = new List<PlannerItemModel>();
            availableRecipes = new List<RecipeModel>();
            PlannedRecipes = new Dictionary<DateTime, List<RecipeModel>>();
            this.DataContext = this;
            initSources();
        }

        private void initSources()
        {
            PlannerDatePicker.SelectedDate = DateTime.Today;
            availableRecipes = getRecipes();
            RecipeCombo.ItemsSource = availableRecipes;
            CurrentDate = DateTime.Today;
            DayNames = new List<string> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            Days = new List<Day>();
            PlannedRecipes.Add(DateTime.Today, new List<RecipeModel> { new RecipeModel() { Name = "Testing" }, new RecipeModel(){Name = "List Test"} });
            BuildCalendar(DateTime.Today);
            //SetAppointments();
        }

        public void BuildCalendar(DateTime targetDate)
        {
            Days.Clear();

            //Calculate when the first day of the month is and work out an 
            //offset so we can fill in any boxes before that.
            DateTime d = new DateTime(targetDate.Year, targetDate.Month, 1);
            int offset = DayOfWeekNumber(d.DayOfWeek);
            if (offset != 1) d = d.AddDays(-offset);
            DateTime start = d;
            DateTime end = start.AddDays(42);
            //PlannedRecipes = getPlansByDateRange(start, end);
            //Show 6 weeks each with 7 days = 42
            for (int box = 0; box < 42; box++)
            {
                Day day = new Day();
                day.Date = d;
                day.IsTargetMonth = (d.Month == DateTime.Today.Month);
                List<RecipeModel> plans;
                if(PlannedRecipes.TryGetValue(d, out plans))
                    day.Recipes = plans;
                else
                    day.Recipes = new List<RecipeModel>();
                Days.Add(day);
                d = d.AddDays(1);
            }
        }

        private static int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }

        public void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if(isValidInput())
                planRecipe();
        }

        public bool isValidInput()
        {
            return isValidMealName(MealName.Text) && isValidMealDate(PlannerDatePicker.SelectedDate) && isValidRecipe((RecipeModel) RecipeCombo.SelectedItem);
        }

        public bool isValidMealName(String mealName)
        {
            return Regex.IsMatch(mealName, @"[a-zA-Z0-9]");
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

        public bool isValidRecipe(RecipeModel mealRecipe)
        {
            if (availableRecipes.Contains(mealRecipe))
            {
                return true;
            }
            return false;
        }

        private void planRecipe()
        {
            String plannerItemName = MealName.Text.Trim();
            DateTime plannerItemDate = PlannerDatePicker.SelectedDate ?? System.DateTime.Today;
            RecipeModel plannerItemRecipe = (RecipeModel)RecipeCombo.SelectedItem;
            PlannerItemModel plannerItemModel = new PlannerItemModel();
            plannerItemModel.Name = plannerItemName;
            plannerItemModel.Date = plannerItemDate;
            plannerItemModel.Recipe = plannerItemRecipe;
            allPlannerItems.Add(plannerItemModel);
            //savePlan();
            initSources();
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
            return allPlannerItems;
        }     
    }

    public class Day
    {
        public bool IsTargetMonth { get; set; }
        public DateTime Date { get; set; }
        public List<RecipeModel> Recipes { get; set; }
    }
}
