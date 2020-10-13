using CopyAndPasteInCanvas;
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
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
        }

        private void TabablzControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DisPlayArrow_Click(object sender, RoutedEventArgs e)
        {

            string pathData = "M 0 115 95 115 65 90 85 90 120 120 85 150 65 150 95 125 0 125 Z";
            System.Windows.Shapes.Path arrow = new System.Windows.Shapes.Path();
            arrow.Fill = Brushes.White;
            arrow.Stroke = Brushes.Black;
            arrow.Data = Geometry.Parse(pathData);
            arrow.IsHitTestVisible = false;
            arrow.Stretch = Stretch.Fill;
            ShapeToolUI shapeUI = new ShapeToolUI();
            shapeUI.DisplayShape(arrow, 70, 140);
        }

        private void DrawByPencil_Click(object sender, RoutedEventArgs e)
        {
            //StreamResourceInfo sri = System.Windows.Application.GetResourceStream(
            //new Uri("/Algorithm Flowchart; ./Resources/Images/pencil.png", UriKind.RelativeOrAbsolute));
            //this.Cursor = new Cursor(sri.Stream);
            InkCanvas inkCanvas = new InkCanvas();
            inkCanvas.Width = 1500;
            inkCanvas.Height = 1500;
            inkCanvas.Background = Brushes.Transparent;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Window1))
                {
                    // Add content control to canvas
                    (window as Window1).Canvas.Children.Add(inkCanvas);
                }
            }
        }
    }
}
