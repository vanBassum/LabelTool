using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace LabelTool.LabelItems
{
    public class Text : LabelItem
    {
        [CategoryAttribute("Positioning")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public _SizeF Size { get; set; } = new _SizeF(new SizeF(20,5));

        [CategoryAttribute("Positioning")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public _PointF Location { get; set; } = new _PointF(new PointF(0,10));

        [CategoryAttribute("Referencing")]
        public string Name { get; set; } = "newText";

        [CategoryAttribute("Referencing")]
        public string VarName { get; set; } = "testVar";
        
        [CategoryAttribute("Font")]
        public Color TextColor { get; set; } = Color.FromArgb(255, 0,0,0);

        [CategoryAttribute("Font")]
        public Color BackgroundColor { get; set; } = Color.FromArgb(0, 0, 0, 0);

        [CategoryAttribute("Font")]
        public Font Font { get; set; } = new Font(FontFamily.GenericSansSerif,8,FontStyle.Regular, GraphicsUnit.World);

        [CategoryAttribute("Font")]
        public _XStringFormats Allignment { get; set; } = _XStringFormats.TopLeft;

        public void Render(XGraphics gfx, XPoint origin)
        {
            Render(gfx, origin, null);
        }

        public void Render(XGraphics gfx, XPoint origin, string var)
        {
            XPoint pos = origin.Add(new XPoint(Location));
            XSize size = new XSize(Size.Width, Size.Height);
            XRect lRect = new XRect(pos, size);


            string str = Name;
            if (var != null)
                str = var;

            gfx.DrawRectangle(new SolidBrush(BackgroundColor), lRect.FromMM());


            gfx.DrawString(
                str,
                new Font(Font.FontFamily, Font.Size, Font.Style, GraphicsUnit.World),
                new XSolidBrush(TextColor),
                lRect.FromMM(),
                FromEnum(Allignment));
        }

        public override string ToString()
        {
            return Name + " (" + VarName!=null?VarName : "" + ")";
        }

        public XStringFormat FromEnum(_XStringFormats format)
        {
            switch(format)
            {
                case _XStringFormats.BottomLeft: return XStringFormats.BottomLeft;
                case _XStringFormats.BottomCenter: return XStringFormats.BottomCenter;
                case _XStringFormats.BottomRight: return XStringFormats.BottomRight;
                case _XStringFormats.CenterLeft: return XStringFormats.CenterLeft;
                case _XStringFormats.Center: return XStringFormats.Center;
                case _XStringFormats.CenterRight: return XStringFormats.CenterRight;
                case _XStringFormats.TopLeft: return XStringFormats.TopLeft;
                case _XStringFormats.TopCenter: return XStringFormats.TopCenter;
                case _XStringFormats.TopRight: return XStringFormats.TopRight;
            }

            return XStringFormats.TopLeft;
        }
    }





    public enum _XStringFormats
    {
        BottomLeft,
        BottomCenter,
        BottomRight,
        CenterLeft,
        Center,
        CenterRight,
        TopLeft,
        TopCenter,
        TopRight
        
    }
}
