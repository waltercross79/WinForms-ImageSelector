/* Copyright (C) 2020 - Present 
 * Walter Cross - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the GPL v3.0 license.
 */

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
    public class IdleStateTests
    {
        IdleState _state;
        Point _location;
        Point _location2;
        Mock<ISelectionManager> _selectionManager;
        int _buffer;

        private const int ZIndex = 1;

        [TestInitialize]
        public void Initialize()
        {
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

            _state = new IdleState(_selectionManager.Object, _location);
        }

        [TestMethod]
        public void test_StatusIsIdle()
        {
            Assert.AreEqual(MainFormStatus.Idle, _state.Status);
        }

        [TestMethod]
        public void test_ActiveSelectionIsNull()
        {
            Assert.IsNull(_state.ActiveSelection);
        }

        [TestMethod]
        public void test_ComponentAtCursorPointIsNull()
        {
            Assert.IsNull(_state.ComponentAtCursorPoint);
        }

        [TestMethod]
        public void test_UpdateStateReturnsSelectingOnMouseDown()
        {
            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseDown);

            Assert.AreEqual(MainFormStatus.Selecting, result.Status);            
        }

        [TestMethod]
        public void test_ChecksComponentsAtPointOnMouseMove()
        {
            _selectionManager.Setup(x => x.GetSelectionComponentNearPoint(It.IsAny<Point>(), It.IsAny<int>()))
                .Returns(default(ISelectionComponent));

            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseMove);

            _selectionManager.Verify(x => x.GetSelectionComponentNearPoint(_location2, _buffer));
        }

        [TestMethod]
        public void test_DoesNotChangeStateOnMouseMoveIfNotOverAnything()
        {
            _selectionManager.Setup(x => x.GetSelectionComponentNearPoint(It.IsAny<Point>(), It.IsAny<int>()))
                .Returns(default(ISelectionComponent));

            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseMove);

            Assert.AreEqual(_state, result);
        }

        [TestMethod]
        public void test_UpdateStateReturnsOverEdgeOnMouseMoveWhenOverSelectionEdge()
        {
            Selection s = new Selection(new Rectangle(new Point(_location2.X, _location2.Y - 3), new Size(10, 10)), 1);

            _selectionManager.Setup(x => x.GetSelectionComponentNearPoint(It.IsAny<Point>(), It.IsAny<int>()))
                .Returns(s.LeftEdge);

            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseMove);

            Assert.AreEqual(MainFormStatus.OverEdge, result.Status);
            Assert.AreEqual(s.LeftEdge, result.ComponentAtCursorPoint);
            Assert.AreEqual(s, result.ActiveSelection);
        }


        [TestMethod]
        public void test_UpdateStateReturnsOverEdgeOnMouseMoveWhenOverSelectionCorner()
        {
            Selection s = new Selection(new Rectangle(new Point(_location2.X, _location2.Y - 3), new Size(10, 10)), 1);

            _selectionManager.Setup(x => x.GetSelectionComponentNearPoint(It.IsAny<Point>(), It.IsAny<int>()))
                .Returns(s.NWCorner);

            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseMove);

            Assert.AreEqual(MainFormStatus.OverEdge, result.Status);
            Assert.AreEqual(s.NWCorner, result.ComponentAtCursorPoint);
            Assert.AreEqual(s, result.ActiveSelection);
        }

        [TestMethod]
        public void test_DoesNotChangeStateOnMouseUp()
        {            
            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseUp);

            Assert.AreEqual(_state, result);
        }
    }
}
