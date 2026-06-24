using System;

namespace BTAriza.Models
{
    public class ArizaKaydi
    {
        public int Id { get; set; }
        public string ArizaNo { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public string Departman { get; set; } = string.Empty;
        public string BilgisayarAdi { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public Oncelik Oncelik { get; set; } = Oncelik.Dusuk;
        public IletilenKisi IletilenKisi { get; set; } = IletilenKisi.Uzman;
        public Durum Durum { get; set; } = Durum.Bekliyor;
        public string CozumNotu { get; set; } = string.Empty;
        public string SistemBilgisi { get; set; } = string.Empty;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        public string OncelikDisplay => Oncelik switch
        {
            Oncelik.Dusuk => "Düşük",
            Oncelik.Orta => "Orta",
            Oncelik.Yuksek => "Yüksek",
            Oncelik.Kritik => "Kritik",
            _ => "Bilinmiyor"
        };

        public string DurumDisplay => Durum switch
        {
            Durum.Bekliyor => "Bekliyor",
            Durum.Islemde => "İşlemde",
            Durum.Cozuldu => "Çözüldü",
            _ => "Bilinmiyor"
        };

        public string IletilenKisiDisplay => IletilenKisi switch
        {
            IletilenKisi.Uzman => "Uzman",
            IletilenKisi.Sef => "Şef",
            IletilenKisi.Mudur => "Müdür",
            _ => "Bilinmiyor"
        };
    }
}
