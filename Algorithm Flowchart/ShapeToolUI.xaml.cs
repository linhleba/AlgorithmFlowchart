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

        private void DisplayRhombus(object sender, MouseButtonEventArgs e)
        {
           
          
            var contentControl = new ContentControl();

            // find MainWindow
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Window1))
                {
                    // Set shape 
                     Ellipse ellipse = new Ellipse();
                     ellipse.Fill = Brushes.White;
                    ellipse.Stroke = Brushes.Black;
                     ellipse.IsHitTestVisible = false;
                  
                    // Set content control 
                    contentControl.Content = ellipse;
                    contentControl.Template = contentControl.FindResource("DesignShape") as ControlTemplate;
                    contentControl.Height = 300;
                    contentControl.Width = 300;
                    Canvas.SetTop(contentControl, 150);
                    Canvas.SetLeft(contentControl, 250);
                    
                    // Add content control to canvas
                    (window as Window1).Canvas.Children.Add(contentControl);
                }
            }
        }

     
    }
}
