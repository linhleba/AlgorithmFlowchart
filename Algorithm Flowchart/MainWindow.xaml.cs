﻿using Algorithm_Flowchart;
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
        InkCanvas inkCanvas;
        public BackRoundPicker newPick;
        public bool isColorPicker;
        public List<Shape> rectList;
        public Point startPoint;
        public int shapeId ;
        public bool move = false; 
        public bool resize = false;
        //variable to test code in stackoverflow 
        public int dragHandle = 0;

        //use in resize
        public double delta = 0;
        public int direction = 0;
        public Window1()
        {
            InitializeComponent();
            DataContext = new ShapeDesigner().Canvas;
            isColorPicker = false;
            rectList = new List<Shape>();
            shapeId = -1;
        }          

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
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
            buttonChooseColor.Background= (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
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
            for (int i=0; i < this.rectList.Count; i++)
            {
                double x0 = Canvas.GetLeft(rectList[i]);
                double y0 = Canvas.GetTop(rectList[i]);
                double x1 = x0 + rectList[i].Width;
                double y1 = y0 + rectList[i].Height;
                //Console.WriteLine($"coor of rect {x0} {y0} {x1} {y1}");

                //making appear arrow to resize of paint shape
                if ((x0 + 10 <= x && x <= x1 - 10) && (y0 + 10 <= y && y <= y1 - 10))
                {
                    this.move = true;
                    if (!isColorPicker)
                        this.Cursor = Cursors.SizeAll;
                    else
                        this.Cursor = Cursors.Pen;
                    return rectList[i].Uid;
                }
                else if ((x0 - 10 <= x && x <= x0 + 10) && (y0 - 10 <= y && y <= y0 + 10))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNWSE;
                    direction = 1;
                    dragHandle = 5;
                    return rectList[i].Uid;
                }
                else if ((x1 - 10 <= x && x <= x1 + 10) && (y1 - 10 <= y && y <= y1 + 10))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNWSE;
                    direction = 1;
                    dragHandle = 5;
                    return rectList[i].Uid;
                }
                else if ((y0 - 10 <= y && y <= y0 + 10) && (x1 - 10 <= x && x <= x1 + 10))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNESW;
                    direction = 1;
                    dragHandle = 6;
                    return rectList[i].Uid;
                }
                else if ((y1 - 10 <= y && y <= y1 + 10) && (x0 - 10 <= x && x <= x0 + 10))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNESW;
                    direction = 1;
                    dragHandle = 6;
                    return rectList[i].Uid;
                }
                else if (x0 - 10 <= x && x <= x0 + 10)
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeWE;
                    direction = 1;
                    dragHandle = 4;
                    return rectList[i].Uid;
                }
                else if (y0 - 10 <= y && y <= y0 + 10)
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNS;
                    direction = -1;
                    dragHandle = 1;
                    return rectList[i].Uid;
                }
                else if (y1 - 10 <= y && y <= y1 + 10)
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNS;
                    direction = -1;
                    dragHandle = 3;
                    return rectList[i].Uid;
                }
                else if (x1 - 10 <= x && x <= x1 + 10)
                {
                    this.resize = true;                    
                    this.Cursor = Cursors.SizeWE;
                    direction = 1;
                    dragHandle = 2;
                    return rectList[i].Uid;
                }
            }
            //if no tin shape cursor is normal and return -1(it mean not shape is catch)
            this.Cursor = null;
            return "-1";

        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(rectList.Count);
            if(shapeId==-1)
            {
                String s = IsContain(e.GetPosition(this).X, e.GetPosition(this).Y);
                //cause IsContain return shapeID - which is String so we have to try to parse it into int
                bool success = Int32.TryParse(s, out shapeId);
            }    
            //when mouse button is release , stop this function
            if (e.LeftButton == MouseButtonState.Released || shapeId <0)
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
                double x = (e.GetPosition(this).X - 140 - rectList[shapeId].Width/2);
                double y = (e.GetPosition(this).Y - 100 - rectList[shapeId].Height / 2);
                Canvas.SetLeft(rectList[shapeId], x);
                Canvas.SetTop(rectList[shapeId], y);
            }
            //action when resize shape
            else if (resize)
            {
                double x = (e.GetPosition(this).X - 140);
                double y = (e.GetPosition(this).Y - 100);
                if (dragHandle == 1 || dragHandle == 3)
                {      
                    
                    if((y-Canvas.GetTop(rectList[shapeId])) > rectList[shapeId].Height)
                        rectList[shapeId].Height += 2;
                    else
                    {                        
                        rectList[shapeId].Height -= 2;
                    }                     
                    
                }
                else if (dragHandle == 2|| dragHandle == 4)
                {
                    if ((x - Canvas.GetLeft(rectList[shapeId])) > rectList[shapeId].Width)
                        rectList[shapeId].Width += 2;
                    else
                        rectList[shapeId].Width -= 2;
                }
                else if (dragHandle == 5 || dragHandle == 6)
                {
                    if ((x - Canvas.GetLeft(rectList[shapeId])) > rectList[shapeId].Width)
                    {
                        rectList[shapeId].Width += 2;
                        rectList[shapeId].Height+= 2;
                    }                        
                    else
                    {
                        rectList[shapeId].Width -= 2;
                        rectList[shapeId].Height -= 2;
                    }
                }


            }        


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

        private void Button_Rectangle_Click_1(object sender, RoutedEventArgs e)
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
            Canvas.SetLeft(rect, 100);
            Canvas.SetTop(rect, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            Console.WriteLine("Coor of shape " + Canvas.GetLeft(rect) + " " + Canvas.GetTop(rect));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
            Canvas.SetLeft(ellipse, 100);
            Canvas.SetTop(ellipse, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(new Point(0, 100));
            myPointCollection.Add(new Point(100, 100));
            myPointCollection.Add(new Point(50, 0));
            Polygon polygon = new Polygon
            {
                Width = 100,
                Height = 100,
                Points = myPointCollection,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Uid = rectList.Count.ToString()
            };
            rectList.Add(polygon);
            Canvas.SetLeft(polygon, 100);
            Canvas.SetTop(polygon, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
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
            Canvas.SetLeft(rect, 100);
            Canvas.SetTop(rect, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
        }

        //paint shape when mouse change into pen
        private void Canvas_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (shapeId == -1)
            {
                String s = IsContain(e.GetPosition(this).X, e.GetPosition(this).Y);
                //cause IsContain return shapeID - which is String so we have to try to parse it into int
                bool success = Int32.TryParse(s, out shapeId);
            }
            if (e.LeftButton == MouseButtonState.Released || shapeId < 0)
            {
                shapeId = -1;
                return;
            }
            var converter = new System.Windows.Media.BrushConverter();
            if (isColorPicker)
                rectList[shapeId].Fill= (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }
    }

}
