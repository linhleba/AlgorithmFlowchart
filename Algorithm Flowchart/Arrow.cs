using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Algorithm_Flowchart
{
    public class Arrow:Shape
    {
        public Path Begin = new Path();
        public Line line = new Line();

        private GeometryGroup arrow = new GeometryGroup();
        protected override Geometry DefiningGeometry
        {
            get
            {
                return arrow;
            }
        }
        public Arrow() { }

        public Arrow(double x0, double y0, double x1, double y1) 
        {
            PathGeometry path = new PathGeometry();
            path = Begin;
            this.arrow.Children.Add(path);
            this.arrow.Children.Add(Begin);
            Begin.Stroke = System.Windows.Media.Brushes.Black;
            //myPath.Fill = System.Windows.Media.Brushes.MediumSlateBlue;
            Begin.StrokeThickness = 4;
            Begin.HorizontalAlignment = HorizontalAlignment.Left;
            Begin.VerticalAlignment = VerticalAlignment.Center;
            Begin.Data = System.Windows.Media.Geometry.Parse("M 0 0 L 4 4 L 8 0 Z");
        }
    }
}
