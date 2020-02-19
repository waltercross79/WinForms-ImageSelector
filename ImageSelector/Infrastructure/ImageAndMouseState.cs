using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSelector.Infrastructure
{
    public class ImageAndMouseState
    {
        public Point MouseCoordinates { get; set; }

        public int ViewPortWidth { get; set; }

        public int ViewPortHeight { get; set; }

        public Size ImageSize { get; set; }

        public Size OverlaySize { get; set; }

        public string ImageSizeString => String.Format("({0}, {1})", ImageSize.Width, ImageSize.Height);

        public string OverlaySizeString => String.Format("({0}, {1})", OverlaySize.Width, OverlaySize.Height);

        public Point ImageLeftCorner { get; set; }

        public Point OverlayLeftCorner { get; set; }

        public string EventName { get; set; }
    }
}
