using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Algorithm_Flowchart
{
    public class SimpleCircleAdorner : Adorner
    {
        enum MousePosition
        {
            TL,
            TR,
            BL,
            BR
        }
        MousePosition _currentPosition;
        Canvas canvas;
        bool _isDraging = false;
        Point _startPosition; 
        double _renderRadius = 5.0;
        // Be sure to call the base class constructor.
        public SimpleCircleAdorner(System.Windows.UIElement adornedElement, Canvas canvas)
            : base(adornedElement)
        {
            this.canvas = canvas;
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            Point point = Mouse.GetPosition(AdornedElement);
            _currentPosition = getMousePosition(point);
            switch (_currentPosition)
            {
                case MousePosition.BR:
                case MousePosition.TL:
                    Cursor = System.Windows.Input.Cursors.SizeNWSE;
                    break;
                case MousePosition.BL:
                case MousePosition.TR:
                    Cursor = System.Windows.Input.Cursors.SizeNESW;
                    break;
            }
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(AdornedElement);
            if (adornerLayer != null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners(AdornedElement);
                if (adorners != null)
                {
                    foreach (Adorner adorner in adorners)
                    {
                        adornerLayer.Remove(adorner);
                    }
                }
            }
        }

        

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (Mouse.Capture(this))
            {
                _isDraging = true;
                _startPosition = Mouse.GetPosition(this.canvas);
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (_isDraging)
            {
                Point newPosition = Mouse.GetPosition(this.canvas);
                double diffX = (newPosition.X - _startPosition.X);
                double diffY = (newPosition.Y - _startPosition.Y);

                // we should decide whether to change Width and Height or to change Canvas.Left and Canvas.Right
                if (Math.Abs(diffX) >= 1 || Math.Abs(diffY) >= 1)
                {
                    switch (_currentPosition)
                    {
                        case MousePosition.TL:
                        case MousePosition.BL:
                            foreach (FrameworkElement ui in this.canvas.Children)
                            {
                                if (ui.GetType() == typeof(Ellipse) || ui.GetType() == typeof(Line))
                                {
                                    Canvas.SetLeft(ui, Math.Max(0, Canvas.GetLeft(ui) + diffX));
                                    ui.Width = Math.Max(0, ui.Width - diffX);
                                }
                            }
                            this.canvas.InvalidateArrange();

                            break;
                        case MousePosition.BR:
                        case MousePosition.TR:

                            foreach (FrameworkElement ui in this.canvas.Children)
                            {
                                if (ui.GetType() == typeof(Ellipse) || ui.GetType() == typeof(Line))
                                {
                                    ui.Width = Math.Max(0, ui.Width + diffX);
                                }
                            }
                            break;
                    }


                    switch (_currentPosition)
                    {
                        case MousePosition.TL:
                        case MousePosition.TR:
                            foreach (FrameworkElement ui in this.canvas.Children)
                            {
                                if (ui.GetType() == typeof(Ellipse) || ui.GetType() == typeof(Line))
                                {
                                    Canvas.SetTop(ui, Math.Max(0, Canvas.GetTop(ui) + diffY));
                                }
                            }
                            foreach (FrameworkElement ui in this.canvas.Children)
                            {
                                if (ui.GetType() == typeof(Ellipse) || ui.GetType() == typeof(Line))
                                {
                                    ui.Height = Math.Max(0, ui.Height - diffY);
                                }
                            }
                            break;
                        case MousePosition.BL:
                        case MousePosition.BR:
                            foreach (FrameworkElement ui in this.canvas.Children)
                            {
                                if (ui.GetType() == typeof(Ellipse) || ui.GetType() == typeof(Line))
                                {
                                    ui.Height = Math.Max(0, ui.Height + diffY);
                                }
                            }
                            break;
                    }
                }
                _startPosition = newPosition;
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {

        }

        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            if (_isDraging)
            {
                Mouse.Capture(null);
                _isDraging = false;
            }
        }

        MousePosition getMousePosition(Point point) // point relative to element
        {
            double h2 = ActualHeight / 2;
            double w2 = ActualWidth / 2;
            if (point.X < w2 && point.Y < h2)
                return MousePosition.TL;
            else if (point.X > w2 && point.Y > h2)
                return MousePosition.BR;
            else if (point.X > w2 && point.Y < h2)
                return MousePosition.TR;
            else
                return MousePosition.BL;



        }

       
        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Black);
            renderBrush.Opacity = 0.3;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Black), 1.5);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, _renderRadius, _renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, _renderRadius, _renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, _renderRadius, _renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, _renderRadius, _renderRadius);
        }
    }
}
