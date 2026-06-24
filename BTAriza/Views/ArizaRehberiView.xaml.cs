using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BTAriza.Views
{
    public partial class ArizaRehberiView : UserControl
    {
        private string _aktifKategori = "";
        private int _aktifAdim = 0;
        private List<(string Baslik, string Aciklama)> _adimlar = new();

        private static readonly Dictionary<string, List<(string, string)>> _rehber = new()
        {
            ["Bilgisayar Açılmıyor"] = new()
            {
                ("Güç Kablosunu Kontrol Et", "Güç kablosunun prize ve bilgisayara sağlam takılı olduğunu kontrol edin. Gevşek bağlantı en yaygın nedendir."),
                ("Güç Düğmesini Test Et", "Güç düğmesine 5-10 saniye basılı tutun. LED ışığı yanıyor mu? Vantilatör dönüyor mu?"),
                ("RAM'i Çıkar-Tak", "Bilgisayarı kapatıp açın, RAM modüllerini çıkarıp yerine takın. Oksitlenme soruna yol açabilir."),
                ("BIOS'u Sıfırla", "Anakart üzerindeki CMOS pilini 5 dakika çıkarın, sonra tekrar takın. Bu BIOS ayarlarını sıfırlar."),
                ("Harici Cihazları Çıkar", "USB, harici disk gibi tüm cihazları çıkarıp tekrar başlatmayı deneyin. Uyumsuz cihaz önyüklemeyi engelleyebilir."),
                ("Teknik Servis", "Sorun devam ediyorsa anakart veya güç kaynağı arızası olabilir. Teknik servis gerekebilir.")
            },
            ["Ekran Sorunu"] = new()
            {
                ("Kablo Bağlantısını Kontrol Et", "Monitör kablosunun (HDMI, VGA, DisplayPort) hem monitöre hem bilgisayara sağlam bağlı olduğunu kontrol edin."),
                ("Monitörü Test Et", "Monitörün güç ışığı yanıyor mu? Başka bir bilgisayara bağlayarak monitörün çalışıp çalışmadığını test edin."),
                ("Parlaklık Ayarını Kontrol Et", "Monitördeki parlaklık ve kontrast düğmelerini kontrol edin. Sıfırlanmış olabilir."),
                ("Ekran Sürücüsünü Güncelle", "Aygıt Yöneticisi > Ekran Bağdaştırıcıları > Sağ tık > Sürücüyü Güncelle adımlarını izleyin."),
                ("Çözünürlüğü Sıfırla", "Masaüstüne sağ tık > Görüntü Ayarları > Çözünürlüğü varsayılan değere getirin."),
                ("Güvenli Mod Testi", "F8 ile Güvenli Mod'da başlatın. Sorun yoksa sürücü veya yazılım kaynaklıdır.")
            },
            ["İnternet Sorunu"] = new()
            {
                ("Modem/Router'ı Yeniden Başlat", "Modemi 30 saniye kapatın, tekrar açın. Bu işlem çoğu bağlantı sorununu çözer."),
                ("Kablo Bağlantısını Kontrol Et", "Ethernet kablosu kullanıyorsanız kablo bağlantısını kontrol edin. Wi-Fi kullanıyorsanız ağ adını doğrulayın."),
                ("IP Ayarlarını Sıfırla", "CMD'de şu komutları çalıştırın: ipconfig /release, ipconfig /flushdns, ipconfig /renew"),
                ("Ağ Bağdaştırıcısını Sıfırla", "Aygıt Yöneticisi > Ağ Bağdaştırıcıları > Devre Dışı Bırak > Tekrar Etkinleştir"),
                ("DNS Sunucusunu Değiştir", "Ağ Bağdaştırıcısı Ayarları > IPv4 > DNS: 8.8.8.8 ve 8.8.4.4 (Google DNS) girin."),
                ("Güvenlik Duvarını Kontrol Et", "Windows Güvenlik Duvarı veya 3. parti antivirüs bağlantıyı engelliyor olabilir. Geçici olarak devre dışı bırakıp test edin.")
            },
            ["Yazıcı Sorunu"] = new()
            {
                ("Yazıcı Bağlantısını Kontrol Et", "USB kablosu veya ağ bağlantısını kontrol edin. Yazıcının açık ve çevrimiçi olduğunu doğrulayın."),
                ("Yazdırma Kuyruğunu Temizle", "Denetim Masası > Aygıtlar ve Yazıcılar > Yazıcıya sağ tık > Yazdırma Kuyruğunu Gör > Tüm İşleri İptal Et"),
                ("Yazıcıyı Varsayılan Yap", "Yazıcıya sağ tık > Varsayılan Yazıcı Olarak Ayarla. Yanlış yazıcıya gönderiliyor olabilir."),
                ("Sürücüyü Yeniden Yükle", "Yazıcı sürücüsünü kaldırıp üretici sitesinden güncel sürücüyü yükleyin."),
                ("Mürekkep/Toner Kontrolü", "Mürekkep veya toner seviyesini kontrol edin. Kağıt sıkışması olup olmadığını kontrol edin."),
                ("Yazıcı Sorun Gidericisi", "Ayarlar > Sorun Giderme > Diğer Sorun Gidericiler > Yazıcı'yı çalıştırın.")
            },
            ["Windows Sorunu"] = new()
            {
                ("Windows Güncellemelerini Kontrol Et", "Ayarlar > Windows Update > Güncellemeleri Denetle ve bekleyen güncellemeleri yükleyin."),
                ("Disk Denetimi Çalıştır", "CMD (Yönetici) > chkdsk C: /f /r komutunu çalıştırın. Disk hatalarını tarar ve düzeltir."),
                ("SFC ve DISM Çalıştır", "CMD (Yönetici) > sfc /scannow, ardından DISM /Online /Cleanup-Image /RestoreHealth komutunu çalıştırın."),
                ("Başlangıç Onarımı", "Ayarlar > Güncelleme ve Güvenlik > Kurtarma > Gelişmiş Başlangıç > Sorun Gider > Başlangıç Onarımı"),
                ("Virüs Taraması", "Windows Defender veya 3. parti antivirüs ile tam sistem taraması yapın."),
                ("Sistem Geri Yükleme", "Oluşturulmuş bir geri yükleme noktası varsa sistemi sorun çıkmadan önceki tarihe geri yükleyin.")
            },
            ["Yavaşlık"] = new()
            {
                ("Görev Yöneticisini Kontrol Et", "Ctrl+Shift+Esc ile Görev Yöneticisini açın. CPU, RAM veya Disk'i %90+ kullanan programları belirleyin."),
                ("Başlangıç Programlarını Düzenle", "Görev Yöneticisi > Başlangıç sekmesinde gereksiz programları devre dışı bırakın."),
                ("Disk Temizliği Yap", "Disk Temizleme aracını çalıştırın. Geçici dosyaları ve önbelleği temizleyin. En az 10GB boş alan bırakın."),
                ("Kötü Amaçlı Yazılım Tara", "Tam sistem taraması yapın. Kötü amaçlı yazılımlar sistemi yavaşlatır."),
                ("RAM Yeterliliğini Kontrol Et", "8GB altı RAM modern kullanım için yetersiz olabilir. Çok sekme açık tutmayın."),
                ("Donanım Termal Durumu", "CPU ve GPU sıcaklıklarını kontrol edin. Aşırı ısınma performans düşüşüne neden olur. Fan ve soğutma sistemini temizleyin.")
            },
            ["Diğer"] = new()
            {
                ("Sorunu Belgele", "Sorunu yaşadığınız an ekran görüntüsü alın veya hata mesajını not edin."),
                ("Yeniden Başlat", "Çoğu geçici sorun için yeniden başlatma çözüm sağlar."),
                ("Son Değişikliği Geri Al", "Son yüklenen program veya güncelleme sonrası başladıysa onu kaldırmayı deneyin."),
                ("Olay Günlüklerini İncele", "Olay Görüntüleyicisi (eventvwr.msc) > Windows Günlükleri > Sistem'de hataları inceleyin."),
                ("Güvenli Modda Test Et", "Güvenli modda sorun yoksa 3. parti yazılım veya sürücü kaynaklıdır."),
                ("BT Desteği Talep Et", "Sorun devam ediyorsa teknik destek talebi oluşturun ve tüm bilgileri eksiksiz iletiniz.")
            }
        };

        public ArizaRehberiView()
        {
            InitializeComponent();
        }

        private void Kategori_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                _aktifKategori = tag;
                _aktifAdim = 0;
                _adimlar = _rehber.ContainsKey(tag) ? _rehber[tag] : _rehber["Diğer"];

                RehberBaslikText.Text = tag;
                KategoriEkrani.Visibility = Visibility.Collapsed;
                RehberEkrani.Visibility = Visibility.Visible;
                TebrikBanner.Visibility = Visibility.Collapsed;
                BtnCozuldu.IsEnabled = true;
                BtnCozulmedi.IsEnabled = true;

                GuncelAdim();
            }
        }

        private void GuncelAdim()
        {
            if (_adimlar.Count == 0) return;
            var adim = _adimlar[_aktifAdim];
            AdimNumaraBadge.Text = (_aktifAdim + 1).ToString();
            AdimNoText.Text = $"Adım {_aktifAdim + 1}";
            AdimToplamText.Text = $" / {_adimlar.Count}";
            AdimProgress.Value = (double)(_aktifAdim + 1) / _adimlar.Count * 100;
            AdimBaslikText.Text = adim.Baslik;
            AdimAciklamaText.Text = adim.Aciklama;
        }

        private void Cozuldu_Click(object sender, RoutedEventArgs e)
        {
            TebrikBanner.Visibility = Visibility.Visible;
            BtnCozuldu.IsEnabled = false;
            BtnCozulmedi.IsEnabled = false;
            AdimProgress.Value = 100;
        }

        private void Cozulmedi_Click(object sender, RoutedEventArgs e)
        {
            if (_aktifAdim < _adimlar.Count - 1)
            {
                _aktifAdim++;
                GuncelAdim();
            }
            else
            {
                // Tüm adımlar bitti, arıza kaydına geç
                var result = MessageBox.Show(
                    "Tüm adımlar denendi ve sorun çözülemedi.\nArıza kaydı oluşturmak ister misiniz?",
                    "Arıza Kaydı", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var mainWindow = Window.GetWindow(this) as MainWindow;
                    mainWindow?.GosteriSayfa("ArizaKaydi", _aktifKategori);
                }
            }
        }

        private void Geri_Click(object sender, RoutedEventArgs e)
        {
            KategoriEkrani.Visibility = Visibility.Visible;
            RehberEkrani.Visibility = Visibility.Collapsed;
            TebrikBanner.Visibility = Visibility.Collapsed;
        }
    }
}
