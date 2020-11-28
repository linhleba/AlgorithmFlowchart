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
using System.Workflow;
using System.Workflow.ComponentModel.Design;

namespace Algorithm_Flowchart
{
    class ArrowAdorner : Adorner
    {
        public Point From { get; set; }
        public Point To { get; set; }

        public ArrowAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Line adornedElementRect = new Line();
            adornedElementRect.X1 = From.X;
            adornedElementRect.Y1 = From.Y;
            adornedElementRect.X2 = To.X;
            adornedElementRect.Y2 = To.Y;
            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            double renderRadius = 5.0;
            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, new Point(adornedElementRect.X1-2.5, adornedElementRect.Y1-2.5), renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, new Point(adornedElementRect.X2, adornedElementRect.Y2), renderRadius, renderRadius);
        }
    }
    
    /*class ArrowAdorner : Adorner
    {
        [Obsolete]
        public Connector conn;
        public ArrowAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }

        // A common way to implement an adorner's rendering behavior is to override the OnRender
        // method, which is called by the layout system as part of a rendering pass.
        [Obsolete]
        protected override void OnRender(DrawingContext drawingContext)
        {           
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            double renderRadius = 5.0;
            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        }
    }*/
}
