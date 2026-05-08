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

namespace Fourier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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