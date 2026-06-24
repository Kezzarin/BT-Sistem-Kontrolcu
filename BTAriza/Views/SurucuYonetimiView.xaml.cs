using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BTAriza.Services;
using ClosedXML.Excel;

namespace BTAriza.Views
{
    public class SurucuItem
    {
        public string Ad { get; set; } = "";
        public string Surum { get; set; } = "";
        public string Uretici { get; set; } = "";
        public string Tarih { get; set; } = "";
        public string Sinif { get; set; } = "";
        public bool Hatali { get; set; }
    }

    public partial class SurucuYonetimiView : UserControl
    {
        private List<SurucuItem> _tumSuruculer = new();

        public SurucuYonetimiView()
        {
            InitializeComponent();
            Loaded += (s, e) => _ = YukleSuruculer();
        }

        private async Task YukleSuruculer()
        {
            YukleniyorPanel.Visibility = Visibility.Visible;
            SurucuGrid.Visibility = Visibility.Collapsed;

            try
            {
                var liste = await Task.Run(() => App.Wmi.Suruculeri());
                _tumSuruculer = liste
                    .Where(d => !string.IsNullOrWhiteSpace(d.GetValueOrDefault("DeviceName", "")))
                    .Select(d => new SurucuItem
                    {
                        Ad = d.GetValueOrDefault("DeviceName", "Bilinmiyor"),
                        Surum = d.GetValueOrDefault("DriverVersion", ""),
                        Uretici = d.GetValueOrDefault("Manufacturer", ""),
                        Tarih = FormatTarih(d.GetValueOrDefault("DriverDate", "")),
                        Sinif = d.GetValueOrDefault("DeviceClass", ""),
                        Hatali = d.GetValueOrDefault("IsSigned", "True") != "True"
                    })
                    .OrderByDescending(x => x.Hatali)
                    .ThenBy(x => x.Ad)
                    .ToList();

                UygulaFiltre();
                ToplamSurucuText.Text = $"{_tumSuruculer.Count} sürücü";
                HataliSurucuText.Text = $"{_tumSuruculer.Count(x => x.Hatali)} hatalı";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sürücü listesi alınamadı: {ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                YukleniyorPanel.Visibility = Visibility.Collapsed;
                SurucuGrid.Visibility = Visibility.Visible;
            }
        }

        private static string FormatTarih(string wmiTarih)
        {
            if (string.IsNullOrEmpty(wmiTarih) || wmiTarih.Length < 8) return "";
            try
            {
                var yil = wmiTarih[..4];
                var ay = wmiTarih.Substring(4, 2);
                var gun = wmiTarih.Substring(6, 2);
                return $"{gun}.{ay}.{yil}";
            }
            catch { return wmiTarih; }
        }

        private void Filtrele(object sender, SelectionChangedEventArgs e) => UygulaFiltre();

        private void UygulaFiltre()
        {
            var kategori = (CmbKategori.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Tümü";
            var filtreli = _tumSuruculer.AsEnumerable();

            if (kategori != "Tümü")
            {
                var anahtar = kategori switch
                {
                    "Ağ" => "NET",
                    "Görüntü" => "DISPLAY",
                    "Ses" => "MEDIA",
                    "Depolama" => "DISK",
                    "USB" => "USB",
                    _ => ""
                };
                if (!string.IsNullOrEmpty(anahtar))
                    filtreli = filtreli.Where(s =>
                        s.Sinif.ToUpper().Contains(anahtar) ||
                        s.Ad.ToUpper().Contains(anahtar));
            }

            SurucuGrid.ItemsSource = filtreli.ToList();
        }

        private void Yenile_Click(object sender, RoutedEventArgs e) => _ = YukleSuruculer();

        private void WindowsUpdate_Click(object sender, RoutedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo("ms-settings:windowsupdate") { UseShellExecute = true }); }
            catch { }
        }

        private void SurucuGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SurucuGrid.SelectedItem is SurucuItem surucu)
            {
                MessageBox.Show(
                    $"Cihaz: {surucu.Ad}\n" +
                    $"Sürüm: {surucu.Surum}\n" +
                    $"Üretici: {surucu.Uretici}\n" +
                    $"Tarih: {surucu.Tarih}\n" +
                    $"Sınıf: {surucu.Sinif}\n" +
                    $"Durum: {(surucu.Hatali ? "İmzasız / Hatalı" : "Normal")}",
                    "Sürücü Detayı",
                    MessageBoxButton.OK,
                    surucu.Hatali ? MessageBoxImage.Warning : MessageBoxImage.Information);
            }
        }

        private void ExcelExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Dosyası|*.xlsx",
                    FileName = $"Suruculer_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                };
                if (dlg.ShowDialog() != true) return;

                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Sürücüler");
                ws.Cell(1, 1).Value = "Cihaz Adı";
                ws.Cell(1, 2).Value = "Sürüm";
                ws.Cell(1, 3).Value = "Üretici";
                ws.Cell(1, 4).Value = "Tarih";
                ws.Cell(1, 5).Value = "Sınıf";
                ws.Cell(1, 6).Value = "Durum";
                ws.Row(1).Style.Font.Bold = true;

                var kayitlar = SurucuGrid.ItemsSource as IEnumerable<SurucuItem> ?? _tumSuruculer;
                int satir = 2;
                foreach (var s in kayitlar)
                {
                    ws.Cell(satir, 1).Value = s.Ad;
                    ws.Cell(satir, 2).Value = s.Surum;
                    ws.Cell(satir, 3).Value = s.Uretici;
                    ws.Cell(satir, 4).Value = s.Tarih;
                    ws.Cell(satir, 5).Value = s.Sinif;
                    ws.Cell(satir, 6).Value = s.Hatali ? "Hatalı" : "Normal";
                    if (s.Hatali)
                        ws.Row(satir).Style.Fill.BackgroundColor = XLColor.LightSalmon;
                    satir++;
                }
                ws.Columns().AdjustToContents();
                wb.SaveAs(dlg.FileName);

                MessageBox.Show($"Excel dosyası kaydedildi:\n{dlg.FileName}", "Başarılı",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excel export hatası: {ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
