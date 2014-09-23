﻿using System;
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

        [TestMethod]
        public void TestEmptyMealName()
        {   
            plannerControl = new PlannerControl();
            plannerControl.InitializeComponent();
            Assert.IsFalse(plannerControl.isValidMealName(""));
        }

        [TestMethod]
        public void TestGarbageMealName()
        {
            plannerControl = new PlannerControl();
            plannerControl.InitializeComponent();
            Assert.IsFalse(plannerControl.isValidMealName("#)(*$"));
        }

        [TestMethod]
        public void TestPastMealDate()
        {
            plannerControl = new PlannerControl();
            plannerControl.InitializeComponent();
            DateTime dateTime = new DateTime(0);
            Assert.IsFalse(plannerControl.isValidMealDate(dateTime));
        }

        [TestMethod]
        public void TestNullMealDate()
        {
            plannerControl = new PlannerControl();
            plannerControl.InitializeComponent();
            DateTime? dateTime = null;
            Assert.IsFalse(plannerControl.isValidMealDate(dateTime));
        }

        [TestMethod]
        public void TestExistingMealRecipe()
        {
            plannerControl = new PlannerControl();
            plannerControl.InitializeComponent();
            RecipeModel recipeModel = new RecipeModel();
            recipeModel.Name = "A really cool recipe that doesn't exist";
            Assert.IsFalse(plannerControl.isValidRecipe(recipeModel));
        }
    }
}