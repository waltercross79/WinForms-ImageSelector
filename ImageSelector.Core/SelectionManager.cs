/* Copyright (C) 2020 - Present 
 * Walter Cross - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the GPL v3.0 license.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSelector.Core
{
    public class SelectionManager : ISelectionManager
    {
        private readonly List<Selection> _selections;

        public SelectionManager()
        {
            _selections = new List<Selection>();
        }

        public Selection CreateSelection(int x, int y, int width, int height)
        {
            return new Selection(new Rectangle(x, y, width, height), _selections.Count);
        }

        public void AddSelection(Selection s)
        {            
            _selections.Add(s);
        }

        public ISelectionComponent GetSelectionComponentNearPoint(Point p, int buffer)
        {
            ISelectionComponent result = null;
            foreach (var s in _selections)
            {
                result = s.GetSelectionAtPoint(p, 2);
                if (result != null)
                    break;
            }
            return result;
        }

        public Selection ResizeSelection(Selection selectionToResize, Point location, Size newSize)
        {
            var existingSelection = _selections.SingleOrDefault(x => x.ID == selectionToResize.ID);
            var result = new Selection(new Rectangle(location, newSize), 
                existingSelection?.ZIndex ?? _selections.Count + 1);
            if (existingSelection != null)
                _selections.Remove(existingSelection);

            _selections.Add(result);

            return result;
        }

        public Selection MoveSelection(Selection selectionToMove, Point location)
        {
            var existingSelection = _selections.SingleOrDefault(x => x.ID == selectionToMove.ID);

            if (existingSelection == null)
                throw new ArgumentNullException("This selection is not tracked by the manager.");

            var result = new Selection(
                new Rectangle(location, existingSelection.LocationAndSize.Size),
                existingSelection.ZIndex);
            
            _selections.Remove(existingSelection);
            _selections.Add(result);

            return result;
        }

        public IEnumerable<Selection> GetAll()
        {
            return _selections;
        }
    }
}
