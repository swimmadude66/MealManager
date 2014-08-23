using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Inventory.Data;
using Inventory.Data.Interfaces;
using Inventory.Managers.Factory;
using Inventory.Models;

namespace Inventory
{
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveIngredient();
        }


//Domain Calls

        private void SaveIngredient()
        {
            string name = txtIngredientName.Text.Trim();
            string description = "";
            if (!string.IsNullOrEmpty(txtIngredientDescription.Text))
            {
                description = txtIngredientDescription.Text.Trim();
            }

            IRecipeManager manager = ManagerFactory.GetRecipeManager();
            manager.SaveIngredient(name, description);
        }


    }
}
