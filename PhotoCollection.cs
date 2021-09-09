// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace PhotoViewerDemo
{
    //brief: This class represents a collection of photos
    //in a specified directory.
    public class PhotoCollection : ObservableCollection<Photo>
    {
        private DirectoryInfo _directory;

        //brief: Constructor of PhotoCollection class
        public PhotoCollection()
        {
        }

        //brief: Constructor of PhotoCollection class
        public PhotoCollection(string path) : this(new DirectoryInfo(path))
        {
        }

        //brief: Constructor of PhotoCollection class
        //param: Directory of images to be displayed in DisplayArea
        public PhotoCollection(DirectoryInfo directory)
        {
            _directory = directory;
            Update();
        }

        public string Path
        {
            set
            {
                _directory = new DirectoryInfo(value);
                Update();
            }
            get { return _directory.FullName; }
        }

        public DirectoryInfo Directory
        {
            set
            {
                _directory = value;
                Update();
            }
            get { return _directory; }
        }

        //brief: Update images in DisplayArea
        //param: None
        private void Update()
        {
            Clear();
            try
            {
                foreach (var f in _directory.GetFiles("*.png"))
                    Add(new Photo(f.FullName));
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("No Such Directory");
            }

            
        }
    }
}