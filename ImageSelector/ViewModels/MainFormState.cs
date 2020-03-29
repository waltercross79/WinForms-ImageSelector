/* Copyright (C) 2020 - Present 
 * Walter Cross - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the GPL v3.0 license.
 */

using ImageSelector.Core;
using System;
using System.Configuration;
using System.Drawing;
using System.Linq;

namespace ImageSelector.ViewModels
{
    abstract class MainFormState
    {
        protected readonly ISelectionManager _selectionManager;
        protected int _buffer;
        protected Selection _activeSelection;

        public MainFormState()
            : this(new SelectionManager(), null) { }

        public MainFormState(ISelectionManager selectionManager, Selection selection)
        {
            _activeSelection = selection;
            _selectionManager = selectionManager;
            Int32.TryParse(ConfigurationManager.AppSettings[Constants.BufferAppSetting], out _buffer);

            System.Diagnostics.Debug.WriteLine("Creating FormState {0}", Status.ToString());
        }

        public abstract MainFormStatus Status { get; }

        public virtual ISelectionComponent ComponentAtCursorPoint { get; protected set; }

        //public virtual Point LastEventPoint { get; protected set; }

        public virtual Selection ActiveSelection => _activeSelection;

        public abstract MainFormState UpdateState(Point location, StateChangingTrigger trigger);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;


            if (!(obj is MainFormState toCompare))
                return false;

            return Status == toCompare.Status &&
                ActiveSelection == toCompare.ActiveSelection;
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(MainFormStatus), Status);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    class IdleState : MainFormState
    {
        public IdleState(ISelectionManager selectionManager, Point actionLocation)
            : base(selectionManager, null)
        {
        }

        public override MainFormStatus Status => MainFormStatus.Idle;

        public override MainFormState UpdateState(Point location, StateChangingTrigger trigger)
        {
            switch (trigger)
            {
                case StateChangingTrigger.MouseDown:
                    {
                        var component = _selectionManager.GetSelectionComponentNearPoint(location, _buffer);
                        if (component != null && component.SelectionComponentType == SelectionComponentType.Inside)
                        {
                            return new MovingSelectionState(_selectionManager, component, location);
                        }
                        return new SelectingState(_selectionManager, location);
                    }
                case StateChangingTrigger.MouseMove:
                    {
                        // Check whether the location is over edge or selection or nothing.
                        // _selectionManager.GetSelectionComponentNearPoint(location, _buffer);
                        var component = _selectionManager.GetSelectionComponentNearPoint(location, _buffer);
                        if (component == null)
                            return this;
                        else if (component.SelectionComponentType == SelectionComponentType.Inside)
                            return new OverSelectionState(_selectionManager, component);
                        else
                        {
                            return new OverEdgeState(_selectionManager, component);
                        }
                    }
                default:
                    return this;
            }
        }
    }

    class SelectingState : MainFormState
    {        
        private readonly Point _start;

        public SelectingState(ISelectionManager selectionManager, Point actionLocation)
            : base(selectionManager, null)
        {
            _start = actionLocation;            
        }

        public override MainFormStatus Status => MainFormStatus.Selecting;

        public override MainFormState UpdateState(Point location, StateChangingTrigger trigger)
        {
            _activeSelection = _selectionManager.CreateSelection(
                Math.Min(location.X, _start.X), 
                Math.Min(location.Y, _start.Y),
                Math.Abs(location.X - _start.X), 
                Math.Abs(location.Y - _start.Y));

            switch (trigger)
            {
                case StateChangingTrigger.MouseUp:
                    if(_activeSelection.LeftEdge.Length > _buffer && _activeSelection.TopEdge.Length > _buffer)
                        _selectionManager.AddSelection(_activeSelection);
                    return new IdleState(_selectionManager, location);
                case StateChangingTrigger.MouseMove:                    
                    return this;
                default:
                    return this;
            }            
        }
    }

    class OverEdgeState : MainFormState
    {
        public OverEdgeState(ISelectionManager selectionManager, ISelectionComponent edge)
            : base(selectionManager, edge.Parent)
        {
            ComponentAtCursorPoint = edge;
        }

        public override MainFormStatus Status => MainFormStatus.OverEdge;

        public override MainFormState UpdateState(Point location, StateChangingTrigger trigger)
        {
            switch (trigger)
            {
                case StateChangingTrigger.MouseDown:
                    return new ResizingSelectionState(_selectionManager, 
                        this.ComponentAtCursorPoint, location);
                case StateChangingTrigger.MouseMove:
                    var activeSelectionComponent = _selectionManager
                        .GetSelectionComponentNearPoint(location, _buffer);
                    if (activeSelectionComponent == null)
                        return new IdleState(_selectionManager, location);
                    else if (activeSelectionComponent.Equals(this.ComponentAtCursorPoint))
                        return this;
                    else if (activeSelectionComponent.SelectionComponentType == SelectionComponentType.Inside)
                        return new OverSelectionState(_selectionManager, activeSelectionComponent);
                    else
                        return new OverEdgeState(_selectionManager, activeSelectionComponent);
                default:
                    return this;
            }
        }
    }

