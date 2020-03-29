using System;
using System.Configuration;
using System.Drawing;
using ImageSelector.Core;
using ImageSelector.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImageSelector.Tests
{
    [TestClass]
    public class OverEdgeStateTests
    {
        OverEdgeState _state;
        Point _location;
        Point _location2;
        Mock<ISelectionManager> _selectionManager;
        int _buffer;
        Selection _selection;

        private const int ZIndex = 1;

        [TestInitialize]
        public void Initialize()
        {
            _selection = new Selection(new Rectangle(0, 0, 10, 10), 1);

            _location = new Point(1, 1);
            _location2 = new Point(2, 2);
            _selectionManager = new Mock<ISelectionManager>();
            _selectionManager
                .Setup(m => m
                    .CreateSelection(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int x, int y, int width, int height) => {
                    return new Selection(new Rectangle(x, y, width, height), ZIndex);
                });
            _buffer = Int32.Parse(ConfigurationManager.AppSettings[Constants.BufferAppSetting]);

            _state = new OverEdgeState(_selectionManager.Object, _selection.TopEdge);
        }

        [TestMethod]
        public void test_StatusIsOverEdge()
        {
            Assert.AreEqual(MainFormStatus.OverEdge, _state.Status);
        }

        [TestMethod]
        public void test_ComponentAtLocationIsTheCorrectEdge()
        {
            Assert.AreEqual(_selection.TopEdge, _state.ComponentAtCursorPoint);
        }

        [TestMethod]
        public void test_OnMouseDownReturnsResizingStatus()
        {
            var state = _state.UpdateState(new Point(5, 0), StateChangingTrigger.MouseDown);
            Assert.AreEqual(MainFormStatus.ResizingSelection, state.Status);
            Assert.AreEqual(_selection.TopEdge, state.ComponentAtCursorPoint);
            Assert.AreEqual(_selection, state.ActiveSelection);
        }

        [TestMethod]
        public void test_OnMouseMoveReturnsIdleStatusWhenOutsideBuffer()
        {
            Point location = new Point(15, 15);
            _selectionManager
                .Setup(m => m
                    .GetSelectionComponentNearPoint(location, _buffer))
                .Returns(default(ISelectionComponent));

            Assert.IsFalse(_state.ComponentAtCursorPoint.IsPointNearComponent(location, _buffer));

            var state = _state.UpdateState(location, StateChangingTrigger.MouseMove);

            Assert.AreEqual(MainFormStatus.Idle, state.Status);
        }

        [TestMethod]
        public void test_ReturnsSelfWhenStillOverTheSameComponent()
        {
            Point location = new Point(5, 0);
            _selectionManager
                .Setup(m => m
                    .GetSelectionComponentNearPoint(location, _buffer))
                .Returns(_selection.TopEdge);

            Assert.IsTrue(_state.ComponentAtCursorPoint.IsPointNearComponent(location, _buffer));

            var state = _state.UpdateState(location, StateChangingTrigger.MouseMove);

            Assert.AreSame(_state, state);
        }

        [TestMethod]
        public void test_OnMouseMoveReturnsOverEdgeWhenOverDifferentEdge()
        {
            Point location = new Point(0, 5);
            _selectionManager
                .Setup(m => m
                    .GetSelectionComponentNearPoint(location, _buffer))
                .Returns(_selection.LeftEdge);

            var state = _state.UpdateState(location, StateChangingTrigger.MouseMove);

            Assert.AreEqual(MainFormStatus.OverEdge, state.Status);
            Assert.AreEqual(_selection.LeftEdge, state.ComponentAtCursorPoint);
            Assert.AreEqual(_selection, state.ActiveSelection);
        }

        [TestMethod]
        public void test_OnMouseMoveReturnsOverEdgeWhenOverACorner()
        {
            Point location = new Point(0, 0);
            _selectionManager
                .Setup(m => m
                    .GetSelectionComponentNearPoint(location, _buffer))
                .Returns(_selection.NWCorner);

            var state = _state.UpdateState(location, StateChangingTrigger.MouseMove);

            Assert.AreEqual(MainFormStatus.OverEdge, state.Status);
            Assert.AreEqual(_selection.NWCorner, state.ComponentAtCursorPoint);
            Assert.AreEqual(_selection, state.ActiveSelection);
        }

        [TestMethod]        
        public void test_OnMouseOverReturnsOverSelectionWhenInsideSelection()
        {
            Point location = new Point(5, 5);
            _selectionManager
                .Setup(m => m
                    .GetSelectionComponentNearPoint(location, _buffer))
                .Returns(_selection.Inside);

            var state = _state.UpdateState(location, StateChangingTrigger.MouseMove);

            Assert.AreEqual(MainFormStatus.OverSelection, state.Status);
        }
    }
}
