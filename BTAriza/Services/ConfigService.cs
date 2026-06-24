using System;
using System.IO;
using System.Text.Json;

namespace BTAriza.Services
{
    public class AppConfig
    {
        public string SirketAdi { get; set; } = "Şirket Adı";
        public string VeritabaniYolu { get; set; } = "ariza.db";
        public string UzmanAdi { get; set; } = "Uzman Adı";
        public string SefAdi { get; set; } = "Şef Adı";
        public string MudurAdi { get; set; } = "Müdür Adı";
        public string Tema { get; set; } = "Dark";
        public string LogoYolu { get; set; } = "";
    }

    public static class ConfigService
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private static AppConfig? _config;

        public static AppConfig Config
        {
            get
            {
                if (_config == null) Yukle();
                return _config!;
            }
        }

        public static void Yukle()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    var json = File.ReadAllText(ConfigPath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    _config = JsonSerializer.Deserialize<AppConfig>(json, options) ?? new AppConfig();
                }
                else
                {
                    _config = new AppConfig();
                }
            }
            catch
            {
                _config = new AppConfig();
            }
        }

        public static void Kaydet()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_config, options);
                File.WriteAllText(ConfigPath, json);
            }
            catch { }
        }
    }
}
