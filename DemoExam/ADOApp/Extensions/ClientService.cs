using System;
using System.Windows.Media;

namespace DemoExam.ADOApp
{
    public partial class ClientService
    {
        public TimeSpan TimeLeft => StartTime - DateTime.Now;
        public string TimeLeftDisplay => string.Format(@"{0} часов {1} минут", TimeLeft.Hours, TimeLeft.Minutes);
        public Brush TimeLeftBrush => TimeLeft.TotalHours < 1 ? Brushes.Red : Brushes.Transparent;
        public Brush Foreground => TimeLeftBrush == Brushes.Red ? Brushes.White : Brushes.Black;
    }
}
