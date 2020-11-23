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
using System.ComponentModel;
using System.Data;



namespace CopyAndPasteInCanvas

{

    public partial class Window1

    {
        int num = -1;
        int size = 0;
        bool clicked = false;
        double [,]whxy = new double[100, 4];
        Rectangle []rectA = new Rectangle[100];
        Rect[] rect = new Rect[100];
        SolidColorBrush redBrush = new SolidColorBrush(Colors.White);
        Pen pen = new Pen(Brushes.Black, 0.1);
        InkCanvas inkCanvas;
        public BackRoundPicker newPick;
        public bool isColorPicker;
        public Window1()

        {
            InitializeComponent();
            DataContext = new ShapeDesigner().Canvas;
            isColorPicker = false;
        }
           

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }
        protected override void OnRender(System.Windows.Media.DrawingContext e)
        {
            base.OnRender(e);
            this.Canvas.Children.Clear();
            if (size == 0) ;
            else
                for (int i = 0; i < size; i++)
                {
                    rectA[i] = new Rectangle();
                    rect[i] = new Rect(whxy[i, 2], whxy[i, 3], whxy[i, 0], whxy[i, 1]);
                    //if(num == 0)
                    //rectA[i].MouseDown += Form1_shape_MouseClick;
                    //whxy[i, 1, 0], whxy[i, 1, 1], whxy[i, 0, 0], whxy[i, 0, 1]
                    //e.DrawRectangle(redBrush, pen, rectA[i]);
                    rectA[i].Width = whxy[i, 0];
                    rectA[i].Height = whxy[i, 1];

                    // Set up the Background color
                    rectA[i].Fill = Brushes.Black;

                    // Set up the position in the window, at mouse coordonate
                    Canvas.SetTop(rectA[i], whxy[i, 3] - 100);
                    Canvas.SetLeft(rectA[i], whxy[i, 2] - 140);

                    this.Canvas.Children.Add(rectA[i]);
                }
        }

