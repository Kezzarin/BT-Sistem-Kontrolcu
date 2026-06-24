using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using BTAriza.Services;

namespace BTAriza.Views
{
    public partial class AyarlarView : UserControl
    {
        public AyarlarView()
        {
            InitializeComponent();
            Loaded += (s, e) => YukleAyarlar();
        }

        private void YukleAyarlar()
        {
            var cfg = ConfigService.Config;
            TxtSirketAdi.Text = cfg.SirketAdi;
            TxtLogoYolu.Text = cfg.LogoYolu;
            TxtUzman.Text = cfg.UzmanAdi;
            TxtSef.Text = cfg.SefAdi;
            TxtMudur.Text = cfg.MudurAdi;
            DbYoluText.Text = Path.GetFullPath(cfg.VeritabaniYolu);
        }

        private void SirketAdiKaydet_Click(object sender, RoutedEventArgs e)
        {
            ConfigService.Config.SirketAdi = TxtSirketAdi.Text.Trim();
            ConfigService.Kaydet();

            // Ana pencere başlığını güncelle
            if (Window.GetWindow(this) is MainWindow mw)
                mw.GuncelSirketAdi(ConfigService.Config.SirketAdi);

            MessageBox.Show("Şirket adı kaydedildi.", "Başarılı",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LogoSec_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PNG Dosyaları|*.png|JPEG Dosyaları|*.jpg;*.jpeg|Tüm Resimler|*.png;*.jpg;*.jpeg;*.bmp"
            };
            if (dlg.ShowDialog() == true)
            {
                TxtLogoYolu.Text = dlg.FileName;
                ConfigService.Config.LogoYolu = dlg.FileName;
                ConfigService.Kaydet();

                if (Window.GetWindow(this) is MainWindow mw)
                    mw.GuncelLogo(dlg.FileName);

                MessageBox.Show("Logo güncellendi.", "Başarılı",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LogoTemizle_Click(object sender, RoutedEventArgs e)
        {
            TxtLogoYolu.Text = "";
            ConfigService.Config.LogoYolu = "";
            ConfigService.Kaydet();

            if (Window.GetWindow(this) is MainWindow mw)
                mw.GuncelLogo("");
        }

        private void PersonelKaydet_Click(object sender, RoutedEventArgs e)
        {
            ConfigService.Config.UzmanAdi = TxtUzman.Text.Trim();
            ConfigService.Config.SefAdi = TxtSef.Text.Trim();
            ConfigService.Config.MudurAdi = TxtMudur.Text.Trim();
            ConfigService.Kaydet();

            MessageBox.Show("Personel bilgileri kaydedildi.", "Başarılı",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Yedekle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var kaynak = Path.GetFullPath(ConfigService.Config.VeritabaniYolu);
                if (!File.Exists(kaynak))
                {
                    MessageBox.Show("Veritabanı dosyası bulunamadı.", "Uyarı",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var dlg = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "SQLite Veritabanı|*.db",
                    FileName = $"ariza_yedek_{DateTime.Now:yyyyMMdd_HHmm}.db"
                };
                if (dlg.ShowDialog() == true)
                {
                    File.Copy(kaynak, dlg.FileName, true);
                    MessageBox.Show($"Yedekleme tamamlandı:\n{dlg.FileName}", "Başarılı",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yedekleme hatası: {ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GeriYukle_Click(object sender, RoutedEventArgs e)
        {
            var uyari = MessageBox.Show(
                "Mevcut veritabanı seçilen dosya ile değiştirilecek.\nBu işlem geri alınamaz. Devam etmek istiyor musunuz?",
                "Dikkat", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (uyari != MessageBoxResult.Yes) return;

            try
            {
                var dlg = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "SQLite Veritabanı|*.db"
                };
                if (dlg.ShowDialog() == true)
                {
                    var hedef = Path.GetFullPath(ConfigService.Config.VeritabaniYolu);
                    File.Copy(dlg.FileName, hedef, true);
                    MessageBox.Show("Veritabanı geri yüklendi. Uygulamayı yeniden başlatın.",
                        "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Geri yükleme hatası: {ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
