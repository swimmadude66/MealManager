namespace Inventory
{
    partial class Inventory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtIngredientName = new System.Windows.Forms.TextBox();
            this.lblIngredientName = new System.Windows.Forms.Label();
            this.lblIngredientDescription = new System.Windows.Forms.Label();
            this.txtIngredientDescription = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.63989F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.36011F));
            this.tableLayoutPanel1.Controls.Add(this.txtIngredientName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblIngredientName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblIngredientDescription, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtIngredientDescription, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 67);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.49425F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.50574F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(438, 248);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtIngredientName
            // 
            this.txtIngredientName.Location = new System.Drawing.Point(132, 3);
            this.txtIngredientName.Name = "txtIngredientName";
            this.txtIngredientName.Size = new System.Drawing.Size(100, 20);
            this.txtIngredientName.TabIndex = 0;
            // 
            // lblIngredientName
            // 
            this.lblIngredientName.AutoSize = true;
            this.lblIngredientName.Location = new System.Drawing.Point(3, 0);
            this.lblIngredientName.Name = "lblIngredientName";
            this.lblIngredientName.Size = new System.Drawing.Size(85, 13);
            this.lblIngredientName.TabIndex = 1;
            this.lblIngredientName.Text = "Ingredient Name";
            // 
            // lblIngredientDescription
            // 
            this.lblIngredientDescription.AutoSize = true;
            this.lblIngredientDescription.Location = new System.Drawing.Point(3, 28);
            this.lblIngredientDescription.Name = "lblIngredientDescription";
            this.lblIngredientDescription.Size = new System.Drawing.Size(60, 13);
            this.lblIngredientDescription.TabIndex = 2;
            this.lblIngredientDescription.Text = "Description";
            // 
            // txtIngredientDescription
            // 
            this.txtIngredientDescription.CausesValidation = false;
            this.txtIngredientDescription.Location = new System.Drawing.Point(132, 31);
            this.txtIngredientDescription.MaximumSize = new System.Drawing.Size(300, 200);
            this.txtIngredientDescription.MinimumSize = new System.Drawing.Size(300, 200);
            this.txtIngredientDescription.Multiline = true;
            this.txtIngredientDescription.Name = "txtIngredientDescription";
            this.txtIngredientDescription.Size = new System.Drawing.Size(300, 200);
            this.txtIngredientDescription.TabIndex = 3;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(161, 321);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(138, 9);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(197, 31);
            this.Title.TabIndex = 2;
            this.Title.Text = "New Ingredient";
            // 
            // Inventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 389);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Inventory";
            this.Text = "Inventory";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtIngredientName;
        private System.Windows.Forms.Label lblIngredientName;
        private System.Windows.Forms.Label lblIngredientDescription;
        private System.Windows.Forms.TextBox txtIngredientDescription;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label Title;
    }
}

