using System;
using System.Windows;
using System.Windows.Media;

namespace DemoExam.ADOApp
{
    public partial class ClientService
    {
        public TimeSpan TimeLeft => StartTime - DateTime.Now;
        public string TimeLeftDisplay => string.Format(@"{0} дней {1} часов {2}", TimeLeft.Days, TimeLeft.Hours, TimeLeft.Minutes);
        public Visibility ShowTime => TimeLeft.TotalMilliseconds > 0 ? Visibility.Visible : Visibility.Collapsed;
        public Brush TimeLeftBrush => TimeLeft.TotalHours < 1 && ShowTime == Visibility.Visible ? Brushes.Red : Brushes.Transparent;
    }
}
