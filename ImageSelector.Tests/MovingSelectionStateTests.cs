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
    public class MovingSelectionStateTests
    {
        MovingSelectionState _state;
        Point _location;
        Point _location2;
        Mock<ISelectionManager> _selectionManager;
        int _buffer;
        Selection _selection;
        Selection _selection2;

        private const int ZIndex = 1;

        [TestInitialize]
        public void Initialize()
        {
            _selection = new Selection(new Rectangle(0, 0, 10, 10), 1);
            _selection2 = new Selection(new Rectangle(2, 0, 10, 10), 1);

            _location = new Point(1, 1);
            _location2 = new Point(2, 2);
            _selectionManager = new Mock<ISelectionManager>();
            _selectionManager
                .Setup(m => m
                    .CreateSelection(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int x, int y, int width, int height) => {
                    return new Selection(new Rectangle(x, y, width, height), ZIndex);
                });
            _selectionManager.Setup(x => x.MoveSelection(
                It.IsAny<Selection>(),
                It.IsAny<Point>()))
                .Returns(_selection2);

            _buffer = Int32.Parse(ConfigurationManager.AppSettings[Constants.BufferAppSetting]);

            _state = new MovingSelectionState(_selectionManager.Object, _selection.Inside, new Point(5,5));
        }

        [TestMethod]
        public void test_StatusIsMovingSelection()
        {
            Assert.AreEqual(MainFormStatus.MovingSelection, _state.Status);
        }

        [TestMethod]
        public void test_ComponentAtLocationIsTheInterior()
        {
            Assert.AreEqual(_selection.Inside, _state.ComponentAtCursorPoint);
        }

        [TestMethod]
        public void test_OnMouseDownReturnsSelf()
        {
            var state = _state.UpdateState(new Point(5, 0), StateChangingTrigger.MouseDown);
            Assert.AreSame(_state, state);
        }

        [TestMethod]
        public void test_OnMouseUpReturnsIdleState()
        {
            var state = _state.UpdateState(new Point(5, 5), StateChangingTrigger.MouseUp);
            Assert.AreEqual(MainFormStatus.Idle, state.Status);
        }

        [TestMethod]
        public void test_OnMouseMoveCallsSelectionManagerToMoveActive()
        {            
            var state = _state.UpdateState(new Point(7, 5), StateChangingTrigger.MouseMove); // Lateral move by 2.

            _selectionManager.Verify(x => x.MoveSelection(_selection,
                _selection.NWCorner.Coordinates + new Size(2, 0)));
        }

        [TestMethod]
        public void test_OnMouseMoveReturnsMovingStateWithUpdatedSelection()
        {
            var state = _state.UpdateState(new Point(7, 5), StateChangingTrigger.MouseMove); // Lateral move by 2.
            Assert.AreSame(_selection2, state.ActiveSelection);
            Assert.AreEqual(MainFormStatus.MovingSelection, state.Status);
        }

        [TestMethod]
        public void test_RepeatedOnMouseDownMovesTheSelectionByCorrectOffsets()
        {
            _selectionManager.Setup(s => s.MoveSelection(
                It.Is<Selection>(_s => _s.NWCorner.Coordinates == new Point(0, 0)),
                new Point(2, 0)))
                .Returns(new Selection(new Rectangle(2, 0, 10, 10), 1));
            _selectionManager.Setup(s => s.MoveSelection(
                It.Is<Selection>(_s => _s.NWCorner.Coordinates == new Point(2, 0)),
                new Point(4, 0)))
                .Returns(new Selection(new Rectangle(4, 0, 10, 10), 1));
            _selectionManager.Setup(s => s.MoveSelection(
                It.Is<Selection>(_s => _s.NWCorner.Coordinates == new Point(4, 0)),
                new Point(4, 2)))
                .Returns(new Selection(new Rectangle(4, 2, 10, 10), 1));
            _selectionManager.Setup(s => s.MoveSelection(
                It.Is<Selection>(_s => _s.NWCorner.Coordinates == new Point(4, 2)),
                new Point(4, 4)))
                .Returns(new Selection(new Rectangle(4, 4, 10, 10), 1));

            var state = _state.UpdateState(new Point(7, 5), StateChangingTrigger.MouseMove); // Lateral move by 2.
            var state2 = state.UpdateState(new Point(9, 5), StateChangingTrigger.MouseMove); // Lateral move by 2.
            var state3 = state2.UpdateState(new Point(9, 7), StateChangingTrigger.MouseMove); // Vertical move by 2.
            var state4 = state3.UpdateState(new Point(9, 9), StateChangingTrigger.MouseMove); // Vertical move by 2.

            _selectionManager.Verify(s => s.MoveSelection(
                It.Is<Selection>(_s => _s.NWCorner.Coordinates == new Point(0, 0)),
                new Point(2, 0)));
            _selectionManager.Verify(s => s.MoveSelection(
                It.Is<Selection>(_s => _s.NWCorner.Coordinates == new Point(2, 0)),
                new Point(4, 0)));
            _selectionManager.Verify(s => s.MoveSelection(
                It.Is<Selection>(_s => _s.NWCorner.Coordinates == new Point(4, 0)),
                new Point(4, 2)));
            _selectionManager.Verify(s => s.MoveSelection(
                It.Is<Selection>(_s => _s.NWCorner.Coordinates == new Point(4, 2)),
                new Point(4, 4)));
        }
    }
}
