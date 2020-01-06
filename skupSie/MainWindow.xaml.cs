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
using System.IO;

namespace SkupSieGra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int highestScore;
        bool isCircle;
        bool expectedCircle;
        int score;
        bool begin = true;
        Random random = new Random();
        int interval = 2;   //use to set duration/timeinterval,
        TranslateTransform translateTransform = new TranslateTransform();
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        /// <summary>
        /// Main function of the game which loads highest score, initializes all components and reads player's keyboard input
        /// </summary>
        public MainWindow()
        {
            LoadFromBinary(highestScore);

            InitializeComponent();
            labelHighScore.Content = highestScore;
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        /// <summary>
        /// Occurs when the element is laid out and rendered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetGoal();

            doubleAnimation.From = 29;
            doubleAnimation.To = 300;
            doubleAnimation.Duration = TimeSpan.FromSeconds(interval);

            this.StartGame(sender, e);
        }

        /// <summary>
        /// Function which loads the highest score from locally stored binary file
        /// </summary>
        /// <param name="highScore"></param>
        void LoadFromBinary(int highScore)
        {
            try
            {
                string dir = @"C:\Users\gocek\Source\Repos\jakubg98\skupSie\skupSie\temp";
                string serializationFile = System.IO.Path.Combine(dir, "highScore.bin");

                using (Stream stream = File.Open(serializationFile, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    int score  = (int)bformatter.Deserialize(stream);
                    highestScore = score;
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Unable to load previous data.");
                return;
            }
        }

        /// <summary>
        /// Function which saves the highest score to locally stored binary file
        /// </summary>
        /// <param name="highScore"></param>
        void SaveToBinary(int highScore)
        {

            string dir = @"C:\Users\gocek\Source\Repos\jakubg98\skupSie\skupSie\temp";
            string serializationFile = System.IO.Path.Combine(dir, "highScore.bin");

            //serialize
            using (Stream stream = File.Open(serializationFile, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bformatter.Serialize(stream, highScore);
            }
        }

        /// <summary>
        /// Function which chooses what obstacle will appear at the top of the screen 
        /// </summary>
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

        /// <summary>
        /// Function which starts the timer and animations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGame(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Interval = TimeSpan.FromSeconds(interval);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
            dispatcherTimer.Start();
            dispatcherTimer.Tick += TimerTicked;
        }

        /// <summary>
        /// Function that for each interval of Timer checks whether player should score or lose. Each animation of falling obstacle lasts the same amount of time as interval of Timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TimerTicked(object sender, EventArgs args)
        {
            labelHighScore.Content = highestScore;

            dispatcherTimer.Stop();
            if (expectedCircle != isCircle)
            {
                if (score > highestScore)
                {
                    SaveToBinary(score);
                }
                textBlock.Opacity = 0;
                labelGameOver.Opacity = 1;
                blackBackground.Opacity = 1;
                labelGameOver2.Opacity = 1;
                score = 0;
            }
            else
            {
                score++;
                label.Content = score;

                SetGoal();
                AddNewObject();
            }
        }

        /// <summary>
        /// Function used in animation of falling obstacles
        /// </summary>
        private void AddNewObject()
        {
            translateTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Function that reads user's keyboard input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                case Key.R:
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                    break;
                case Key.Escape:
                    if (score > highestScore)
                    {
                        SaveToBinary(score);
                    }
                    Application.Current.MainWindow.Close();
                    break;
                case Key.Space:
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                    break;
                default:
                    break;                
            }
        }

        /// <summary>
        /// Funcionality for Escape button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (score > highestScore)
            {
                SaveToBinary(score);
            }

            score = 0;
            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// Funcionality for Question Mark button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonQ_Click_1(object sender, RoutedEventArgs e)
        {
            if (textBlock.Opacity == 1)
                textBlock.Opacity = 0;
            else
                textBlock.Opacity = 1;
        }

        /// <summary>
        /// Funcionality for Pause button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            if(playButton.Opacity==0)
            {
                playButton.Opacity = 1;
                dispatcherTimer.Stop();
            }
            else
            {
                playButton.Opacity = 0;
                dispatcherTimer.Start();
            }
        }
    }
}