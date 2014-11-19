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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HomeBtn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pantryCtl.Visibility = Visibility.Collapsed;
            recipeCtl.Visibility = Visibility.Collapsed;
            plannerCtl.Visibility = Visibility.Collapsed;
        }

        private void PantryBtn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pantryCtl.Visibility = Visibility.Visible;
            recipeCtl.Visibility = Visibility.Collapsed;
            plannerCtl.Visibility = Visibility.Collapsed;
        }

        private void RecipesBtn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pantryCtl.Visibility = Visibility.Collapsed;
            recipeCtl.Visibility = Visibility.Visible;
            plannerCtl.Visibility = Visibility.Collapsed;
        }

        private void PlannerBtn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pantryCtl.Visibility = Visibility.Collapsed;
            recipeCtl.Visibility = Visibility.Collapsed;
            plannerCtl.Visibility = Visibility.Visible;
        }

        private void RandomBtn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pantryCtl.Visibility = Visibility.Collapsed;
            recipeCtl.Visibility = Visibility.Collapsed;
            plannerCtl.Visibility = Visibility.Collapsed;
        }
    }
}
