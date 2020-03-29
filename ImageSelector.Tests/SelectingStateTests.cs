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
    public class SelectingStateTests
    {
        SelectingState _state;
        Point _location;
        Point _location2;
        Mock<ISelectionManager> _selectionManager;
        int _buffer;

        private const int ZIndex = 1;

        [TestInitialize]
        public void Initialize()
        {
            _location = new Point(1, 1);
            _location2 = new Point(5, 5);
            _selectionManager = new Mock<ISelectionManager>();
            _selectionManager
                .Setup(m => m
                    .CreateSelection(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int x, int y, int width, int height) => {
                    return new Selection(new Rectangle(x, y, width, height), ZIndex);
                });
            _buffer = Int32.Parse(ConfigurationManager.AppSettings[Constants.BufferAppSetting]);

            _state = new SelectingState(_selectionManager.Object, _location);
        }

        [TestMethod]
        public void test_StatusIsSelecting()
        {
            Assert.AreEqual(MainFormStatus.Selecting, _state.Status);
        }

        [TestMethod]
        public void test_InitialActiveSelectionIsNull()
        {
            Assert.IsNull(_state.ActiveSelection);
        }

        [TestMethod]
        public void test_InitialComponentAtCursorPointIsNull()
        {
            Assert.IsNull(_state.ComponentAtCursorPoint);
        }

        [TestMethod]
        public void test_DoesNotChangeStateOnMouseDown()
        {
            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseDown);

            Assert.AreEqual(_state, result);
        }

        [TestMethod]
        public void test_CreatesNewActiveSelectionOnMouseMove()
        {
            var s = new Selection(new Rectangle(_location, new Size(1, 1)), 1);

            _selectionManager.Setup(x => x.CreateSelection(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(s);

            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseMove);

            Assert.AreEqual(s, result.ActiveSelection);
            Assert.AreEqual(MainFormStatus.Selecting, result.Status);
            Assert.IsNull(result.ComponentAtCursorPoint);
            _selectionManager.Verify(x => x.CreateSelection(
                _location.X, _location.Y, _location2.X - _location.X, _location2.Y - _location.Y));
        }

        [TestMethod]
        public void test_AddsNewSelectionToManagerOnMOuseUp()
        {
            var s = new Selection(new Rectangle(_location, new Size(4, 4)), 1);

            _selectionManager.Setup(x => x.CreateSelection(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(s);
            _selectionManager.Setup(x => x.AddSelection(It.IsAny<Selection>()));

            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseUp);

            _selectionManager.Verify(x => x.AddSelection(s));
        }

        [TestMethod]
        public void test_DoesNotAddsNewSelectionIfAnyDimensionSmallerThanBuffer()
        {
            var s = new Selection(new Rectangle(_location, new Size(_buffer - 1, 4)), 1);

            _selectionManager.Setup(x => x.CreateSelection(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(s);
            _selectionManager.Setup(x => x.AddSelection(It.IsAny<Selection>()));

            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseUp);

            _selectionManager.Verify(x => x.AddSelection(It.IsAny<Selection>()), Times.Never());
        }

        [TestMethod]
        public void test_CreatesNewActiveSelectionOnMouseUp()
        {
            var s = new Selection(new Rectangle(_location, new Size(1, 1)), 1);

            _selectionManager.Setup(x => x.CreateSelection(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(s);

            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseUp);

            Assert.AreEqual(s, _state.ActiveSelection);
            Assert.IsNull(result.ComponentAtCursorPoint);
        }

        [TestMethod]
        public void test_ReturnsIdleStateOnMouseUp()
        {
            var s = new Selection(new Rectangle(_location, new Size(1, 1)), 1);

            _selectionManager.Setup(x => x.CreateSelection(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(s);
            _selectionManager.Setup(x => x.AddSelection(It.IsAny<Selection>()));

            var result = _state.UpdateState(_location2, StateChangingTrigger.MouseUp);

            Assert.AreEqual(MainFormStatus.Idle, result.Status);
        }
    }
}
