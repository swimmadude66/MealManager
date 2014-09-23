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

        private List<PlannerItemModel> allPlannerItems;
        private List<PlannerItemModel> selectedPlannerItems;
        private List<Appointment> _myAppointmentsList;
        private DateTime tempDateTime;


        public PlannerControl()
        {
            InitializeComponent();
            allPlannerItems = new List<PlannerItemModel>();
            selectedPlannerItems = new List<PlannerItemModel>();
            _myAppointmentsList = new List<Appointment>();
            PlannerDatePicker.SelectedDate = DateTime.Today;
            Loaded += Planner_Loaded;
            initSources();
            this.DataContext = this;
            
        }

        private void initSources()
        {
            RecipeCombo.ItemsSource = getRecipes();
            //PlannerGrid.ItemsSource = getPlannerItems();
            //PlannerGrid.Items.Refresh();
        }

        private void SetAppointments()
        {
            //-- Use whatever function you want to load the MonthAppointments list, I happen to have a list filled by linq that has
            //   many (possibly the past several years) of them loaded, so i filter to only pass the ones showing up in the displayed
            //   month.  Note that the "setter" for MonthAppointments also triggers a redraw of the display.
            AptCalendar.MonthAppointments = _myAppointmentsList.FindAll(new System.Predicate<Appointment>((Appointment apt) => apt.StartTime != null && Convert.ToDateTime(apt.StartTime).Month == this.AptCalendar.DisplayStartDate.Month && Convert.ToDateTime(apt.StartTime).Year == this.AptCalendar.DisplayStartDate.Year));
        }

        private void Planner_Loaded(Object sender, EventArgs eventArgs){
            Random rand = new Random(System.DateTime.Now.Second);

            for (int i = 1; i <= 50; i++)
            {
                Appointment apt = new Appointment();
                apt.AppointmentID = i;
                apt.StartTime = new System.DateTime(System.DateTime.Now.Year, rand.Next(1, 12), rand.Next(1, System.DateTime.DaysInMonth(System.DateTime.Now.Year, System.DateTime.Now.Month)));
                apt.EndTime = apt.StartTime;
                apt.Subject = "Random apt, blah blah";
                _myAppointmentsList.Add(apt);
            }

            SetAppointments();
        }

        //private void plannerCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    updateRecipesByDate();
        //    //Console.WriteLine("Something at all");
        //    //Console.WriteLine(plannerCalendar.DisplayDate);
        //}

        //private void recipeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Console.Write(e.ToString());
        //}

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            saveRecipe();
        }

        private void updateRecipesByDate()
        {
            if (allPlannerItems != null && allPlannerItems.Count > 0)
            {
                tempDateTime = PlannerDatePicker.SelectedDate ?? default(DateTime);
                selectedPlannerItems = allPlannerItems.FindAll(x => x.Date.CompareTo(PlannerDatePicker.SelectedDate) == 0);
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

            //MainPlannerGrid.Visibility = Visibility.Visible;
            //PlannerItemGrid.Visibility = Visibility.Collapsed;

            initSources();
            //updateRecipesByDate();
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
