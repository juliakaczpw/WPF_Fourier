using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private Bitmap drawingBitmap;
        private const int CanvasWidth = 900;
        private const int CanvasHeight = 600;

        private bool circleVisible = false;

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

            //subscription do zmian radius/time dla kazdego el
            foreach (var c in Circles)
                c.PropertyChanged += Circle_PropertyChanged;

            timer = new DispatcherTimer(); //tworzymy timer
            timer.Interval = TimeSpan.FromMilliseconds(100); //co 100ms wywoluje event Tick
            timer.Tick += Timer_Tick; //event subscription, dodajemy susbcriber do listy sluchaczy eventu

            stopwatch = new Stopwatch(); //stoper, nie liczy dopoki nie ma start

            DataContext = this;

            //inicjalizujemy bitmapy
            drawingBitmap = new Bitmap(CanvasWidth, CanvasHeight);
            ClearBitmap();
            UpdateImage();
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
                //rysujemy kolo o promieniu pierwszego el z kolekcjio
                if (Circles.Count > 0)
                {
                    ClearBitmap();
                    DrawCircle(Circles[0].Radius);
                    UpdateImage();
                    circleVisible = true;
                }
                return;
            }
            //percent wyliczany na nowo po kazdym ticku, przypisywany na nowo do value
            MainProgressBar.Value = percent;
        }

        private void ClearBitmap()
        {
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.Clear(System.Drawing.Color.White);
            }
        }

        private void DrawCircle(double radius)
        {
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                float r = (float)radius;
                float cx = CanvasWidth / 2f;
                float cy = CanvasHeight / 2f;

                //drawellipse rysuje elipse wpisana w prostokat, wiec daje lewy-gorny rog i jego width/height
                g.DrawEllipse(System.Drawing.Pens.Black, cx - r, cy - r, 2 * r, 2 * r);
            }
        }

        private void UpdateImage()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                drawingBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;

                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.StreamSource = ms;
                bmp.EndInit();
                bmp.Freeze();

                PlotterImage.Source = bmp;
            }
        }

        private void Circle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //tylko jak kolos widoczne
            if (!circleVisible) return;

            
            //i tylko jak zmieni sie pierwszy el kolekcji cicrcles[0]
            if (sender == Circles[0] && e.PropertyName == nameof(Circle.Radius))
            {
                ClearBitmap();
                DrawCircle(Circles[0].Radius);
                UpdateImage();
            }
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

            ClearBitmap();
            UpdateImage();
            circleVisible = false;
        }
    }
}