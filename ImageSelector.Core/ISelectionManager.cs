/* Copyright (C) 2020 - Present 
 * Walter Cross - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the GPL v3.0 license.
 */

using System.Collections.Generic;
using System.Drawing;

namespace ImageSelector.Core
{
    public interface ISelectionManager
    {
        /// <summary>
        /// Adds selection to list of selections to track.
        /// </summary>
        /// <param name="s">Instance of <see cref="Selection"/> to track.</param>
        void AddSelection(Selection s);

        /// <summary>
        /// Creates new instance of <see cref="Selection"/> with given NW corner coordinates and size.
        /// Does not keep track of the instance.
        /// </summary>
        /// <param name="x">NW corner X coordinate.</param>
        /// <param name="y">NW corner Y coordinate.</param>
        /// <param name="width">Selection width.</param>
        /// <param name="height">Selection height.</param>
        /// <returns>Instance of <see cref="Selection"/> at given coordinates.</returns>
        Selection CreateSelection(int x, int y, int width, int height);
        
        /// <summary>
        /// Checks whether given point is on or near and edge, corner or selection interior.
        /// </summary>
        /// <param name="p">Instance of <see cref="Point"/> to evaluate.</param>
        /// <param name="buffer">Area around points and edges in which to search.</param>
        /// <returns>Edge, corner or selection interior, if point is on or near one. Null, if point not near anything.</returns>
        ISelectionComponent GetSelectionComponentNearPoint(Point p, int buffer);

        /// <summary>
        /// Resizes given selection and returns the updated selection.
        /// </summary>
        /// <param name="selectionToResize">Selection to resize.</param>
        /// <param name="location">Coordinates of the top left corner.</param>
        /// <param name="newSize">New width and height.</param>
        /// <returns>Resized selection.</returns>
        Selection ResizeSelection(Selection selectionToResize, Point location, Size newSize);

        /// <summary>
        /// Moves given selection to the new location.
        /// </summary>
        /// <param name="selectionToMove"></param>
        /// <param name="location">New coordinates of the top-left corner.</param>
        /// <returns>Moved selection.</returns>
        Selection MoveSelection(Selection selectionToMove, Point location);

        /// <summary>
        /// Get all selections tracked by the manager.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Selection> GetAll();
    }
}