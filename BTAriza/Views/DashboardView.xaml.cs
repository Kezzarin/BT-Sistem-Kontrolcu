using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BTAriza.Views
{
    public partial class DashboardView : UserControl
    {
        private DispatcherTimer? _sistemTimer;
        private PerformanceCounter? _cpuCounter;
        private PerformanceCounter? _ramCounter;

        public DashboardView()
        {
            InitializeComponent();
            Loaded += DashboardView_Loaded;
            Unloaded += DashboardView_Unloaded;
        }

        private void DashboardView_Loaded(object sender, RoutedEventArgs e)
        {
            YukleIstatistikler();
            YukleSonKayitlar();
            BilgisayarBilgisiniYukle();
            BaslatSistemTimer();
        }

        private void DashboardView_Unloaded(object sender, RoutedEventArgs e)
        {
            _sistemTimer?.Stop();
            _cpuCounter?.Dispose();
            _ramCounter?.Dispose();
        }

        private void YukleIstatistikler()
        {
            try
            {
                var db = ((App)Application.Current).DB;
                if (db == null) return;
                BekliyenSayi.Text = db.BekliyenSayisi().ToString();
                IslemdeSayi.Text = db.IslemdeSayisi().ToString();
                CozulenSayi.Text = db.CozulduSayisi().ToString();
            }
            catch { }
        }

        private void YukleSonKayitlar()
        {
            try
            {
                var db = ((App)Application.Current).DB;
                if (db == null) return;
                var liste = db.SonKayitlariGetir(5);
                if (liste.Count == 0)
                {
                    SonKayitlarBos.Visibility = Visibility.Visible;
                    SonKayitlarGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    SonKayitlarBos.Visibility = Visibility.Collapsed;
                    SonKayitlarGrid.Visibility = Visibility.Visible;
                    SonKayitlarGrid.ItemsSource = liste;
                }
            }
            catch { }
        }

        private void BilgisayarBilgisiniYukle()
        {
            HostnameText.Text = Environment.MachineName;
            OsText.Text = Environment.OSVersion.ToString();
        }

        private void BaslatSistemTimer()
        {
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                _cpuCounter.NextValue(); // ilk okuma sıfır gelir
            }
            catch { }

            _sistemTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            _sistemTimer.Tick += (s, e) => GuncelSistemBilgisi();
            _sistemTimer.Start();

            // İlk güncelleme 2 sn sonra
            var ilkTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            ilkTimer.Tick += (s, e) => { GuncelSistemBilgisi(); ilkTimer.Stop(); };
            ilkTimer.Start();
        }

        private void GuncelSistemBilgisi()
        {
            try
            {
                if (_cpuCounter != null)
                {
                    float cpu = _cpuCounter.NextValue();
                    CpuBar.Value = Math.Min(cpu, 100);
                    CpuText.Text = $"{cpu:F0}%";
                }

                if (_ramCounter != null)
                {
                    float kullanilabilir = _ramCounter.NextValue();
                    long toplam = App.Wmi.ToplamRamMb();
                    if (toplam > 0)
                    {
                        float kullanilan = toplam - kullanilabilir;
                        float yuzde = kullanilan / toplam * 100;
                        RamBar.Value = Math.Min(yuzde, 100);
                        RamText.Text = $"{yuzde:F0}%";
                    }
                }
            }
            catch { }
        }
    }
}