        private void Form1_KeyPress(object sender, KeyEventArgs e)
        {
            // for(int i= 0;, i < sizeof.rect)
            //if(num != -1)
            if (num > -1)
            {
                if (e.Key == Key.OemPlus)
                {
                    whxy[num, 0] += 10;
                    whxy[num, 1] += 10;
                }
                //if (e.KeyChar == '-')
                //{
                //    whxy[num, 0, 0] -= 10;
                //    whxy[num, 0, 1] -= 10;
                //}
            }
            this.InvalidateVisual();
        }
        private bool Check(double Mx, double My)
        {
            //MessageBox.Show(Convert.ToString(num));
            for (int i = 0; i < size; i++)
            {
                // if(i != num)
                Console.WriteLine("Rect" + i+ "  " + rect[i].X);
                Console.WriteLine(Mx);
                if (rect[i].Contains(Mx, My))
                {
                    if (i == num)
                    {
                        num = -1;
                    }
                    else
                        num = i;
                    //MessageBox.Show(Convert.ToString(num));
                    Console.WriteLine(1);
                    return true;
                }
            }
            Console.WriteLine(-1);
            if (num > -1)
            {
                num = -2;
                return true;
            }
            //MessageBox.Show(Convert.ToString(num));
            return false;
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!Check(e.GetPosition(this).X, e.GetPosition(this).Y))//&& num == -1)
            {
                whxy[size, 0] = 35;
                whxy[size, 1] = 35;
                whxy[size, 2] = e.GetPosition(this).X;
                whxy[size, 3] = e.GetPosition(this).Y;

                rectA[size] = new Rectangle();

                rect[size] = new Rect(whxy[size, 2], whxy[size, 3], whxy[size, 0], whxy[size, 1]);

                this.InvalidateVisual();
                size++;
            }
            if (num == -2)
                num = -1;
            //clicked = true;
        }
        private void Form1_shape_MouseClick(object sender, MouseEventArgs e)
        {
            //if (!clicked)
            {
                if (!Check(e.GetPosition(this).X, e.GetPosition(this).Y))//&& num == -1)
                {
                    whxy[size, 0] = 35;
                    whxy[size, 1] = 35;
                    whxy[size, 2] = e.GetPosition(this).X;
                    whxy[size, 3] = e.GetPosition(this).Y;

                    rectA[size] = new Rectangle();

                    rect[size] = new Rect(whxy[size, 2], whxy[size, 3], whxy[size, 0], whxy[size, 1]);

                    this.InvalidateVisual();
                    size++;
                }
                if (num == -2)
                    num = -1;
            }
        }
        private void button1_Click(object sender, System.Windows.Media.DrawingContext e)
        {
            Pen pen = new Pen(Brushes.Red, 15);
            //g.DrawLine(pen, 0, 0, 200, 200);
            //g.Dispose();
            Rect rect = new Rect((this.Width / 2 - 50), (this.Height / 2 - 50), 100, 50);
            //int x = (this.Width / 2 - 50 ), y = (this.Height / 2 - 50), width = 100, height = 50;
            e.DrawRectangle(redBrush, pen, rect);
            //g.FillRectangle(redBrush, x, y, width, height);
        }
        private void Form1_Move(object sender, MouseEventArgs e)
        {
            if (num > -1)
            {
                whxy[num, 2] = e.GetPosition(this).X + 1;
                whxy[num, 3] = e.GetPosition(this).Y + 1;// - 15;
                this.InvalidateVisual();
            }
        }

    

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();

        }
        private void FontPicker_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                case 1:
                    /*newPick = new BackRoundPicker(this);
                    newPick.ShowDialog();
                    {
                        try
                        {
                            var converter = new System.Windows.Media.BrushConverter();
                            Canvas.Background = (Brush)converter.ConvertFromString($"{newPick.colorCode}");
                            tabCnntrol.BorderBrush = (Brush)converter.ConvertFromString($"{newPick.colorCode}");
                            //rightPanel.Background= (Brush)converter.ConvertFromString($"{newPick.colorCode}");
                            this.ShapeTool.shapeToolBackround.Background = (Brush)converter.ConvertFromString($"{newPick.colorCode}");
                            panel.Background = (Brush)converter.ConvertFromString($"{newPick.colorCode}");
                            gridColumn2.Background = (Brush)converter.ConvertFromString($"{newPick.colorCode}");
                            this.rightPanel.Background = (Brush)converter.ConvertFromString($"{newPick.colorCode}");
                        }
                        catch (Exception ea) { }
                        
                    }*/

                    break;
                
            }
        }
        private void File_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                case 1:
                    filePicker file = new filePicker();
                    file.ShowDialog();
                    break;
                /*case 2:
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
                    break;*/
            }
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
               /* case 1:
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
                    break;*/
            }
        }
        private void View_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                /*case 1:
                    this.Canvas.Background = Brushes.Black;
                    break;
                case 2:
                    this.Canvas.Background = Brushes.Red;
                    break;*/
                case 3:
                    if(this.ResizeMode != System.Windows.ResizeMode.NoResize)
                    {
                        //this.WindowState = System.Windows.WindowState.Maximized;
                        this.ResizeMode = System.Windows.ResizeMode.NoResize;
                        this.WindowState = System.Windows.WindowState.Maximized;
                    }
                    else
                    {
                        this.ResizeMode = System.Windows.ResizeMode.CanResize;
                        this.WindowState = System.Windows.WindowState.Normal;
                    }
                    break;
            }
        }
        private void Insert_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                /*case 1:
                    this.Canvas.Background = Brushes.Black;
                    break;
                case 2:
                    this.Canvas.Background = Brushes.Red;
                    break;*/
                case 3:
                    //this.Canvas.Background = Brushes.Green;
                    Canvas.Children.Remove(inkCanvas);

                    break;
                case 4:
                    {
                        inkCanvas = new InkCanvas();
                        inkCanvas.Width = 1500;
                        inkCanvas.Height = 1500;
                        inkCanvas.Background = Brushes.Transparent;
                        /*foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Window1))
                            {
                                // Add content control to canvas
                                (window as Window1).
                            }
                        }*/
                        Canvas.Children.Add(inkCanvas);
                        break;
                    }                    
            }
        }
        private void Tool_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                /*case 1:
                    this.Canvas.Background = Brushes.Black;
                    break;*/
                case 2:
                    var converter = new System.Windows.Media.BrushConverter();
                    if (!isColorPicker)
                    {
                        buttonChooseColor.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
                        buttonChooseColor.Visibility = Visibility.Visible;
                    }
                    else
                        buttonChooseColor.Visibility = Visibility.Hidden;
                    if (isColorPicker)
                        isColorPicker = false;
                    else
                        isColorPicker = true;
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
        
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            Canvas.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
            
        }

        
        private void tabCnntrol_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            Brush b= (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
            if (isColorPicker)
            {
                tabCnntrol.BorderBrush = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
                this.buttonFile.Background = b;
                this.buttonOpen.Background = b;
                this.buttonSave.Background = b;
                this.buttonImport.Background = b;
                this.buttonExport.Background = b;
                this.buttonCut.Background = b;
                this.buttonCopy.Background = b;
                this.buttonPaste.Background = b;
                this.buttonDelete.Background = b;
                this.buttonClear.Background = b;
                this.buttonUndo.Background = b;
                this.buttonRedo.Background = b;
                this.buttonZoomin.Background = b;
                this.buttonZoomout.Background = b;
                this.buttonFull.Background = b;
                this.buttonColor.Background = b;
                this.buttonShape.Background = b;
                this.buttonPencil.Background = b;
                this.buttonArrow.Background = b;
                this.buttonTuto.Background = b;
                this.buttonFeedback.Background = b;
                this.buttonText.Background = b;
                this.buttonStyle.Background = b;
                this.buttonAbout.Background = b;
                this.buttonSearch.Background = b;
            }
            //Brush1 = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");

        }

        private void ShapeTool_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (isColorPicker) ;
                //ShapeTool.shapeToolBackround.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }

        private void tabCnntrol_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (isColorPicker)
                tabCnntrol.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }

        private void rightPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (isColorPicker)
                rightPanel.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }

        private void gridColumn2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (isColorPicker)
                gridColumn2.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
            
        }

        private void colorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            buttonChooseColor.Background= (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }
    }

}
