using DayShift_Overview_Kaufland.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DayShift_Overview_Kaufland;

public partial class MainWindow : Window
{
    public List<User>? users;
    public ObservableCollection<Shift> Shifts { get; set; } = new ObservableCollection<Shift>();

    public MainWindow()
    {
        InitializeComponent();
        this.WindowState = WindowState.Maximized;
        DataContext = this;
    }

    private void Date_Change(object sender, SelectionChangedEventArgs e)
    {
        DateTime? date = DatePicker.SelectedDate;
        if (date.HasValue)
        {
            DateTime selectedDate = date.Value;
            FetchShiftsByDate(selectedDate);
        }
    }

    private void FetchShiftsByDate(DateTime selectedDate)
    {
        var shiftCollection = App.Database.GetCollection<Shift>("shifts");

        var startOfDayUTC = selectedDate.Date.ToUniversalTime();
        var endOfDayUTC = startOfDayUTC.AddDays(1).AddSeconds(-1);

        var filter = Builders<Shift>.Filter.Gte(s => s.ShiftDate, startOfDayUTC) &
                     Builders<Shift>.Filter.Lte(s => s.ShiftDate, endOfDayUTC);

        var shifts = shiftCollection.Find(filter).ToList();

        Shifts.Clear();
        foreach (var shift in shifts)
        {
            Shifts.Add(shift);
        }

        MessageBox.Show($"Found {shifts.Count} shifts for {selectedDate.ToShortDateString()}");
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
    }


}