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

namespace Fourier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Circle> Circles { get; set; }

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

            DataContext = this;
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

        }

        private void PauseButtonItem(object sender, RoutedEventArgs e)
        {

        }

        private void ResetButtonItem(object sender, RoutedEventArgs e)
        {

        }
    }
}