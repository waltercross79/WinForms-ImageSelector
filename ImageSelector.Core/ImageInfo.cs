using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageSelector.Core
{
    public class ImageInfo
    {
        private string _path;
        private readonly Bitmap _image;
        private readonly int _width;
        private readonly int _height;

        /// <summary>
        /// Creates new instance of the class with binary image data and metadata.
        /// </summary>
        /// <param name="path">File system path where the image file is loaded from.</param>
        public ImageInfo(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Image file does not exist.");

            _path = path;
            _image = new Bitmap(new FileStream(path, FileMode.Open));
            _width = _image.Width;
            _height = _image.Height;
            ID = Guid.NewGuid();
        }

        /// <summary>
        /// Creates new instance of the class with binary image data and metadata.
        /// </summary>
        /// <param name="image">Image to represent by the info class.</param>
        public ImageInfo(Guid id, Bitmap image)
        {
            _image = image;
            _width = image.Width;
            _height = image.Height;
            ID = id;
        }

        /// <summary>
        /// Image object that can be drawn using Graphics object.
        /// </summary>
        public Bitmap Image
        {
            get
            {
                return _image; 
            }
        }

        /// <summary>
        /// File/image name.
        /// </summary>
        public string Name => Path.GetFileNameWithoutExtension(_path);

        /// <summary>
        /// Original size of the image.
        /// </summary>
        public Size Size => new Size(_width, _height);

        public Guid ID { get; set; }
    }
}
