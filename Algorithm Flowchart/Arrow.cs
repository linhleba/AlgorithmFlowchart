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

    public class Arrow : Shape
    {
        #region Fields

        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register("EndPoint", typeof(Point), typeof(Arrow), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register("StartPoint", typeof(Point), typeof(Arrow), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(new Double(), FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(new Double(), FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty ShapeID1Property = DependencyProperty.Register("ShapeID1", typeof(int), typeof(Arrow), new FrameworkPropertyMetadata(-1 , FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty ShapeID2Property = DependencyProperty.Register("ShapeID2", typeof(int), typeof(Arrow), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty TypePoint1Property = DependencyProperty.Register("TypePoint1", typeof(int), typeof(Arrow), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty TypePoint2Property = DependencyProperty.Register("TypePoint2", typeof(int), typeof(Arrow), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsMeasure));

        private GeometryGroup linegeo;

        #endregion Fields

        #region Constructors

        public Arrow()
        {
            linegeo = new GeometryGroup();

            this.Stroke = Brushes.Black;
            this.StrokeThickness = 2;
        }

        #endregion Constructors

        #region Properties

        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }
        
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }
        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }
        public int ShapeID1
        {
            get { return (int)GetValue(ShapeID1Property); }
            set { SetValue(ShapeID1Property, value); }
        }
        public int ShapeID2
        {
            get { return (int)GetValue(ShapeID2Property); }
            set { SetValue(ShapeID2Property, value); }
        }
        public int TypePoint1
        {
            get { return (int)GetValue(TypePoint1Property); }
            set { SetValue(TypePoint1Property, value); }
        }
        public int TypePoint2
        {
            get { return (int)GetValue(TypePoint2Property); }
            set { SetValue(TypePoint2Property, value); }
        }
        public PathGeometry triangle { get; set; }

        protected override Geometry DefiningGeometry
        {
            get
            {                
                double Y1 = StartPoint.Y;
                double X1 = StartPoint.X;
                double Y2 = EndPoint.Y;
                double X2 = EndPoint.X;
                double theta = Math.Atan2(Y1 - Y2, X1 - X2);
                double sint = Math.Sin(theta);
                double cost = Math.Cos(theta);
                Point pt1 = new Point(X1, Y1);
                Point pt2 = new Point(X2, Y2);
                Point pt3 = new Point(
                    X2 + (4 * cost - 4 * sint),
                    Y2 + (4 * sint + 4 * cost));
                Point pt4 = new Point(
                    X2 + (4 * cost + 4 * sint),
                    Y2 - (4 * cost - 4 * sint));
                triangle = new PathGeometry();
                triangle.AddGeometry(Geometry.Parse($"M {pt1.X},{pt1.Y} {pt2.X},{pt2.Y} {pt3.X },{pt3.Y} {pt4.X},{pt4.Y} {pt2.X},{pt2.Y} "));
                linegeo.Children.Add(triangle);
                return linegeo;
            }
        }

        #endregion Properties
    }
}      
            