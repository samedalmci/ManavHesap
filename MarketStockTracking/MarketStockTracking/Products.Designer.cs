namespace MarketStockTracking
{
    partial class Products
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
            txtUrunAdi = new TextBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            txtUrunCesidi = new ComboBox();
            button1 = new Button();
            dgvUrunler = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvUrunler).BeginInit();
            SuspendLayout();
            // 
            // txtUrunAdi
            // 
            txtUrunAdi.Location = new Point(47, 47);
            txtUrunAdi.Name = "txtUrunAdi";
            txtUrunAdi.Size = new Size(161, 23);
            txtUrunAdi.TabIndex = 2;
            txtUrunAdi.Text = "Ürün İsmi\r\n";
            txtUrunAdi.TextChanged += txtUrunAdi_TextChanged;
            // 
            // txtUrunCesidi
            // 
            txtUrunCesidi.FormattingEnabled = true;
            txtUrunCesidi.Items.AddRange(new object[] { "Meyve", "Sebze" });
            txtUrunCesidi.Location = new Point(237, 47);
            txtUrunCesidi.Name = "txtUrunCesidi";
            txtUrunCesidi.Size = new Size(121, 23);
            txtUrunCesidi.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(383, 47);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dgvUrunler
            // 
            dgvUrunler.AllowUserToAddRows = false;
            dgvUrunler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUrunler.Location = new Point(47, 213);
            dgvUrunler.Name = "dgvUrunler";
            dgvUrunler.ReadOnly = true;
            dgvUrunler.Size = new Size(411, 164);
            dgvUrunler.TabIndex = 6;
            // 
            // Products
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(503, 411);
            Controls.Add(dgvUrunler);
            Controls.Add(button1);
            Controls.Add(txtUrunCesidi);
            Controls.Add(txtUrunAdi);
            Name = "Products";
            Text = "Product";
            ((System.ComponentModel.ISupportInitialize)dgvUrunler).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtUrunAdi;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ComboBox txtUrunCesidi;
        private Button button1;
        private DataGridView dgvUrunler;
    }
}