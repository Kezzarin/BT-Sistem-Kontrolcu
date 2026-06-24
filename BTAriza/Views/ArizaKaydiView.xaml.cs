using System;
using System.Windows;
using System.Windows.Controls;
using BTAriza.Models;
using BTAriza.Services;

namespace BTAriza.Views
{
    public partial class ArizaKaydiView : UserControl
    {
        public ArizaKaydiView(string? kategori = null)
        {
            InitializeComponent();
            TxtBilgisayarAdi.Text = Environment.MachineName;
            Loaded += (s, e) =>
            {
                GuncelleArizaNo();
                if (!string.IsNullOrEmpty(kategori))
                {
                    foreach (ComboBoxItem item in CmbKategori.Items)
                    {
                        if (item.Content?.ToString() == kategori)
                        {
                            CmbKategori.SelectedItem = item;
                            break;
                        }
                    }
                }
            };
        }

        private void GuncelleArizaNo()
        {
            try
            {
                var db = ((App)Application.Current).DB;
                if (db != null)
                    TxtArizaNo.Text = db.ArizaNoUret();
            }
            catch { }
        }

        private void Oncelik_Changed(object sender, RoutedEventArgs e)
        {
            // renk vurgusu zaten XAML'da
        }

        private void SistemBilgisi_Toggle(object sender, RoutedEventArgs e)
        {
            if (TglSistemBilgisi.IsChecked == true)
            {
                SistemBilgisiPanel.Visibility = Visibility.Visible;
                TglSistemBilgisi.Content = "Sistem Bilgisi Eklendi ✓";
                TxtSistemBilgisi.Text = ToplamSistemBilgisi();
            }
            else
            {
                SistemBilgisiPanel.Visibility = Visibility.Collapsed;
                TglSistemBilgisi.Content = "Sistem Bilgisi Ekle";
                TxtSistemBilgisi.Text = "";
            }
        }

        private string ToplamSistemBilgisi()
        {
            try
            {
                var wmi = App.Wmi;
                return $"Bilgisayar: {Environment.MachineName}\n" +
                       $"İşlemci: {wmi.IslemciBilgisi()}\n" +
                       $"RAM: {wmi.RamBilgisi()}\n" +
                       $"OS: {wmi.WindowsBilgisi()}";
            }
            catch
            {
                return $"Bilgisayar: {Environment.MachineName}\nOS: {Environment.OSVersion}";
            }
        }

        private void Kaydet_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAdSoyad.Text))
            { MessageBox.Show("Ad Soyad zorunludur.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (CmbDepartman.SelectedItem == null)
            { MessageBox.Show("Departman seçimi zorunludur.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (CmbKategori.SelectedItem == null)
            { MessageBox.Show("Kategori seçimi zorunludur.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (string.IsNullOrWhiteSpace(TxtAciklama.Text))
            { MessageBox.Show("Açıklama zorunludur.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            var oncelik = Oncelik.Dusuk;
            if (RbOrta.IsChecked == true) oncelik = Oncelik.Orta;
            else if (RbYuksek.IsChecked == true) oncelik = Oncelik.Yuksek;
            else if (RbKritik.IsChecked == true) oncelik = Oncelik.Kritik;

            var iletilen = IletilenKisi.Uzman;
            if (CmbIletilenKisi.SelectedIndex == 1) iletilen = IletilenKisi.Sef;
            else if (CmbIletilenKisi.SelectedIndex == 2) iletilen = IletilenKisi.Mudur;

            var kayit = new ArizaKaydi
            {
                ArizaNo = TxtArizaNo.Text,
                AdSoyad = TxtAdSoyad.Text.Trim(),
                Departman = (CmbDepartman.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                BilgisayarAdi = TxtBilgisayarAdi.Text.Trim(),
                Kategori = (CmbKategori.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                Aciklama = TxtAciklama.Text.Trim(),
                Oncelik = oncelik,
                IletilenKisi = iletilen,
                Durum = Durum.Bekliyor,
                SistemBilgisi = TglSistemBilgisi.IsChecked == true ? TxtSistemBilgisi.Text : "",
                OlusturmaTarihi = DateTime.Now
            };

            try
            {
                var db = ((App)Application.Current).DB;
                db?.KayitEkle(kayit);

                if (oncelik == Oncelik.Kritik)
                {
                    GonderToastBildirimi(kayit);
                }

                MessageBox.Show($"Arıza kaydı başarıyla oluşturuldu.\nArıza No: {kayit.ArizaNo}",
                    "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);

                Temizle_Click(sender, e);
                GuncelleArizaNo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt sırasında hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GonderToastBildirimi(ArizaKaydi kayit)
        {
            try
            {
                // Windows 10+ Toast Notification
                var toastXml = $@"
                <toast>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>KRİTİK ARIZA KAYDI</text>
                            <text>{kayit.ArizaNo} - {kayit.AdSoyad}</text>
                            <text>{kayit.Aciklama}</text>
                        </binding>
                    </visual>
                </toast>";

                var xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
                xmlDoc.LoadXml(toastXml);
                var toast = new Windows.UI.Notifications.ToastNotification(xmlDoc);
                var notifier = Windows.UI.Notifications.ToastNotificationManager
                    .CreateToastNotifier("BTAriza");
                notifier.Show(toast);
            }
            catch
            {
                // Toast desteklenmiyorsa sessizce geç
            }
        }

        private void Temizle_Click(object sender, RoutedEventArgs e)
        {
            TxtAdSoyad.Text = "";
            CmbDepartman.SelectedIndex = -1;
            TxtBilgisayarAdi.Text = Environment.MachineName;
            CmbKategori.SelectedIndex = -1;
            TxtAciklama.Text = "";
            RbDusuk.IsChecked = true;
            CmbIletilenKisi.SelectedIndex = 0;
            TglSistemBilgisi.IsChecked = false;
            SistemBilgisiPanel.Visibility = Visibility.Collapsed;
            TxtSistemBilgisi.Text = "";
        }
    }
}
