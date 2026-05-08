using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

        
        private const int CanvasWidth = 900;
        private const int CanvasHeight = 600;

        //listy kontrolek na canvas zeby moc aktualizowac a kazdym ticku
        private List<Ellipse> circleShapes = new List<Ellipse>();
        private List<Line> radiusLines = new List<Line>();
        private Ellipse penDot;
        private Polyline penTrail;

        
        // flagi opcji
        private bool drawCircles = true;
        private bool drawLines = true;

        private bool circleVisible = false;

        public MainWindow()
        {
            InitializeComponent();

            Circles = new ObservableCollection<Circle>
            {
                new Circle { Radius = 200, Frequency = 1 },
                new Circle { Radius = 80, Frequency = -3 },
                new Circle { Radius = 30, Frequency = 5 },
                //new Circle { Radius = 10, Frequency = 1 }
            };

            //subscription do zmian radius/time dla kazdego el
            foreach (var c in Circles)
                c.PropertyChanged += Circle_PropertyChanged;

            timer = new DispatcherTimer(); //tworzymy timer
            timer.Interval = TimeSpan.FromMilliseconds(100); //co 100ms wywoluje event Tick
            timer.Tick += Timer_Tick; //event subscription, dodajemy susbcriber do listy sluchaczy eventu

            stopwatch = new Stopwatch(); //stoper, nie liczy dopoki nie ma start

            DataContext = this;

            
        }

        private void BuildShapes()
        {
            //czyscimy
            PlotterCanvas.Children.Clear();
            circleShapes.Clear();
            radiusLines.Clear();

            //okregi pomocznicze
            foreach (var c in Circles)
            {
                var ellipse = new Ellipse
                {
                    Width = c.Radius * 2,
                    Height = c.Radius * 2,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                circleShapes.Add(ellipse);
                if (drawCircles)
                    PlotterCanvas.Children.Add(ellipse);
            }

            //linie promienie
            foreach (var c in Circles)
            {
                var line = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                radiusLines.Add(line);
                if (drawLines)
                    PlotterCanvas.Children.Add(line);
            }

            //kreska lamana niebieska
            penTrail = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };
            PlotterCanvas.Children.Add(penTrail);

            //pendot czerwony
            penDot = new Ellipse
            {
                Width = 6,
                Height = 6,
                Fill = Brushes.Red
            };
            PlotterCanvas.Children.Add(penDot);
        }

        private void UpdatePositions(double t)
        {
            if (Circles.Count == 0) return;

            //srodek canvasu - first okrag
            Point prev = new Point(PlotterCanvas.ActualWidth / 2, PlotterCanvas.ActualHeight / 2);

            for (int i = 0; i < Circles.Count; i++)
            {
                var c = Circles[i];

                //okrag-pomocniczny,, srodek = prev, lewy gorny rog = prev-r
                Canvas.SetLeft(circleShapes[i], prev.X - c.Radius);
                Canvas.SetTop(circleShapes[i], prev.Y - c.Radius);
                circleShapes[i].Width = c.Radius * 2;
                circleShapes[i].Height = c.Radius * 2;

                //koniec promienia - theta
                double theta = 2 * Math.PI * c.Frequency * (t / TotalSeconds);
                Point next = new Point(
                    prev.X + c.Radius * Math.Cos(theta),
                    prev.Y + c.Radius * Math.Sin(theta)
                );

                //ustawiam linie promiec od prev do next
                radiusLines[i].X1 = prev.X;
                radiusLines[i].Y1 = prev.Y;
                radiusLines[i].X2 = next.X;
                radiusLines[i].Y2 = next.Y;

                prev = next;  //next okrag ma srodek tu
            }

            //pen na koncu ostatniego promienia
            Canvas.SetLeft(penDot, prev.X - penDot.Width / 2);
            Canvas.SetTop(penDot, prev.Y - penDot.Height / 2);

            //aktualna pozycja piora do sladu
            penTrail.Points.Add(prev);
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
                    
                    circleVisible = true;
                }
                return;
            }
            //percent wyliczany na nowo po kazdym ticku, przypisywany na nowo do value
            MainProgressBar.Value = percent;
        }

       


        private void Circle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //tylko jak kolos widoczne
            if (!circleVisible) return;

            
            //i tylko jak zmieni sie pierwszy el kolekcji cicrcles[0]
            if (sender == Circles[0] && e.PropertyName == nameof(Circle.Radius))
            {
                
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
            if (stopwatch.ElapsedMilliseconds == 0)
            {
                //start od nowa
                BuildShapes();
                UpdatePositions(0); //position poczatkowy
            }
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
            stopwatch.Reset();
            MainProgressBar.Value = 0;

            //czyscimy
            PlotterCanvas.Children.Clear();
            circleShapes.Clear();
            radiusLines.Clear();
            penTrail = null;
            penDot = null;
        }
    }
}