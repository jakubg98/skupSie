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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SkupSieGra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isCircle;
        bool expectedCircle;
        int score;
        bool begin = true;
        Random random = new Random();
        int interval = 2;   //use to set duration/timeinterval,
        TranslateTransform translateTransform = new TranslateTransform();
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetGoal();

            doubleAnimation.From = 29;
            doubleAnimation.To = 300;
            doubleAnimation.Duration = TimeSpan.FromSeconds(interval);

            this.StartGame(sender, e);
        }

        public void SetGoal()
        {
            bool randomBool = random.Next(0, 2) > 0;
            if (randomBool)
            {
                expectedCircle = true;
                elipsa.RenderTransform = translateTransform;
                elipsa.Visibility = Visibility.Visible;
                kwadrat.Visibility = Visibility.Collapsed;
            }
            else
            {
                expectedCircle = false;
                kwadrat.RenderTransform = translateTransform;
                elipsa.Visibility = Visibility.Collapsed;
                kwadrat.Visibility = Visibility.Visible;
            }
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Interval = TimeSpan.FromSeconds(interval);

            translateTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
            dispatcherTimer.Start();
            expectedCircle = true;

            dispatcherTimer.Tick += TimerTicked;
        }

        private void TimerTicked(object sender, EventArgs args)
        {
            dispatcherTimer.Stop();
            if (expectedCircle != isCircle)
            {
                MessageBox.Show("game over");
                score = 0;
                Application.Current.MainWindow.Close();
            }

            score++;
            label.Content = score;
            SetGoal();
            AddNewObject();
        }

        private void AddNewObject()
        {
            translateTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
            dispatcherTimer.Start();
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    gracz1.Opacity = 1;
                    gracz2.Opacity = 0;
                    isCircle = false;
                    break;
                case Key.Down:
                    gracz1.Opacity = 0;
                    gracz2.Opacity = 1;
                    isCircle = true;
                    break;
                default:
                    break;
            }
        }

    }
}