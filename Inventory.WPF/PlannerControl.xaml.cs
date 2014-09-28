﻿using Inventory.Data.Interfaces;
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
        public List<RecipeModel> availableRecipes;
        private ILookup<DateTime, PlannerItemModel> PlannedRecipes;
        public List<Day> Days { get; set; }
        public List<string> DayNames { get; set; }
        public DateTime CurrentDate { get; set; }

        public PlannerControl()
        {
            InitializeComponent();
            availableRecipes = new List<RecipeModel>();
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
            BuildCalendar(DateTime.Today);
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
            PlannedRecipes = getPlannerItems(start, end).ToLookup(p => p.Date.Date);
            //PlannedRecipes = getPlansByDateRange(start, end);
            //Show 6 weeks each with 7 days = 42
            for (int box = 0; box < 42; box++)
            {
                Day day = new Day();
                day.Date = d;
                day.IsTargetMonth = (d.Month == DateTime.Today.Month);
                day.isToday = (d.Date == DateTime.Today.Date);
                day.Recipes = new List<RecipeModel>();
                if (PlannedRecipes.Contains(d.Date)){
                    foreach(PlannerItemModel m in PlannedRecipes[d].ToList()){
                        day.Recipes.Add(m.Recipe);
                    }
                }
                    
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
            return isValidMealDate(PlannerDatePicker.SelectedDate) && isValidRecipe((RecipeModel) RecipeCombo.SelectedItem);
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
            DateTime plannerItemDate = PlannerDatePicker.SelectedDate ?? System.DateTime.Today;
            RecipeModel plannerItemRecipe = (RecipeModel)RecipeCombo.SelectedItem;
            PlannerItemModel model = new PlannerItemModel();
            model.Date = plannerItemDate;
            model.Recipe = plannerItemRecipe;
            savePlan(model, false);
            initSources();
        }

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
    }

    public class Day
    {
        public bool IsTargetMonth { get; set; }
        public bool isToday { get; set; }
        public DateTime Date { get; set; }
        public List<RecipeModel> Recipes { get; set; }
    }
}
