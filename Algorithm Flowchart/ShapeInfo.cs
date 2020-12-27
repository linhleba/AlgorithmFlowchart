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

namespace Algorithm_Flowchart
{

    public class ShapeInfo
    {
        public double X;
        public double Y;
        public Stretch Stretch;
        public bool havePoints = false;
        public PointCollection Points;
        public int Width;
        public int Height;
        public Brush Fill;
        public Brush Stroke;
        public int StrokeThickness;
        public string Uid;
        public string text;
        public Brush color;
        int type;

        //Arrow
        public Point EndPoint;

        public Point StartPoint;

        public double Left;

        public double Top;

        public int ShapeID1;

        public int ShapeID2;

        public int TypePoint1;

        public int TypePoint2;

        public List<Point> ListPoint;
    }
}
