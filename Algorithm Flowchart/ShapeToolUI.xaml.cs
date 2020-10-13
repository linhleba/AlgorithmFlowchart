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
    /// Interaction logic for ShapeToolUI.xaml
    /// </summary>
    public partial class ShapeToolUI : UserControl
    {
        public ShapeToolUI()
        {
            InitializeComponent();
        }

        // Display shape in the main area, parameters -> shape, height and width
        private void DisplayShape(Shape shape,int height, int width)
        {
            var contentControl = new ContentControl();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Window1))
                {

                    // Set content control 
                    contentControl.Content = shape;
                    contentControl.Template = contentControl.FindResource("DesignShape") as ControlTemplate;
                    contentControl.Height = height;
                    contentControl.Width = width;
                    Canvas.SetTop(contentControl, 150);
                    Canvas.SetLeft(contentControl, 350);

                    // Add content control to canvas
                    (window as Window1).Canvas.Children.Add(contentControl);
                }
            }
        }

        private void DisplayRhombus(object sender, MouseButtonEventArgs e)
        {

            string pathData = "M 0,5 5,0 10,5 5,10 ZZ";
            Path rhombus = new Path();
            rhombus.Fill = Brushes.White;
            rhombus.Stroke = Brushes.Black;
            rhombus.Data = Geometry.Parse(pathData);
            rhombus.IsHitTestVisible = false;
            rhombus.Stretch = Stretch.Fill;

            DisplayShape(rhombus, 70, 140);
        }

        private void DisplayParallelogram(object sender, MouseButtonEventArgs e)
        {
           
            string pathData = "M 0,10 2.5,0 10,0 7.5,10 Z";
            Path parallelogram = new Path();
            parallelogram.Fill = Brushes.White;
            parallelogram.Stroke = Brushes.Black;
            parallelogram.Data = Geometry.Parse(pathData);
            parallelogram.IsHitTestVisible = false;
            parallelogram.Stretch = Stretch.Fill;

            DisplayShape(parallelogram, 70, 140);
        }

        private void DisplayRectangle(object sender, MouseButtonEventArgs e)
        {
            // Set rectangle shape
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = Brushes.White;
            rectangle.Stroke = Brushes.Black;
            rectangle.IsHitTestVisible = false;

            DisplayShape(rectangle, 70, 140);
        }

        private void DisplayCircle(object sender, MouseButtonEventArgs e)
        {
            // Set shape 
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = Brushes.White;
            ellipse.Stroke = Brushes.Black;
            ellipse.IsHitTestVisible = false;

            // Call DisplayShape function 
            DisplayShape(ellipse,100,100);
        }
    }
}
