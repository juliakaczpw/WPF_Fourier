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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace Fourier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Circle> Circles { get; set; }

        private DispatcherTimer timer;
        private Stopwatch stopwatch;
        private const double TotalSeconds = 10.0;

        public MainWindow()
        {
            InitializeComponent();

            Circles = new ObservableCollection<Circle>
            {
                new Circle { Radius = 100, Time = 1 },
                new Circle { Radius = 5, Time = 1.54 },
                new Circle { Radius = 100, Time = 0.1 },
                new Circle { Radius = 10, Time = 1 }
            };

            timer = new DispatcherTimer(); //tworzymy timer
            timer.Interval = TimeSpan.FromMilliseconds(100); //co 100ms wywoluje event Tick
            timer.Tick += Timer_Tick; //event subscription, dodajemy susbcriber do listy sluchaczy eventu

            stopwatch = new Stopwatch(); //stoper, nie liczy dopoki nie ma start

            DataContext = this;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
            double elapsed = stopwatch.Elapsed.TotalSeconds; //elapsed zwraca przedzial czasu (timespan), totaseconds daje ten przedzial jako double
            double percent = (elapsed / TotalSeconds) * 100.0;

            if (percent >= 100.0)
            {
                MainProgressBar.Value = 100.0;
                timer.Stop();
                stopwatch.Stop();
                return;
            }
            //percent wyliczany na nowo po kazdym ticku, przypisywany na nowo do value
            MainProgressBar.Value = percent;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        private void NewMenuItem(object sender, RoutedEventArgs e)
        {

        }

        private void OpenMenuItem(object sender, RoutedEventArgs e)
        {

        }

        private void SaveMenuItem(object sender, RoutedEventArgs e)
        {

        }

        private void StartButtonItem(object sender, RoutedEventArgs e)
        {
            timer.Start();
            stopwatch.Start();

        }

        private void PauseButtonItem(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            stopwatch.Stop();

        }

        private void ResetButtonItem(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            stopwatch.Reset(); //zeruje nie wznawia
            MainProgressBar.Value = 0; //pasek na zero
        }
    }
}