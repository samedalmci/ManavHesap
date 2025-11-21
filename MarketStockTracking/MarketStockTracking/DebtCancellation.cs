using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Globalization;
using ClosedXML.Excel;


namespace MarketStockTracking
{

    public partial class DebtCancellation : Form
    {
        // Bağlantı dizesi (Kendi LocalDB örneğinizle eşleştiğinden emin olun!)
        string baglanti = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;";
        SqlConnection conn;

        public DebtCancellation()
        {
            InitializeComponent();

            // HATA ÇÖZÜMÜ: Veritabanı işlemleri (conn oluşturma ve MagazalariYukle) buradan kaldırıldı.

            // *** Olay Atamaları KALIYOR ***
            txtMagza.SelectedIndexChanged += txtMagza_SelectedIndexChanged;
            dgvBorclar.CellClick += dgvBorclar_CellClick;

            // Ödeme giriş alanına formatlama olaylarını bağla
            txtOdenenMiktar.KeyPress += TxtCurrency_KeyPress;
            txtOdenenMiktar.Leave += TxtCurrency_Leave;

            // Kalan Borç alanını okunamaz yap
            txtKalanBorc.ReadOnly = true;
        }

        private void DebtCancellation_Load(object sender, EventArgs e)
        {
            // HATA ÇÖZÜMÜ: Veritabanı işlemleri buraya taşındı.
            conn = new SqlConnection(baglanti);
            MagazalariYukle();

            // DataGridView başlangıçta görünür olduğu için yüklenir yüklenmez borçları listele.
            LoadDebts();
        }

        // **********************************************
        // * COMBOBOX VE DATAGRIDVIEW İŞLEMLERİ
        // **********************************************

        private void MagazalariYukle()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT MagazaAdi FROM Magazalar", conn);
                SqlDataReader dr = cmd.ExecuteReader();

                txtMagza.Items.Clear();
                txtMagza.Items.Add("Tüm Mağazalar");

                while (dr.Read())
                {
                    txtMagza.Items.Add(dr["MagazaAdi"].ToString());
                }

                dr.Close();
                txtMagza.SelectedIndex = 0;
            }
            finally
            {
                // Bağlantı her zaman kapatılır.
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void txtMagza_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Mağaza değiştiğinde DataGridView'i yeni filtreye göre yükle.
            LoadDebts();

            // Seçim değiştiğinde üstteki alanları temizle.
            txtKalanBorc.Text = "";
            txtOdenenMiktar.Text = "";
        }

        private void LoadDebts()
        {
            string secilenMagaza = txtMagza.SelectedIndex > 0 ? txtMagza.SelectedItem.ToString() : null;

            try
            {
                // Bağlantıyı sadece kapalıysa aç.
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = "SELECT ID, Magaza, UrunAdi, EklenmeTarihi AS Tarih, Pesin AS [Ödenen Tutar], Borc AS [Kalan Borç Tutarı] FROM Satislar WHERE Borc > 0";

                if (!string.IsNullOrEmpty(secilenMagaza))
                {
                    query += " AND Magaza = @MagazaAdi";
                }

                query += " ORDER BY EklenmeTarihi DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(secilenMagaza))
                {
                    cmd.Parameters.AddWithValue("@MagazaAdi", secilenMagaza);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvBorclar.DataSource = dt;

                if (dgvBorclar.Columns.Count > 0)
                {
                    var trCulture = new CultureInfo("tr-TR");
                    // DataGridView'de görünmesini istediğimiz başlık adı. Veriye "ID" sütun adıyla erişeceğiz.
                    dgvBorclar.Columns["ID"].HeaderText = "Satış ID";
                    dgvBorclar.Columns["ID"].Width = 60;
                    dgvBorclar.Columns["Tarih"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                    dgvBorclar.Columns["Ödenen Tutar"].DefaultCellStyle.Format = "N2";
                    dgvBorclar.Columns["Kalan Borç Tutarı"].DefaultCellStyle.Format = "N2";
                    dgvBorclar.Columns["Ödenen Tutar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvBorclar.Columns["Kalan Borç Tutarı"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvBorclar.Columns["Ödenen Tutar"].DefaultCellStyle.FormatProvider = trCulture;
                    dgvBorclar.Columns["Kalan Borç Tutarı"].DefaultCellStyle.FormatProvider = trCulture;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Borçlar yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        // **********************************************
        // * DATAGRIDVIEW SATIR SEÇİMİ (CELL CLICK)
        // **********************************************
        private void dgvBorclar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                DataGridViewRow selectedRow = dgvBorclar.Rows[e.RowIndex];

                string kalanBorcString = selectedRow.Cells["Kalan Borç Tutarı"].Value.ToString();

                // 1. Kalan Borç Tutarını okunamaz alana yaz
                txtKalanBorc.Text = kalanBorcString + " TL";

                // 2. Ödeme Giriş Alanını tamamen boşalt (Kullanıcı yeni ödemeyi girecek)
                txtOdenenMiktar.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Satır seçimi hatası: " + ex.Message);
            }
        }

        // **********************************************
        // * FORMATLAMA VE PARSE METOTLARI
        // **********************************************

        private void TxtCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                return;
            if (e.KeyChar == ',' && !txt.Text.Contains(","))
                return;
            e.Handled = true;
        }

        private void TxtCurrency_Leave(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string input = txt.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                txt.Text = "";
                return;
            }

            CultureInfo trCulture = new CultureInfo("tr-TR");
            string temizGiris = input.Replace(" TL", "").Replace(".", "");

            if (!temizGiris.Contains(","))
            {
                temizGiris += ",00";
            }

            if (decimal.TryParse(temizGiris, NumberStyles.Number, trCulture, out decimal result))
            {
                txt.Text = result.ToString("N2", trCulture) + " TL";
            }
        }

        private decimal ParseCurrencyInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            CultureInfo trCulture = new CultureInfo("tr-TR");
            string temizGiris = input.Trim().Replace(" TL", "");

            if (decimal.TryParse(temizGiris, NumberStyles.Number, trCulture, out decimal result))
            {
                return result;
            }

            return 0;
        }

        // **********************************************
        // * GÜNCEL METOT: BUTON TIKLAMA OLAYI (Ödeme Yap)
        // **********************************************
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtMagza.SelectedIndex == 0)
            {
                MessageBox.Show("Lütfen mağaza seçin.");
                return;
            }

