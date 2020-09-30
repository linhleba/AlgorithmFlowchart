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

namespace Algorithm_Flowchart
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            gridColor.Background = Brushes.Black;
            switch (index)
            {
                case 1:
                    gridColor.Background = Brushes.Black;
                    break;
                case 2:
                    gridColor.Background = Brushes.CadetBlue;
                    break;
                case 3:
                    gridColor.Background = Brushes.DarkBlue;
                    break;
                case 4:
                    gridColor.Background = Brushes.Firebrick;
                    break;
                case 5:
                    gridColor.Background = Brushes.Gainsboro;
                    break;

            }
        }
    }
}
