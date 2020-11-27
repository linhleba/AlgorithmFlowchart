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
using Path = System.Windows.Shapes.Path;


namespace CopyAndPasteInCanvas
{
    public partial class Window1
    {
        InkCanvas inkCanvas;
        public BackRoundPicker newPick;
        public bool isColorPicker;
        public List<Shape> rectList;
        public List<int> typeOfShape;
        public Point startPoint;
        public int shapeId;
        public bool move = false;
        public bool resize = false;
        //variable to test code in stackoverflow 
        public int dragHandle = 0;

        //use in resize
        public double delta = 0;
        public int direction = 0;
        //add adorner
        System.Windows.Documents.AdornerLayer myAdornerLayer;
        public List<SimpleCircleAdorner> adornerList;
        public bool showAdorner = false;
        public Window1()
        {
            InitializeComponent();
            DataContext = new ShapeDesigner().Canvas;
            isColorPicker = false;
            rectList = new List<Shape>();
            typeOfShape = new List<int>();  // 1:Rectangle,  2:Circle, 3:Parallelogram, 4:...
            adornerList = new List<SimpleCircleAdorner>();
            shapeId = -1;
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
                    if (this.ResizeMode != System.Windows.ResizeMode.NoResize)
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
                case 3:
                    var converter = new System.Windows.Media.BrushConverter();
                    if (!isColorPicker)
                    {
                        buttonChooseColor.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
                        buttonChooseColor.Content = "P";
                    }
                    else
                        buttonChooseColor.Content = "NP";
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

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            Canvas.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");

        }


        private void tabCnntrol_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            Brush b = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
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
                //this.buttonClear.Background = b;
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
            //if (isColorPicker) ;
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
            buttonChooseColor.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }

