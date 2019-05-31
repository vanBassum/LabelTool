using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LabelTool.LabelItems
{
    class MultiplePicture : LabelItem
    {
        [CategoryAttribute("Positioning")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public _SizeF Size { get; set; } = new _SizeF(new SizeF(2013, 2400).Div(183));

        [CategoryAttribute("Positioning")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public _PointF Location { get; set; } = new _PointF(new PointF(0, 10));

        [CategoryAttribute("Positioning")]
        public Orientation Orientation { get; set; } = Orientation.Horizontal;

        [CategoryAttribute("Referencing")]
        public string Name { get; set; } = "newText";

        [CategoryAttribute("Referencing")]
        public string VarName { get; set; } = "testVar";




        public void Render(XGraphics gfx, XPoint origin)
        {
            Render(gfx, origin, null);
        }

        public void Render(XGraphics gfx, XPoint origin, string var)
        {
            XSize lSize = new XSize(XUnit.FromMillimeter(Size.Width), XUnit.FromMillimeter(Size.Height));
            XPoint lPt = new XPoint(XUnit.FromMillimeter(Location.X), XUnit.FromMillimeter(Location.Y)).Add(origin);
            XRect lRect = new XRect(lPt, lSize);


            if(var==null)
            {
                gfx.DrawRectangle(Brushes.LightGray, lRect);
            }
            else
            {
                double x = 0;
                double y = 0;
                foreach (Match m in Regex.Matches(var, "\"(.+?)\""))
                {
                    string imgFile = m.Groups[1].Value;

                    XImage img = File.Exists(imgFile) ? XImage.FromFile(imgFile) : XImage.FromGdiPlusImage(Properties.Resources.not_found_icon_29);

                    double w = img.Width;
                    double h = img.Height;



                    //gfx.DrawImage(img, lPt.Add(new XRect(x, y, )));

                    switch(Orientation)
                    {
                        case Orientation.Horizontal:
                            x += img.Width;
                            break;
                    }


                }
            }
        }

        public override string ToString()
        {
            return Name + " (" + VarName != null ? VarName : "" + ")";
        }
    }

    public enum Orientation
    {
        Horizontal
    }
}
