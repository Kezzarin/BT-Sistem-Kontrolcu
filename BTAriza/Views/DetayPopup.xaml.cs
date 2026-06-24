using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BTAriza.Models;

namespace BTAriza.Views
{
    public partial class DetayPopup : Window
    {
        private readonly ArizaKaydi _kayit;

        public DetayPopup(ArizaKaydi kayit)
        {
            InitializeComponent();
            _kayit = kayit;
            DoldurBilgiler();
        }

        private void DoldurBilgiler()
        {
            ArizaNoText.Text = _kayit.ArizaNo;
            AdSoyadText.Text = _kayit.AdSoyad;
            DepartmanText.Text = _kayit.Departman;
            BilgisayarText.Text = _kayit.BilgisayarAdi;
            KategoriText.Text = _kayit.Kategori;
            IletilenText.Text = _kayit.IletilenKisiDisplay;
            TarihText.Text = _kayit.OlusturmaTarihi.ToString("dd.MM.yyyy HH:mm");
            AciklamaText.Text = _kayit.Aciklama;

            // Öncelik badge
            OncelikText.Text = _kayit.OncelikDisplay;
            OncelikBadge.Background = _kayit.Oncelik switch
            {
                Oncelik.Dusuk => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#27AE60")),
                Oncelik.Orta => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F39C12")),
                Oncelik.Yuksek => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E67E22")),
                Oncelik.Kritik => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C")),
                _ => Brushes.Gray
            };

            // Mevcut durum
            CmbDurum.SelectedIndex = (int)_kayit.Durum;
            if (!string.IsNullOrEmpty(_kayit.CozumNotu))
            {
                TxtCozumNotu.Text = _kayit.CozumNotu;
                if (_kayit.Durum == Durum.Cozuldu)
                    CozumNotuPanel.Visibility = Visibility.Visible;
            }
        }

        private void Durum_Changed(object sender, SelectionChangedEventArgs e)
        {
            CozumNotuPanel.Visibility = CmbDurum.SelectedIndex == 2
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Kaydet_Click(object sender, RoutedEventArgs e)
        {
            _kayit.Durum = (Durum)CmbDurum.SelectedIndex;
            _kayit.CozumNotu = TxtCozumNotu.Text;

            try
            {
                var db = ((App)Application.Current).DB;
                db?.KayitGuncelle(_kayit);
                DialogResult = true;
                Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Güncelleme hatası: {ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Kapat_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
