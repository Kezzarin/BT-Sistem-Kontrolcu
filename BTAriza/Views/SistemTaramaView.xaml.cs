using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BTAriza.Services;

namespace BTAriza.Views
{
    public partial class SistemTaramaView : UserControl
    {
        private readonly WmiService _wmi = App.Wmi;
        private string _raporIcerik = "";

        public SistemTaramaView()
        {
            InitializeComponent();
        }

        private async void Tara_Click(object sender, RoutedEventArgs e)
        {
            BtnTara.IsEnabled = false;
            BaslangicPanel.Visibility = Visibility.Collapsed;
            SonuclarPanel.Visibility = Visibility.Collapsed;
            TaramaDurumuPanel.Visibility = Visibility.Visible;
            BtnPdf.Visibility = Visibility.Collapsed;

            try
            {
                TaramaMesaji.Text = "İşlemci taranıyor...";
                await Task.Delay(400);
                var cpu = await Task.Run(() => _wmi.IslemciBilgisi());

                TaramaMesaji.Text = "RAM taranıyor...";
                await Task.Delay(400);
                var ram = await Task.Run(() => _wmi.RamBilgisi());
                var ramMb = await Task.Run(() => _wmi.ToplamRamMb());

                TaramaMesaji.Text = "Diskler taranıyor...";
                await Task.Delay(400);
                var diskler = await Task.Run(() => _wmi.DiskBilgisi());

                TaramaMesaji.Text = "GPU taranıyor...";
                await Task.Delay(400);
                var gpu = await Task.Run(() => _wmi.GpuBilgisi());

                TaramaMesaji.Text = "Ağ bağdaştırıcıları taranıyor...";
                await Task.Delay(400);
                var ag = await Task.Run(() => _wmi.AgBilgisi());

                TaramaMesaji.Text = "Windows bilgileri alınıyor...";
                await Task.Delay(400);
                var windows = await Task.Run(() => _wmi.WindowsBilgisi());

                TaramaMesaji.Text = "Antivirüs durumu kontrol ediliyor...";
                await Task.Delay(400);
                var antivirus = await Task.Run(() => _wmi.AntivirusDurumu());

                // Sonuçları göster
                CpuBilgiText.Text = cpu;
                GpuBilgiText.Text = gpu;
                RamBilgiText.Text = ram;

                // RAM progress bar
                try
                {
                    var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                    float kullanilabilir = ramCounter.NextValue();
                    if (ramMb > 0)
                    {
                        float yuzde = (ramMb - kullanilabilir) / ramMb * 100;
                        RamProgressBar.Value = Math.Min(yuzde, 100);
                    }
                    ramCounter.Dispose();
                }
                catch { }

                // Disk listesi
                var diskItems = new List<string>();
                foreach (var d in diskler)
                {
                    long boyut = 0;
                    long.TryParse(d.GetValueOrDefault("Size", "0"), out boyut);
                    double gb = boyut / (1024.0 * 1024 * 1024);
                    diskItems.Add($"📀 {d.GetValueOrDefault("Model", "Bilinmiyor")} — {gb:F0} GB ({d.GetValueOrDefault("InterfaceType", "")})");
                }
                DiskListesi.ItemsSource = diskItems.Count > 0 ? diskItems : new List<string> { "Disk bilgisi alınamadı" };

                // Ağ listesi
                var agItems = new List<string>();
                foreach (var a in ag.Take(5))
                    agItems.Add($"🌐 {a.GetValueOrDefault("Name", "Bilinmiyor")}");
                AgListesi.ItemsSource = agItems.Count > 0 ? agItems : new List<string> { "Aktif ağ bağdaştırıcısı yok" };

                WindowsBilgiText.Text = windows;
                AntivirusText.Text = antivirus;

                // Rapor içeriği oluştur
                _raporIcerik = $"SİSTEM TARAMA RAPORU\n{DateTime.Now:dd.MM.yyyy HH:mm}\n" +
                    $"Bilgisayar: {Environment.MachineName}\n\n" +
                    $"İŞLEMCİ\n{cpu}\n\nGPU\n{gpu}\n\nRAM\n{ram}\n\n" +
                    $"DİSKLER\n{string.Join("\n", diskItems)}\n\n" +
                    $"AĞ\n{string.Join("\n", agItems)}\n\n" +
                    $"WINDOWS\n{windows}\n\nANTİVİRÜS\n{antivirus}";

                TaramaDurumuPanel.Visibility = Visibility.Collapsed;
                SonuclarPanel.Visibility = Visibility.Visible;
                BtnPdf.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                TaramaDurumuPanel.Visibility = Visibility.Collapsed;
                BaslangicPanel.Visibility = Visibility.Visible;
                MessageBox.Show($"Tarama hatası: {ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                BtnTara.IsEnabled = true;
            }
        }

        private void PdfRapor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF Dosyası|*.pdf",
                    FileName = $"SistemRaporu_{DateTime.Now:yyyyMMdd_HHmm}.pdf"
                };
                if (dlg.ShowDialog() != true) return;

                var doc = new PdfSharp.Pdf.PdfDocument();
                doc.Info.Title = "Sistem Tarama Raporu";
                var sayfa = doc.AddPage();
                var gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(sayfa);

                var baslikFont = new PdfSharp.Drawing.XFont("Arial", 16, PdfSharp.Drawing.XFontStyleEx.Bold);
                var normalFont = new PdfSharp.Drawing.XFont("Arial", 10, PdfSharp.Drawing.XFontStyleEx.Regular);
                var kucukFont = new PdfSharp.Drawing.XFont("Arial", 9, PdfSharp.Drawing.XFontStyleEx.Regular);

                double y = 40;
                gfx.DrawString("SİSTEM TARAMA RAPORU", baslikFont,
                    PdfSharp.Drawing.XBrushes.DarkBlue,
                    new PdfSharp.Drawing.XRect(40, y, sayfa.Width - 80, 30),
                    PdfSharp.Drawing.XStringFormats.TopLeft);
                y += 30;

                gfx.DrawString($"Tarih: {DateTime.Now:dd.MM.yyyy HH:mm}  |  Bilgisayar: {Environment.MachineName}",
                    kucukFont, PdfSharp.Drawing.XBrushes.Gray,
                    new PdfSharp.Drawing.XRect(40, y, sayfa.Width - 80, 20),
                    PdfSharp.Drawing.XStringFormats.TopLeft);
                y += 30;

                gfx.DrawLine(PdfSharp.Drawing.XPens.LightGray, 40, y, sayfa.Width - 40, y);
                y += 15;

                var satirlar = _raporIcerik.Split('\n');
                foreach (var satir in satirlar)
                {
                    if (y > sayfa.Height - 60)
                    {
                        sayfa = doc.AddPage();
                        gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(sayfa);
                        y = 40;
                    }

                    var font = satir.All(c => char.IsUpper(c) || c == ' ' || c == '\r') && satir.Trim().Length > 0
                        ? new PdfSharp.Drawing.XFont("Arial", 11, PdfSharp.Drawing.XFontStyleEx.Bold)
                        : normalFont;

                    gfx.DrawString(satir, font, PdfSharp.Drawing.XBrushes.Black,
                        new PdfSharp.Drawing.XRect(40, y, sayfa.Width - 80, 16),
                        PdfSharp.Drawing.XStringFormats.TopLeft);
                    y += 16;
                }

                doc.Save(dlg.FileName);
                MessageBox.Show($"PDF raporu kaydedildi:\n{dlg.FileName}", "Başarılı",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF oluşturma hatası: {ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
