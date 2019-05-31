using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelTool
{
    public static class Ext
    {
        public static XPoint Add(this XPoint p1, XSize p2)
        {
            return new XPoint(p1.X + p2.Width, p1.Y + p2.Height);
        }

        public static XPoint Substract(this XPoint p1, XSize p2)
        {
            return new XPoint(p1.X - p2.Width, p1.Y - p2.Height);
        }

        public static XPoint Add(this XPoint p1, XPoint p2)
        {
            return new XPoint(p1.X + p2.X, p1.Y+ p2.Y);
        }

        public static XPoint Substract(this XPoint p1, XPoint p2)
        {
            return new XPoint(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point Add(this Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point Substract(this Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static PointF Add(this PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static PointF Substract(this PointF p1, PointF p2)
        {
            return new PointF(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static SizeF Div(this SizeF p1, float p2)
        {
            return new SizeF(p1.Width / p2, p1.Height / p2);
        }


        public static XRect FromMM(this XRect rect)
        {
            return new XRect
                (
                XUnit.FromMillimeter(rect.X),
                XUnit.FromMillimeter(rect.Y),
                XUnit.FromMillimeter(rect.Width),
                XUnit.FromMillimeter(rect.Height)
                );
        }


        /*
        public static bool CollidesWith(this XRect rect, XPoint pt)
        {
            if (pt.X < rect.Location.X)
                return false;
            if (pt.Y < rect.Location.Y)
                return false;
            if (pt.X > rect.Location.X + rect.Size.Width)
                return false;
            if (pt.Y > rect.Location.Y + rect.Size.Height)
                return false;
            return true;
        }

        public static bool CollidesWith(this RectangleF rect, PointF pt)
        {
            if (pt.X < rect.Location.X)
                return false;
            if (pt.Y < rect.Location.Y)
                return false;
            if (pt.X > rect.Location.X + rect.Size.Width)
                return false;
            if (pt.Y > rect.Location.Y + rect.Size.Height)
                return false;
            return true;
        }
        */
    }
}
