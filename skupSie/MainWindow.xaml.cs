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

namespace SkupSieGra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TranslateTransform trans = new TranslateTransform();
            elipsa.RenderTransform = trans;
            DoubleAnimation anim = new DoubleAnimation(29, 300, TimeSpan.FromSeconds(3));
            trans.BeginAnimation(TranslateTransform.YProperty, anim);

            //if(elipsa.Top>=)
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.K)
            {
                gracz1.Opacity = 1;
                gracz2.Opacity = 0;
                //needs to be done : add variable that changes its value when each key is down; this will be used later in collision 
                //eg. if(Key=K) collisionHelper = 0; if key=O then collisionHelper = 1; 
                //each time shape gets to character this is being checked and result is shown in messageBox for now
            }
            if (e.Key == Key.O)
            {
                gracz1.Opacity = 0;
                gracz2.Opacity = 1;
            }
        }
    }
}
