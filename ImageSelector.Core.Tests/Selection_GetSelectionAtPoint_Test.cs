using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageSelector.Core.Tests
{
    [TestClass]
    public class Selection_GetSelectionAtPoint_Test
    {
        // Selection is a rectangle in location (0,0) of size (100, 1000).
        // Bottom edge is <0, 100> <--> <100, 1000>
        Selection _s;
        int[] _widths = { 0, 1, 2, 3 };

        [TestInitialize]
        public void Initialize()
        {
            _s = new Selection(new Rectangle(0, 0, 100, 1000), 0);
        }

        [TestMethod]
        public void test_GetsNWCorner()
        {
            var result = _s.GetSelectionAtPoint(0, 0);

            Assert.AreEqual(SelectionComponentType.NWCorner, result.SelectionComponentType);
        }

        [TestMethod]
        public void test_GetsSWCorner()
        {
            var result = _s.GetSelectionAtPoint(0, 1000);

            Assert.AreEqual(SelectionComponentType.SWCorner, result.SelectionComponentType);
        }

        [TestMethod]
        public void test_GetsSECorner()
        {
            var result = _s.GetSelectionAtPoint(100, 1000);

            Assert.AreEqual(SelectionComponentType.SECorner, result.SelectionComponentType);
        }

        [TestMethod]
        public void test_GetsNECorner()
        {
            var result = _s.GetSelectionAtPoint(100, 0);

            Assert.AreEqual(SelectionComponentType.NECorner, result.SelectionComponentType);
        }

        [TestMethod]
        public void test_GetsTopEdge()
        {
            var result = _s.GetSelectionAtPoint(50, 0);

            Assert.AreEqual(SelectionComponentType.TopEdge, result.SelectionComponentType);
        }

        [TestMethod]
        public void test_GetsLeftEdge()
        {
            var result = _s.GetSelectionAtPoint(0, 500);

            Assert.AreEqual(SelectionComponentType.LeftEdge, result.SelectionComponentType);
        }

        [TestMethod]
        public void test_GetsBottomEdge()
        {
            var result = _s.GetSelectionAtPoint(50, 1000);

            Assert.AreEqual(SelectionComponentType.BottomEdge, result.SelectionComponentType);
        }

        [TestMethod]
        public void test_GetsRightEdge()
        {
            var result = _s.GetSelectionAtPoint(100, 500);

            Assert.AreEqual(SelectionComponentType.RightEdge, result.SelectionComponentType);
        }

        [TestMethod]
        public void test_GetsRightEdgeWhenWithinWidth()
        {
            var p = new Point(100, 500);

            foreach (var w in _widths)
            {
                for (int x = -w; x <= w; x++)
                {
                    var result =
                        _s.GetSelectionAtPoint(p + new Size(x, 0), w);
                    Assert.AreEqual(SelectionComponentType.RightEdge, result.SelectionComponentType);
                }
            }            
        }

        [TestMethod]
        public void test_GetsLeftEdgeWhenWithinWidth()
        {
            var p = new Point(0, 500);

            foreach (var w in _widths)
            {
                for (int x = -w; x <= w; x++)
                {
                    var result =
                        _s.GetSelectionAtPoint(p + new Size(x, 0), w);
                    Assert.AreEqual(SelectionComponentType.LeftEdge, result.SelectionComponentType);
                }
            }
        }

        [TestMethod]
        public void test_GetsTopEdgeWhenWithinWidth()
        {
            var p = new Point(50, 0);

            foreach (var w in _widths)
            {
                for (int y = -w; y <= w; y++)
                {
                    var result =
                        _s.GetSelectionAtPoint(p + new Size(0, y), w);
                    Assert.AreEqual(SelectionComponentType.TopEdge, result.SelectionComponentType);
                }
            }
        }

        [TestMethod]
        public void test_GetsBottomEdgeWhenWithinWidth()
        {
            var p = new Point(50, 1000);

            foreach (var w in _widths)
            {
                for (int y = -w; y <= w; y++)
                {
                    var result =
                        _s.GetSelectionAtPoint(p + new Size(0, y), w);
                    Assert.AreEqual(SelectionComponentType.BottomEdge, result.SelectionComponentType);
                }
            }
        }
    }
}
