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
        Canvas canvas = new Canvas();
        int top = 100, left = 200;
        string xaml = "";
        Shape shape;
        private List<Shape> Shape = new List<Shape>();
        public static Random rand = new Random();
        public Window1()

        {
            InitializeComponent();

            DataContext = new ShapeDesigner().Canvas;
            // Initializing CutCmdBinding when system send a Cut signal
            CommandBinding DelCmdBinding = new CommandBinding();

            DelCmdBinding.Command = ApplicationCommands.Delete;

            DelCmdBinding.Executed += DelCmdBinding_Executed;

            DelCmdBinding.CanExecute += DelCmdBinding_CanExecute;

            this.CommandBindings.Add(DelCmdBinding);

            CommandBinding CutCmdBinding = new CommandBinding();

            CutCmdBinding.Command = ApplicationCommands.Cut;

            CutCmdBinding.Executed += CutCmdBinding_Executed;

            CutCmdBinding.CanExecute += CutCmdBinding_CanExecute;

            this.CommandBindings.Add(CutCmdBinding);

            //Same as CutCmd
            CommandBinding pasteCmdBinding = new CommandBinding();

            pasteCmdBinding.Command = ApplicationCommands.Paste;

            pasteCmdBinding.Executed += pasteCmdBinding_Executed;

            pasteCmdBinding.CanExecute += pasteCmdBinding_CanExecute;

            this.CommandBindings.Add(pasteCmdBinding);


            //Same as CutCmd
            CommandBinding copyCmdBinding = new CommandBinding();

            copyCmdBinding.Command = ApplicationCommands.Copy;

            copyCmdBinding.Executed += copyCmdBinding_Executed;

            copyCmdBinding.CanExecute += copyCmdBinding_CanExecute;

            this.CommandBindings.Add(copyCmdBinding);

        }
        private void pasteCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)

        {
            e.CanExecute = Clipboard.ContainsText(TextDataFormat.Xaml);
        }

        private void DelCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)

        {
            e.CanExecute = true;
        }

        private void CutCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)

        {
            e.CanExecute = true;
        }

        private void copyCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)

        {
            e.CanExecute = true;
        }

        private void copyCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //this code allow us to save shape info to Clipboard by turn shape info to string and the save it
            xaml = XamlWriter.Save(shape);
            Clipboard.SetText(xaml, TextDataFormat.Xaml);
        }

        private void DelCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //remove shape from canvas
            canvas.Children.Remove(shape);
        }
        private void CutCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //canvas.Children.Remove allow to remove a canvas children member as shape in this canvas
            canvas.Children.Remove(shape);
            string xaml = XamlWriter.Save(shape);
            Clipboard.SetText(xaml, TextDataFormat.Xaml);
        }
        private void pasteCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)

        {
            //this extract what we just save to Clipboard as CopyCmd or CutCmd

            xaml = Clipboard.GetText(TextDataFormat.Xaml);

            //Check and then load it as shape type to canvas

            if (xaml != null)

            {

                using (MemoryStream stream = new MemoryStream(xaml.Length))

                {

                    using (StreamWriter sw = new StreamWriter(stream))

                    {

                        sw.Write(xaml);

                        sw.Flush();

                        stream.Seek(0, SeekOrigin.Begin);

                        Shape shape = XamlReader.Load(stream) as Shape;

                        Canvas.SetLeft(shape, left);

                        Canvas.SetTop(shape, top);

                        top = 100;

                        left = 200;

                        Canvas.Children.Add(shape);

                    }

                }

            }

        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //set canvas to working-on canvas
            canvas = sender as Canvas;
            //check and set shape to our just-clicked shape
            if (canvas == null)
                return;
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(canvas, e.GetPosition(canvas));
            shape = hitTestResult.VisualHit as Shape;
            if (shape == null)
                return;
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
