using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Algorithm_Flowchart
{
    [Serializable]
    public class NewShape : Shape
    {
        protected override Geometry DefiningGeometry => throw new NotImplementedException();
    }
}
