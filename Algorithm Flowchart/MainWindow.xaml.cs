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

        private void DisPlayArrow_Click(object sender, RoutedEventArgs e)
        {

            string pathData = "M 0 115 95 115 65 90 85 90 120 120 85 150 65 150 95 125 0 125 Z";
            System.Windows.Shapes.Path arrow = new System.Windows.Shapes.Path();
            arrow.Fill = Brushes.White;
            arrow.Stroke = Brushes.Black;
            arrow.Data = Geometry.Parse(pathData);
            arrow.IsHitTestVisible = false;
            arrow.Stretch = Stretch.Fill;
            ShapeToolUI shapeUI= new ShapeToolUI();
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
            Canvas.Children.Add(inkCanvas);
        }
    }

}
