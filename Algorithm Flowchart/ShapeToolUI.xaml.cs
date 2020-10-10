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
            //DataContext = new ShapeDesigner().Circle;
            //var contentControl = new ShapeDesigner().;
            //var textBox = new TextBox();
            //textBox.Text = "Hehe";
            Rectangle rectangle = new Rectangle();
            rectangle.Width = 50;
            rectangle.Height = 60;
            rectangle.Fill = Brushes.Red;
            var Ellipse = new Ellipse();
            Ellipse.Fill = Brushes.Red;
            Ellipse.Width = 50;
            Ellipse.Height = 60;

          
            var contentControl = new ContentControl();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Window1))
                {
             
                    //(window as Window1).MainArea.Children.Add(contentControl);
                    // BindingOperations.SetBinding((window as Window1).MainCotentControl, ContentControl.ContentProperty, new Binding());
                    Binding binding = new Binding("DesignShape");
                    //  binding.Source = DesignShape;
                    //(window as Window1).MainCotentControl.SetBinding(Template.Resources("DesignShape")) = 300;
                    // (window as Window1).MainCotentControl.Template = (window as Window1).MainCotentControl.FindResource("DesignShape") as ControlTemplate;
                    //(window as Window1).MainCotentControl.Height = 300;
                    // (window as Window1).MainCotentControl.Width = 300;
                     Ellipse ellipse = new Ellipse();
                     ellipse.Fill = Brushes.Red;
                    // (window as Window1).MainCotentControl.Content = ellipse;
                    // (window as Window1).MainCotentControl.SetBinding(ContentControl.ContentProperty, new Binding());
                    // (window as Window1).MainCotentControl.SetValue(Canvas.LeftProperty, 50);
                    //(window as Window1).MainCotentControl.R;
                   
                    contentControl.Content = ellipse;
                    contentControl.Template = contentControl.FindResource("DesignShape") as ControlTemplate;
                    contentControl.Height = 300;
                    contentControl.Width = 300;
                    Canvas.SetTop(contentControl, 60);
                    Canvas.SetLeft(contentControl, 60);
                    (window as Window1).Canvas.Children.Add(contentControl);
                    

                }
            }


            //Width = "202" Height = "202" Canvas.Top = "50" Canvas.Left = "50"
            //MinWidth = "50" MaxWidth = "200" MinHeight = "50" MaxHeight = "200"
            //        Template = "{StaticResource DesignShape}" RenderTransformOrigin = "3.005,2.555" >
        }

     
    }
}
