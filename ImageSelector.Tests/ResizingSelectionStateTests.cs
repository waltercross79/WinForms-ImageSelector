/* Copyright (C) 2020 - Present 
 * Walter Cross - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the GPL v3.0 license.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using ImageSelector.Core;
using ImageSelector.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImageSelector.Tests
{
    [TestClass]
    public class ResizingSelectionStateTests
    {
        List<Tuple<MainFormState, Point, Selection>> _stateVariations;
        Point _selectionStart;
        Size _selectionSize;
        Mock<ISelectionManager> _selectionManager;
        int _buffer;
        Selection _selection;

        private const int ZIndex = 1;

        [TestInitialize]
        public void Initialize()
        {
            _selectionStart = new Point(0, 0);
            _selectionSize = new Size(10, 10);
            _selectionManager = new Mock<ISelectionManager>();
            _selection = new Selection(new Rectangle(_selectionStart, _selectionSize), 0);
            _selectionManager
                .Setup(m => m
                    .CreateSelection(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(_selection);
            _selectionManager.Setup(x => x.ResizeSelection(It.IsAny<Selection>(), It.IsAny<Point>(), It.IsAny<Size>()));
            _buffer = Int32.Parse(ConfigurationManager.AppSettings[Constants.BufferAppSetting]);

            _stateVariations = new List<Tuple<MainFormState, Point, Selection>>();
            _stateVariations.Add(new Tuple<MainFormState, Point, Selection>(
                new ResizingSelectionState(_selectionManager.Object, _selection.LeftEdge, new Point(0, 5)),
                new Point(5, 5), new Selection(new Rectangle(5, 0, 5, 10), _selection.ZIndex)));
            _stateVariations.Add(new Tuple<MainFormState, Point, Selection>(
                new ResizingSelectionState(_selectionManager.Object, _selection.TopEdge, new Point(5, 0)),
                new Point(5, 5), new Selection(new Rectangle(0, 5, 10, 5), _selection.ZIndex)));
            _stateVariations.Add(new Tuple<MainFormState, Point, Selection>(
                new ResizingSelectionState(_selectionManager.Object, _selection.RightEdge, new Point(10, 5)),
                new Point(5, 5), new Selection(new Rectangle(0, 0, 5, 10), _selection.ZIndex)));
            _stateVariations.Add(new Tuple<MainFormState, Point, Selection>(
                new ResizingSelectionState(_selectionManager.Object, _selection.BottomEdge, new Point(5, 10)),
                new Point(5, 5), new Selection(new Rectangle(0, 0, 10, 5), _selection.ZIndex)));
            _stateVariations.Add(new Tuple<MainFormState, Point, Selection>(
                new ResizingSelectionState(_selectionManager.Object, _selection.NWCorner, new Point(0, 0)),
                new Point(5, 5), new Selection(new Rectangle(5, 5, 5, 5), _selection.ZIndex)));
            _stateVariations.Add(new Tuple<MainFormState, Point, Selection>(
                new ResizingSelectionState(_selectionManager.Object, _selection.NECorner, new Point(10, 0)),
                new Point(5, 5), new Selection(new Rectangle(0, 5, 5, 5), _selection.ZIndex)));
            _stateVariations.Add(new Tuple<MainFormState, Point, Selection>(
                new ResizingSelectionState(_selectionManager.Object, _selection.SECorner, new Point(10, 10)),
                new Point(5, 5), new Selection(new Rectangle(0, 0, 5, 5), _selection.ZIndex)));
            _stateVariations.Add(new Tuple<MainFormState, Point, Selection>(
                new ResizingSelectionState(_selectionManager.Object, _selection.SWCorner, new Point(0, 10)),
                new Point(5, 5), new Selection(new Rectangle(5, 0, 5, 5), _selection.ZIndex)));
        }

        [TestMethod]
        public void test_StatusIsResizingSelection()
        {
            foreach (var s in _stateVariations)
            {
                Assert.AreEqual(MainFormStatus.ResizingSelection, s.Item1.Status);
            }            
        }

        [TestMethod]
        public void test_InitialActiveSelectionIsNotNull()
        {
            foreach (var s in _stateVariations)
            {
                Assert.IsNotNull(s.Item1.ActiveSelection);
            }            
        }

        [TestMethod]
        public void test_InitialComponentAtCursorPointIsNotNull()
        {
            foreach (var s in _stateVariations)
            {
                Assert.IsNotNull(s.Item1.ComponentAtCursorPoint);
            }
        }

        [TestMethod]
        public void test_DoesNotChangeStateOnMouseDown()
        {
            foreach (var s in _stateVariations)
            {
                var result = s.Item1.UpdateState(new Point(0,0), StateChangingTrigger.MouseDown);

                Assert.AreEqual(s.Item1, result);
            }            
        }

        [TestMethod]
        public void test_ResizesActiveSelectionOnMouseMove()
        {            
            foreach (var s in _stateVariations)
            {
                _selectionManager.Setup(x => x.ResizeSelection(s.Item1.ActiveSelection,
                    s.Item3.NWCorner.Coordinates,
                    new Size(s.Item3.TopEdge.Length, s.Item3.LeftEdge.Length)))
                    .Returns(s.Item3);

                var result = s.Item1.UpdateState(s.Item2, StateChangingTrigger.MouseMove);

                _selectionManager.Verify(x => x.ResizeSelection(s.Item1.ActiveSelection,
                    s.Item3.NWCorner.Coordinates,
                    new Size(s.Item3.TopEdge.Length, s.Item3.LeftEdge.Length)));
            }
        }
       
        [TestMethod]
        public void test_ReturnsOverEdgeOnMouseUp()
        {
            foreach (var s in _stateVariations)
            {
                var result = s.Item1.UpdateState(new Point(0, 0), StateChangingTrigger.MouseUp);

                Assert.AreEqual(MainFormStatus.OverEdge, result.Status);
            }
        }
    }
}
