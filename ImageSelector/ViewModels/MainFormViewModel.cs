/* Copyright (C) 2020 - Present 
 * Walter Cross - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the GPL v3.0 license.
 */

using ImageSelector.Core;
using ImageSelector.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ImageSelector.ViewModels
{
    public class MainFormViewModel
    {
        private readonly SelectionManager _selectionManager;        
        private MainFormState _currentState;
        private ImageInfo _image;
        private string _exportLocation;

        public MainFormViewModel(SelectionManager selectionManager)
        {
            _selectionManager = selectionManager;
            _currentState = new IdleState(selectionManager, new Point());
            _exportLocation = ConfigurationManager.AppSettings[Constants.ExportLocationAppSetting];            
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
                RaiseStateChange();
            }
        }

        private void RaiseStateChange()
        {
            EventHandler<CustomEventArgs<MainFormStatus>> temp = Volatile.Read(ref StateChange);
            if (temp != null) temp(this, new CustomEventArgs<MainFormStatus>(Status));
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

        internal void ExtractImagesWithTag(string data)
        {
            Guid batchID = Guid.NewGuid();

            foreach (var s in _selectionManager.GetAll())
            {
                SaveSelection(s, GenerateFileName(batchID, data, s.ZIndex));
            }
            ResetSelections();
        }

        private void ResetSelections()
        {

            // Clear selections.
            _selectionManager.Clear();
            _currentState = new IdleState(_selectionManager, new Point(0, 0));
            RaiseStateChange();
        }

        private string GenerateFileName(Guid batchID, string data, int zIndex)
        {
            return String.Format(@"{0}\{1}\{2}_{3}.jpg", _exportLocation, batchID, data, GetSequenceNo(zIndex));
        }

        private string GetSequenceNo(int zindex)
        {
            return String.Format("{0}{1}", zindex < 10 ? "000" : (zindex < 100 ? "00" : (zindex < 1000 ? "0" : "")), zindex);
        }

        private void SaveSelection(Selection s, string fileName)
        {
            // Clone section of the original image in given location/rectangle.
            Bitmap selected = _image.Image.Clone(s.LocationAndSize, _image.Image.PixelFormat);

            string directory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Save copy of the section to a new file.            
            selected.Save(fileName, ImageFormat.Jpeg);
        }

        internal ImageInfo LoadImage(string path)
        {
            ResetSelections();

            _image = new ImageInfo(path);
            return _image;
        }

        internal void DeleteActiveSelection()
        {
            _selectionManager.RemoveSelection(_currentState.ActiveSelection);
            _currentState = new IdleState(_selectionManager, new Point());
        }
    }
}
