/* Copyright (C) 2020 - Present 
 * Walter Cross - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the GPL v3.0 license.
 */

using ImageSelector.Core;
using ImageSelector.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ImageSelector.ViewModels
{
    public class MainFormViewModel
    {
        private readonly SelectionManager _selectionManager;        
        private MainFormState _currentState;

        public MainFormViewModel(SelectionManager selectionManager)
        {
            _selectionManager = selectionManager;
            _currentState = new IdleState(selectionManager, new Point());
        }

        public MainFormStatus Status => _currentState.Status;

        public ISelectionComponent ComponentAtCursorPoint => _currentState.ComponentAtCursorPoint;

        //public Point LastEventPoint => _currentState.LastEventPoint;

        public Selection ActiveSelection => _currentState.ActiveSelection;


        public event EventHandler<CustomEventArgs<MainFormStatus>> StateChange;

        public void MouseDown(Point location, MouseButtons mouseButtons)
        {
            HandleMouseEvent(location, mouseButtons, StateChangingTrigger.MouseDown);              
        }

        public void MouseUp(Point location, MouseButtons mouseButtons)
        {
            HandleMouseEvent(location, mouseButtons, StateChangingTrigger.MouseUp);
        }

        public void MouseMove(Point location)
        {
            HandleMouseEvent(location, MouseButtons.None, StateChangingTrigger.MouseMove);
        }

        public IEnumerable<Selection> Selections
        {
            get
            {
                return _selectionManager.GetAll();
            }
        }

        private void HandleMouseEvent(Point location, MouseButtons mouseButtons, StateChangingTrigger mouseTrigger)
        {
            if (IsStateChanging(location, mouseTrigger, out MainFormState state))
            {
                _currentState = state;

                EventHandler<CustomEventArgs<MainFormStatus>> temp = Volatile.Read(ref StateChange);
                if (temp != null) temp(this, new CustomEventArgs<MainFormStatus>(Status));
            }
        }

        private bool IsStateChanging(Point location, StateChangingTrigger trigger, out MainFormState state)
        {
            var newState = _currentState.UpdateState(location, trigger);
            var isStateChanging = _currentState != newState;
            if (isStateChanging)
            {
                _currentState = newState;
            }

            state = _currentState;
            return isStateChanging;
        }
    }
}
