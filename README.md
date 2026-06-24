# 🔧 BT Arıza Takip Sistemi

<div align="center">

![Python](https://img.shields.io/badge/Python-3.11%2B-blue?style=for-the-badge&logo=python)
![CustomTkinter](https://img.shields.io/badge/CustomTkinter-5.2%2B-orange?style=for-the-badge)
![SQLite](https://img.shields.io/badge/SQLite-3-green?style=for-the-badge&logo=sqlite)
![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey?style=for-the-badge&logo=windows)
![License](https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge)

**Küçük ve orta ölçekli şirketler için BT arıza takip ve yönetim uygulaması.**  
Kullanıcılar arıza kaydı oluşturur, adminler tüm kayıtları yönetir.

[Özellikler](#-özellikler) • [Kurulum](#-kurulum) • [Kullanım](#-kullanım) • [Ekran Görüntüleri](#-ekran-görüntüleri) • [Katkı](#-katkıda-bulunma)

</div>

---

## 📋 İçindekiler

- [Proje Hakkında](#-proje-hakkında)
- [Özellikler](#-özellikler)
- [Gereksinimler](#-gereksinimler)
- [Kurulum](#-kurulum)
  - [EXE ile Kurulum (Kolay)](#1-exe-ile-kurulum-önerilen)
  - [Kaynak Koddan Çalıştırma](#2-kaynak-koddan-çalıştırma)
  - [EXE Derleme](#3-exe-derleme-geliştiriciler-için)
- [Kullanım](#-kullanım)
  - [Admin Paneli](#-admin-paneli)
  - [Kullanıcı Paneli](#-kullanıcı-paneli)
- [Varsayılan Hesaplar](#-varsayılan-hesaplar)
- [Proje Yapısı](#-proje-yapısı)
- [Veritabanı Şeması](#-veritabanı-şeması)
- [Katkıda Bulunma](#-katkıda-bulunma)
- [Lisans](#-lisans)

---

## 🎯 Proje Hakkında

BT Arıza Takip Sistemi, şirket içi BT birimlerinin arıza yönetimini kolaylaştırmak için geliştirilmiş masaüstü uygulamasıdır. **İki ayrı EXE** ile çalışır:

| Uygulama | Hedef Kitle | Açıklama |
|----------|-------------|----------|
| `BTAriza_Admin.exe` | BT Yöneticileri | Tüm arızaları görür, yönetir, raporlar |
| `BTAriza_Kullanici.exe` | Son Kullanıcılar | Arıza kaydı oluşturur, kendi kayıtlarını takip eder |

Her iki uygulama aynı `ariza.db` SQLite veritabanını paylaşır. Ağ üzerinden ortak bir klasöre konulduğunda tüm şirket genelinde kullanılabilir.

---

## ✨ Özellikler

### 👥 Kullanıcı Paneli
- 🔐 **Güvenli giriş** — kullanıcı adı ve şifre ile kimlik doğrulama
- 📝 **Arıza kaydı oluşturma** — departman, kategori, öncelik seçimi
- 📖 **Adım adım arıza rehberi** — 7 kategori, her biri 6 çözüm adımı
- 📋 **Kayıtlarım** — sadece kendi kayıtlarını filtreleyerek görme
- 💻 **Sistem bilgisi** — CPU, RAM, disk, OS bilgileri

### ⚙️ Admin Paneli
- 📊 **Dashboard** — istatistik kartları (toplam/bekleyen/işlemde/çözülen/kritik) + son 10 arıza
- 📋 **Tüm arızalar** — gelişmiş filtreler (durum/öncelik/departman/arama), renk kodlu satırlar
- ✏️ **Arıza güncelleme** — çift tıkla detay penceresi, durum değiştirme, çözüm notu ekleme
- 🗑️ **Arıza silme** — onay dialogu ile güvenli silme
- 📤 **CSV dışa aktarma** — filtrelenmiş kayıtları CSV olarak kaydetme
- 👥 **Kullanıcı yönetimi** — kullanıcı ekleme/silme, rol atama (admin/kullanici)
- 📈 **İstatistikler** — öncelik, durum ve kategori dağılımı (progress bar grafikleri)
- 💾 **Veritabanı yönetimi** — yedekleme ve geri yükleme
- 🔑 **Şifre yönetimi** — admin şifresini değiştirme

### 🎨 Arayüz
- Modern dark tema (özel renk paleti)
- Renkli satır vurgulama (kritik: kırmızı, çözüldü: yeşil, işlemde: mavi)
- Öncelik renk kodları (Düşük: yeşil / Orta: sarı / Yüksek: turuncu / Kritik: kırmızı)
- Canlı saat ve bilgisayar adı üst çubukta
- Scrollable sayfa yapısı

---

## 💻 Gereksinimler

### EXE Kullanımı İçin
- **İşletim Sistemi:** Windows 10 / 11 (64-bit)
- **Ek kurulum gerekmez** — EXE tüm bağımlılıkları içerir

### Kaynak Koddan Çalıştırma İçin
- **Python:** 3.11 veya üzeri
- **Kütüphaneler:**

```
customtkinter>=5.2.2
Pillow>=10.0.0
psutil>=5.9.0       # Sistem bilgisi için (opsiyonel)
```
---
[Ekran Görüntüleri](#-ekran-görüntüleri)
https://i.hizliresim.com/79cbovz.png
https://i.hizliresim.com/29s3g63.png
---

## 🚀 Kurulum

### 1. EXE ile Kurulum (Önerilen)

En kolay yöntem. Python veya herhangi bir kurulum gerektirmez.

**Adım 1:** Releases sayfasından son sürümü indirin:
```
BTAriza_Admin.exe
BTAriza_Kullanici.exe
```

**Adım 2:** İki dosyayı **aynı klasöre** koyun:
```
📁 BTAriza/
├── BTAriza_Admin.exe
├── BTAriza_Kullanici.exe
└── ariza.db          ← İlk çalıştırmada otomatik oluşur
```

**Adım 3:** İlgili EXE'yi çift tıklayarak başlatın.

> ⚠️ **Önemli:** İki EXE mutlaka aynı klasörde bulunmalıdır.  
> `ariza.db` dosyası EXE ile aynı dizinde oluşur ve paylaşılır.

---

### Ağ Üzerinde Paylaşımlı Kullanım

Tüm şirket aynı veritabanını kullanabilir:

1. `BTAriza_Admin.exe` ve `BTAriza_Kullanici.exe` dosyalarını bir **ağ klasörüne** koyun (örn. `\\sunucu\BT\BTAriza\`)
2. Kullanıcılar `BTAriza_Kullanici.exe`'ye kısayol oluştursun
3. BT yöneticisi `BTAriza_Admin.exe`'yi çalıştırsın
4. `ariza.db` paylaşımlı klasörde otomatik oluşur

> 💡 SQLite eş zamanlı yazma konusunda sınırlıdır. Aynı anda çok sayıda kullanıcı kayıt oluşturuyorsa PostgreSQL gibi bir veritabanına geçiş önerilir.

---

### 2. Kaynak Koddan Çalıştırma

**Adım 1:** Repoyu klonlayın:
```bash
git clone https://github.com/kezzarin/bt-ariza-takip.git
cd bt-ariza-takip
```

**Adım 2:** Sanal ortam oluşturun (önerilir):
```bash
python -m venv venv

# Windows
venv\Scripts\activate

# Linux/macOS
source venv/bin/activate
```

**Adım 3:** Bağımlılıkları yükleyin:
```bash
pip install -r requirements.txt
```

**Adım 4:** Uygulamayı başlatın:
```bash
# Admin paneli
python admin_app.py

# Kullanıcı paneli
python kullanici_app.py
```

---

### 3. EXE Derleme (Geliştiriciler İçin)

```bash
# Bağımlılıkları yükle
pip install pyinstaller customtkinter pillow psutil

# Admin EXE
python -m PyInstaller --clean --onefile --windowed \
  --name "BTAriza_Admin" \
  --add-data "database.py;." \
  admin_app.py

# Kullanıcı EXE
python -m PyInstaller --clean --onefile --windowed \
  --name "BTAriza_Kullanici" \
  --add-data "database.py;." \
  kullanici_app.py
```

Derlenmiş dosyalar `dist/` klasöründe oluşur.

---

## 📖 Kullanım

### 🔐 İlk Giriş

Her iki uygulama da başlarken bir giriş ekranı gösterir.

**Admin girişi:**
```
Kullanıcı Adı : admin
Şifre         : admin123
```

**Kullanıcı girişi:**
```
Kullanıcı Adı : kullanici
Şifre         : 123456
```

> 🔒 İlk kullanımda admin şifresini mutlaka değiştirin:  
> `Admin Paneli → Ayarlar → Admin Şifresi Değiştir`

---

### ⚙️ Admin Paneli

#### Dashboard
Ana ekranda 5 istatistik kartı görünür:

| Kart | Renk | Açıklama |
|------|------|----------|
| Toplam | Mavi | Sistemdeki toplam arıza sayısı |
| Bekliyor | Sarı | Henüz işlem yapılmayan arızalar |
| İşlemde | Mavi | Üzerinde çalışılan arızalar |
| Çözüldü | Yeşil | Kapatılmış arızalar |
| Kritik | Kırmızı | Kritik öncelikli açık arızalar |

Alt kısımda son 10 arıza kaydı tablo halinde listelenir. **Çift tıklayarak** detay penceresini açabilirsiniz.

---

#### Tüm Arızalar

Tüm arızaları listeler ve yönetir.

**Filtreler:**
- 🔍 Metin arama (arıza no, ad soyad, kategori, açıklama)
- Durum filtresi (Bekliyor / İşlemde / Çözüldü)
- Öncelik filtresi (Düşük / Orta / Yüksek / Kritik)
- Departman filtresi

**Satır renkleri:**
- 🔴 Kırmızı arka plan → Kritik öncelikli
- 🟢 Yeşil metin → Çözüldü
- 🔵 Mavi metin → İşlemde

**Arıza detayı / güncelleme:**
1. İstenen satıra **çift tıklayın**
2. Açılan pencerede mevcut bilgiler görünür
3. **Durum** açılır menüsünden yeni durumu seçin
4. Durum "Çözüldü" seçilirse **Çözüm Notu** alanı açılır
5. **💾 Kaydet** butonuna tıklayın

**CSV Export:**
Sayfanın sağ üstündeki **📤 CSV Export** butonu, o an filtreli görünen kayıtları CSV dosyası olarak kaydeder.

---

#### Kullanıcı Yönetimi

| İşlem | Nasıl |
|-------|-------|
| Yeni kullanıcı ekle | `+ Yeni Kullanıcı` butonu |
| Kullanıcı sil | Tablodan seçip `🗑 Seçiliyi Sil` |
| Rol atama | Kullanıcı eklerken `kullanici` veya `admin` seç |

> ⚠️ Varsayılan `admin` kullanıcısı silinemez.

---

#### İstatistikler

Grafiksel dağılım sayfası:
- **Öncelik Dağılımı** — Düşük/Orta/Yüksek/Kritik yüzdesi
- **Durum Dağılımı** — Bekliyor/İşlemde/Çözüldü yüzdesi
- **Kategori Dağılımı** — En çok arıza yaşanan kategoriler
- **Son 5 Kayıt** — Hızlı erişim listesi

---

#### Ayarlar

| Özellik | Açıklama |
|---------|----------|
| Veritabanı Yolu | Mevcut `ariza.db` konumu |
| 💾 Yedekle | Veritabanını `.db` dosyası olarak kaydet |
| 📂 Geri Yükle | Önceki yedekten geri yükle |
| Şifre Değiştir | Admin hesabının şifresini güncelle |

---

### 👤 Kullanıcı Paneli

#### Arıza Kaydı

Yeni arıza oluşturmak için formu doldurun:

| Alan | Zorunlu | Açıklama |
|------|---------|----------|
| Ad Soyad | ✅ | Giriş yapan kullanıcının adı otomatik gelir |
| Departman | ✅ | IT / Muhasebe / İK / Satış / Yönetim / Diğer |
| Bilgisayar Adı | ❌ | Otomatik doldurulur, değiştirilebilir |
| Kategori | ✅ | 7 kategori seçeneği |
| Açıklama | ✅ | Arızanın detaylı açıklaması |
| Öncelik | ❌ | Düşük / Orta / Yüksek / Kritik (varsayılan: Orta) |
| İletilecek Kişi | ❌ | Uzman / Şef / Müdür (varsayılan: Uzman) |

**💾 Kaydet** butonuna tıkladığınızda kayıt oluşturulur ve `ARZ-XXX` formatında arıza numarası atanır.

---

#### Arıza Rehberi

Sorun çözümünde adım adım rehberlik:

1. **Kategori seçin** (7 kart arasından)
2. Her adımda açıklanan işlemi uygulayın
3. Sorunu çözdüyseniz **✓ Çözüldü** butonuna tıklayın
4. Çözülemediyse **✗ Çözülmedi** ile sonraki adıma geçin
5. Tüm adımlar denendiyse otomatik olarak Arıza Kaydı sayfasına yönlendirilirsiniz

**Desteklenen kategoriler:**

| Kategori | Adım Sayısı |
|----------|-------------|
| Bilgisayar Açılmıyor | 6 |
| Ekran Sorunu | 6 |
| İnternet Sorunu | 6 |
| Yazıcı Sorunu | 6 |
| Windows Sorunu | 6 |
| Yavaşlık | 6 |
| Diğer | 6 |

---

#### Kayıtlarım

Sadece kendi oluşturduğunuz arızaları görür:
- Durum filtresi ile arama
- Metin ile arama
- **Çift tıklayarak** detay penceresi açma
- Çözüm notu varsa yeşil renkte gösterilir

---

#### Sistem Bilgisi

Bilgisayarınızın anlık durumu:
- Bilgisayar adı ve kullanıcı
- İşletim sistemi ve versiyon
- İşlemci adı ve kullanım yüzdesi
- RAM kullanımı
- Disk kullanımı
- Python sürümü

---

## 🔑 Varsayılan Hesaplar

| Rol | Kullanıcı Adı | Şifre | Uygulama |
|-----|---------------|-------|----------|
| Admin | `admin` | `admin123` | BTAriza_Admin.exe |
| Kullanıcı | `kullanici` | `123456` | BTAriza_Kullanici.exe |

> 🔐 **Güvenlik notu:** Canlı ortama geçmeden önce varsayılan şifreleri değiştirin!

---

## 📁 Proje Yapısı

```
bt-ariza-takip/
│
├── admin_app.py          # Admin uygulaması (giriş + tüm sayfalar)
├── kullanici_app.py      # Kullanıcı uygulaması (giriş + tüm sayfalar)
├── database.py           # SQLite veritabanı katmanı (CRUD işlemleri)
├── theme.py              # Renk sabitleri (referans)
│
├── requirements.txt      # Python bağımlılıkları
├── README.md             # Bu dosya
│
├── dist/                 # Derlenmiş EXE dosyaları (PyInstaller çıktısı)
│   ├── BTAriza_Admin.exe
│   └── BTAriza_Kullanici.exe
│
├── build/                # PyInstaller geçici dosyaları (git'e ekleme)
└── ariza.db              # SQLite veritabanı (çalışma zamanında oluşur)
```

---

## 🗄️ Veritabanı Şeması

### `kullanicilar` Tablosu

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| `id` | INTEGER PK | Otomatik artan ID |
| `kullanici_adi` | TEXT UNIQUE | Giriş için kullanıcı adı |
| `sifre` | TEXT | Şifre (düz metin — üretim için hash önerilir) |
| `rol` | TEXT | `admin` veya `kullanici` |
| `ad_soyad` | TEXT | Kullanıcının tam adı |

### `arizalar` Tablosu

| Kolon | Tip | Açıklama |
|-------|-----|----------|
| `id` | INTEGER PK | Otomatik artan ID |
| `ariza_no` | TEXT | `ARZ-001` formatında arıza numarası |
| `ad_soyad` | TEXT | Arızayı bildiren kişi |
| `departman` | TEXT | IT, Muhasebe, İK, Satış, Yönetim, Diğer |
| `bilgisayar_adi` | TEXT | Hostname |
| `kategori` | TEXT | Arıza kategorisi |
| `aciklama` | TEXT | Detaylı açıklama |
| `oncelik` | TEXT | Düşük / Orta / Yüksek / Kritik |
| `iletilen` | TEXT | Uzman / Şef / Müdür |
| `durum` | TEXT | Bekliyor / İşlemde / Çözüldü |
| `cozum_notu` | TEXT | Admin tarafından eklenen çözüm notu |
| `sistem_bilgisi` | TEXT | Opsiyonel sistem bilgisi |
| `olusturma_tarihi` | TEXT | `YYYY-MM-DD HH:MM:SS` formatı |

---

## 🛠️ Geliştirme

### requirements.txt

```
customtkinter==5.2.2
Pillow==10.4.0
psutil==6.0.0
pyinstaller==6.0.0
```

### Yeni Özellik Eklemek

1. **Yeni sayfa eklemek** (`admin_app.py`):
   ```python
   class PageYeniSayfa(ctk.CTkFrame):
       def __init__(self, parent):
           super().__init__(parent, fg_color=BG)
           self._build()
       def _build(self):
           # Sayfanızı buraya inşa edin
           pass
   ```
   Sonra `_goto` metodundaki `pages` sözlüğüne ekleyin ve sidebar menüsüne bir buton ekleyin.

2. **Yeni veritabanı fonksiyonu** (`database.py`):
   ```python
   def yeni_sorgu():
       conn = get_conn()
       c = conn.cursor()
       c.execute("SELECT ...")
       rows = c.fetchall()
       conn.close()
       return [dict(r) for r in rows]
   ```

3. **Yeni kullanıcı eklemek** (çalışma zamanında):
   `Admin Paneli → Kullanıcı Yönetimi → + Yeni Kullanıcı`

---

## 🐛 Sık Karşılaşılan Sorunlar

### EXE açılmıyor / hata veriyor
- Windows Defender veya antivirüs EXE'yi engelliyor olabilir → İstisna tanımlayın
- `ariza.db` dosyasının yazma izni olmayan bir klasördeyse sorun çıkabilir → EXE'yi masaüstüne veya yetkili bir klasöre taşıyın

### Giriş yapılamıyor
- Varsayılan bilgileri kullanın: `admin / admin123`
- `ariza.db` dosyası silinmişse → EXE'yi yeniden çalıştırın, otomatik oluşur

### Tablo (DataGrid) görünmüyor
- İki EXE'nin aynı dizinde olduğundan emin olun
- `ariza.db` dosyasının mevcut olduğunu kontrol edin

### Sistem bilgisi "psutil modülü gerekli" diyor
- Kaynak koddan çalıştırıyorsanız: `pip install psutil`
- EXE kullanıyorsanız: psutil EXE içine dahildir, sorun çıkmamalıdır

---

## 🤝 Katkıda Bulunma

Katkılarınızı memnuniyetle karşılıyoruz!

1. Repoyu fork edin
2. Feature branch oluşturun: `git checkout -b feature/yeni-ozellik`
3. Değişikliklerinizi commit edin: `git commit -m 'feat: yeni özellik eklendi'`
4. Branch'i push edin: `git push origin feature/yeni-ozellik`
5. Pull Request açın

### Commit Mesajı Formatı

```
feat: yeni özellik
fix: hata düzeltmesi
docs: dokümantasyon güncellemesi
style: kod formatı
refactor: kod yeniden düzenleme
test: test ekleme
```

---

## 📊 Yol Haritası

- [ ] Şifre hash'leme (bcrypt)
- [ ] E-posta bildirimleri (kritik arızalarda)
- [ ] Excel/PDF rapor çıktısı
- [ ] Arıza fotoğrafı ekleme
- [ ] Arıza geçmişi / aktivite logu
- [ ] Çoklu dil desteği (EN/TR)
- [ ] Web arayüzü (Flask/FastAPI)
- [ ] PostgreSQL desteği (büyük ölçekli kullanım)
- [ ] Mobil uygulama (REST API üzerinden)

---

## 📜 Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.

```
MIT License

Copyright (c) 2025

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction...
```

---

## 📞 İletişim

Sorularınız için: stajabdul@gmail.com
- GitHub Issues açabilirsiniz
- Pull Request gönderebilirsiniz

---

<div align="center">

**⭐ Projeyi beğendiyseniz yıldız vermeyi unutmayın!**


</div>
---

<div align="center">

**👨‍💻 Bu proje Kezzarin tarafından geliştirilmiştir.**

Made with ❤️ using Python & CustomTkinter

</div>
