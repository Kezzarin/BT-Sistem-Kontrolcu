using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using BTAriza.Services;
using BTAriza.Views;

namespace BTAriza
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _saatTimer;
        private string _aktifSayfa = "Dashboard";

        public MainWindow()
        {
            InitializeComponent();

            // Bilgisayar adı
            BilgisayarAdiText.Text = Environment.MachineName;

            // Şirket adı
            SirketAdiText.Text = ConfigService.Config.SirketAdi;

            // Logo
            if (!string.IsNullOrEmpty(ConfigService.Config.LogoYolu) &&
                System.IO.File.Exists(ConfigService.Config.LogoYolu))
            {
                var bitmap = new System.Windows.Media.Imaging.BitmapImage(
                    new Uri(ConfigService.Config.LogoYolu));
                LogoImage.Source = bitmap;
                LogoImage.Visibility = Visibility.Visible;
            }

            // Saat timer
            _saatTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _saatTimer.Tick += (s, e) => SaatText.Text = DateTime.Now.ToString("HH:mm:ss  dd.MM.yyyy");
            _saatTimer.Start();
            SaatText.Text = DateTime.Now.ToString("HH:mm:ss  dd.MM.yyyy");

            // Dashboard yükle
            GosteriSayfa("Dashboard");
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                GosteriSayfa(tag);
            }
        }

        public void GosteriSayfa(string sayfa, object? parametre = null)
        {
            _aktifSayfa = sayfa;
            GuncelleAktifButon(sayfa);

            UserControl? view = sayfa switch
            {
                "Dashboard" => new DashboardView(),
                "ArizaRehberi" => new ArizaRehberiView(),
                "SistemTarama" => new SistemTaramaView(),
                "SurucuYonetimi" => new SurucuYonetimiView(),
                "ArizaKaydi" => parametre is string kat ? new ArizaKaydiView(kat) : new ArizaKaydiView(),
                "KayitPaneli" => new KayitPaneliView(),
                "Ayarlar" => new AyarlarView(),
                _ => null
            };

            if (view != null)
            {
                SayfaBasligiText.Text = sayfa switch
                {
                    "Dashboard" => "Dashboard",
                    "ArizaRehberi" => "Arıza Rehberi",
                    "SistemTarama" => "Sistem Tarama",
                    "SurucuYonetimi" => "Sürücü Yönetimi",
                    "ArizaKaydi" => "Arıza Kaydı",
                    "KayitPaneli" => "Kayıt Paneli",
                    "Ayarlar" => "Ayarlar",
                    _ => sayfa
                };

                MainContent.Content = view;
                FadeInContent();
            }
        }

        private void FadeInContent()
        {
            ContentArea.Opacity = 0;
            var anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            ContentArea.BeginAnimation(OpacityProperty, anim);
        }

        private void GuncelleAktifButon(string aktif)
        {
            var buttons = new[]
            {
                (BtnDashboard, "Dashboard"),
                (BtnArizaRehberi, "ArizaRehberi"),
                (BtnSistemTarama, "SistemTarama"),
                (BtnSurucuYonetimi, "SurucuYonetimi"),
                (BtnArizaKaydi, "ArizaKaydi"),
                (BtnKayitPaneli, "KayitPaneli"),
                (BtnAyarlar, "Ayarlar")
            };

            foreach (var (btn, tag) in buttons)
            {
                btn.Style = tag == aktif
                    ? (Style)FindResource("SidebarButtonActiveStyle")
                    : (Style)FindResource("SidebarButtonStyle");
            }
        }

        public void GuncelSirketAdi(string ad)
        {
            SirketAdiText.Text = ad;
        }

        public void GuncelLogo(string yol)
        {
            if (!string.IsNullOrEmpty(yol) && System.IO.File.Exists(yol))
            {
                var bitmap = new System.Windows.Media.Imaging.BitmapImage(new Uri(yol));
                LogoImage.Source = bitmap;
                LogoImage.Visibility = Visibility.Visible;
            }
            else
            {
                LogoImage.Visibility = Visibility.Collapsed;
            }
        }
    }
}
