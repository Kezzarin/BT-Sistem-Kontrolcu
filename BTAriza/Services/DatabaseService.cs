using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using BTAriza.Models;

namespace BTAriza.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS ArizaKayitlari (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ArizaNo TEXT NOT NULL,
                    AdSoyad TEXT,
                    Departman TEXT,
                    BilgisayarAdi TEXT,
                    Kategori TEXT,
                    Aciklama TEXT,
                    Oncelik INTEGER,
                    IletilenKisi INTEGER,
                    Durum INTEGER,
                    CozumNotu TEXT,
                    SistemBilgisi TEXT,
                    OlusturmaTarihi TEXT
                );";
            cmd.ExecuteNonQuery();
        }

        public void KayitEkle(ArizaKaydi kayit)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO ArizaKayitlari 
                (ArizaNo, AdSoyad, Departman, BilgisayarAdi, Kategori, Aciklama,
                 Oncelik, IletilenKisi, Durum, CozumNotu, SistemBilgisi, OlusturmaTarihi)
                VALUES
                (@ArizaNo, @AdSoyad, @Departman, @BilgisayarAdi, @Kategori, @Aciklama,
                 @Oncelik, @IletilenKisi, @Durum, @CozumNotu, @SistemBilgisi, @OlusturmaTarihi)";

            cmd.Parameters.AddWithValue("@ArizaNo", kayit.ArizaNo);
            cmd.Parameters.AddWithValue("@AdSoyad", kayit.AdSoyad);
            cmd.Parameters.AddWithValue("@Departman", kayit.Departman);
            cmd.Parameters.AddWithValue("@BilgisayarAdi", kayit.BilgisayarAdi);
            cmd.Parameters.AddWithValue("@Kategori", kayit.Kategori);
            cmd.Parameters.AddWithValue("@Aciklama", kayit.Aciklama);
            cmd.Parameters.AddWithValue("@Oncelik", (int)kayit.Oncelik);
            cmd.Parameters.AddWithValue("@IletilenKisi", (int)kayit.IletilenKisi);
            cmd.Parameters.AddWithValue("@Durum", (int)kayit.Durum);
            cmd.Parameters.AddWithValue("@CozumNotu", kayit.CozumNotu ?? string.Empty);
            cmd.Parameters.AddWithValue("@SistemBilgisi", kayit.SistemBilgisi ?? string.Empty);
            cmd.Parameters.AddWithValue("@OlusturmaTarihi", kayit.OlusturmaTarihi.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }

        public List<ArizaKaydi> KayitlariGetir()
        {
            var liste = new List<ArizaKaydi>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM ArizaKayitlari ORDER BY OlusturmaTarihi DESC";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(OkuKayit(reader));
            }
            return liste;
        }

        public void KayitGuncelle(ArizaKaydi kayit)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE ArizaKayitlari SET
                    AdSoyad = @AdSoyad,
                    Departman = @Departman,
                    BilgisayarAdi = @BilgisayarAdi,
                    Kategori = @Kategori,
                    Aciklama = @Aciklama,
                    Oncelik = @Oncelik,
                    IletilenKisi = @IletilenKisi,
                    Durum = @Durum,
                    CozumNotu = @CozumNotu,
                    SistemBilgisi = @SistemBilgisi
                WHERE Id = @Id";

            cmd.Parameters.AddWithValue("@Id", kayit.Id);
            cmd.Parameters.AddWithValue("@AdSoyad", kayit.AdSoyad);
            cmd.Parameters.AddWithValue("@Departman", kayit.Departman);
            cmd.Parameters.AddWithValue("@BilgisayarAdi", kayit.BilgisayarAdi);
            cmd.Parameters.AddWithValue("@Kategori", kayit.Kategori);
            cmd.Parameters.AddWithValue("@Aciklama", kayit.Aciklama);
            cmd.Parameters.AddWithValue("@Oncelik", (int)kayit.Oncelik);
            cmd.Parameters.AddWithValue("@IletilenKisi", (int)kayit.IletilenKisi);
            cmd.Parameters.AddWithValue("@Durum", (int)kayit.Durum);
            cmd.Parameters.AddWithValue("@CozumNotu", kayit.CozumNotu ?? string.Empty);
            cmd.Parameters.AddWithValue("@SistemBilgisi", kayit.SistemBilgisi ?? string.Empty);
            cmd.ExecuteNonQuery();
        }

        public string ArizaNoUret()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM ArizaKayitlari";
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            return $"ARZ-{(count + 1):D3}";
        }

        public int BekliyenSayisi()
        {
            return DurumSayisiGetir(Models.Durum.Bekliyor);
        }

        public int IslemdeSayisi()
        {
            return DurumSayisiGetir(Models.Durum.Islemde);
        }

        public int CozulduSayisi()
        {
            return DurumSayisiGetir(Models.Durum.Cozuldu);
        }

        private int DurumSayisiGetir(Models.Durum durum)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM ArizaKayitlari WHERE Durum = @Durum";
            cmd.Parameters.AddWithValue("@Durum", (int)durum);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<ArizaKaydi> SonKayitlariGetir(int adet = 5)
        {
            var liste = new List<ArizaKaydi>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM ArizaKayitlari ORDER BY OlusturmaTarihi DESC LIMIT {adet}";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(OkuKayit(reader));
            }
            return liste;
        }

        private ArizaKaydi OkuKayit(SqliteDataReader reader)
        {
            return new ArizaKaydi
            {
                Id = reader.GetInt32(0),
                ArizaNo = reader.GetString(1),
                AdSoyad = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Departman = reader.IsDBNull(3) ? "" : reader.GetString(3),
                BilgisayarAdi = reader.IsDBNull(4) ? "" : reader.GetString(4),
                Kategori = reader.IsDBNull(5) ? "" : reader.GetString(5),
                Aciklama = reader.IsDBNull(6) ? "" : reader.GetString(6),
                Oncelik = (Oncelik)(reader.IsDBNull(7) ? 0 : reader.GetInt32(7)),
                IletilenKisi = (IletilenKisi)(reader.IsDBNull(8) ? 0 : reader.GetInt32(8)),
                Durum = (Durum)(reader.IsDBNull(9) ? 0 : reader.GetInt32(9)),
                CozumNotu = reader.IsDBNull(10) ? "" : reader.GetString(10),
                SistemBilgisi = reader.IsDBNull(11) ? "" : reader.GetString(11),
                OlusturmaTarihi = reader.IsDBNull(12) ? DateTime.Now : DateTime.Parse(reader.GetString(12))
            };
        }
    }
}
