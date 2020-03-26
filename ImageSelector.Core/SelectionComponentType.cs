using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSelector.Core
{
    /// <summary>
    /// Part of the selection that can be target of mouse hover.
    /// </summary>
    public enum SelectionComponentType
    {
        NWCorner = 1,
        SWCorner = 2,
        SECorner = 3,
        NECorner = 4,
        TopEdge = 5,
        LeftEdge = 6,
        BottomEdge = 7,
        RightEdge = 8,
        Inside = 9
    }
}
