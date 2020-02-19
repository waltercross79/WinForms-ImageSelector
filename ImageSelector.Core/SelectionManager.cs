using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSelector.Core
{
    public class SelectionManager
    {
        private readonly List<Selection> _selections;

        public SelectionManager()
        {
            _selections = new List<Selection>();
        }

        public void AddSelection(Selection s)
        {
            _selections.Add(s);
        }
    }
}
