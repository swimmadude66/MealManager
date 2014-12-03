using Inventory.Data.Interfaces;
using Inventory.Managers.Factory;
using Inventory.Models;
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

namespace Inventory.WPF
{
    /// <summary>
    /// Interaction logic for ShoppingListControl.xaml
    /// </summary>
    public partial class ShoppingListControl : UserControl
    {
        private List<IngredientModel> shoppingList;

        public ShoppingListControl()
        {
            InitializeComponent();
            dpfrom.SelectedDate = DateTime.Today;
            dpuntil.SelectedDate = DateTime.Today.AddDays(7);
            DoComparison();
        }

        public void DoComparison()
        { 
            DateTime? fromDate = dpfrom.SelectedDate;
            DateTime? untilDate = dpuntil.SelectedDate;
            shoppingList = getShoppingList(fromDate, untilDate);
            shoppingListCtl.ItemsSource = shoppingList;
        }

        private void btn_compare_clicked(object sender, RoutedEventArgs e)
        {
            DoComparison();
        }

        //Domain Calls
        public List<IngredientModel> getShoppingList(DateTime? begin, DateTime? end)
        {
            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            return manager.GenerateShoppingList(begin, end);
        }

        
        
    }
}
