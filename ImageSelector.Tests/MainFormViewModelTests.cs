/* Copyright (C) 2020 - Present 
 * Walter Cross - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the GPL v3.0 license.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using ImageSelector.Core;
using ImageSelector.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImageSelector.Tests
{
    [TestClass]
    public class MainFormViewModelTests
    {
        private MainFormViewModel _model;
        private Mock<MainFormState> _formState;

        [TestInitialize]
        public void Initialize()
        {
            _model = new MainFormViewModel(new SelectionManager());            
        }

        [TestMethod]
        public void test_MouseDownWhenIdleChangesStateToSelecting()
        {
            _model.MouseDown(new System.Drawing.Point(0, 0), MouseButtons.Left);

            Assert.AreEqual(MainFormStatus.Selecting, _model.Status);
        }

        [TestMethod]
        public void test_MouseDownSendsTheEventToMainFormState()
        {
            _formState = new Mock<MainFormState>();
            _formState.Setup(x => x.UpdateState(It.IsAny<Point>(), It.IsAny<StateChangingTrigger>()));

            Type t = typeof(MainFormViewModel);
            var field = t.GetField("_currentState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(_model, _formState.Object);

            Point p = new Point(1, 1);

            _model.MouseDown(p, MouseButtons.Left);
            _formState.Verify(x => x.UpdateState(p, StateChangingTrigger.MouseDown));
        }

        [TestMethod]
        public void test_MouseUpSendsTheEventToMainFormState()
        {
            _formState = new Mock<MainFormState>();
            _formState.Setup(x => x.UpdateState(It.IsAny<Point>(), It.IsAny<StateChangingTrigger>()));

            Type t = typeof(MainFormViewModel);
            var field = t.GetField("_currentState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(_model, _formState.Object);

            Point p = new Point(1, 1);

            _model.MouseUp(p, MouseButtons.Left);
            _formState.Verify(x => x.UpdateState(p, StateChangingTrigger.MouseUp));
        }

        [TestMethod]
        public void test_MouseMoveSendsTheEventToMainFormState()
        {
            _formState = new Mock<MainFormState>();
            _formState.Setup(x => x.UpdateState(It.IsAny<Point>(), It.IsAny<StateChangingTrigger>()));

            Type t = typeof(MainFormViewModel);
            var field = t.GetField("_currentState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(_model, _formState.Object);

            Point p = new Point(1, 1);

            _model.MouseMove(p);
            _formState.Verify(x => x.UpdateState(p, StateChangingTrigger.MouseMove));
        }
    }
}
