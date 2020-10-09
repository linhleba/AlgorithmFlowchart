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
            // Declare shape in ShapeDesigner 
            var designerWindow = new ShapeDesigner();

            //var circleShape = designerWindow.test;
            //this.RemoveLogicalChild(circleShape);

            var canvas = new Canvas();
            var contentControl = new ContentControl();
            var ellipse = new Ellipse();


            canvas.Children.Add(ellipse);


            //Binding binding = new Binding("Shape");
            //binding.Source = designerWindow.test;

            

            // Add shape to the main window
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Window1))
                {
                    (window as Window1).MainArea.Children.Add(canvas);
                    //(window as Window1).MainArea.SetBinding(TextBlock.TextProperty, binding);
                }
            }
        }

    }
}
