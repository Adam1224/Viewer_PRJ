// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace PhotoViewerDemo
{
    //brief: This class describes a single photo - 
    //its location, the image and the metadata extracted from the image.

    public class Photo
    {
        private readonly Uri _source;

        public Photo(string path)
        {
            Source = path;
            _source = new Uri(path, UriKind.Absolute);
            Image = BitmapFrame.Create(_source);

            Metadata = new ExifMetadata(_source);

            PhotoHeight = Image.PixelHeight;
            PhotoWidth = Image.PixelWidth;
            PhotoDpiX = (int)Image.DpiX;
            PhotoDpiY = (int)Image.DpiY;

            title = Path.GetFileName(path);

            var fil = new FileInfo(_source.AbsolutePath);
            size = (int)(fil.Length/1024);
            lastWriteTime = fil.LastWriteTime;

            PhotoMetadata = Image.Metadata as BitmapMetadata;

            PixelWidth = Image.PixelWidth;
            IsAnnotated = false;
        }


        public string title { get; set; }
        public string Source { get; }
        public BitmapFrame Image { get; set; }
        public ExifMetadata Metadata { get; set; }
        public BitmapSource ImageSource { get; set; }
        public BitmapMetadata PhotoMetadata { get;}
        public DateTime lastWriteTime { get; set; }
        public bool IsAnnotated { get; set; }
        public string Category { get; set; }

        public int PhotoWidth { get; set; }
        public int PhotoHeight { get; set; }

        public int PhotoDpiX { get; }
        public int PhotoDpiY { get; }
        public string format { get; set; }

        public int size { get; set; }

        public Int32 PixelWidth { get; set; }




        public override string ToString() => _source.ToString();
    }
}