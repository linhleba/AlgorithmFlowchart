using Algorithm_Flowchart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;


namespace CopyAndPasteInCanvas

{

    public partial class Window1

    {
        public Window1()

        {
            InitializeComponent();
            DataContext = new ShapeDesigner().Canvas;
        }
           

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }



        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();

        }
        private void File_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                case 1:
                    this.Canvas.Background = Brushes.Black;
                    break;
                case 2:
                    this.Canvas.Background = Brushes.Red;
                    break;
                case 3:
                    this.Canvas.Background = Brushes.Green;
                    break;
                case 4:
                    this.Canvas.Background = Brushes.Gray;
                    break;
                case 5:
                    this.Canvas.Background = Brushes.AliceBlue;
                    break;
            }
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                case 1:
                    this.Canvas.Background = Brushes.Black;
                    break;
                case 2:
                    this.Canvas.Background = Brushes.Red;
                    break;
                case 3:
                    this.Canvas.Background = Brushes.Green;
                    break;
                case 4:
                    this.Canvas.Background = Brushes.Gray;
                    break;
                case 5:
                    this.Canvas.Background = Brushes.AliceBlue;
                    break;
                case 6:
                    this.Canvas.Background = Brushes.YellowGreen;
                    break;
                case 7:
                    this.Canvas.Background = Brushes.Purple;
                    break;
            }
        }
        private void View_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                case 1:
                    this.Canvas.Background = Brushes.Black;
                    break;
                case 2:
                    this.Canvas.Background = Brushes.Red;
                    break;
                case 3:
                    this.Canvas.Background = Brushes.Green;
                    break;
            }
        }
        private void Insert_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                case 1:
                    this.Canvas.Background = Brushes.Black;
                    break;
                case 2:
                    this.Canvas.Background = Brushes.Red;
                    break;
                case 3:
                    this.Canvas.Background = Brushes.Green;
                    break;
                case 4:
                    this.Canvas.Background = Brushes.Gray;
                    break;
            }
        }
        private void Tool_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                case 1:
                    this.Canvas.Background = Brushes.Black;
                    break;
                case 2:
                    this.Canvas.Background = Brushes.Red;
                    break;
            }
        }
        private void Help_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                case 1:
                    this.Canvas.Background = Brushes.Black;
                    break;
                case 2:
                    this.Canvas.Background = Brushes.Red;
                    break;
                case 3:
                    this.Canvas.Background = Brushes.Green;
                    break;
            }
        }

        private void DisplayCircle(object sender, RoutedEventArgs e)
        {

        }
    }

}
