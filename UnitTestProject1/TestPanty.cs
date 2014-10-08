using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory.Models;
using Inventory.Managers;
using Inventory.Managers.Factory;
using Inventory.Data.Interfaces;

namespace TestHarness
{
    [TestClass]
    public class TestPanty
    {
        [TestMethod]
        public void TestEditPantyItem()
        {
            IPantryManager manager = ManagerFactory.GetPantryManager();
            //save to put back to orginal state after test
            PantryItemModel originalItem = manager.GetPantryItemById(25);

            PantryItemModel item = manager.GetPantryItemById(25);
            item.Quantity++;
            item.Description = "Testing woo!";
            manager.SavePantryItem(item, true);
            PantryItemModel updatedItem = manager.GetPantryItemById(25);

            Assert.AreEqual<double>(item.Quantity, updatedItem.Quantity);
            Assert.AreEqual<String>(item.Description, updatedItem.Description);

            //cleanup
            manager.SavePantryItem(originalItem, true);
        }
    }
}
