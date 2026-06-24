using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BTAriza.Models;

namespace BTAriza.Views
{
    public partial class KayitPaneliView : UserControl
    {
        private List<ArizaKaydi> _tumKayitlar = new();

        public KayitPaneliView()
        {
            InitializeComponent();
            Loaded += (s, e) => YukleKayitlar();
        }

        private void YukleKayitlar()
        {
            try
            {
                var db = ((App)Application.Current).DB;
                if (db == null) return;
                _tumKayitlar = db.KayitlariGetir();
                UygulaFiltre();
            }
            catch { }
        }

        private void Filtrele(object sender, RoutedEventArgs e) => UygulaFiltre();
        private void Filtrele(object sender, SelectionChangedEventArgs e) => UygulaFiltre();

        private void UygulaFiltre()
        {
            var filtreli = _tumKayitlar.AsEnumerable();

            var arama = TxtArama.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(arama))
            {
                filtreli = filtreli.Where(k =>
                    k.ArizaNo.ToLower().Contains(arama) ||
                    k.AdSoyad.ToLower().Contains(arama) ||
                    k.Kategori.ToLower().Contains(arama) ||
                    k.Departman.ToLower().Contains(arama) ||
                    k.Aciklama.ToLower().Contains(arama));
            }

            var durum = (CmbDurumFiltre.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (durum != null && durum != "Tüm Durumlar")
            {
                var d = durum switch { "Bekliyor" => Durum.Bekliyor, "İşlemde" => Durum.Islemde, _ => Durum.Cozuldu };
                filtreli = filtreli.Where(k => k.Durum == d);
            }

            var oncelik = (CmbOncelikFiltre.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (oncelik != null && oncelik != "Tüm Öncelikler")
            {
                var o = oncelik switch { "Düşük" => Oncelik.Dusuk, "Orta" => Oncelik.Orta, "Yüksek" => Oncelik.Yuksek, _ => Oncelik.Kritik };
                filtreli = filtreli.Where(k => k.Oncelik == o);
            }

            var dept = (CmbDepartmanFiltre.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (dept != null && dept != "Tüm Departmanlar")
                filtreli = filtreli.Where(k => k.Departman == dept);

            var liste = filtreli.ToList();
            ToplamKayitText.Text = $"({liste.Count} kayıt)";

            if (liste.Count == 0)
            {
                BosPanel.Visibility = Visibility.Visible;
                KayitGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                BosPanel.Visibility = Visibility.Collapsed;
                KayitGrid.Visibility = Visibility.Visible;
                KayitGrid.ItemsSource = liste;
            }
        }

        private void KayitGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (KayitGrid.SelectedItem is ArizaKaydi kayit)
            {
                var detay = new DetayPopup(kayit);
                detay.Owner = Window.GetWindow(this);
                if (detay.ShowDialog() == true)
                {
                    YukleKayitlar();
                }
            }
        }

        private void CsvExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "CSV Dosyası|*.csv",
                    FileName = $"ArizaKayitlari_{DateTime.Now:yyyyMMdd_HHmm}.csv"
                };
                if (dlg.ShowDialog() != true) return;

                var satirlar = new List<string>
                {
                    "ArizaNo,AdSoyad,Departman,BilgisayarAdi,Kategori,Oncelik,IletilenKisi,Durum,OlusturmaTarihi,Aciklama"
                };

                var kayitlar = KayitGrid.ItemsSource as List<ArizaKaydi> ?? _tumKayitlar;
                foreach (var k in kayitlar)
                {
                    satirlar.Add($"{k.ArizaNo},\"{k.AdSoyad}\",{k.Departman},{k.BilgisayarAdi}," +
                                 $"{k.Kategori},{k.OncelikDisplay},{k.IletilenKisiDisplay}," +
                                 $"{k.DurumDisplay},{k.OlusturmaTarihi:dd.MM.yyyy HH:mm}," +
                                 $"\"{k.Aciklama.Replace("\"", "'").Replace("\n", " ")}\"");
                }
                File.WriteAllLines(dlg.FileName, satirlar, System.Text.Encoding.UTF8);
                MessageBox.Show($"CSV dosyası kaydedildi:\n{dlg.FileName}", "Başarılı",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSV export hatası: {ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
