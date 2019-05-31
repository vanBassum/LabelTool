using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace LabelTool
{
    public interface LabelItem
    {
        string Name { get; set; }
        string VarName { get; set;}
        void Render(XGraphics gfx, XPoint origin, string var);
        void Render(XGraphics gfx, XPoint origin);

    }

}