    class ResizingSelectionState : MainFormState
    {
        Point _actionStartLocation;

        public ResizingSelectionState(ISelectionManager selectionManager, ISelectionComponent selectionComponent, Point location)
            : base(selectionManager, selectionComponent.Parent)
        {
            ComponentAtCursorPoint = selectionComponent;
            _actionStartLocation = location;
        }

        public override MainFormStatus Status => MainFormStatus.ResizingSelection;

        public override MainFormState UpdateState(Point location, StateChangingTrigger trigger)
        {
            switch (trigger)
            {
                case StateChangingTrigger.MouseUp:
                    return new OverEdgeState(_selectionManager, ComponentAtCursorPoint);
                case StateChangingTrigger.MouseMove:
                    var resized = ResizeActiveSelection(location);
                    return new ResizingSelectionState(_selectionManager, 
                        resized.GetSelectionComponentByType(ComponentAtCursorPoint.SelectionComponentType), location);
                case StateChangingTrigger.MouseDown:
                default:
                    return this;
            }
        }

        private Selection ResizeActiveSelection(Point location)
        {
            Tuple<Point, Size> newValues = new Tuple<Point, Size>(_activeSelection.NWCorner.Coordinates,
                new Size(_activeSelection.TopEdge.Length, _activeSelection.LeftEdge.Length));

            switch (ComponentAtCursorPoint.SelectionComponentType)
            {
                case SelectionComponentType.NWCorner:
                    newValues = MoveNWCorner(location);
                    break;
                case SelectionComponentType.SWCorner:
                    newValues = MoveSWCorner(location);
                    break;
                case SelectionComponentType.SECorner:
                    newValues = MoveSECorner(location);
                    break;
                case SelectionComponentType.NECorner:
                    newValues = MoveNECorner(location);
                    break;
                case SelectionComponentType.TopEdge:
                    newValues = MoveTopEdge(location);
                    break;
                case SelectionComponentType.LeftEdge:
                    newValues = MoveLeftEdge(location);
                    break;
                case SelectionComponentType.BottomEdge:
                    newValues = MoveBottomEdge(location);
                    break;
                case SelectionComponentType.RightEdge:
                    newValues = MoveRightEdge(location);
                    break;
                case SelectionComponentType.Inside:
                    break;
                default:
                    break;
            }

            return _selectionManager.ResizeSelection(_activeSelection, newValues.Item1, newValues.Item2);            
        }

        private Tuple<Point, Size> MoveTopEdge(Point location)
        {
            int newTopEdgeY = Math.Min(location.Y, _activeSelection.SWCorner.Coordinates.Y);
            int newBottomEdgeY = Math.Max(location.Y, _activeSelection.SWCorner.Coordinates.Y);

            return new Tuple<Point, Size>(
                new Point(_activeSelection.NWCorner.Coordinates.X, newTopEdgeY),
                new Size(_activeSelection.TopEdge.Length, newBottomEdgeY - newTopEdgeY));
        }

        private Tuple<Point, Size> MoveBottomEdge(Point location)
        {
            int newTopEdgeY = Math.Min(location.Y, _activeSelection.NWCorner.Coordinates.Y);
            int newBottomEdgeY = Math.Max(location.Y, _activeSelection.NWCorner.Coordinates.Y);

            return new Tuple<Point, Size>(
                new Point(_activeSelection.SWCorner.Coordinates.X, newTopEdgeY),
                new Size(_activeSelection.BottomEdge.Length, newBottomEdgeY - newTopEdgeY));
        }

        private Tuple<Point, Size> MoveLeftEdge(Point location)
        {
            int newLeftEdgeX = Math.Min(location.X, _activeSelection.NECorner.Coordinates.X);
            int newRightEdgeX = Math.Max(location.X, _activeSelection.NECorner.Coordinates.X);

            return new Tuple<Point, Size>(
                new Point(newLeftEdgeX, _activeSelection.NECorner.Coordinates.Y),
                new Size(newRightEdgeX - newLeftEdgeX, _activeSelection.LeftEdge.Length));
        }

        private Tuple<Point, Size> MoveRightEdge(Point location)
        {
            int newLeftEdgeX = Math.Min(location.X, _activeSelection.NWCorner.Coordinates.X);
            int newRightEdgeX = Math.Max(location.X, _activeSelection.NWCorner.Coordinates.X);

            return new Tuple<Point, Size>(
                new Point(newLeftEdgeX, _activeSelection.NWCorner.Coordinates.Y),
                new Size(newRightEdgeX - newLeftEdgeX, _activeSelection.RightEdge.Length));
        }

        private Tuple<Point, Size> MoveNWCorner(Point location)
        {
            int newLeftEdgeX = Math.Min(location.X, _activeSelection.NECorner.Coordinates.X);
            int newRightEdgeX = Math.Max(location.X, _activeSelection.NECorner.Coordinates.X);
            int newTopEdgeY = Math.Min(location.Y, _activeSelection.SWCorner.Coordinates.Y);
            int newBottomEdgeY = Math.Max(location.Y, _activeSelection.SWCorner.Coordinates.Y);

            return new Tuple<Point, Size>(
                new Point(newLeftEdgeX, newTopEdgeY),
                new Size(newRightEdgeX - newLeftEdgeX, newBottomEdgeY - newTopEdgeY));
        }