        protected override void OnRender(System.Windows.Media.DrawingContext e)
        {
            base.OnRender(e);
            //this.Canvas.Children.Clear();
            //Console.WriteLine("aaaa\n");
        }
        public String IsContain(double x, double y)
        {
            x -= 140;
            y -= 100;
            for (int i = 0; i < this.rectList.Count; i++)
            {
                double x0 = Canvas.GetLeft(rectList[i]);
                double y0 = Canvas.GetTop(rectList[i]);
                double x1 = x0 + rectList[i].Width;
                double y1 = y0 + rectList[i].Height;
                int valueOfDistance = 0;
                // Set value to handle point for the different shape
                if (typeOfShape[i] == 1 || typeOfShape[i] == 2)
                {
                    valueOfDistance = 10;
                }
                else if (typeOfShape[i] == 3 || typeOfShape[i] == 4)
                {
                    valueOfDistance = 20;
                }
                //Console.WriteLine($"coor of rect {x0} {y0} {x1} {y1}");

                //making appear arrow to resize of paint shape
                if (x0 + valueOfDistance <= x && x <= x1 - valueOfDistance && (y0 + valueOfDistance <= y && y <= y1 - valueOfDistance))
                {
                    this.move = true;
                    if (!isColorPicker)
                        this.Cursor = Cursors.SizeAll;
                    else
                        this.Cursor = Cursors.Pen;
                    return rectList[i].Uid;
                }
                else if ((x0 - valueOfDistance <= x && x <= x0 + valueOfDistance) && (y0 - 10 <= y && y <= y0 + 10))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNWSE;
                    rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 7;
                    return rectList[i].Uid;
                }
                else if ((x1 - valueOfDistance <= x && x <= x1 + valueOfDistance) && (y1 - valueOfDistance <= y && y <= y1 + valueOfDistance))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNWSE;
                    rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 5;
                    return rectList[i].Uid;
                }
                else if ((y0 - valueOfDistance <= y && y <= y0 + valueOfDistance) && (x1 - valueOfDistance <= x && x <= x1 + valueOfDistance))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNESW;
                    rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 8;
                    return rectList[i].Uid;
                }
                else if ((y1 - valueOfDistance <= y && y <= y1 + valueOfDistance) && (x0 - valueOfDistance <= x && x <= x0 + valueOfDistance))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNESW;
                    rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 6;
                    return rectList[i].Uid;
                }
                else if (x0 - valueOfDistance <= x && x <= x0 + valueOfDistance && y0 <= y && y <= y1)
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeWE;
                    rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 4;
                    return rectList[i].Uid;
                }
                else if (y0 - valueOfDistance <= y && y <= y0 + valueOfDistance && x0 <= x && x <= x1)
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNS;
                    rectList[i].Stroke = Brushes.Red;
                    direction = -1;
                    dragHandle = 1;
                    return rectList[i].Uid;
                }
                else if (y1 - valueOfDistance <= y && y <= y1 + valueOfDistance && x0 <= x && x <= x1)
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNS;

                    direction = 1;
                    dragHandle = 3;
                    return rectList[i].Uid;
                }
                else if (x1 - valueOfDistance <= x && x <= x1 + valueOfDistance && y0 <= y && y <= y1)
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeWE;
                    rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 2;
                    return rectList[i].Uid;
                }
            }
            //if no tin shape cursor is normal and return -1(it mean not shape is catch)
            this.Cursor = null;
            for (int i = 0; i < this.rectList.Count; i++)
            {
                rectList[i].Stroke = Brushes.Black;
            }
                return "-1";

        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(rectList.Count);
            if (shapeId == -1)
            {
                String s = IsContain(e.GetPosition(this).X, e.GetPosition(this).Y);
                //cause IsContain return shapeID - which is String so we have to try to parse it into int
                bool success = Int32.TryParse(s, out shapeId);
                
            }
            //when mouse button is release , stop this function
            if (e.LeftButton == MouseButtonState.Released || shapeId < 0)
            {
                shapeId = -1;
                if (move) move = !move;
                if (resize) resize = !resize;
                delta = direction = 0;                
                return;
            }
            
            //action when moving shape   
            if (move)
            {                                 
                double x = (e.GetPosition(this).X - 140 - rectList[shapeId].Width / 2);
                double y = (e.GetPosition(this).Y - 100 - rectList[shapeId].Height / 2);
                Canvas.SetLeft(rectList[shapeId], x);
                Canvas.SetTop(rectList[shapeId], y);
            }
            //action when resize shape
            else if (resize)
            {

                // If shape is rectangle or circle

                // Get current pos x
                double x = (e.GetPosition(this).X - 140);
                // Get currennt pos y
                double y = (e.GetPosition(this).Y - 100);


                double x0 = Canvas.GetLeft(rectList[shapeId]);
                double y0 = Canvas.GetTop(rectList[shapeId]);

                // Get the bottom pos x1,y1
                double x1 = x0 + rectList[shapeId].Width;
                double y1 = y0 + rectList[shapeId].Height;

                Console.WriteLine("Drag handle is " + dragHandle);


                double deltaDistanceY = y - y1;
                double deltaDistanceX = x - x1;
                switch (dragHandle)
                {

                    // Case handle vertical alignment for shapes
                    case 1:
                        Canvas.SetTop(rectList[shapeId], y);
                        rectList[shapeId].Height = y1 - Canvas.GetTop(rectList[shapeId]);
                        break;
                    case 3:
                        rectList[shapeId].Height += deltaDistanceY;
                        break;

                    // Case handle horizontal alignment for shapes
                    case 2:
                        rectList[shapeId].Width += deltaDistanceX;
                        break;
                    case 4:
                        Canvas.SetLeft(rectList[shapeId], x);
                        rectList[shapeId].Width = x1 - Canvas.GetLeft(rectList[shapeId]);
                        break;
                    // Case handle diagonal alignment right-bottom for shapes
                    case 5:
                        double deltaX = x - x1;
                        rectList[shapeId].Width += deltaX;
                        double deltaY = y - y1;
                        rectList[shapeId].Height += deltaY;
                        break;
                    // Case handle diagonal alignment left-bottom for shapes
                    case 6:
                        rectList[shapeId].Height += deltaDistanceY;
                        Canvas.SetLeft(rectList[shapeId], x);
                        rectList[shapeId].Width = x1 - Canvas.GetLeft(rectList[shapeId]);
                        break;

                    // Case handle diagonal alignment left-top for shapes
                    case 7:
                        Canvas.SetTop(rectList[shapeId], y);
                        rectList[shapeId].Height = y1 - Canvas.GetTop(rectList[shapeId]);
                        Canvas.SetLeft(rectList[shapeId], x);
                        rectList[shapeId].Width = x1 - Canvas.GetLeft(rectList[shapeId]);
                        break;

                    // Case handle diagonal alignment right-top for shapes
                    case 8:
                        Canvas.SetTop(rectList[shapeId], y);
                        rectList[shapeId].Height = y1 - Canvas.GetTop(rectList[shapeId]);
                        rectList[shapeId].Width += deltaDistanceX;
                        break;
                }
            }
            //else myAdornerLayer.Remove(adornerList[shapeId]);

        }


        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //reset every flag (such as move , resize flag) when mouse up
            if (move) move = !move;
            if (resize)
            {
                direction = 0;
                resize = !resize;
                delta = 0;
            }
            this.Cursor = null;
            if (shapeId >= 0)
            {
                shapeId = -1;
            }
            
        }

        private void Button_Rectangle_Click(object sender, RoutedEventArgs e)
        {
            //this func make a shape when press button
            Rectangle rect = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Uid = rectList.Count.ToString()
            };
            rectList.Add(rect);

            // Add type of shape of the rectangle
            typeOfShape.Add(1);
            Canvas.SetLeft(rect, 100);
            Canvas.SetTop(rect, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            Console.WriteLine("Coor of shape " + Canvas.GetLeft(rect) + " " + Canvas.GetTop(rect));
            //add adorner for shape     
            myAdornerLayer = AdornerLayer.GetAdornerLayer(rect);
            adornerList.Add(new SimpleCircleAdorner(rect));
        }

        private void Button_Circle_Click(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Uid = rectList.Count.ToString()
            };
            rectList.Add(ellipse);            
            typeOfShape.Add(2);
            Canvas.SetLeft(ellipse, 100);
            Canvas.SetTop(ellipse, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            //add adorner for shape            
            myAdornerLayer = AdornerLayer.GetAdornerLayer(ellipse);
            adornerList.Add(new SimpleCircleAdorner(ellipse));

        }

        private void Button_Parallelogram_Click(object sender, RoutedEventArgs e)
        {
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(new Point(0, 100));
            myPointCollection.Add(new Point(25, 0));
            myPointCollection.Add(new Point(100, 0));
            myPointCollection.Add(new Point(75, 100));


            Polygon polygon = new Polygon
            {
                Width = 100,
                Height = 100,

                Points = myPointCollection,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Stretch = Stretch.Fill,
                Uid = rectList.Count.ToString()
            };
            rectList.Add(polygon);
            typeOfShape.Add(3);
            Canvas.SetLeft(polygon, 100);
            Canvas.SetTop(polygon, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            //add adorner for shape            
            myAdornerLayer = AdornerLayer.GetAdornerLayer(polygon);
            adornerList.Add(new SimpleCircleAdorner(polygon));
        }

        private void Button_Rhombus_Click(object sender, RoutedEventArgs e)
        {
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(new Point(0, 50));
            myPointCollection.Add(new Point(50, 0));
            myPointCollection.Add(new Point(100, 50));
            myPointCollection.Add(new Point(50, 100));

            Polygon polygon = new Polygon
            {
                Width = 100,
                Height = 100,

                Points = myPointCollection,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Stretch = Stretch.Fill,
                Uid = rectList.Count.ToString()
            };
            rectList.Add(polygon);
            typeOfShape.Add(4);
            Canvas.SetLeft(polygon, 100);
            Canvas.SetTop(polygon, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            //add adorner for shape            
            myAdornerLayer = AdornerLayer.GetAdornerLayer(polygon);
            adornerList.Add(new SimpleCircleAdorner(polygon));
        }

        //paint shape when mouse change into pen
        private void Canvas_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine($" showadorner {showAdorner.ToString()}");
            if (shapeId == -1)
            {
                String s = IsContain(e.GetPosition(this).X, e.GetPosition(this).Y);
                //cause IsContain return shapeID - which is String so we have to try to parse it into int
                bool success = Int32.TryParse(s, out shapeId);
                if ((shapeId > -1 ) && !showAdorner)
                    myAdornerLayer.Remove(adornerList[shapeId]);
            }
            if (e.LeftButton == MouseButtonState.Released || shapeId < 0)
            {
                if (showAdorner)
                    showAdorner = false;
                clearAllAdorner();
                shapeId = -1;
                return;
            }

            var converter = new System.Windows.Media.BrushConverter();
            if (isColorPicker)
                rectList[shapeId].Fill = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
            if (!showAdorner)
            {
                showAdorner = true;
                myAdornerLayer.Add(adornerList[shapeId]);
            }
        }

        

        private void buttonClear1_Click(object sender, RoutedEventArgs e)
        {
            rectList.Clear();
            adornerList.Clear();
            this.Canvas.Children.Clear();
        }
        public void clearAllAdorner()
        {
            for(int i=0; i <adornerList.Count; i++)
                this.myAdornerLayer.Remove(adornerList[i]);
        }

        
    }

}
