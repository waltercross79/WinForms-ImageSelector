using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSelector.ViewModels
{
    public enum MainFormStatus
    {
        Idle,
        Selecting,
        ResizingSelection,
        MovingSelection,
        OverEdge,
        OverSelection,
    }

    public enum StateChangingTrigger
    {
        MouseDown,
        MouseUp,
        MouseMove
    }
}
