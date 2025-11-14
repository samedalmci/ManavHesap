namespace MarketStockTracking
{
    partial class Stores
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
            dgvMagazalar = new DataGridView();
            btnEkle = new Button();
            txtMagazaAdi = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvMagazalar).BeginInit();
            SuspendLayout();
            // 
            // dgvMagazalar
            // 
            dgvMagazalar.AllowUserToAddRows = false;
            dgvMagazalar.AllowUserToDeleteRows = false;
            dgvMagazalar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMagazalar.Location = new Point(44, 228);
            dgvMagazalar.Name = "dgvMagazalar";
            dgvMagazalar.ReadOnly = true;
            dgvMagazalar.Size = new Size(411, 164);
            dgvMagazalar.TabIndex = 10;
            // 
            // btnEkle
            // 
            btnEkle.Location = new Point(380, 62);
            btnEkle.Name = "btnEkle";
            btnEkle.Size = new Size(75, 23);
            btnEkle.TabIndex = 9;
            btnEkle.Text = "button1";
            btnEkle.UseVisualStyleBackColor = true;
            btnEkle.Click += btnEkle_Click_1;
            // 
            // txtMagazaAdi
            // 
            txtMagazaAdi.Location = new Point(44, 62);
            txtMagazaAdi.Name = "txtMagazaAdi";
            txtMagazaAdi.Size = new Size(161, 23);
            txtMagazaAdi.TabIndex = 7;
            txtMagazaAdi.Text = "Ürün İsmi\r\n";
            // 
            // Stores
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 450);
            Controls.Add(dgvMagazalar);
            Controls.Add(btnEkle);
            Controls.Add(txtMagazaAdi);
            Name = "Stores";
            Text = "Stores";
            Load += Stores_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMagazalar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvMagazalar;
        private Button btnEkle;
        private TextBox txtMagazaAdi;
    }
}