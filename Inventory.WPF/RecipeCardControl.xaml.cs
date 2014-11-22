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
    public partial class RecipeCardControl : UserControl
    {

        public event RoutedEventHandler Click;

        public RecipeCardControl()
        {
            InitializeComponent();
        }

        void onButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
                this.Click(this, e);
        }
    }
}
