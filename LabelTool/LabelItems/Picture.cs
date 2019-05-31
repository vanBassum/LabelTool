using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace LabelTool.LabelItems
{
    public class Picture : LabelItem
    {
        [CategoryAttribute("Positioning")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public _SizeF Size { get; set; } = new _SizeF(new SizeF(2013, 2400).Div(183));

        [CategoryAttribute("Positioning")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public _PointF Location { get; set; } = new _PointF(new PointF(0, 10));

        [CategoryAttribute("Referencing")]
        public string Name { get; set; } = "newText";

        [CategoryAttribute("Referencing")]
        public string VarName { get; set; } = "testVar";

        [CategoryAttribute("Image")]
        public string Filename { get; set; } = "";

        public void Render(XGraphics gfx, XPoint origin)
        {
            Render(gfx, origin, "true");
        }

        public void Render(XGraphics gfx, XPoint origin, string var)
        {
            XSize lSize = new XSize(XUnit.FromMillimeter(Size.Width), XUnit.FromMillimeter(Size.Height));
            XPoint lPt = new XPoint(XUnit.FromMillimeter(Location.X), XUnit.FromMillimeter(Location.Y)).Add(origin);
            XRect lRect = new XRect(lPt, lSize);

            bool draw = true;
            bool canParse = bool.TryParse(var, out draw);
            bool fileExists = File.Exists(Filename);


            if (!canParse)
                draw = true;

            if(draw)
            {
                if(fileExists)
                    gfx.DrawImage(XImage.FromFile(Filename), lRect);
                else
                    gfx.DrawImage(XImage.FromGdiPlusImage(Properties.Resources.not_found_icon_29), lRect);
            }
        }

        public override string ToString()
        {
            return Name + " (" + Path.GetFileName(Filename) + ")";
        }
    }
}
