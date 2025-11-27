using System;
using System.Windows.Forms;
using MarketStockTracking.Models;
using MarketStockTracking.Repositories;

namespace MarketStockTracking
{
    public partial class Stores : Form
    {
        private readonly IStoreRepository _storeRepository;

        public Stores()
        {
            InitializeComponent();

            // Repository oluştur
            _storeRepository = new SqlStoreRepository(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;"
            );
        }

        // Form Load event
        private void Stores_Load(object sender, EventArgs e)
        {
            LoadStores();
        }

        private void LoadStores()
        {
            dgvMagazalar.DataSource = _storeRepository.GetAll();

            // Sütun başlıklarını Türkçe yap
            foreach (DataGridViewColumn col in dgvMagazalar.Columns)
            {
                switch (col.Name)
                {
                    case "StoreName":
                        col.HeaderText = "Mağaza Adı";
                        break;
                    case "StoreDate":
                        col.HeaderText = "Eklenme Tarihi";
                        col.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm"; // Tarih formatı
                        break;
                    case "Id":
                        col.HeaderText = "ID"; // Eğer görünmesini istemezseniz col.Visible = false; yapabilirsiniz
                        break;
                }
            }
        }


        // "Mağaza Ekle" butonu tıklandığında
        private void btnEkle_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStoreName.Text))
            {
                MessageBox.Show(
                    "Lütfen mağaza adı girin.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var store = new Store
            {
                StoreName = txtStoreName.Text.Trim(),
                StoreDate = DateTime.Now
            };

            int result = _storeRepository.Add(store);

            if (result > 0)
            {
                MessageBox.Show(
                    "Mağaza başarıyla eklendi!",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                txtStoreName.Clear();
                LoadStores();
            }
        }
    }
}
