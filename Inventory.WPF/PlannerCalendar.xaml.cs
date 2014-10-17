using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory.WPF
{
    /// <summary>
    /// Interaction logic for PlannerCalendar.xaml
    /// </summary>
    public partial class PlannerCalendar : UserControl
    {

        public List<Day> Days { get; set; }
        public List<string> DayNames { get; set; }
        public DateTime CurrentDate { get; set; }

        public PlannerCalendar()
        {
            InitializeComponent();

            this.DataContext = this;
            CurrentDate = DateTime.Today;
            DayNames = new List<string> { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            Days = new List<Day>();
            BuildCalendar(DateTime.Today);
        }

        public void BuildCalendar(DateTime targetDate)
        {
            Days.Clear();

            //Calculate when the first day of the month is and work out an 
            //offset so we can fill in any boxes before that.
            DateTime d = new DateTime(targetDate.Year, targetDate.Month, 1);
            int offset = DayOfWeekNumber(d.DayOfWeek);
            if (offset != 1) d = d.AddDays(-offset);

            //Show 6 weeks each with 7 days = 42
            for (int box = 1; box <= 42; box++)
            {
                Day day = new Day();
                day.Date = d;
                day.IsTargetMonth = (d.Month == DateTime.Today.Month);
                Days.Add(day);
                d = d.AddDays(1);
            }
        }

        private static int DayOfWeekNumber(DayOfWeek dow)
        {
            return Convert.ToInt32(dow.ToString("D"));
        }
    }

    public class Day
    {
        public bool IsTargetMonth { get; set; }
        public DateTime Date { get; set; }
    }
}