            if (dgvBorclar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen ödeme yapmak istediğiniz satırı seçin.");
                return;
            }

            int selectedRowIndex = dgvBorclar.SelectedRows[0].Index;

            // HATA DÜZELTMESİ: Veri kaynağındaki sütun adı olan "ID" kullanılıyor.
            int satisID = (int)dgvBorclar.Rows[selectedRowIndex].Cells["ID"].Value;

            decimal kalanBorc = ParseCurrencyInput(txtKalanBorc.Text);
            decimal odenenMiktar = ParseCurrencyInput(txtOdenenMiktar.Text);

            if (odenenMiktar <= 0)
            {
                MessageBox.Show("Ödenecek miktar sıfırdan büyük olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (odenenMiktar > kalanBorc)
            {
                DialogResult dr = MessageBox.Show(
                    $"Girilen {odenenMiktar.ToString("N2", new CultureInfo("tr-TR"))} TL miktarı, kalan borç olan {kalanBorc.ToString("N2", new CultureInfo("tr-TR"))} TL'den fazladır.\n" +
                    "Borç sıfırlanacak. Yine de devam etmek istiyor musunuz?",
                    "Miktar Uyarısı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (dr == DialogResult.No)
                {
                    return;
                }
            }

            decimal yeniBorc = Math.Max(0, kalanBorc - odenenMiktar);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Pesin artırılır (yeni ödeme eklenir), Borc azaltılır (yeni borç kaydedilir).
                string updateQuery = "UPDATE Satislar SET Pesin = Pesin + @OdenenMiktar, Borc = @YeniBorc WHERE ID = @SatisID";

                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@OdenenMiktar", odenenMiktar);
                cmd.Parameters.AddWithValue("@YeniBorc", yeniBorc);
                cmd.Parameters.AddWithValue("@SatisID", satisID);

                int affectedRows = cmd.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    MessageBox.Show($"Ödeme başarıyla alındı. Yeni Kalan Borç: {yeniBorc.ToString("N2", new CultureInfo("tr-TR"))} TL", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDebts(); // İşlem sonrası listeyi yeniden yükle

                    txtKalanBorc.Text = "";
                    txtOdenenMiktar.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Borç güncellemesi sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void ExportToExcel()
        {
            if (dgvBorclar.Rows.Count == 0)
            {
                MessageBox.Show("Dışa aktarılacak veri bulunamadı.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Dosyaları (*.xlsx)|*.xlsx|Tüm Dosyalar (*.*)|*.*",
                FileName = $"Borc_Hesaplama_{DateTime.Now:yyyy_MM_dd}.xlsx"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    // DataTable oluştur
                    DataTable dt = new DataTable();
                    foreach (DataGridViewColumn col in dgvBorclar.Columns)
                        dt.Columns.Add(col.HeaderText);

                    foreach (DataGridViewRow row in dgvBorclar.Rows)
                    {
                        if (row.IsNewRow) continue;
                        dt.Rows.Add(row.Cells.Cast<DataGridViewCell>().Select(c => c.Value).ToArray());
                    }

                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Borçlar");

                        // Başlıkları yaz ve kalın yap
                        for (int i = 0; i < dgvBorclar.Columns.Count; i++)
                        {
                            var header = ws.Cell(1, i + 1);
                            header.Value = dgvBorclar.Columns[i].HeaderText;
                            header.Style.Font.Bold = true;
                            header.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            header.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 220, 220)); // Hafif gri
                            header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }

                        // Verileri yaz
                        for (int i = 0; i < dgvBorclar.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvBorclar.Columns.Count; j++)
                            {
                                var cell = ws.Cell(i + 2, j + 1);
                                var val = dgvBorclar.Rows[i].Cells[j].Value;

                                if (val != null && decimal.TryParse(val.ToString(), out decimal num))
                                    cell.Value = num;
                                else
                                    cell.Value = val?.ToString() ?? "";

                                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                // Market sütunu sola, sayılar sağa hizalı
                                if (j == 1) // Market sütun indexi, istersen dgvBorclar’da kontrol et
                                    cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                else
                                    cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            }
                        }

                        ws.Columns().AdjustToContents();

                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Borç listesi Excel dosyası olarak kaydedildi.",
                        "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Excel’e aktarılırken bir hata oluştu: " + ex.Message,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
    }
}