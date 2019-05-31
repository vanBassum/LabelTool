using System.Drawing;

namespace LabelTool.LabelItems
{
    public class _SizeF : Shell<SizeF>
    {
        public float Width { get => BaseObject.Width; set => BaseObject.Width = value; }
        public float Height { get => BaseObject.Height; set => BaseObject.Height = value; }


        public _SizeF(SizeF baseObject) : base(baseObject)
        {
        }
    }
}
