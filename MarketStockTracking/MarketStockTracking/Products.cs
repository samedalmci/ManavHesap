using System;
using System.Data;
using System.Windows.Forms;
using MarketStockTracking.Repositories;
using Microsoft.Data.SqlClient;
using MarketStockTracking.Models;


namespace MarketStockTracking
{
    public partial class Products : Form
    {
        private readonly IProductRepository _productRepository;          

        public Products()
        {
            InitializeComponent();

            _productRepository = new SqlProductRepository(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;"
            );

            txtUrunCesidi.DropDownStyle = ComboBoxStyle.DropDownList;
            txtUrunCesidi.Items.Clear();
            txtUrunCesidi.Items.Add("Seçiniz...");
            txtUrunCesidi.Items.Add("Sebze");
            txtUrunCesidi.Items.Add("Meyve");
            txtUrunCesidi.SelectedIndex = 0;

            ListeleUrunler(); 
        }
        private void button1_Click(object sender, EventArgs e)
        {
            btnEkle_Click(sender, e);
        }

        private void ListeleUrunler()
        {
            var products = _productRepository.GetAll();
            dgvUrunler.DataSource = products;

            // Sütun başlıklarını Türkçe yapalım
            foreach (DataGridViewColumn col in dgvUrunler.Columns)
            {
                switch (col.Name)
                {
                    case "ProductName":
                        col.HeaderText = "Ürün Adı";
                        break;
                    case "ProductType":
                        col.HeaderText = "Ürün Çeşidi";
                        break;
                    case "Quantity":
                        col.HeaderText = "Adet";
                        col.DefaultCellStyle.Format = "N2"; // Sayıyı formatlamak için
                        break;
                    case "StoreName":
                        col.HeaderText = "Mağaza";
                        break;
                    case "AddedDate":
                        col.HeaderText = "Eklenme Tarihi";
                        col.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm"; // Tarih formatı
                        break;
                }
            }
        }


        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrunAdi.Text))
            {
                MessageBox.Show("Lütfen ürün adı girin.");
                return;
            }

            if (txtUrunCesidi.SelectedIndex == 0)
            {
                MessageBox.Show("Lütfen ürün çeşidi seçin.");
                return;
            }

            var product = new Product
            {
                ProductName = txtUrunAdi.Text.Trim(),
                ProductType = txtUrunCesidi.Text,
                AddedDate = DateTime.Now
            };

            int result = _productRepository.Add(product);

            if (result > 0)
            {
                MessageBox.Show("Ürün başarıyla eklendi!");

                txtUrunAdi.Clear();
                txtUrunCesidi.SelectedIndex = 0;

                ListeleUrunler();
            }
        }
    }
}