using System.Drawing;

namespace LabelTool.LabelItems
{
    public class _PointF : Shell<PointF>
    {
        public float X { get => BaseObject.X; set => BaseObject.X = value; }
        public float Y { get => BaseObject.Y; set => BaseObject.Y = value; }


        public _PointF(PointF baseObject) : base(baseObject)
        {
        }
    }
}
