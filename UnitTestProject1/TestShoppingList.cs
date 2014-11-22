using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory.Models;
using Inventory.Data.Interfaces;
using Inventory.Managers.Factory;

namespace TestHarness
{
    /// <summary>
    /// Summary description for TestShoppingList
    /// </summary>
    [TestClass]
    public class TestShoppingList
    {
        [TestMethod]
        public void TestGenerateShoppingList()
        {
            List<IngredientModel> shoppingList = new List<IngredientModel>();
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            shoppingList = manager.GenerateShoppingList(null, null);
            Assert.IsNotNull(shoppingList);
            DateTime start = new DateTime(1970, 1, 1);
            DateTime end = DateTime.Today;
            shoppingList = manager.GenerateShoppingList(start, null);
            Assert.IsNotNull(shoppingList);
            shoppingList = manager.GenerateShoppingList(null, end);
            Assert.IsNotNull(shoppingList);
            shoppingList = manager.GenerateShoppingList(start, end);
            Assert.IsNotNull(shoppingList);
        }
    }
}
