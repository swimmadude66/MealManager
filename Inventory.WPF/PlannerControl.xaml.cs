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
        //private List<Appointment> _myAppointmentsList;
        public List<PlannerItemModel> allPlannerItems;
        public List<RecipeModel> availableRecipes;
        public List<Appointment> plannerAppointments;

        public PlannerControl()
        {
            InitializeComponent();
            allPlannerItems = new List<PlannerItemModel>();
            availableRecipes = new List<RecipeModel>();
            plannerAppointments = new List<Appointment>();
            PlannerDatePicker.SelectedDate = DateTime.Today;
            initSources();
            this.DataContext = this;
        }

        private void initSources()
        {
            availableRecipes = getRecipes();
            RecipeCombo.ItemsSource = availableRecipes;
            SetAppointments();
        }

        private void SetAppointments()
        {
            //-- Use whatever function you want to load the MonthAppointments list, I happen to have a list filled by linq that has
            //   many (possibly the past several years) of them loaded, so i filter to only pass the ones showing up in the displayed
            //   month.  Note that the "setter" for MonthAppointments also triggers a redraw of the display.
            //AptCalendar.MonthAppointments = _myAppointmentsList.FindAll(new System.Predicate<Appointment>((Appointment apt) => apt.StartTime != null && Convert.ToDateTime(apt.StartTime).Month == this.AptCalendar.DisplayStartDate.Month && Convert.ToDateTime(apt.StartTime).Year == this.AptCalendar.DisplayStartDate.Year));
            plannerAppointments.Clear();

            foreach (PlannerItemModel plannerItemModel in allPlannerItems)
            {
                Appointment appointment = new Appointment();
                appointment.StartTime = plannerItemModel.Date;
                appointment.EndTime = appointment.StartTime;
                appointment.Subject = plannerItemModel.Name;
                //appointment.AppointmentID = plannerItemModel.ID;
                appointment.Details = plannerItemModel.Recipe.Name;
                plannerAppointments.Add(appointment);
            }
            AptCalendar.MonthAppointments = plannerAppointments;
        }

        public void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if(isValidInput())
                saveRecipe();
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

        private void saveRecipe()
        {
            String plannerItemName = MealName.Text.Trim();
            DateTime plannerItemDate = PlannerDatePicker.SelectedDate ?? System.DateTime.Today;
            RecipeModel plannerItemRecipe = (RecipeModel)RecipeCombo.SelectedItem;
            PlannerItemModel plannerItemModel = new PlannerItemModel();
            plannerItemModel.Name = plannerItemName;
            plannerItemModel.Date = plannerItemDate;
            plannerItemModel.Recipe = plannerItemRecipe;
            allPlannerItems.Add(plannerItemModel);

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
}