        private Tuple<Point, Size> MoveNECorner(Point location)
        {
            int newTopEdgeY = Math.Min(location.Y, _activeSelection.SWCorner.Coordinates.Y);
            int newBottomEdgeY = Math.Max(location.Y, _activeSelection.SWCorner.Coordinates.Y);
            int newLeftEdgeX = Math.Min(location.X, _activeSelection.NWCorner.Coordinates.X);
            int newRightEdgeX = Math.Max(location.X, _activeSelection.NWCorner.Coordinates.X);

            return new Tuple<Point, Size>(
                new Point(newLeftEdgeX, newTopEdgeY),
                new Size(newRightEdgeX - newLeftEdgeX, newBottomEdgeY - newTopEdgeY));
        }

        private Tuple<Point, Size> MoveSWCorner(Point location)
        {
            int newLeftEdgeX = Math.Min(location.X, _activeSelection.NECorner.Coordinates.X);
            int newRightEdgeX = Math.Max(location.X, _activeSelection.NECorner.Coordinates.X);
            int newTopEdgeY = Math.Min(location.Y, _activeSelection.NWCorner.Coordinates.Y);
            int newBottomEdgeY = Math.Max(location.Y, _activeSelection.NWCorner.Coordinates.Y);

            return new Tuple<Point, Size>(
                new Point(newLeftEdgeX, newTopEdgeY),
                new Size(newRightEdgeX - newLeftEdgeX, newBottomEdgeY - newTopEdgeY));
        }

        private Tuple<Point, Size> MoveSECorner(Point location)
        {
            int newLeftEdgeX = Math.Min(location.X, _activeSelection.NWCorner.Coordinates.X);
            int newRightEdgeX = Math.Max(location.X, _activeSelection.NWCorner.Coordinates.X);
            int newTopEdgeY = Math.Min(location.Y, _activeSelection.NWCorner.Coordinates.Y);
            int newBottomEdgeY = Math.Max(location.Y, _activeSelection.NWCorner.Coordinates.Y);

            return new Tuple<Point, Size>(
                new Point(newLeftEdgeX, newTopEdgeY),
                new Size(newRightEdgeX - newLeftEdgeX, newBottomEdgeY - newTopEdgeY));
        }
    }

    class MovingSelectionState : MainFormState
    {
        Point _actionStartLocation;

        public MovingSelectionState(ISelectionManager selectionManager, ISelectionComponent selectionComponent, Point location)
            : base(selectionManager, selectionComponent.Parent)
        {
            ComponentAtCursorPoint = selectionComponent;
            _actionStartLocation = location;
        }

        public override MainFormStatus Status => MainFormStatus.MovingSelection;

        public override MainFormState UpdateState(Point location, StateChangingTrigger trigger)
        {
            switch (trigger)
            {
                case StateChangingTrigger.MouseDown:
                    return this;
                case StateChangingTrigger.MouseUp:
                    return new OverSelectionState(_selectionManager, _activeSelection.Inside);
                case StateChangingTrigger.MouseMove:
                    // Calculate the move coordinates from the _actionStartLocation.
                    var offset = new Size(location.X - _actionStartLocation.X, location.Y - _actionStartLocation.Y);
                    Point newLocation = _activeSelection.NWCorner.Coordinates + offset;
                    var s = _selectionManager.MoveSelection(_activeSelection, newLocation);
                    return new MovingSelectionState(_selectionManager, s.Inside, location);
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }

    class OverSelectionState : MainFormState
    {
        public OverSelectionState(ISelectionManager selectionManager, ISelectionComponent interior)
            : base(selectionManager, interior.Parent)
        {
            ComponentAtCursorPoint = interior;
        }

        public override MainFormStatus Status => MainFormStatus.OverSelection;

        public override MainFormState UpdateState(Point location, StateChangingTrigger trigger)
        {
            switch (trigger)
            {
                case StateChangingTrigger.MouseDown:
                    return new MovingSelectionState(_selectionManager,
                        this.ComponentAtCursorPoint, location);
                case StateChangingTrigger.MouseMove:
                    var activeSelectionComponent = _selectionManager
                        .GetSelectionComponentNearPoint(location, _buffer);
                    if (activeSelectionComponent == null)
                        return new IdleState(_selectionManager, location);
                    else if (activeSelectionComponent.Equals(this.ComponentAtCursorPoint))
                        return this;
                    else if (activeSelectionComponent.SelectionComponentType != SelectionComponentType.Inside)
                        return new OverEdgeState(_selectionManager, activeSelectionComponent);
                    else
                        return new OverSelectionState(_selectionManager, activeSelectionComponent);
                default:
                    return this;
            }
        }
    }
}
