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
    }

}
