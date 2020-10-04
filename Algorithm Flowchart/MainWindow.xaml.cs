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

        private Rectangle rect;

        private double top = 100;

        private double left = 300;



        public Window1()

        {

            InitializeComponent();


            CommandBinding pasteCmdBinding = new CommandBinding();

            pasteCmdBinding.Command = ApplicationCommands.Paste;

            pasteCmdBinding.Executed += pasteCmdBinding_Executed;

            pasteCmdBinding.CanExecute += pasteCmdBinding_CanExecute;

            this.CommandBindings.Add(pasteCmdBinding);



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



        private void copyCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)

        {

            e.CanExecute = true;

        }



        private void copyCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)

        {

            string xaml = XamlWriter.Save(rect);

            Clipboard.SetText(xaml, TextDataFormat.Xaml);

        }



        private void pasteCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)

        {

            string xaml = Clipboard.GetText(TextDataFormat.Xaml);

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

                        left = 300;

                        drawingCanvas.Children.Add(shape);

                    }

                }

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rect = red;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            rect = yellow;
        }
    }

}
