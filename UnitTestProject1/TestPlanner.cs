using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory.Data.Interfaces;
using Inventory.Managers.Factory;
using Inventory.Models;
using System.Collections.Generic;
using Inventory.WPF;

namespace TestHarness
{
    [TestClass]
    public class TestPlanner
    {

        public PlannerControl plannerControl;

        [Ignore]
        public void TestPastMealDate()
        {
            plannerControl = new PlannerControl();
            plannerControl.InitializeComponent();
            DateTime dateTime = new DateTime(0);
            Assert.IsFalse(plannerControl.isValidMealDate(dateTime));
        }

        [Ignore]
        public void TestNullMealDate()
        {
            plannerControl = new PlannerControl();
            plannerControl.InitializeComponent();
            DateTime? dateTime = null;
            Assert.IsFalse(plannerControl.isValidMealDate(dateTime));
        }
        
        [TestMethod]
        public void TestGettingPlannedRecipes()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            List<DateTime?> starts = new List<DateTime?>(){ null, new DateTime(DateTime.Today.Year - 5, DateTime.Today.Month, 1) };
            List<DateTime?> ends = new List<DateTime?>() { null, new DateTime(DateTime.Today.Year + 5, DateTime.Today.Month, 1) };
            List<PlannerItemModel> results;
            foreach (DateTime? start in starts)
            {
                foreach (DateTime? end in ends)
                {
                    results = manager.GetPlannedRecipes(start, end);
                    Assert.IsTrue(results.Count > 0);
                }
            }           
        }

        [TestMethod]
        public void TestCancel()
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            PlannerItemModel plan = new PlannerItemModel();
            DateTime testdate = DateTime.Parse("01/01/1970");
            plan.Date = testdate;
            plan.Recipe = manager.getRecipes(1, 0, false)[0];
            int id = manager.PlanRecipe(plan, false);
            List<String> results = new List<String>();
            List<PlannerItemModel> planned = manager.GetPlannedRecipes(testdate, testdate);
            foreach(PlannerItemModel rec in planned){
                results.Add(rec.Recipe.Name);
            }
            Assert.IsTrue(results.Contains(plan.Recipe.Name));
            manager.cancelPlan(id);
            results = new List<String>();
            foreach (PlannerItemModel rec in manager.GetPlannedRecipes(testdate, testdate))
            {
                results.Add(rec.Recipe.Name);
            }
            Assert.IsFalse(results.Contains(plan.Recipe.Name));
        }

    }
}
