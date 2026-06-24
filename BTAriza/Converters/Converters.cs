using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using BTAriza.Models;

namespace BTAriza.Converters
{
    public class OncelikRenkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Oncelik oncelik)
            {
                return oncelik switch
                {
                    Oncelik.Dusuk => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#27AE60")),
                    Oncelik.Orta => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F39C12")),
                    Oncelik.Yuksek => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E67E22")),
                    Oncelik.Kritik => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C")),
                    _ => Brushes.Gray
                };
            }
            return Brushes.Gray;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class DurumRenkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Durum durum)
            {
                return durum switch
                {
                    Durum.Bekliyor => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F39C12")),
                    Durum.Islemde => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB")),
                    Durum.Cozuldu => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#27AE60")),
                    _ => Brushes.Gray
                };
            }
            return Brushes.Gray;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && b ? Visibility.Visible : Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && !b ? Visibility.Visible : Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class OncelikStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Oncelik o) return o switch
            {
                Oncelik.Dusuk => "Düşük",
                Oncelik.Orta => "Orta",
                Oncelik.Yuksek => "Yüksek",
                Oncelik.Kritik => "Kritik",
                _ => ""
            };
            return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class DurumStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Durum d) return d switch
            {
                Durum.Bekliyor => "Bekliyor",
                Durum.Islemde => "İşlemde",
                Durum.Cozuldu => "Çözüldü",
                _ => ""
            };
            return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class IletilenStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IletilenKisi i) return i switch
            {
                IletilenKisi.Uzman => "Uzman",
                IletilenKisi.Sef => "Şef",
                IletilenKisi.Mudur => "Müdür",
                _ => ""
            };
            return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
