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
        public BackRoundPicker newPick;
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
        private void FontPicker_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            switch (index)
            {
                case 1:
                    newPick = new BackRoundPicker(this);
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
                        
                    }

                    break;
                
            }
        }
        private void File_Button_Click(object sender, RoutedEventArgs e)
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
                case 4:
                    this.Canvas.Background = Brushes.Gray;
                    break;
                case 5:
                    this.Canvas.Background = Brushes.AliceBlue;
                    break;
            }
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
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
                    break;
            }
        }
        private void View_Button_Click(object sender, RoutedEventArgs e)
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
                case 1:
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
            }
        }
        private void Tool_Button_Click(object sender, RoutedEventArgs e)
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
            tabCnntrol.BorderBrush = b;
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

        private void ShapeTool_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            ShapeTool.shapeToolBackround.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }

        private void tabCnntrol_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            tabCnntrol.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }

        private void rightPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            rightPanel.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
        }

        private void gridColumn2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            gridColumn2.Background = (Brush)converter.ConvertFromString($"{colorPicker.SelectedColor.ToString()}");
            
        }
    }

}
