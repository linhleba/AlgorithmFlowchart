using Algorithm_Flowchart;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using Path = System.Windows.Shapes.Path;
using Size = System.Windows.Size;

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
        public bool drawArrow = false;
        //variable to  choose which point of arrow is chosen
        public int pointArrow = -1;
        //vector bind arrow with shape 
        public bool isDrawArrow = false;
        public bool isDrawPointArrow = false;
        public bool isResizeArrow = false;
        public int positionBreakPoint = -2;
        //1= left ;2 top ;3 right; 4 bottom
        public int typePoint1 = 0;
        public int typePoint2 = 0;
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
                case 4:
                    SaveFileDialog diag = new SaveFileDialog();
                    diag.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                    if (diag.ShowDialog() == true)
                    {
                        SaveCanvasToFile(Canvas, diag.FileName);
                        MessageBox.Show("Saved successfully!");
                    }
                    break;

            }
        }

        private void UpdateInfo(List<ShapeInfo> infoList, List<Shape> rectList)
        {
            for (int i = 0; i < rectList.Count; i++)
            {
                MessageBox.Show($"{InfoList.Count}");
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
                    }
                    else buttonChooseColor.Background = Brushes.Black;
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
                if (typeOfShape[i] == 5 && isDrawArrow==false)
                {
                    /*NOTE
                        1 mũi tên gồm 2 điểm : dấu chấm (type point =1)
                                                mũi tên (type =2) */
                    dynamic a = rectList[i];
                    Point p = new Point(x/zoom, y/zoom);                    
                    //tính khoảng cách từ con chuột -> trung điểm arrow , nếu nó ở gần trugn điểm thì cho phép bẻ mũi tên thành 2 phần 
                    if(a.ListPoint.Count > 0)
                    {
                        CalcAddPoint(p, a);
                        if (positionBreakPoint >= -1)
                        {
                            this.Cursor = Cursors.Cross;
                            rectList[i].Stroke = Brushes.Green;
                            if (!isDrawPointArrow)
                            {
                                isDrawPointArrow = true;
                            }
                            return rectList[i].Uid;
                        }
                        else if(CalcMoveArrow(p,a) ==true && !isDrawPointArrow)
                        {
                            move = true;
                            rectList[i].Stroke = Brushes.Red;
                            this.Cursor = Cursors.SizeAll;
                            return rectList[i].Uid;
                        }
                        else if (DistanceFromPointToPoint(p, a, 2) < 5 && DistanceFromPointToPoint(p, a, 2) > 0 && !isDrawPointArrow)
                        {
                            resize = true;
                            this.Cursor = Cursors.ScrollE;
                            pointArrow = 2;
                            return rectList[i].Uid;
                        }
                        //tính khoảng cách từ con chuột -> phần dấu chấm , nếu nó ở gần thì cho phép kéo mũi tên ở dấu chấm
                        else if (DistanceFromPointToPoint(p, a, 1) < 5 && DistanceFromPointToPoint(p, a, 1) > 0 && !isDrawPointArrow)
                        {
                            resize = true;
                            this.Cursor = Cursors.ScrollW;
                            pointArrow = 1;
                            return rectList[i].Uid;
                        }
                    }
                    else
                    {
                        if (DistanceFromPointToPoint(p, a, 3) < 5 && DistanceFromPointToPoint(p, a, 1) > 0 && !isDrawPointArrow)
                        {
                            this.Cursor = Cursors.Cross;
                            rectList[i].Stroke = Brushes.Green;
                            if (!isDrawPointArrow)
                            {
                                isDrawPointArrow = true;
                            }
                            return rectList[i].Uid;
                        }
                        //tính khoảng cách từ con chuột -> phần mũi tên , nếu nó ở gần thì cho phép kéo mũi tên ở phía mũi tên
                        else if (DistanceFromPointToPoint(p, a, 2) < 5 && DistanceFromPointToPoint(p, a, 2) > 0 && !isDrawPointArrow)
                        {
                            resize = true;
                            this.Cursor = Cursors.ScrollE;
                            pointArrow = 2;                            
                            return rectList[i].Uid;
                        }
                        //tính khoảng cách từ con chuột -> phần dấu chấm , nếu nó ở gần thì cho phép kéo mũi tên ở dấu chấm
                        else if (DistanceFromPointToPoint(p, a, 1) < 5 && DistanceFromPointToPoint(p, a, 1) > 0 && !isDrawPointArrow)
                        {
                            resize = true;
                            this.Cursor = Cursors.ScrollW;
                            pointArrow = 1;
                            return rectList[i].Uid;
                        }
                        //khoẳng cách giữa chuột và đương thẳng tạo bởi 2 điểm của mũi tên
                        else if (DistanceFromPointToLine(p, a) < 5 && DistanceFromPointToLine(p, a) > 0 && !isDrawPointArrow)
                        {
                            move = true;
                            rectList[i].Stroke = Brushes.Red;
                            this.Cursor = Cursors.SizeAll;
                            return rectList[i].Uid;
                        }
                    }
                    this.Cursor = null;
                    rectList[i].Stroke = Brushes.Black;
                }
                else if(typeOfShape[i] != 5)
                {
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
                        if (typeOfShape[i] != 5)
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
                        if (!showAdorner)
                        {
                            this.isDrawArrow = true;
                            typePoint1 = 1;
                            this.Cursor = Cursors.Cross;
                        }
                        else
                        {
                            this.resize = true;
                            this.Cursor = Cursors.SizeWE;
                        }
                        //Console.WriteLine($"isDRAW {isDrawArrow.ToString()}");
                        if (isDrawArrow || isResizeArrow)
                        {
                            typePoint2 = 1;
                            rectList[i].Stroke = Brushes.Red;
                        }
                        //
                        direction = 1;
                        dragHandle = 4;
                        return rectList[i].Uid;
                    }
                    else if (y0 - valueOfDistance <= y && y <= y0 + valueOfDistance && x0 <= x && x <= x1)
                    {
                        if (!showAdorner)
                        {
                            this.isDrawArrow = true;
                            typePoint1 = 2;
                            this.Cursor = Cursors.Cross;

                        }
                        else
                        {
                            this.resize = true;
                            this.Cursor = Cursors.SizeNS;
                        }
                        if (isDrawArrow || isResizeArrow)
                        {
                            typePoint2 = 2;
                            rectList[i].Stroke = Brushes.Red;
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
                            typePoint1 = 4;
                            this.Cursor = Cursors.Cross;

                        }
                        else
                        {
                            this.resize = true;
                            this.Cursor = Cursors.SizeNS;
                        }
                        if (isDrawArrow || isResizeArrow)
                        {
                            typePoint2 = 4;
                            rectList[i].Stroke = Brushes.Red;
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
                            typePoint1 = 3;
                            this.Cursor = Cursors.Cross;

                        }
                        else
                        {
                            this.resize = true;
                            this.Cursor = Cursors.SizeWE;
                        }
                        if (isDrawArrow || isResizeArrow)
                        {
                            typePoint2 = 3;
                            rectList[i].Stroke = Brushes.Red;
                        }
                        //rectList[i].Stroke = Brushes.Red;
                        direction = 1;
                        dragHandle = 2;
                        return rectList[i].Uid;
                    }
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
                double x1 = (x0 + onlyTextBoxes[i].ActualWidth * zoom);
                double y1 = (y0 + onlyTextBoxes[i].ActualHeight * zoom);
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
                    typePoint1 = 0;
                    typePoint2 = 0;
                    isDrawArrow = !isDrawArrow;
                }
                isResizeArrow = false;
                positionBreakPoint = -2;
                //if (isDrawPointArrow)
                    isDrawPointArrow = false;
                delta = direction = 0;
                return;
            }
            
            //Console.WriteLine($"shape id = {shapeId}");
            //Console.WriteLine($"shapeid ={shapeId}");
            //action when moving shape  
            if(isDrawPointArrow)
            {
                MovePointArrow(e.GetPosition(this).X/zoom, e.GetPosition(this).Y/zoom, shapeId,positionBreakPoint);
            }
            else if (isDrawArrow && textBoxId == -1)
            {
                int id = rectList.Count - 1;
                dynamic a = rectList[id];
                ResizeArrow(2, e.GetPosition(this).X, e.GetPosition(this).Y, id);
                int temp = Int32.Parse(IsContain(e.GetPosition(this).X, e.GetPosition(this).Y));
                if (temp != -1 && typeOfShape[temp] != 5 && temp != a.ShapeID1)
                {
                    //Console.WriteLine($"arrow AT SHAPE ID {temp}");
                    //Console.WriteLine($"TYPE POINT {typePoint2}");
                    Point p = GetPositionOf4Point(typePoint2, temp);
                    ResizeArrow(2, p.X + 140, p.Y + 100, id, temp,typePoint2);
                    //ResizeArrow(2, Canvas.GetLeft(rectList[temp]) + 140, Canvas.GetTop(rectList[temp]) + 100 + rectList[temp].Width / 2, id, temp);
                    if (!bindingArrowShape[temp].Contains(id))
                        bindingArrowShape[temp].Add(id);
                    resize = false;
                    //typePoint = 0; ;
                    //return;
                }
                return;                
            }
            else if (move)
            {
                //type= 5 is arrow

                if (textBoxId != -1)
                {
                    //if (shapeId == -1)
                    {
                        double x = (e.GetPosition(this).X / zoom - (onlyTextBoxes[textBoxId].ActualWidth / zoom) / 2) - 140 / zoom;
                        double y = (e.GetPosition(this).Y / zoom - (onlyTextBoxes[textBoxId].ActualHeight / zoom) / 2) - 100 / zoom;
                        this.Cursor = Cursors.SizeAll;
                        Canvas.SetLeft(onlyTextBoxes[textBoxId], x);
                        Canvas.SetTop(onlyTextBoxes[textBoxId], y);
                    }
                }
                else
                {
                    if (typeOfShape[shapeId] != 5)
                    {
                        // Check to not move if textbox is enable
                        if (textBoxes[shapeId].IsEnabled == false)
                        {
                            double x = (e.GetPosition(this).X / zoom - (rectList[shapeId].Width / zoom) / 2) - 140 / zoom;
                            double y = (e.GetPosition(this).Y / zoom - (rectList[shapeId].Height / zoom) / 2) - 100 / zoom;
                            Canvas.SetLeft(rectList[shapeId], x);
                            Canvas.SetTop(rectList[shapeId], y);
                            Canvas.SetLeft(textBoxes[shapeId], x + (rectList[shapeId].Width - textBoxes[shapeId].MinWidth) / 2);
                            Canvas.SetTop(textBoxes[shapeId], y + (rectList[shapeId].Height - textBoxes[shapeId].MinHeight) / 2);
                            //Console.WriteLine($"left = {Canvas.GetLeft(rectList[shapeId])}  top = {Canvas.GetTop(rectList[shapeId])}");
                            if (bindingArrowShape[shapeId].Count > 1)
                            {
                                for (int i = 1; i < bindingArrowShape[shapeId].Count; i++)
                                {
                                    //Console.WriteLine($"THIS IS AT MOVE SHAPE : i ={i}");
                                    dynamic temp = rectList[bindingArrowShape[shapeId][i]];
                                   // Console.WriteLine($"ShapeID1 = {temp.TypePoint1}\n SHAPEID2 = {temp.TypePoint2}");
                                    if (temp.ShapeID1 == shapeId)
                                    {
                                        Point p = GetPositionOf4Point(temp.TypePoint1, shapeId);
                                        ResizeArrow(1, p.X + 140, p.Y + 100, bindingArrowShape[shapeId][i]);
                                    }
                                    if (temp.ShapeID2 == shapeId)
                                    {
                                        Point p = GetPositionOf4Point(temp.TypePoint2, shapeId);
                                        ResizeArrow(2, p.X + 140, p.Y + 100, bindingArrowShape[shapeId][i]);
                                    }
                                    //ResizeArrow(2, Canvas.GetLeft(rectList[shapeId]) + 140, Canvas.GetTop(rectList[shapeId]) + 100 + rectList[shapeId].Width / 2, bindingArrowShape[shapeId][i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        double x = (e.GetPosition(this).X) / zoom;
                        double y = (e.GetPosition(this).Y) / zoom;
                        MoveArrow(x, y, shapeId);
                        Canvas.SetLeft(textBoxes[shapeId], 99999);
                        Canvas.SetTop(textBoxes[shapeId], 99999);
                    }
                }
            }
            //action when resize shape
            else if (resize)
            {
                if(typeOfShape[shapeId]==5)
                {
                    isResizeArrow = true;
                    dynamic a = rectList[shapeId];                    
                    ResizeArrow(pointArrow, e.GetPosition(this).X/zoom, e.GetPosition(this).Y/zoom, shapeId);
                    //Console.WriteLine($"x & y=  {e.GetPosition(this).X} {e.GetPosition(this).Y}");
                    int temp = Int32.Parse(IsContain(e.GetPosition(this).X/zoom, e.GetPosition(this).Y/zoom));
                    if (temp != -1 && typeOfShape[temp] != 5)
                    {
                        //Console.WriteLine($"arrow AT SHAPE ID {temp}");
                        Point p = GetPositionOf4Point(typePoint2, temp);
                        ResizeArrow(2, (p.X + 140)/zoom, (p.Y + 100)/zoom, shapeId, temp, typePoint2);
                        if (!bindingArrowShape[temp].Contains(shapeId))
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
                    if(typeOfShape[shapeId]!=5)
                    {
                        Canvas.SetLeft(textBoxes[shapeId], x0 + (rectList[shapeId].Width - textBoxes[shapeId].MinWidth) / 2);
                        Canvas.SetTop(textBoxes[shapeId], y0 + (rectList[shapeId].Height - textBoxes[shapeId].MinHeight) / 2);
                    }
                   
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
            //Console.WriteLine($"TESSSTSTSTSTSTSTST {e.GetPosition(this).X}   {e.GetPosition(this).Y}");
            //AddPointArrow(e.GetPosition(this).X, e.GetPosition(this).Y, 0);
            /*for(int i=0; i < bindingArrowShape.Count; i++)
            {
                Console.Write($"i= {i}:     ");
                for (int j = 0; j < bindingArrowShape[i].Count; j++)
                    Console.Write($"{bindingArrowShape[i][j]}    ");
                Console.WriteLine();
            }
            Console.WriteLine();*/
            if (shapeId != -1)
            {
                preShapeId = shapeId;
            }
            clearAllAdorner();
            //Console.WriteLine($" DRAWPOINT {isDrawPointArrow.ToString()}");
            if (shapeId == -1)
            {
                String s = IsContain(e.GetPosition(this).X, e.GetPosition(this).Y);
                //cause IsContain return shapeID - which is String so we have to try to parse it into int
                bool success = Int32.TryParse(s, out shapeId);
                if ((shapeId > -1) && !showAdorner)
                    myAdornerLayer.Remove(adornerList[shapeId]);
                //isDrawPointArrow = false;
            }
            if (e.LeftButton == MouseButtonState.Released || shapeId < 0)
            {
                if (showAdorner)
                    showAdorner = false;
                isDrawArrow = false;

                isDrawPointArrow = false;
                typePoint1 = 0;
                typePoint2 = 0;

                clearAllAdorner();
                shapeId = -1;
                return;
            }
            var converter = new System.Windows.Media.BrushConverter();
            if (isColorPicker)
            {
                rectList[shapeId].Fill = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
                textBoxes[shapeId].Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");

                if (textBoxId != -1)
                {
                    Style st = new Style();
                    Trigger tg = new Trigger()
                    {
                        Property = Control.IsEnabledProperty,
                        Value = false
                    };
                    st.Setters.Add(new Setter()
                    {
                        Property = ForegroundProperty,
                        Value = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}")
                    });

                    st.Triggers.Add(tg);

                    onlyTextBoxes[textBoxId].Style = st;
                }
            }
            if(isDrawArrow)
            {
                double x0=-1000, y0=-1000;
                Point p=GetPositionOf4Point(typePoint1, shapeId);
                x0 = p.X;
                y0 = p.Y;  
                DrawArrow(x0,y0, shapeId,typePoint1);
                if (!bindingArrowShape[shapeId].Contains(rectList.Count-1))
                    bindingArrowShape[shapeId].Add(rectList.Count - 1);
            }
            if(isDrawPointArrow)
            {
                dynamic a = rectList[shapeId];
                AddPointArrow(200, 200, shapeId,positionBreakPoint);
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
            for(int i=0; i < rectList.Count; i++)
            {
                if(myAdornerLayer.GetAdorners(rectList[i])!= null)
                    myAdornerLayer.Remove(adornerList[i]);
            }
           /* for (int i = 0; i < adornerList.Count; i++)
                this.myAdornerLayer.Remove(adornerList[i]);*/
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
                bool succesTextBox = Int32.TryParse(sText, out textBoxId);

                if (shapeId > -1 && typeOfShape[shapeId]!=5)
                    textBoxes[shapeId].IsEnabled = true;

                if (textBoxId > -1)
                    onlyTextBoxes[textBoxId].IsEnabled = true;
                if (move) move = false;
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
            textBox.IsEnabled = false;
            textBox.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        //tạo arrow, giá trị left top là vị trí đặt canvas 
        private void Button_Arrow_Click(object sender, RoutedEventArgs e)
        {
            double x0 = 0;
            double y0 = 0;
            double x1 = 100;
            double y1 = 100;
            double distance = Math.Sqrt(Math.Pow((x1 - x0), 2) + Math.Pow((y1 - y0), 2));
            List<Point> pList = new List<Point>();
            //pList.Add(new Point(100, 100));
            Arrow arrow = new Arrow
            {
                StartPoint = new Point(x0, y0),
                EndPoint = new Point(x1, y1),
                Left= 400,
                Top=200,
                Stroke = Brushes.Black,
                ListPoint=pList,
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
            dynamic a = arrow;
            ArrowAdorner myAdorner = new ArrowAdorner(arrow);            
            myAdorner.From = a.StartPoint;
            myAdorner.To = a.EndPoint;
            adornerList.Add(myAdorner);
            CreateTextBoxForShapes(textBoxes, arrow, "Arrow");
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

            if (text == "Arrow")
            {
                Canvas.SetLeft(textBox, 99999);
                Canvas.SetTop(textBox, 99999);
            }
            else
            {
                Canvas.SetLeft(textBox, 100 + (shape.Width - textBox.MinWidth) / 2);
                Canvas.SetTop(textBox, 10 + (shape.Height - textBox.MinHeight) / 2);
            }
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


        public double DistanceFromPointToLine(Point p, Arrow arrow,int p1 = -1, int p2 = -1)
        {
            Point l1 = new Point(arrow.Left + arrow.StartPoint.X, arrow.Top + arrow.StartPoint.Y);
            Point l2 = new Point(arrow.Left + arrow.EndPoint.X, arrow.Top + arrow.EndPoint.Y);
            if (p1 != -1)
                l1 = new Point((arrow.Left + arrow.ListPoint[p1].X), (arrow.Top + arrow.ListPoint[p1].Y));
            if (p2 != -1)
                l2 = new Point((arrow.Left + arrow.ListPoint[p2].X), (arrow.Top + arrow.ListPoint[p2].Y));
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
            //Console.WriteLine($"MAX MIN {xMax} {xMin} {yMax} {yMin}     {p.X}  {p.Y}");
            if ((xMin <= p.X && p.X <= xMax) || (yMin <= p.Y && p.Y <= yMax))
            {
                return Math.Abs((l2.X - l1.X) * (l1.Y - p.Y) - (l1.X - p.X) * (l2.Y - l1.Y)) /

                    Math.Sqrt(Math.Pow(l2.X - l1.X, 2) + Math.Pow(l2.Y - l1.Y, 2));
            }
            return 999;
        }
        public double DistanceFromPointToPoint(Point p, Arrow arrow, double typeOfPoint,int p1=-1, int p2=-1)
        {
            Point l1 = new Point((arrow.Left + arrow.StartPoint.X), (arrow.Top + arrow.StartPoint.Y));
            Point l2 = new Point((arrow.Left + arrow.EndPoint.X), (arrow.Top + arrow.EndPoint.Y));
            if(p1 !=-1 )
                l1 = new Point((arrow.Left + arrow.ListPoint[p1].X), (arrow.Top + arrow.ListPoint[p1].Y));
            if (p2 != -1)
                l2 = new Point((arrow.Left + arrow.ListPoint[p2].X), (arrow.Top + arrow.ListPoint[p2].Y));
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
            else if (typeOfPoint == 3)
            {
                Point l3 = new Point((l1.X + l2.X) / 2, (l1.Y + l2.Y) / 2);
                return Math.Sqrt(Math.Pow(p.X - l3.X, 2) +
                Math.Pow(p.Y - l3.Y, 2) * 1.0);
            }
            return 999;
            
        }

        private void Button_TextBoxClick(object sender, RoutedEventArgs e)
        {
            Style st = new Style();
            Trigger tg = new Trigger()
            {
                Property = Control.IsEnabledProperty,
                Value = false
            };
            st.Setters.Add(new Setter()
            {
                Property = ForegroundProperty,
                Value = System.Windows.Media.Brushes.Green
            }); 

            st.Triggers.Add(tg);
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
                IsEnabled = true,
            };
            textBox.MouseDoubleClick += TextBox_MouseDoubleClick;
            textBox.LostFocus += TextBox_LostFocus2;
            textBox.Style = st;
            onlyTextBoxes.Add(textBox);

            Canvas.SetLeft(textBox, 150);
            Canvas.SetTop(textBox, 150);
            Canvas.Children.Add(onlyTextBoxes[onlyTextBoxes.Count - 1]);
        }

        //cách thựuc hiện: luu lại toàn bộ thông tin của mũi tên được gọi 
        // sau đó tạo 1 mũi tên mới giống hết cái cũ và thay thế nó vào rectList
        //tương tự cho hàm moveArrow , DrawArrow, và nhưung hàm nào có thay đổi chỉ số các thành phần trogn arrow

        //hàm resize arrow k nó nối vào shape
        //chỉ di chuyển trên canvas
        public void MoveArrow(double x, double y, int id)
        {
            x -= 140;
            y -= 100;
            try
            {
                dynamic a1 = rectList[id];
                double leftCanvas = x;
                double topCanvas = y;
                Canvas.Children.Remove(rectList[id]);
                Point newStart = a1.StartPoint;
                Point newEnd = a1.EndPoint;
                int shapeid1 = a1.ShapeID1;
                int shapeid2 = a1.ShapeID2;
                int typepoint1 = a1.TypePoint1;
                int typepoint2 = a1.TypePoint2;
                List<Point> pList = a1.ListPoint;
                
                //tạo 1 arrow mới với cá thông tin ở trên và thay thế vào rectList
                Arrow arrow = new Arrow
                {
                    StartPoint = newStart,
                    EndPoint = newEnd,
                    Left = leftCanvas,
                    Top = topCanvas,
                    ShapeID1 = shapeid1,
                    ShapeID2 = shapeid2,
                    TypePoint1 = typepoint1,
                    TypePoint2 = typepoint2,
                    ListPoint = pList,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Uid = a1.Uid
                };
                rectList[id] = arrow;
                this.Canvas.Children.Add(rectList[id]);
                Canvas.SetLeft(rectList[id], leftCanvas);
                Canvas.SetTop(rectList[id], topCanvas);
            }
            catch (Exception e)
            {
                //Console.WriteLine($"EXCEPTION id ={id}");
            }

        }
        public void ResizeArrow(int typeOfPoint, double x, double y, int id)
        {
            //Console.WriteLine("RESIZE");
            Console.WriteLine($" MOUSE : {x}  {y}");
            x -= 140;
            y -= 100;
            try
            {
                //lưu lại toàn bộ thông tin của arrow cũ
                //cập nhật các thông tin đã thay đổi (nếu có)
                //Console.WriteLine($"id= {id}");
                dynamic a1 = rectList[id];
                double leftCanvas = a1.Left;
                double topCanvas = a1.Top;
                Canvas.Children.Remove(rectList[id]);
                Point newStart = a1.StartPoint;
                Point newEnd = a1.EndPoint;
                int shapeid1 = a1.ShapeID1;
                int shapeid2 = a1.ShapeID2;
                int typepoint1 = a1.TypePoint1;
                int typepoint2 = a1.TypePoint2;
                List<Point> pList = a1.ListPoint;
                if (typeOfPoint == 1)
                {
                    //Console.WriteLine($" MOUSE = {x} ; {y}");
                   // Console.WriteLine($" Canvas = {leftCanvas} ; {topCanvas}\n");
                    //Console.WriteLine($" dx= {x}   dy={y}");
                    double dx = 0 - (x-leftCanvas);
                    double dy = 0 - (y - topCanvas);
                    newEnd = new Point(a1.EndPoint.X + dx, a1.EndPoint.Y + dy);
                    leftCanvas = x;
                    topCanvas = y;
                }
                else
                {
                    x -= leftCanvas;
                    y -= topCanvas ;
                    Console.WriteLine($" SHAPE : {leftCanvas}  {topCanvas}");
                    Console.WriteLine($" SHAPE : {x}  {y}\n");

                    newEnd = new Point(x, y);
                }
                //tạo 1 arrow mới với cá thông tin ở trên và thay thế vào rectList
                Arrow arrow = new Arrow
                {
                    StartPoint = newStart,
                    EndPoint = newEnd,
                    Left = leftCanvas,
                    Top = topCanvas,
                    ShapeID1= shapeid1,
                    ShapeID2= shapeid2,
                    TypePoint1= typepoint1,
                    TypePoint2 = typepoint2,
                    ListPoint=pList,
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
        //réize nối vào shape tại phần mũi tên
        //shape: id của shape đc nối
        //typepoint2 : là điểm mà ta sẽ nối shape vào 
        //1 shape có 4 điểm nối , chi tiết xem tại :GetPositionOf4Point(int, int)
        public void ResizeArrow(int typeOfPoint, double x, double y, int id, int shape, int typepoint2)
        {
            //Console.WriteLine("RESIZE");
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
                int typePoint2 = a1.TypePoint2;
                List<Point> pList = a1.ListPoint;
                if (typepoint2 != 0)
                    typePoint2 = typepoint2;
                if (typeOfPoint == 1)
                    shapeid1 = shape;
                else
                    shapeid2 = shape;
                
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
                    TypePoint2= typePoint2,
                    ListPoint=pList,
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
        //hàm vẽ mũi tên của shape
        public void DrawArrow(double x, double y,int id, int typePoint)
        {
            //Console.WriteLine(  "DRAW ARROW");
            double x0 = 0;
            double y0 = 0;
            double x1 = 100;
            double y1 = 100;
            List<Point> pList = new List<Point>();
            Arrow arrow = new Arrow
            {
                StartPoint = new Point(x0, y0),
                EndPoint = new Point(x1, y1),
                Left = x,
                Top = y,
                ShapeID1=id,
                ListPoint= pList,
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
           // Console.WriteLine($"LIST POINT = {a.ListPoint.Count}");
            myAdorner.From = a.StartPoint;
            myAdorner.To = a.EndPoint;
            adornerList.Add(myAdorner);
            CreateTextBoxForShapes(textBoxes, arrow, "Arrow");
            textBoxes[rectList.Count - 1].Width = 0;
            textBoxes[rectList.Count - 1].Height = 0;
        }
        // 2 hàm AddPointArrow & MovePointArrow phục vụ cho việc bẻ đôi mũi tên
        // addpoint: thêm 1 điểm trên mũi tên
        //drawpoint: kéo thả vị trí điểm đó
        public void AddPointArrow(double x, double y, int id, int position)
        {
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
                int typepoint1 = a1.TypePoint1;
                int typepoint2 = a1.TypePoint2;
                List<Point> pList = a1.ListPoint;
                if(position==-1 || position == -2)
                    pList.Add(new Point(x - leftCanvas, y - topCanvas));
                else 
                    pList.Insert(position, new Point(x - leftCanvas, y - topCanvas));
                Arrow arrow = new Arrow
                {
                    StartPoint = newStart,
                    EndPoint = newEnd,
                    Left = leftCanvas,
                    Top = topCanvas,
                    ShapeID1 = shapeid1,
                    ShapeID2 = shapeid2,
                    TypePoint1 = typepoint1,
                    TypePoint2 = typepoint2,
                    ListPoint=pList,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Uid = a1.Uid
                };
                rectList[id] = arrow;
                this.Canvas.Children.Add(rectList[id]);
                Canvas.SetLeft(rectList[id], leftCanvas);
                Canvas.SetTop(rectList[id], topCanvas);
            }
            catch (Exception e)
            {
            }
        }
        public void MovePointArrow(double x, double y, int id,int position)
        {
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
                int typepoint1 = a1.TypePoint1;
                int typepoint2 = a1.TypePoint2;
                List<Point> pList = a1.ListPoint;
                //pList[pList.Count - 1] = new Point(x - leftCanvas, y - topCanvas);
                if (position == -1 || position == -2)
                    pList[pList.Count - 1] = new Point(x - leftCanvas, y - topCanvas);
                else
                    pList[position] = new Point(x - leftCanvas, y - topCanvas);

                Arrow arrow = new Arrow
                {
                    StartPoint = newStart,
                    EndPoint = newEnd,
                    Left = leftCanvas,
                    Top = topCanvas,
                    ShapeID1 = shapeid1,
                    ShapeID2 = shapeid2,
                    TypePoint1 = typepoint1,
                    TypePoint2 = typepoint2,
                    ListPoint = pList,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Uid = a1.Uid
                };
                rectList[id] = arrow;
                this.Canvas.Children.Add(rectList[id]);
                Canvas.SetLeft(rectList[id], leftCanvas);
                Canvas.SetTop(rectList[id], topCanvas);
            }
            catch (Exception e)
            {
            }
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
        public static void SaveCanvasToFile(Canvas surface, string filename)
        {
            //surface.S
            Size size = new Size(surface.Width, surface.Height);
           
            //Size size = new Size(100, 100);
           // surface.Measure(new Size((int)surface.Width, (int)surface.Height));
           // surface.Arrange(new Rect(new Size((int)surface.Width, (int)surface.Height)));
            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32) ;
                //PixelFormats.Default);


            //// Image source to set to bitmap
            BitmapImage bitmap = new BitmapImage(new Uri("page.jpg", UriKind.Relative));
            Image img = new Image() { Width = size.Width, Height = size.Height, Stretch = Stretch.Uniform, StretchDirection = StretchDirection.Both };
            img.Source = bitmap;
            img.Measure(size);
            img.Arrange(new Rect(size));


            // drawing virtual
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                VisualBrush visualBrush = new VisualBrush(surface);
                drawingContext.DrawRectangle(visualBrush,null,
                  new Rect(new System.Windows.Point(), new Size(size.Width, size.Height)));
            }
            renderBitmap.Render(surface);
            renderBitmap.Render(img);


            // Create a file stream for saving image
            using (FileStream outStream = new FileStream(filename, FileMode.Create))
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                // save the data to the stream
                encoder.Save(outStream);
            }
        }
        //hàm bổ sung cho mũi tên : tính khảong cách đến mũi tên có nhiều đường gấp khúc
        public void CalcAddPoint(Point p, Arrow a)
        {
            if (DistanceFromPointToPoint(p, a, 3, -1, 0) < 5 && DistanceFromPointToPoint(p, a, 1, -1, 0) > 0 && !isDrawPointArrow)
                positionBreakPoint = 0;
            else if (DistanceFromPointToPoint(p, a, 3, a.ListPoint.Count - 1, -1) < 5 && DistanceFromPointToPoint(p, a, 3, a.ListPoint.Count - 1, -1) > 0 && !isDrawPointArrow)
                positionBreakPoint = -1;
            for (int j = 0; j < a.ListPoint.Count - 1; j++)
            {
                if (DistanceFromPointToPoint(p, a, 3, j, j + 1) < 5 && DistanceFromPointToPoint(p, a, 1, j, j + 1) > 0 && !isDrawPointArrow)
                {
                    positionBreakPoint = j + 1;
                    break;
                }
            }
        }
        public bool CalcMoveArrow(Point p, Arrow a)
        {
            if (DistanceFromPointToLine(p, a, -1, 0) < 5 && DistanceFromPointToLine(p, a, -1, 0) > 0 && !isDrawPointArrow)
                return true;
            else if (DistanceFromPointToLine(p, a, a.ListPoint.Count - 1, -1) < 5 && DistanceFromPointToLine(p, a, a.ListPoint.Count - 1, -1) > 0 && !isDrawPointArrow)
                return true;
            for (int j = 0; j < a.ListPoint.Count - 1; j++)
            {
                if (DistanceFromPointToLine(p, a,j, j + 1) < 5 && DistanceFromPointToLine(p, a,j, j + 1) > 0 && !isDrawPointArrow)
                    return true;
            }
            return false;
        }
        
    }

}

