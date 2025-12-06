namespace MarketStockTracking
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Veritabanýný oluþtur/kontrol et
            DatabaseHelper.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // veya senin ana formun hangisiyse
        }
    }
}