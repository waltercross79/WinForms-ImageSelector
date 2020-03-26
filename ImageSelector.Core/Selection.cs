using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSelector.Core
{
    public interface ISelectionComponent
    {
        Selection Parent { get; }

        SelectionComponentType SelectionComponentType { get; }

        bool IsPointNearComponent(Point point, int width);
    }

    public class Interior : ISelectionComponent
    {
        private Point _start;
        private Point _end;

        public Interior(Point p1, Point p2, Selection parent)
        {
            Parent = parent;
            SelectionComponentType = SelectionComponentType.Inside;

            var first = p1.X < p2.X ? p1 :
                p1.Y < p2.Y ? p1 : p2;
            var second = first == p1 ? p2 : p1;

            _start = first;
            _end = second;
        }

        public Selection Parent { get; private set; }
        public SelectionComponentType SelectionComponentType { get; private set; }

        public bool IsPointNearComponent(Point point, int width)
        {           
            return point.X >= _start.X + width && point.Y >= _start.Y + width
                && point.X <= _end.X - width && point.Y <= _end.Y - width;
        }

        public override string ToString()
        {
            return String.Format("Interior[{0}, {1}]", _start.ToString(), _end.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Interior i = obj as Interior;

            if (i == null)
                return false;

            return this.GetHashCode() == i.GetHashCode();
        }
    }

    public class Corner : ISelectionComponent
    {
        private Point _point;

        public Corner(int x, int y, 
            SelectionComponentType type, Selection parent)
        {
            Parent = parent;
            SelectionComponentType = type;
            _point = new Point(x, y);
        }

        public Selection Parent { get; private set; }
        public SelectionComponentType SelectionComponentType { get; private set; }
        public Point Coordinates
        {
            get
            {
                return _point;
            }
        }

        public bool IsPointNearComponent(Point point, int width)
        {
            if ((point.X >= _point.X - width &&
                point.X <= _point.X + width &&
                point.Y >= _point.Y - width &&
                point.Y <= _point.Y + width))
                return true;

            return false;
        }

        public override string ToString()
        {
            return _point.ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Corner c = obj as Corner;

            if (c == null)
                return false;

            return this.GetHashCode() == c.GetHashCode();
        }
    }

    public class Edge : ISelectionComponent
    {
        private Corner _start;
        private Corner _end;

        public Edge(Corner c1, Corner c2)
        {
            var first = c1.Coordinates.X <= c2.Coordinates.X ? c1 :
                c1.Coordinates.Y <= c2.Coordinates.Y ? c1 : c2;
            var second = first == c1 ? c2 : c1;

            _start = first;
            _end = second;

            Points = new List<Point> { _start.Coordinates, _end.Coordinates };

            if (first.SelectionComponentType == SelectionComponentType.NWCorner)
            {
                if (second.SelectionComponentType == SelectionComponentType.SWCorner)
                    SelectionComponentType = SelectionComponentType.LeftEdge;
                else if (second.SelectionComponentType == SelectionComponentType.NECorner)
                    SelectionComponentType = SelectionComponentType.TopEdge;
            }
            else if (first.SelectionComponentType == SelectionComponentType.SWCorner)
            {
                if (second.SelectionComponentType == SelectionComponentType.SECorner)
                    SelectionComponentType = SelectionComponentType.BottomEdge;
            }
            else if (first.SelectionComponentType == SelectionComponentType.NECorner)
            {
                if (second.SelectionComponentType == SelectionComponentType.SECorner)
                    SelectionComponentType = SelectionComponentType.RightEdge;
            }
            else
                throw new ArgumentException("Invalid combination of corners.");

            Parent = c1.Parent;
        }

        public Selection Parent { get; private set; }
        public SelectionComponentType SelectionComponentType { get; private set; }
        public IEnumerable<Point> Points { get; private set; }

        public bool IsPointNearComponent(Point point, int width)
        {
            if (_start.Coordinates.Y == _end.Coordinates.Y) // Horizontal edge
            {
                if (point.X >= _start.Coordinates.X &&
                    point.X <= _end.Coordinates.X &&
                    point.Y <= _start.Coordinates.Y + width &&
                    point.Y >= _start.Coordinates.Y - width &&
                    point.Y <= _end.Coordinates.Y + width &&
                    point.Y >= _end.Coordinates.Y - width)
                    return true;
            }
            else if (_start.Coordinates.X == _end.Coordinates.X) // Vertical edge
            {
                if (point.X >= _start.Coordinates.X - width &&
                    point.X <= _start.Coordinates.X + width &&
                    point.X >= _end.Coordinates.X - width &&
                    point.X <= _end.Coordinates.X + width &&
                    point.Y >= _start.Coordinates.Y &&
                    point.Y <= _end.Coordinates.Y)
                    return true;
            }
            else
                throw new NotImplementedException("Only horizontal and vertical edges are supported.");

            return false;
        }

        public int Length
        {
            get
            {
                if (SelectionComponentType == SelectionComponentType.TopEdge ||
                    SelectionComponentType == SelectionComponentType.BottomEdge)
                {
                    return _end.Coordinates.X - _start.Coordinates.X;
                }
                else if (SelectionComponentType == SelectionComponentType.LeftEdge ||
                    SelectionComponentType == SelectionComponentType.RightEdge)
                {
                    return _end.Coordinates.Y - _start.Coordinates.Y;
                }
                else
                    return 0;
            }
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}]", _start.ToString(), _end.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Edge c = obj as Edge;

            if (c == null)
                return false;

            return this.GetHashCode() == c.GetHashCode();
        }
    }

    public class Selection
    {
        private readonly List<ISelectionComponent> _components;

        public Selection(Rectangle locationAndSize, int zindex)
        {
            ID = Guid.NewGuid();
            LocationAndSize = locationAndSize;
            ZIndex = zindex;

            _components = new List<ISelectionComponent>();

            SetCorners(locationAndSize);
            SetEdges();
            SetInterior();
        }

        /// <summary>
        /// Unique ID that identifies individual instances of the entity.
        /// </summary>
        internal Guid ID { get; private set; }

        public int ZIndex { get; set; }

        public Corner NWCorner { get; private set; }
        
        public Corner SWCorner { get; private set; }

        public Corner SECorner { get; private set; }
        
        public Corner NECorner { get; private set; }

        public Edge LeftEdge { get; private set; }

        public Edge RightEdge { get; private set; }

        public Edge BottomEdge { get; private set; }

        public Edge TopEdge { get; private set; }

        public Interior Inside { get; private set; }

        public Rectangle LocationAndSize { get; set; }

        public string Description { 
            get
            {
                return String.Format("X: {0}, Y: {1}", LocationAndSize.X, LocationAndSize.Y); 
            } 
        }

        private void SetCorners(Rectangle locationAndSize)
        {
            NWCorner = new Corner(locationAndSize.X, locationAndSize.Y, SelectionComponentType.NWCorner, this);
            SWCorner = new Corner(locationAndSize.X, locationAndSize.Y + locationAndSize.Height, SelectionComponentType.SWCorner, this);
            SECorner = new Corner(locationAndSize.X + locationAndSize.Width, locationAndSize.Y + locationAndSize.Height, SelectionComponentType.SECorner, this);
            NECorner = new Corner(locationAndSize.X + locationAndSize.Width, locationAndSize.Y, SelectionComponentType.NECorner, this);

            _components.AddRange(new ISelectionComponent[] { NWCorner, SWCorner, SECorner, NECorner });
        }            

        private void SetEdges()
        {
            LeftEdge = new Edge(NWCorner, SWCorner); // Left
            RightEdge = new Edge(NECorner, SECorner); // Right
            TopEdge = new Edge(NWCorner, NECorner); //Top
            BottomEdge = new Edge(SWCorner, SECorner); //Bottom

            _components.AddRange(new ISelectionComponent[] { LeftEdge, RightEdge, TopEdge, BottomEdge });
        }         

        private void SetInterior()
        {
            Inside = new Interior(NWCorner.Coordinates, SECorner.Coordinates, this);

            _components.Add(Inside);
        }

        /// <summary>
        /// Gets a component of the selection at a specified coordinates.
        /// </summary>
        /// <param name="x">Horizontal coordinate.</param>
        /// <param name="y">Vertical coordinate.</param>
        /// <returns>Value from <see cref="SelectionComponentType"/> enumeration.</returns>
        public ISelectionComponent GetSelectionAtPoint(int x, int y, int width = 0)
        {
            return GetSelectionAtPoint(new Point(x, y), width);
        }

        /// <summary>
        /// Gets a component of the selection at a specified point.
        /// </summary>
        /// <param name="p">Coordinates to examine.</param>
        /// <returns>Value from <see cref="SelectionComponentType"/> enumeration.</returns>
        public ISelectionComponent GetSelectionAtPoint(Point p, int width = 0)
        {
            foreach (var c in _components)
            {
                if (c.IsPointNearComponent(p, width))
                    return c;
            }

            return null;
        }

        public ISelectionComponent GetSelectionComponentByType(SelectionComponentType componentType)
        {
            return this._components.Where(x => x.SelectionComponentType == componentType).Single();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;


            if (!(obj is Selection toCompare))
                return false;

            return NWCorner == toCompare.NWCorner &&
                SECorner == toCompare.SECorner &&
                ZIndex == toCompare.ZIndex;
        }

        public override string ToString()
        {
            return String.Format("NW: {0}, SE: {1}, ZIndex: {2}", NWCorner, SECorner, ZIndex);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }        
    }
}
