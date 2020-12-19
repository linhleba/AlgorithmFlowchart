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
        public SimpleCircleAdorner(UIElement adornedElement): base(adornedElement)
        {
        }

        // A common way to implement an adorner's rendering behavior is to override the OnRender
        // method, which is called by the layout system as part of a rendering pass.
        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            double renderRadius = 5.0;
            Point top = new Point(adornedElementRect.TopLeft.X + adornedElementRect.Width / 2, adornedElementRect.TopLeft.Y);
            Point bot = new Point(adornedElementRect.TopLeft.X + adornedElementRect.Width / 2, adornedElementRect.BottomLeft.Y);
            Point left = new Point(adornedElementRect.TopLeft.X,adornedElementRect.TopLeft.Y + adornedElementRect.Height/ 2);
            Point right = new Point(adornedElementRect.TopRight.X, adornedElementRect.TopLeft.Y + adornedElementRect.Height / 2);
            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, top, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, left, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, bot, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, right, renderRadius, renderRadius);

        }
    }
}
