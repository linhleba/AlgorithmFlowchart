using Algorithm_Flowchart;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        double zoom = 1;
        double zoomDelta = 0.1;
        int top = 100, left = 200;
        string xaml = "";
        Shape shape;
        Canvas savedCanvas = new Canvas();
        Canvas canvas = new Canvas();
        InkCanvas inkCanvas;
        public BackRoundPicker newPick;
        public bool isColorPicker;
        public List<Shape> rectList;
        public List<ShapeInfo> InfoList;
        public List<int> typeOfShape;
        public List<TextBox> textBoxes;
        public List<TextBox> onlyTextBoxes;
        public Point startPoint;
        public int shapeId;
        public int textBoxId;
        public int preShapeId;
        public bool move = false;
        public bool resize = false;
        public bool isAbleMove = false;
        //variable to test code in stackoverflow 
        public int dragHandle = 0;
        //use in resize
        public double delta = 0;
        public int direction = 0;
        //add adorner
        System.Windows.Documents.AdornerLayer myAdornerLayer;
        public List<Adorner> adornerList;
        public bool showAdorner = false;
        //variable to  choose which point of arrow is chosen
        public int pointArrow = -1;
        //vector bind arrow with shape 
        public bool isDrawArrow = false;
        //1= left ;2 top ;3 right; 4 bottom
        public int typePoint = 0;
        public List<List<int>> bindingArrowShape = new List<List<int>>();
        public Window1()
        {
            InitializeComponent();
            DataContext = new ShapeDesigner().Canvas;
            isColorPicker = false;
            rectList = new List<Shape>();
            InfoList = new List<ShapeInfo>();
            onlyTextBoxes = new List<TextBox>();
            textBoxes = new List<TextBox>();
            typeOfShape = new List<int>();  // 1:Rectangle,  2:Circle, 3:Parallelogram, 4:...
            adornerList = new List<Adorner>();
            shapeId = -1;
            textBoxId = -1;
            CommandBinding SaveCmdBinding = new CommandBinding();

            SaveCmdBinding.Command = ApplicationCommands.Save;

            SaveCmdBinding.Executed += SaveCmdBinding_Executed;

            SaveCmdBinding.CanExecute += SaveCmdBinding_CanExecute;

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
                case 2:
                    canvas.Children.Clear();
                    this.Open(sender, e);
                    for (int i = 0; i < textBoxes.Count; i++)
                    {
                        Canvas.SetLeft(textBoxes[i], InfoList[i].Y + (InfoList[i].Width - textBoxes[i].MinWidth) / 2);
                        Canvas.SetTop(textBoxes[i], InfoList[i].X + (InfoList[i].Height - textBoxes[i].MinHeight) / 2);
                    }
                    break;
                case 3:
                    UpdateInfo(InfoList, rectList);
                    FileStream fs = File.Open("SaveCanvas.xaml", FileMode.Create);
                    XamlWriter.Save(Canvas, fs);
                    fs.Close();
                    string jsonStateOfShape = JsonConvert.SerializeObject(InfoList);
                    using (FileStream stream = new FileStream("Info.json", FileMode.Create))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(jsonStateOfShape);
                    }
                    jsonStateOfShape = JsonConvert.SerializeObject(typeOfShape);
                    JsonConvert.SerializeObject(typeOfShape);
                    using (FileStream stream = new FileStream("Type.json", FileMode.Create))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(jsonStateOfShape);
                    }

                    break;

            }
        }

        private void UpdateInfo(List<ShapeInfo> infoList, List<Shape> rectList)
        {
            for (int i = 0; i < rectList.Count; i++)
            {
                //MessageBox.Show(Convert.ToString(i));
                InfoList[i].X = Canvas.GetTop(rectList[i]);
                InfoList[i].Y = Canvas.GetLeft(rectList[i]);
                InfoList[i].Width = Convert.ToInt32(rectList[i].Width);
                InfoList[i].Height = Convert.ToInt32(rectList[i].Height);
                InfoList[i].StrokeThickness = Convert.ToInt32(rectList[i].StrokeThickness);
                InfoList[i].Stretch = rectList[i].Stretch;
                InfoList[i].Uid = rectList[i].Uid;
                InfoList[i].Fill = rectList[i].Fill;
                InfoList[i].text = textBoxes[i].Text;
                InfoList[i].color = textBoxes[i].Background;
            }
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
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
                case 3:
                    try
                    {
                        int currentIndex = Canvas.GetZIndex(rectList[preShapeId]);
                        int zIndex = 0;
                        int maxZ = 0;
                        for (int i = 0; i < rectList.Count; i++)
                        {
                            zIndex = Canvas.GetZIndex(rectList[i]);
                            maxZ = Math.Max(maxZ, zIndex);
                            if (zIndex >= currentIndex && i != preShapeId)
                            {
                                Canvas.SetZIndex(rectList[i], zIndex - 1);
                                Canvas.SetZIndex(textBoxes[i], zIndex - 1);
                            }

                        }
                        Canvas.SetZIndex(rectList[preShapeId], maxZ);
                        Canvas.SetZIndex(textBoxes[preShapeId], maxZ);
                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                case 4:
                    try
                    {
                        int currentIndex = Canvas.GetZIndex(rectList[preShapeId]);
                        int zIndex = 0;
                        int minZ = 0;
                        for (int i = 0; i < rectList.Count; i++)
                        {
                            zIndex = Canvas.GetZIndex(rectList[i]);
                            minZ = Math.Min(minZ, zIndex);
                            if (zIndex <= currentIndex && i != preShapeId)
                            {
                                Canvas.SetZIndex(rectList[i], zIndex + 1);
                                Canvas.SetZIndex(textBoxes[i], zIndex + 1);
                            }

                        }
                        Canvas.SetZIndex(rectList[preShapeId], minZ);
                        Canvas.SetZIndex(textBoxes[preShapeId], minZ);
                    }
                    catch (Exception ex)
                    {
                    }
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

        /*private void rightPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (isColorPicker)
                rightPanel.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }*/



        private void colorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            buttonChooseColor.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }

        protected override void OnRender(System.Windows.Media.DrawingContext e)
        {
            base.OnRender(e);
            //AddShape(InfoList, rectList);
            //this.Canvas.Children.Clear();
            //Console.WriteLine("aaaa\n");
        }
        public String IsContain(double x, double y)
        {
            x -= 140;
            y -= 100;
            for (int i = this.rectList.Count - 1; i >= 0; i--)
            {
                if (typeOfShape[i] == 5)
                {
                    dynamic a = rectList[i];
                    Point p = new Point(x, y);

                    if (DistanceFromPointToPoint(p, a, 2) < 5 && DistanceFromPointToPoint(p, a, 2) > 0)
                    {
                        resize = true;
                        this.Cursor = Cursors.ScrollE;
                        pointArrow = 2;
                        return rectList[i].Uid;
                    }
                    else if (DistanceFromPointToPoint(p, a, 1) < 5 && DistanceFromPointToPoint(p, a, 1) > 0)
                    {
                        resize = true;
                        this.Cursor = Cursors.ScrollW;
                        pointArrow = 1;
                        return rectList[i].Uid;
                    }
                    else if (DistanceFromPointToLine(p, a) < 5 && DistanceFromPointToLine(p, a) > 0)
                    {
                        move = true;
                        rectList[i].Stroke = Brushes.Red;
                        //Console.WriteLine("return "+ rectList[i].Uid);
                        return rectList[i].Uid;
                    }
                    //Console.WriteLine($" {DistanceFromPointToPoint(p, a, Canvas.GetLeft(rectList[i]), Canvas.GetTop(rectList[i]), 1)}");
                    this.Cursor = null;
                    rectList[i].Stroke = Brushes.Black;
                }
                double x0 = Canvas.GetLeft(rectList[i]) * zoom;
                double y0 = Canvas.GetTop(rectList[i]) * zoom;
                double x1 = (x0 + rectList[i].Width * zoom);
                double y1 = (y0 + rectList[i].Height * zoom);
                double valueOfDistance = 0;
                // Set value to handle point for the different shape
                if (typeOfShape[i] == 1 || typeOfShape[i] == 2)
                {
                    valueOfDistance = 10;
                }
                else if (typeOfShape[i] == 3 || typeOfShape[i] == 4)
                {
                    valueOfDistance = 20;
                }
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
                    if(typeOfShape[i]!=5)
                        this.Cursor = Cursors.SizeNWSE;
                    //rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 7;
                    return rectList[i].Uid;
                }
                else if ((x1 - valueOfDistance <= x && x <= x1 + valueOfDistance) && (y1 - valueOfDistance <= y && y <= y1 + valueOfDistance))
                {
                    this.resize = true;
                    if (typeOfShape[i] != 5)
                        this.Cursor = Cursors.SizeNWSE;
                    //rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 5;
                    return rectList[i].Uid;
                }
                else if ((y0 - valueOfDistance <= y && y <= y0 + valueOfDistance) && (x1 - valueOfDistance <= x && x <= x1 + valueOfDistance))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNESW;
                    //rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 8;
                    return rectList[i].Uid;
                }
                else if ((y1 - valueOfDistance <= y && y <= y1 + valueOfDistance) && (x0 - valueOfDistance <= x && x <= x0 + valueOfDistance))
                {
                    this.resize = true;
                    this.Cursor = Cursors.SizeNESW;
                    //rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 6;
                    return rectList[i].Uid;
                }
                else if (x0 - valueOfDistance <= x && x <= x0 + valueOfDistance && y0 <= y && y <= y1)
                {
                    if(!showAdorner)
                    {
                        this.isDrawArrow = true;
                        typePoint = 1;
                        this.Cursor = Cursors.Cross;
                    }
                        
                    else
                    {
                        this.resize = true;
                        this.Cursor = Cursors.SizeWE;
                    }

                    //rectList[i].Stroke = Brushes.Red;
                    direction = 1;
                    dragHandle = 4;
                    return rectList[i].Uid;
                }
                else if (y0 - valueOfDistance <= y && y <= y0 + valueOfDistance && x0 <= x && x <= x1)
                {
                    if (!showAdorner)
                    {
                        this.isDrawArrow = true;
                        typePoint = 2;
                        this.Cursor = Cursors.Cross;
                    }
                    else
                    {
                        this.resize = true;
                        this.Cursor = Cursors.SizeNS;
                    }
                    
                    //rectList[i].Stroke = Brushes.Red;
                    direction = -1;
                    dragHandle = 1;
                    return rectList[i].Uid;
                }
                else if (y1 - valueOfDistance <= y && y <= y1 + valueOfDistance && x0 <= x && x <= x1)
                {
                    if (!showAdorner)
                    {
                        this.isDrawArrow = true;
                        typePoint = 4;
                        this.Cursor = Cursors.Cross;
                    }
                    else
                    {
                        this.resize = true;
                        this.Cursor = Cursors.SizeNS;
                    }
                       
                    direction = 1;
                    dragHandle = 3;
                    return rectList[i].Uid;
                }
                else if (x1 - valueOfDistance <= x && x <= x1 + valueOfDistance && y0 <= y && y <= y1)
                {
                    if (!showAdorner)
                    {
                        this.isDrawArrow = true;
                        typePoint = 3;
                        this.Cursor = Cursors.Cross;
                    }
                    else
                    {
                        this.resize = true;
                        this.Cursor = Cursors.SizeWE;
                    }
                        
                    //rectList[i].Stroke = Brushes.Red;
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

        public String IsContainTextBox(double x, double y)
        {
            x -= 140;
            y -= 100;
            for (int i = this.onlyTextBoxes.Count - 1; i >= 0; i--)
            {
                double x0 = Canvas.GetLeft(onlyTextBoxes[i]) * zoom;
                //Console.WriteLine("x0 is: " +  x0);
                double y0 = Canvas.GetTop(onlyTextBoxes[i]) * zoom;
                double x1 = (x0 + onlyTextBoxes[i].MinWidth * zoom);
                double y1 = (y0 + onlyTextBoxes[i].MinHeight * zoom);
                double valueOfDistance = 0;
                if (x0 + valueOfDistance <= x && x <= x1 - valueOfDistance && (y0 + valueOfDistance <= y && y <= y1 - valueOfDistance))
                {
                    this.move = true;
                    if (!isColorPicker)
                        this.Cursor = Cursors.SizeAll;
                    else
                        this.Cursor = Cursors.Pen;
                    return onlyTextBoxes[i].Uid;
                }
            }
            return "-1";
        }
        // Move and resize shape func
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //this.Cursor = Cursors.SizeNWSE;
            if (shapeId != -1)
            {
                preShapeId = shapeId;
            }
            //Console.WriteLine(shapeId);
            if (shapeId == -1)
            {
                String s = IsContain(e.GetPosition(this).X, e.GetPosition(this).Y);
                //cause IsContain return shapeID - which is String so we have to try to parse it into int
                bool success = Int32.TryParse(s, out shapeId);
            }

            // Check if textbox id is exists or not
            if (textBoxId == -1)
            {
                String s = IsContainTextBox(e.GetPosition(this).X, e.GetPosition(this).Y);
                bool succes = Int32.TryParse(s, out textBoxId);
            }
            //when mouse button is release , stop this function
            if (e.LeftButton == MouseButtonState.Released || (shapeId < 0 && textBoxId <0))
            {
                shapeId = -1;
                textBoxId = -1;
                if (move) move = !move;
                if (resize) resize = !resize;
                if (isDrawArrow)
                {
                    typePoint = 0;
                    isDrawArrow = !isDrawArrow;
                }
                delta = direction = 0;
                return;
            }
            
            //Console.WriteLine($"shape id = {shapeId}");
            //Console.WriteLine($"shapeid ={shapeId}");
            //action when moving shape  
            if (isDrawArrow)
            {
                int id = rectList.Count - 1;
                dynamic a = rectList[id];
                ResizeArrow(2, e.GetPosition(this).X, e.GetPosition(this).Y, id);
                //Console.WriteLine($"x & y=  {e.GetPosition(this).X} {e.GetPosition(this).Y}");
                int temp = Int32.Parse(IsContain(e.GetPosition(this).X, e.GetPosition(this).Y));
                if (temp != -1 && typeOfShape[temp] != 5 && temp != a.ShapeID1)
                {
                    Console.WriteLine($"arrow AT SHAPE ID {temp}");
                    ResizeArrow(2, Canvas.GetLeft(rectList[temp]) + 140, Canvas.GetTop(rectList[temp]) + 100 + rectList[temp].Width / 2, id, temp);
                    if (!bindingArrowShape[temp].Contains(id))
                        bindingArrowShape[temp].Add(id);
                    resize = false;
                    return;
                }
                return;                
            }
            else if (move)
            {
                //type= 5 is arrow
                if (textBoxId != -1)
                {
                    double x = (e.GetPosition(this).X / zoom - (onlyTextBoxes[textBoxId].MinWidth / zoom) / 2) - 140 / zoom;
                    double y = (e.GetPosition(this).Y / zoom - (onlyTextBoxes[textBoxId].MinHeight / zoom) / 2) - 100 / zoom;
                    Canvas.SetLeft(onlyTextBoxes[textBoxId], x);
                    Canvas.SetTop(onlyTextBoxes[textBoxId], y);
                }

                else if (typeOfShape[shapeId] != 5)
                {
                    double x = (e.GetPosition(this).X/zoom - (rectList[shapeId].Width/zoom) / 2)  - 140/zoom;
                    double y = (e.GetPosition(this).Y/zoom - (rectList[shapeId].Height/zoom) / 2) - 100/zoom;
                    Canvas.SetLeft(rectList[shapeId], x);
                    Canvas.SetTop(rectList[shapeId], y);
                    Canvas.SetLeft(textBoxes[shapeId], x + (rectList[shapeId].Width - textBoxes[shapeId].MinWidth) / 2);
                    Canvas.SetTop(textBoxes[shapeId], y + (rectList[shapeId].Height - textBoxes[shapeId].MinHeight) / 2);
                    //Console.WriteLine($"left = {Canvas.GetLeft(rectList[shapeId])}  top = {Canvas.GetTop(rectList[shapeId])}");
                    if (bindingArrowShape[shapeId].Count >1)
                    {                        
                        for(int i=1; i < bindingArrowShape[shapeId].Count; i++)
                        {
                            //Console.WriteLine($"THIS IS AT MOVE SHAPE : i ={i}");
                            dynamic temp = rectList[bindingArrowShape[shapeId][i]];
                            Console.WriteLine($"ShapeID1 = {temp.ShapeID1}\n SHAPEID2 = {temp.ShapeID2}");
                            if (temp.ShapeID1 == shapeId)
                            {
                                Point p = GetPositionOf4Point(temp.TypePoint1, shapeId);
                                ResizeArrow(1, p.X+140, p.Y+100, bindingArrowShape[shapeId][i]);
                            }
                            if (temp.ShapeID2 == shapeId)
                                ResizeArrow(2, Canvas.GetLeft(rectList[shapeId]) + 140, Canvas.GetTop(rectList[shapeId]) + 100 + rectList[shapeId].Width / 2, bindingArrowShape[shapeId][i]);
                        }                            
                    }                        
                }
                else
                {
                    double x = (e.GetPosition(this).X  - 140);
                    double y = (e.GetPosition(this).Y  - 100) ;     
                    dynamic a1 = rectList[shapeId];
                    Canvas.Children.Remove(rectList[shapeId]);
                    Point newStart = a1.StartPoint;
                    Point newEnd = a1.EndPoint;
                    int shapeid1 = a1.ShapeID1;
                    int shapeid2 = a1.ShapeID2;
                    int typepoint = a1.TypePoint1;
                    Arrow arrow = new Arrow
                    {
                        StartPoint = newStart,
                        EndPoint = newEnd,
                        Left=x,
                        Top=y,
                        Stroke = Brushes.Black,
                        ShapeID1=shapeid1,
                        ShapeID2=shapeid2,
                        TypePoint1 = typepoint,
                        StrokeThickness = 2,
                        Uid = shapeId.ToString()
                    };
                    rectList[shapeId] = arrow;
                    this.Canvas.Children.Add(rectList[shapeId]);
                    Canvas.SetLeft(rectList[shapeId], x);
                    Canvas.SetTop(rectList[shapeId], y);
                   
                }
            }
            //action when resize shape
            else if (resize)
            {
                if(typeOfShape[shapeId]==5)
                {
                    dynamic a = rectList[shapeId];                    
                    ResizeArrow(pointArrow, e.GetPosition(this).X, e.GetPosition(this).Y, shapeId);
                    //Console.WriteLine($"x & y=  {e.GetPosition(this).X} {e.GetPosition(this).Y}");
                    int temp = Int32.Parse(IsContain(e.GetPosition(this).X, e.GetPosition(this).Y));
                    if (temp != -1 && typeOfShape[temp] != 5)
                    {
                        Console.WriteLine($"arrow AT SHAPE ID {temp}");
                        ResizeArrow(2, Canvas.GetLeft(rectList[temp]) + 140, Canvas.GetTop(rectList[temp]) + 100 + rectList[temp].Width / 2, shapeId,temp);
                        if(!bindingArrowShape[temp].Contains(shapeId))
                            bindingArrowShape[temp].Add(shapeId);
                        resize=false;
                        return;
                    }
                    return;
                }
                // If shape is rectangle or circle
                // Get current pos x
                double x = (e.GetPosition(this).X/zoom   - 140);
                // Get currennt pos y
                double y = (e.GetPosition(this).Y/zoom  - 100);
                double x0 = Canvas.GetLeft(rectList[shapeId]);
                double y0 = Canvas.GetTop(rectList[shapeId]);
                // Get the bottom pos x1,y1
                double x1 = x0 + rectList[shapeId].Width / zoom;
                double y1 = y0 + rectList[shapeId].Height / zoom;
                //Console.WriteLine("Drag handle is " + dragHandle);
                double deltaDistanceY = y - y1;
                double deltaDistanceX = x - x1;
                try
                {
                    switch (dragHandle)
                    {
                        // Case handle vertical alignment for shapes
                        case 1:
                            Canvas.SetTop(rectList[shapeId], y * zoom);
                            rectList[shapeId].Height = (y1 - Canvas.GetTop(rectList[shapeId]))*zoom;
                            break;
                        case 3:
                            rectList[shapeId].Height += deltaDistanceY * zoom;
                            break;

                        // Case handle horizontal alignment for shapes
                        case 2:
                            rectList[shapeId].Width += deltaDistanceX * zoom;
                            break;
                        case 4:
                            Canvas.SetLeft(rectList[shapeId], x * zoom);
                            rectList[shapeId].Width = (x1 - Canvas.GetLeft(rectList[shapeId])) * zoom;
                            break;
                        // Case handle diagonal alignment right-bottom for shapes
                        case 5:
                            double deltaX = x - x1;
                            rectList[shapeId].Width += deltaX * zoom;
                            double deltaY = y - y1;
                            rectList[shapeId].Height += deltaY * zoom;
                            break;
                        // Case handle diagonal alignment left-bottom for shapes
                        case 6:
                            rectList[shapeId].Height += deltaDistanceY * zoom;
                            Canvas.SetLeft(rectList[shapeId], x * zoom);
                            rectList[shapeId].Width = (x1 - Canvas.GetLeft(rectList[shapeId])) * zoom;
                            break;

                        // Case handle diagonal alignment left-top for shapes
                        case 7:
                            Canvas.SetTop(rectList[shapeId], y * zoom);
                            rectList[shapeId].Height = (y1 - Canvas.GetTop(rectList[shapeId])) * zoom;
                            Canvas.SetLeft(rectList[shapeId], x * zoom);
                            rectList[shapeId].Width = (x1 - Canvas.GetLeft(rectList[shapeId])) * zoom;
                            break;

                        // Case handle diagonal alignment right-top for shapes
                        case 8:
                            Canvas.SetTop(rectList[shapeId], y * zoom);
                            rectList[shapeId].Height = (y1 - Canvas.GetTop(rectList[shapeId])) * zoom;
                            rectList[shapeId].Width += deltaDistanceX * zoom;
                            break;
                    }
                    Canvas.SetLeft(textBoxes[shapeId], x0 + (rectList[shapeId].Width - textBoxes[shapeId].MinWidth) / 2);
                    Canvas.SetTop(textBoxes[shapeId], y0 + (rectList[shapeId].Height - textBoxes[shapeId].MinHeight) / 2);
                    this.InvalidateVisual();
                }
                catch (Exception exception){}
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

            if (shapeId != -1)
            {
                preShapeId = shapeId;
            }
            if (shapeId >= 0)
            {
                shapeId = -1;
            }
            if (textBoxId >= 0)
            {
                textBoxId = -1;
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
            ShapeInfo info = new ShapeInfo();
            InfoList.Add(info);
            rectList.Add(rect);
            List<int> temp= new List<int>() { -1 };
            bindingArrowShape.Add(temp);
            // Add type of shape of the rectangle
            typeOfShape.Add(1);
            Canvas.SetLeft(rect, 100);
            Canvas.SetTop(rect, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
           // Console.WriteLine("Coor of shape " + Canvas.GetLeft(rect) + " " + Canvas.GetTop(rect));
            //add adorner for shape     
            myAdornerLayer = AdornerLayer.GetAdornerLayer(rect);
            adornerList.Add(new SimpleCircleAdorner(rect));




            CreateTextBoxForShapes(textBoxes, rect);

            //Canvas.SetLeft(textBox, 110);
            //Canvas.SetTop(textBox, 35);

            this.InvalidateVisual();
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
            ShapeInfo info = new ShapeInfo();
            InfoList.Add(info);
            rectList.Add(ellipse);
            List<int> temp = new List<int>() { -1 };
            bindingArrowShape.Add(temp);
            typeOfShape.Add(2);
            Canvas.SetLeft(ellipse, 100);
            Canvas.SetTop(ellipse, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            //add adorner for shape            
            myAdornerLayer = AdornerLayer.GetAdornerLayer(ellipse);
            adornerList.Add(new SimpleCircleAdorner(ellipse));
            CreateTextBoxForShapes(textBoxes, ellipse);
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
                Width = 150,
                Height = 100,
                Points = myPointCollection,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Stretch = Stretch.Fill,
                Uid = rectList.Count.ToString()
            };
            ShapeInfo info = new ShapeInfo();
            info.Points = myPointCollection;
            info.havePoints = true;
            InfoList.Add(info);
            rectList.Add(polygon);
            List<int> temp = new List<int>() { -1 };
            bindingArrowShape.Add(temp);
            typeOfShape.Add(3);
            Canvas.SetLeft(polygon, 100);
            Canvas.SetTop(polygon, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            //add adorner for shape            
            myAdornerLayer = AdornerLayer.GetAdornerLayer(polygon);
            adornerList.Add(new SimpleCircleAdorner(polygon));
            CreateTextBoxForShapes(textBoxes, polygon);
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
                Width = 150,
                Height = 150,
                Points = myPointCollection,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Stretch = Stretch.Fill,
                Uid = rectList.Count.ToString()
            };
            ShapeInfo info = new ShapeInfo();
            info.Points = myPointCollection;
            info.havePoints = true;
            InfoList.Add(info);
            rectList.Add(polygon);
            List<int> temp = new List<int>() { -1 };
            bindingArrowShape.Add(temp);
            typeOfShape.Add(4);
            Canvas.SetLeft(polygon, 100);
            Canvas.SetTop(polygon, 10);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            //add adorner for shape           
            myAdornerLayer = AdornerLayer.GetAdornerLayer(polygon);
            adornerList.Add(new SimpleCircleAdorner(polygon));
            CreateTextBoxForShapes(textBoxes, polygon);
            this.InvalidateVisual();
        }

        //paint shape when mouse change into pen
        private void Canvas_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            for(int i=0; i < bindingArrowShape.Count; i++)
            {
                Console.Write($"i= {i}:     ");
                for (int j = 0; j < bindingArrowShape[i].Count; j++)
                    Console.Write($"{bindingArrowShape[i][j]}    ");
                Console.WriteLine();
            }
            Console.WriteLine();
            if (shapeId != -1)
            {
                preShapeId = shapeId;
            }

            //Console.WriteLine($" showadorner {showAdorner.ToString()}");
            if (shapeId == -1)
            {
                String s = IsContain(e.GetPosition(this).X, e.GetPosition(this).Y);
                //cause IsContain return shapeID - which is String so we have to try to parse it into int
                bool success = Int32.TryParse(s, out shapeId);
                if ((shapeId > -1) && !showAdorner)
                    myAdornerLayer.Remove(adornerList[shapeId]);
            }
            if (e.LeftButton == MouseButtonState.Released || shapeId < 0)
            {
                if (showAdorner)
                    showAdorner = false;
                isDrawArrow = false;
                typePoint = 0;
                clearAllAdorner();
                shapeId = -1;
                return;
            }
            var converter = new System.Windows.Media.BrushConverter();
            if (isColorPicker)
            {
                rectList[shapeId].Fill = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
                textBoxes[shapeId].Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
            }
            if(isDrawArrow)
            {
                double x0=-1000, y0=-1000;
                Point p=GetPositionOf4Point(typePoint, shapeId);
                x0 = p.X;
                y0 = p.Y;  
                DrawArrow(x0,y0, shapeId,typePoint);
                if (!bindingArrowShape[shapeId].Contains(rectList.Count-1))
                    bindingArrowShape[shapeId].Add(rectList.Count - 1);
            }
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
            typeOfShape.Clear();
            InfoList.Clear();
            textBoxes.Clear();
            clearAllAdorner();
            this.Canvas.Children.Clear();
        }
        public void clearAllAdorner()
        {
            for (int i = 0; i < adornerList.Count; i++)
                this.myAdornerLayer.Remove(adornerList[i]);
        }
        private void pasteCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsText(TextDataFormat.Xaml);
        }

        private void DelCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCmdBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)

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
            if (shape == null) return;
            //this code allow us to save shape info to Clipboard by turn shape info to string and the save it
            xaml = XamlWriter.Save(shape);
            Clipboard.SetText(xaml, TextDataFormat.Xaml);
        }

        private void DelCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //remove shape from canvas
            if (shape == null) return;
            canvas.Children.Remove(shape);
            canvas.Children.Remove(textBoxes[Convert.ToInt32(shape.Uid)]);
            canvas.Children.Remove(adornerList[Convert.ToInt32(shape.Uid)]);
        }
        private void SaveCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FileStream fs = File.Open("SaveCanvas.xaml", FileMode.Create);
            XamlWriter.Save(Canvas, fs);
            fs.Close();
        }
        private void CutCmdBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //canvas.Children.Remove allow to remove a canvas children member as shape in this canvas
            if (shape == null) return;
            canvas.Children.Remove(shape);
            canvas.Children.Remove(textBoxes[Convert.ToInt32(shape.Uid)]);
            canvas.Children.Remove(adornerList[Convert.ToInt32(shape.Uid)]);
            string xaml = XamlWriter.Save(shape);
            Clipboard.SetText(xaml, TextDataFormat.Xaml);
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //set canvas to working-on canvas
            canvas = sender as Canvas;
            //check and set shape to our just-clicked shape

            if (e.ClickCount >= 2)
            {
                String s = IsContain(e.GetPosition(this).X, e.GetPosition(this).Y);
                //cause IsContain return shapeID - which is String so we have to try to parse it into int
                bool success = Int32.TryParse(s, out shapeId);
                String sText = IsContainTextBox(e.GetPosition(this).X, e.GetPosition(this).Y);
                bool succesTextBox = Int32.TryParse(s, out textBoxId);

                if (shapeId > -1)

                    textBoxes[shapeId].IsEnabled = true;
            }
            if (canvas == null)
                return;
            if (rectList.Count.ToString() != "0")
            {
                HitTestResult hitTestResult = VisualTreeHelper.HitTest(canvas, e.GetPosition(canvas));
                shape = hitTestResult.VisualHit as Shape;
            }
            if (shape == null)

                return;
            
            
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

                        ShapeInfo info = new ShapeInfo();

                        InfoList.Add(info);

                        sw.Write(xaml);

                        sw.Flush();

                        stream.Seek(0, SeekOrigin.Begin);

                        Shape shape = XamlReader.Load(stream) as Shape;

                        shape.Uid = rectList.Count.ToString();

                        rectList.Add(shape);

                        typeOfShape.Add(typeOfShape[Int32.Parse(shape.Uid) - 1]);

                        Canvas.SetLeft(rectList[rectList.Count - 1], left);

                        Canvas.SetTop(rectList[rectList.Count - 1], top);

                        top = 100;

                        left = 200;

                        Canvas.Children.Add(rectList[rectList.Count - 1]);

                        CreateTextBoxForShapes(textBoxes, shape, textBoxes[Int32.Parse(shape.Uid) - 1].Text, textBoxes[Int32.Parse(shape.Uid) - 1].Background);

                        left = 200;

                        Canvas.SetLeft(textBoxes[Int32.Parse(shape.Uid)], Canvas.GetLeft(shape) + (shape.Width - textBoxes[Int32.Parse(shape.Uid)].Width) / 2);

                        Canvas.SetTop(textBoxes[Int32.Parse(shape.Uid)], Canvas.GetTop(shape) + (rectList[Int32.Parse(shape.Uid)].Height - textBoxes[Int32.Parse(shape.Uid) - 1].Height) / 2);

                        myAdornerLayer = AdornerLayer.GetAdornerLayer(shape);

                        adornerList.Add(new SimpleCircleAdorner(shape));

                        this.InvalidateVisual();

                    }

                }

            }

        }

        private void Open(object sender, RoutedEventArgs e)
        {
            string reopenedState = string.Empty;
            using (FileStream stream = new FileStream("Type.json", FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                reopenedState = reader.ReadToEnd();
            }
            var type = JsonConvert.DeserializeObject<List<int>>(reopenedState);
            type.ForEach(typeOfShape.Add);
            using (FileStream stream = new FileStream("Info.json", FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                reopenedState = reader.ReadToEnd();
            }
            var info = JsonConvert.DeserializeObject<List<ShapeInfo>>(reopenedState);
            InfoList = new List<ShapeInfo>();
            info.ForEach(InfoList.Add);

            AddShape(InfoList, rectList);

           
            reopenedState = string.Empty;
        }

        private void AddShape(List<ShapeInfo> infoList, List<Shape> rectList)
        {
            rectList.Clear();
            adornerList.Clear();
            textBoxes.Clear();
            this.Canvas.Children.Clear();
            for (int i = 0; i < infoList.Count; i++)
            {
                Shape shape;
                switch (typeOfShape[i])
                {
                    case 1:
                        shape = new Rectangle();
                        break;
                    case 2:
                        shape = new Ellipse();
                        break;
                    default:
                        shape = new Polygon()
                        {
                            Points = infoList[i].Points,
                        };
                        break;
                }
                shape.Width = infoList[i].Width;
                shape.Height = infoList[i].Height;
                shape.Fill = infoList[i].Fill;
                shape.Stroke = infoList[i].Stroke;
                shape.StrokeThickness = infoList[i].StrokeThickness;
                shape.Stretch = infoList[i].Stretch;
                shape.Uid = rectList.Count.ToString();
                rectList.Add(shape);
                Canvas.SetLeft(shape, infoList[i].Y);
                Canvas.SetTop(shape, infoList[i].X);
                Canvas.Children.Add(rectList[rectList.Count - 1]);
                //add adorner for shape            
                myAdornerLayer = AdornerLayer.GetAdornerLayer(shape);
                adornerList.Add(new SimpleCircleAdorner(shape));
                CreateTextBoxForShapes(textBoxes, shape, infoList[i].text, infoList[i].color);

            }
        }
        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.IsEnabled = true;
            textBox.IsReadOnly = false;
            textBox.BorderThickness = new Thickness(1, 1, 1, 1);
            //textBox.CaretIndex = textBox.Text.Count();
            textBox.SelectAll();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.IsReadOnly = true;
            textBox.IsEnabled = false;
            textBox.BorderThickness = new Thickness(0, 0, 0, 0);

        }

        private void TextBox_LostFocus2(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.IsReadOnly = true;
            textBox.IsEnabled = true;
            textBox.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void Button_Arrow_Click(object sender, RoutedEventArgs e)
        {
            double x0 = 0;
            double y0 = 0;
            double x1 = 100;
            double y1 = 100;
            double distance = Math.Sqrt(Math.Pow((x1 - x0), 2) + Math.Pow((y1 - y0), 2));
            Arrow arrow = new Arrow
            {
                StartPoint = new Point(x0, y0),
                EndPoint = new Point(x1, y1),
                Left= 400,
                Top=200,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Uid = rectList.Count.ToString()
            };
            rectList.Add(arrow);
            List<int> temp = new List<int>() { -1 };
            bindingArrowShape.Add(temp);
            typeOfShape.Add(5);
            Canvas.SetLeft(arrow, 400);
            Canvas.SetTop(arrow, 200);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            //add adorner for shape           
            myAdornerLayer = AdornerLayer.GetAdornerLayer(arrow);
            ArrowAdorner myAdorner = new ArrowAdorner(arrow);
            dynamic a = arrow;
            myAdorner.From = a.StartPoint;
            myAdorner.To = a.EndPoint;
            adornerList.Add(myAdorner);
            CreateTextBoxForShapes(textBoxes, arrow);
            textBoxes[rectList.Count - 1].Width = 0;
            textBoxes[rectList.Count - 1].Height = 0;
            this.InvalidateVisual();
        }

        private void CreateTextBoxForShapes(List<TextBox> textBoxes, Shape shape, string text = null, Brush color = null)
        {
            TextBox textBox = new TextBox
            {
                MinWidth = 80,
                MinHeight = 50,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                MaxHeight = 2000,
                Text = "Shape " + rectList.Count,
                FontSize = 16,
                Background = Brushes.White,
                BorderThickness = new Thickness(0, 0, 0, 0),
                IsReadOnly = true,
                IsEnabled = false
            };
            if (text != null)
            {
                textBox.Text = text;
            }
            if (color != null)
            {
                textBox.Background = color;
            }
            textBoxes.Add(textBox);
            textBox.MouseDoubleClick += TextBox_MouseDoubleClick;
            textBox.LostFocus += TextBox_LostFocus;

            Canvas.SetLeft(textBox, 100 + (shape.Width - textBox.MinWidth) / 2);
            Canvas.SetTop(textBox, 10 + (shape.Height - textBox.MinHeight) / 2);
            Canvas.Children.Add(textBoxes[textBoxes.Count - 1]);
        }

        private void buttonZoomin_Click(object sender, RoutedEventArgs e)
        {
            zoom += zoomDelta;

            Canvas.RenderTransform = new ScaleTransform(zoom, zoom);

        }

        private void buttonZoomout_Click(object sender, RoutedEventArgs e)
        {
            zoom -= zoomDelta; 
            Canvas.RenderTransform = new ScaleTransform(zoom, zoom);

        }


        public double DistanceFromPointToLine(Point p, Arrow arrow)
        {
            Point l1 = new Point(arrow.Left+ arrow.StartPoint.X, arrow.Top + arrow.StartPoint.Y);
            Point l2 = new Point(arrow.Left + arrow.EndPoint.X, arrow.Top + arrow.EndPoint.Y);
            // border to limit coordinate of p
            double xMax; double xMin; double yMax; double yMin;
            if (l1.X < l2.X)
            {
                xMax = l2.X;
                xMin = l1.X;
            }
            else
            {
                xMin = l2.X;
                xMax = l1.X;
            }
            if (l1.Y < l2.Y)
            {
                yMax = l2.Y;
                yMin = l1.Y;
            }
            else
            {
                yMin = l2.Y;
                yMax = l1.Y;
            }
            xMax -= 5;
            xMin += 5;
            yMax -= 5;
            yMin += 5;
            if (xMin <= p.X && p.X <= xMax && yMin <= p.Y && p.Y <= yMax)
            {
                return Math.Abs((l2.X - l1.X) * (l1.Y - p.Y) - (l1.X - p.X) * (l2.Y - l1.Y)) /

                    Math.Sqrt(Math.Pow(l2.X - l1.X, 2) + Math.Pow(l2.Y - l1.Y, 2));
            }
            return 999;
        }
        public double DistanceFromPointToPoint(Point p, Arrow arrow, double typeOfPoint)
        {
            Point l1 = new Point(arrow.Left + arrow.StartPoint.X, arrow.Top + arrow.StartPoint.Y);
            Point l2 = new Point(arrow.Left + arrow.EndPoint.X, arrow.Top + arrow.EndPoint.Y);
            if (typeOfPoint==1)
            {
                return Math.Sqrt(Math.Pow(p.X - l1.X, 2) +
                Math.Pow(p.Y - l1.Y, 2) * 1.0);
            }
            else if(typeOfPoint == 2)
            {
                return Math.Sqrt(Math.Pow(p.X - l2.X, 2) +
                Math.Pow(p.Y - l2.Y, 2) * 1.0);
            }
            return 999;
            
        }

        private void Button_TextBoxClick(object sender, RoutedEventArgs e)
        {
            TextBox textBox = new TextBox
            {
                MinWidth = 80,
                MinHeight = 50,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                Text = "Text " + onlyTextBoxes.Count,
                FontSize = 16,
                Background = Brushes.White,
                BorderThickness = new Thickness(0, 0, 0, 0),
                Uid = onlyTextBoxes.Count.ToString(),
                IsEnabled = true
            };
            onlyTextBoxes.Add(textBox);
       
            textBox.MouseDoubleClick += TextBox_MouseDoubleClick;
            textBox.LostFocus += TextBox_LostFocus2;

            Canvas.SetLeft(textBox, 150);
            Canvas.SetTop(textBox, 150);
            Canvas.Children.Add(onlyTextBoxes[onlyTextBoxes.Count - 1]);
        }

        public void ResizeArrow(int typeOfPoint, double x, double y, int id)
        {
            //Console.WriteLine($"arrow id is {shapeId}");
            x -= 140;
            y -= 100;
            try
            {
                //Console.WriteLine($"id= {id}");
                dynamic a1 = rectList[id];
                double leftCanvas = a1.Left;
                double topCanvas = a1.Top;
                Canvas.Children.Remove(rectList[id]);
                Point newStart = a1.StartPoint;
                Point newEnd = a1.EndPoint;
                int shapeid1 = a1.ShapeID1;
                int shapeid2 = a1.ShapeID2;
                int typepoint = a1.TypePoint1;
                if (typeOfPoint == 1)
                {
                    Console.WriteLine($" MOUSE = {x} ; {y}");
                    Console.WriteLine($" Canvas = {leftCanvas} ; {topCanvas}\n");
                    //Console.WriteLine($" dx= {x}   dy={y}");
                    double dx = 0 - (x-leftCanvas);
                    double dy = 0 - (y-topCanvas);
                    newEnd = new Point(a1.EndPoint.X + dx, a1.EndPoint.Y + dy);
                    leftCanvas = x;
                    topCanvas = y;
                }
                else
                {
                    x -= leftCanvas;
                    y -= topCanvas;
                    newEnd = new Point(x, y);
                }
                Arrow arrow = new Arrow
                {
                    StartPoint = newStart,
                    EndPoint = newEnd,
                    Left = leftCanvas,
                    Top = topCanvas,
                    ShapeID1= shapeid1,
                    ShapeID2= shapeid2,
                    TypePoint1= typepoint,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Uid = a1.Uid
                };
                rectList[id] = arrow;
                this.Canvas.Children.Add(rectList[id]);               
                Canvas.SetLeft(rectList[id], leftCanvas);
                Canvas.SetTop(rectList[id], topCanvas);
        }
            catch(Exception e)
            {
                //Console.WriteLine($"EXCEPTION id ={id}");
            }
            
        }
        public void ResizeArrow(int typeOfPoint, double x, double y, int id, int shapeid2)
        {
            //Console.WriteLine($"arrow id is {shapeId}");
            x -= 140;
            y -= 100;
            try
            {
                //Console.WriteLine($"id= {id}");
                dynamic a1 = rectList[id];
                double leftCanvas = a1.Left;
                double topCanvas = a1.Top;
                Canvas.Children.Remove(rectList[id]);
                Point newStart = a1.StartPoint;
                Point newEnd = a1.EndPoint;
                int shapeid1 = a1.ShapeID1;
                int typepoint = a1.TypePoint1;
                if (typeOfPoint == 1)
                {

                }
                else
                {
                    x -= leftCanvas;
                    y -= topCanvas;
                    newEnd = new Point(x, y);
                }
                Arrow arrow = new Arrow
                {
                    StartPoint = newStart,
                    EndPoint = newEnd,
                    Left = leftCanvas,
                    Top = topCanvas,
                    ShapeID1 = shapeid1,
                    ShapeID2 = shapeid2,
                    TypePoint1 = typepoint,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Uid = a1.Uid
                };
                rectList[id] = arrow;
                this.Canvas.Children.Add(rectList[id]);
                if (typeOfPoint == 1)
                {
                }
                else
                {
                    Canvas.SetLeft(rectList[id], leftCanvas);
                    Canvas.SetTop(rectList[id], topCanvas);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine($"EXCEPTION id ={id}");
            }

        }

        public void DrawArrow(double x, double y,int id, int typePoint)
        {
            double x0 = 0;
            double y0 = 0;
            double x1 = 100;
            double y1 = 100;
            Arrow arrow = new Arrow
            {
                StartPoint = new Point(x0, y0),
                EndPoint = new Point(x1, y1),
                Left = x,
                Top = y,
                ShapeID1=id,
                TypePoint1=typePoint,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Uid = rectList.Count.ToString()
            };
            rectList.Add(arrow);
            List<int> temp = new List<int>() { -1 };
            bindingArrowShape.Add(temp);
            typeOfShape.Add(5);
            Canvas.SetLeft(arrow, x);
            Canvas.SetTop(arrow, y);
            Canvas.Children.Add(rectList[rectList.Count - 1]);
            //add adorner for shape           
            myAdornerLayer = AdornerLayer.GetAdornerLayer(arrow);
            ArrowAdorner myAdorner = new ArrowAdorner(arrow);
            dynamic a = arrow;
            myAdorner.From = a.StartPoint;
            myAdorner.To = a.EndPoint;
            adornerList.Add(myAdorner);
            CreateTextBoxForShapes(textBoxes, arrow);
            textBoxes[rectList.Count - 1].Width = 0;
            textBoxes[rectList.Count - 1].Height = 0;
        }
        //function to get  point -which use to connect arrow in shape 
        public Point GetPositionOf4Point(int typePoint, int id)
        {
            double x = 0, y = 0;
            switch (typePoint)
            {
                case 1: //left 
                    {
                        x = Canvas.GetLeft(rectList[id]);
                        y = Canvas.GetTop(rectList[id]) + rectList[id].Height / 2;
                        break;
                    }
                case 2: //top 
                    {
                        x = Canvas.GetLeft(rectList[id]) + rectList[id].Width / 2;
                        y = Canvas.GetTop(rectList[id]);
                        break;
                    }
                case 3: //right 
                    {
                        x = Canvas.GetLeft(rectList[id]) + rectList[id].Width;
                        y = Canvas.GetTop(rectList[id]) + rectList[id].Height / 2;
                        break;
                    }
                case 4: //bottom 
                    {
                        x = Canvas.GetLeft(rectList[id]) + rectList[id].Width / 2;
                        y = Canvas.GetTop(rectList[id]) + rectList[id].Height;
                        break;
                    }
            }
            return new Point(x, y);
        }
    }

}

