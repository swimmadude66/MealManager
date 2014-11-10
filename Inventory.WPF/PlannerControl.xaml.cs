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
using System.Collections.ObjectModel;



namespace Inventory.WPF
{
    /// <summary>
    /// Interaction logic for PlannerControl.xaml
    /// </summary>
    public partial class PlannerControl : UserControl
    {
        private ILookup<DateTime, PlannerItemModel> PlannedRecipes;
        public ObservableCollection<Day> Days { get; set; }
        public ObservableCollection<string> DayNames { get; set; }
        public int numWeeks { get; set; }
        private String[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private String[] SunStart = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        public PlannerControl()
        {
            InitializeComponent();
            //RecipeCombo.ItemsSource = getRecipes();
            DayNames = new ObservableCollection<string>();
            Days = new ObservableCollection<Day>();
            numWeeks = 6;
            if (numWeeks > 1){
                DayNames.Clear();
                foreach(String day in SunStart)
                {
                    DayNames.Add(day);
                }
            }
            else
            {
                DayNames.Clear();
                DateTime d = DateTime.Today;
                for (int i = 0; i < 7; i++)
                {
                    DayNames.Add(d.DayOfWeek.ToString());
                    d = d.AddDays(1);
                }
            }
            this.DataContext = this;
            initSources();
        }

        private void initSources()
        {
            //PlannerDatePicker.SelectedDate = DateTime.Today;
            //RecipeCombo.SelectedIndex = -1;
            BuildCalendar(DateTime.Today, true);
        }

        public void BuildCalendar(DateTime targetDate, bool newStuff)
        {
            Days.Clear();
            //Calculate when the first day of the month is and work out an 
            //offset so we can fill in any boxes before that.
            DateTime d;
            if (numWeeks > 1) {
                d = new DateTime(targetDate.Year, targetDate.Month, 1);
                int offset = DayOfWeekNumber(d.DayOfWeek);
                if (offset != 1) d = d.AddDays(-offset);
            }
            else
            {
                d = DateTime.Today;
            }
            DateTime start = d;
            DateTime end = start.AddDays(numWeeks*7);
            if (newStuff)
            {
                PlannedRecipes = getPlannerItems(start, end).ToLookup(p => p.Date.Date);
            }
            for (int box = 0; box < numWeeks*7; box++)
            {
                Day day = new Day();
                day.Date = d;
                day.IsTargetMonth = (d.Month == DateTime.Today.Month);
                day.isToday = (d.Date == DateTime.Today.Date);
                day.MonthName = months[d.Month - 1];
                day.PlannedRecipes = new ObservableCollection<PlannerItemModel>();
                if (PlannedRecipes.Contains(d.Date)){
                    foreach(PlannerItemModel m in PlannedRecipes[d].ToList()){
                        day.PlannedRecipes.Add(m);
                    }
                }
                Days.Add(day);
                d = d.AddDays(1);
            }
        }

        private void changeView()
        {
            bool newstuff;
            if (numWeeks > 1)
            {
                DayNames.Clear();
                foreach (String day in SunStart)
                {
                    DayNames.Add(day);
                }
            }
            else
            {
                DayNames.Clear();
                DateTime d = DateTime.Today;
                for (int i = 0; i < 7; i++)
                {
                    DayNames.Add(d.DayOfWeek.ToString());
                    d = d.AddDays(1);
                }
            }
            BuildCalendar(DateTime.Today, false);
        }

        private static int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }

        //public void btnSubmit_Click(object sender, RoutedEventArgs e)
        //{
        //    if (isValidMealDate(PlannerDatePicker.SelectedDate) && RecipeCombo.SelectedIndex >=0)
        //        planRecipe();
        //}

        private void BtnChangeView_Click(object sender, RoutedEventArgs e)
        {
            if (numWeeks > 1)
                numWeeks = 1;
            else
                numWeeks = 6;
            changeView();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            PlannerItemModel plannedRecipe = (PlannerItemModel)button.DataContext;
            cancelPlan(plannedRecipe.ID);
            BuildCalendar(DateTime.Today, true);
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

        //private void planRecipe()
        //{
        //    DateTime plannerItemDate = (DateTime)PlannerDatePicker.SelectedDate;
        //    RecipeModel plannerItemRecipe = (RecipeModel)RecipeCombo.SelectedItem;
        //    PlannerItemModel model = new PlannerItemModel();
        //    model.Date = plannerItemDate;
        //    model.Recipe = plannerItemRecipe;
        //    savePlan(model, false);
        //    initSources();
        //}

        //Domain Calls

        private List<RecipeModel> getRecipes()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.getRecipes();
        }

        private List<PlannerItemModel> getPlannerItems(DateTime? start, DateTime? end)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.GetPlannedRecipes(start, end);
        }

        private int savePlan(PlannerItemModel model, bool isEdit)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.PlanRecipe(model, isEdit);
        }

        private void cancelPlan(int id){
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            manager.cancelPlan(id);
        }
    }

    public class Day
    {
        public bool IsTargetMonth { get; set; }
        public bool isToday { get; set; }
        public DateTime Date { get; set; }
        public String MonthName { get; set; }
        public ObservableCollection<PlannerItemModel> PlannedRecipes { get; set; }
    }
}
