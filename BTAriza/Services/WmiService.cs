using System;
using System.Collections.Generic;
using System.Management;

namespace BTAriza.Services
{
    public class WmiService
    {
        private string WmiQuery(string query, string property)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject obj in searcher.Get())
                    return obj[property]?.ToString() ?? "N/A";
            }
            catch { }
            return "N/A";
        }

        private List<Dictionary<string, string>> WmiQueryAll(string query, params string[] properties)
        {
            var result = new List<Dictionary<string, string>>();
            try
            {
                using var searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    var dict = new Dictionary<string, string>();
                    foreach (var prop in properties)
                        dict[prop] = obj[prop]?.ToString() ?? "";
                    result.Add(dict);
                }
            }
            catch { }
            return result;
        }

        public string IslemciBilgisi()
        {
            try
            {
                var list = WmiQueryAll("SELECT * FROM Win32_Processor", "Name", "NumberOfCores", "NumberOfLogicalProcessors", "MaxClockSpeed");
                if (list.Count > 0)
                {
                    var p = list[0];
                    return $"{p["Name"]}\n{p["NumberOfCores"]} Çekirdek / {p["NumberOfLogicalProcessors"]} Mantıksal İşlemci\n{p["MaxClockSpeed"]} MHz";
                }
            }
            catch { }
            return "İşlemci bilgisi alınamadı";
        }

        public string RamBilgisi()
        {
            try
            {
                long total = 0;
                var list = WmiQueryAll("SELECT * FROM Win32_PhysicalMemory", "Capacity", "Speed", "Manufacturer");
                foreach (var item in list)
                    if (long.TryParse(item["Capacity"], out long cap)) total += cap;
                double totalGb = total / (1024.0 * 1024 * 1024);
                return $"Toplam RAM: {totalGb:F1} GB ({list.Count} modül)";
            }
            catch { }
            return "RAM bilgisi alınamadı";
        }

        public long ToplamRamMb()
        {
            try
            {
                long total = 0;
                var list = WmiQueryAll("SELECT * FROM Win32_PhysicalMemory", "Capacity");
                foreach (var item in list)
                    if (long.TryParse(item["Capacity"], out long cap)) total += cap;
                return total / (1024 * 1024);
            }
            catch { return 0; }
        }

        public List<Dictionary<string, string>> DiskBilgisi()
        {
            return WmiQueryAll("SELECT * FROM Win32_DiskDrive", "Model", "Size", "InterfaceType", "SerialNumber");
        }

        public string GpuBilgisi()
        {
            try
            {
                var list = WmiQueryAll("SELECT * FROM Win32_VideoController", "Name", "AdapterRAM", "DriverVersion");
                if (list.Count > 0)
                {
                    var g = list[0];
                    long ram = 0;
                    long.TryParse(g["AdapterRAM"], out ram);
                    double ramGb = ram / (1024.0 * 1024 * 1024);
                    return $"{g["Name"]}\nVRAM: {ramGb:F1} GB\nSürücü: {g["DriverVersion"]}";
                }
            }
            catch { }
            return "GPU bilgisi alınamadı";
        }

        public List<Dictionary<string, string>> AgBilgisi()
        {
            return WmiQueryAll(
                "SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled = True",
                "Name", "MACAddress", "Speed");
        }

        public List<Dictionary<string, string>> Suruculeri()
        {
            return WmiQueryAll(
                "SELECT * FROM Win32_PnPSignedDriver",
                "DeviceName", "DriverVersion", "Manufacturer", "DriverDate", "DeviceClass", "IsSigned");
        }

        public string WindowsBilgisi()
        {
            try
            {
                var list = WmiQueryAll("SELECT * FROM Win32_OperatingSystem", "Caption", "Version", "BuildNumber", "OSArchitecture", "LastBootUpTime");
                if (list.Count > 0)
                {
                    var w = list[0];
                    return $"{w["Caption"]}\nSürüm: {w["Version"]} (Build {w["BuildNumber"]})\nMimari: {w["OSArchitecture"]}";
                }
            }
            catch { }
            return "Windows bilgisi alınamadı";
        }

        public string AntivirusDurumu()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    @"root\SecurityCenter2",
                    "SELECT * FROM AntiVirusProduct");
                foreach (ManagementObject obj in searcher.Get())
                {
                    var name = obj["displayName"]?.ToString() ?? "Bilinmiyor";
                    var state = obj["productState"]?.ToString() ?? "0";
                    int stateInt = int.Parse(state);
                    bool aktif = ((stateInt >> 12) & 0xF) == 1;
                    bool güncel = ((stateInt >> 4) & 0xF) == 0;
                    return $"{name}\nDurum: {(aktif ? "Aktif ✓" : "Pasif ✗")}\nGüncel: {(güncel ? "Evet ✓" : "Hayır ✗")}";
                }
            }
            catch { }
            return "Antivirüs bilgisi alınamadı";
        }
    }
}
