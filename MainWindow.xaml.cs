// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.ComponentModel;

namespace PhotoViewerDemo
{
    public sealed partial class MainWindow : Window
    {
        public PhotoCollection Photos;

        private CollectionViewSource _listingDataView;
        //private readonly CollectionViewSource _listingDataView;

        public static int SelectedPhotoIndex = 0;

        public string currentDir
        {
            get
            {
                return ImagesRootDir.Text;
            }

            set
            {
                ImagesRootDir.Text = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _listingDataView = (CollectionViewSource)(Resources["ListingDataView"]);

            Photos = (PhotoCollection)(Application.Current.Resources["Photos"] as ObjectDataProvider)?.Data;
            Photos.Path = (Environment.CurrentDirectory + "\\Images");

            ViewColumnLoad(Environment.CurrentDirectory + "\\Images");

            //brief: load infocolumn at the buttom of DisplayArea
            //fileInfo.AutoGeneratingColumn += fileInfoColumn_Load;
        }

        private void Ratio_ComboBox_Close(object sender, EventArgs e)
        {
            if (Ratio_ComboBox.SelectedIndex == 0)
            {
                PhotosListBox.SetResourceReference(ListBox.ItemTemplateProperty, "NoneTemplate");
            }
            else if (Ratio_ComboBox.SelectedIndex == 1)
            {
                PhotosListBox.SetResourceReference(ListBox.ItemTemplateProperty, "FillTemplate");
            }

        }

        private void AddWidthSorting(object sender, RoutedEventArgs args)
        {
            _listingDataView.SortDescriptions.Add(
                new SortDescription("PixelWidth", ListSortDirection.Ascending));
        }

        private void AddSizeSorting(object sender, RoutedEventArgs args)
        {
            _listingDataView.SortDescriptions.Add(
                new SortDescription("size", ListSortDirection.Descending));
        }

        private void RemoveSorting(object sender, RoutedEventArgs args)
        {
            _listingDataView.SortDescriptions.Clear();
        }

        private void OnPhotoDoubleClick(object sender, RoutedEventArgs e)
        {
            var pvWindow = new PhotoViewer { SelectedPhoto = (Photo)PhotosListBox.SelectedItem };
            pvWindow.Show();
        }

        private void EditPhoto(object sender, RoutedEventArgs e)
        {
            var pvWindow = new PhotoViewer { SelectedPhoto = (Photo)PhotosListBox.SelectedItem };
            pvWindow.Show();
        }

        //brief: update images in DisplayArea with the dir in the RootDir textbox
        private void Browse(object sender, RoutedEventArgs e)
        {
            Photos.Path = ImagesRootDir.Text;
            ViewColumnLoad(Photos.Path);
            PhotCurrentDir.Text = ImagesRootDir.Text;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo upperDir = new DirectoryInfo(PhotCurrentDir.Text);

            upperDir = upperDir.Parent;

            PhotCurrentDir.Text = upperDir.FullName;

            Photos.Path = PhotCurrentDir.Text;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo upperDir = new DirectoryInfo(PhotCurrentDir.Text);

            PhotCurrentDir.Text = upperDir.FullName;

            Photos.Path = PhotCurrentDir.Text;
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (directoryTreeView.SelectedItem != null)
            {
                string tempRoute = PhotCurrentDir.Text;

                tempRoute += "\\" + (directoryTreeView.SelectedItem as DirectoryRecord).Info.ToString();

                if (Directory.Exists(tempRoute))
                {
                    PhotCurrentDir.Text = tempRoute;

                    Photos.Path = PhotCurrentDir.Text;
                }
                else
                {
                    MessageBox.Show("Error! Please select subfolder at next level first");
                }
            }
            else
            {
                MessageBox.Show("Error!  Please select subfolder at next level first");
            }
        }

        //brief: Initialization operation at viewer PRJ start-up 
        //set images' root directory
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //ImagesDir.Text = Environment.CurrentDirectory + "\\images";
            ImagesRootDir.Text = Environment.CurrentDirectory + "\\Images";
            PhotCurrentDir.Text = ImagesRootDir.Text;
        }

        //brief: load folder tree in ViewColumn
        //param: Root Dir of images to display in DisplayArea
        private void ViewColumnLoad(string Path)
        {
            var directory = new ObservableCollection<DirectoryRecord>();

            foreach (var drive in DriveInfo.GetDrives())
            {
                directory.Add(
                    new DirectoryRecord
                    {
                        ///Info = new DirectoryInfo(drive.RootDirectory.FullName)
                        Info = new DirectoryInfo(Path)
                    }

                    );
            }

            directoryTreeView.ItemsSource = directory;

            currentDir = Path;
        }

        private void fileInfoColumn_Load(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            List<String> requiredProperties = new List<String> { "Name", "Length", "FullName", "LastWriteTime","Size" };

            if (!requiredProperties.Contains(e.PropertyName))
            {
                e.Cancel = true;
            }
            else
            {
                e.Column.Header = e.Column.Header.ToString();
            }

        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void PhotosListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PhotosListBox.SelectedItem != null)
            {
                Photo SelectedPhoto = new Photo(PhotosListBox.SelectedItem.ToString());

                SelectedPhotoSource.Content = SelectedPhoto.Source;
                SelectedPhotoWidth.Content = SelectedPhoto.PhotoWidth;
                SelectedPhotoHeight.Content = SelectedPhoto.PhotoHeight;
                SelectedPhotoDpiX.Content = SelectedPhoto.PhotoDpiX;
                SelectedPhotoTitle.Content = SelectedPhoto.title;
                SelectedPhotoSize.Content = SelectedPhoto.size;
                SelectedPhotoLastWriteTime.Content = SelectedPhoto.lastWriteTime;
                SelectedPhotoIsAnnotated.Content = SelectedPhoto.IsAnnotated;


                SelectedPhotoIndex = PhotosListBox.SelectedIndex;

            }
        }

        private void InforPanelCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            Infopanel.Visibility = Visibility.Visible;
        }

        private void InforPanelCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Infopanel.Visibility = Visibility.Collapsed;
        }

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            PhotosListBox.Focus();

            if (SelectedPhotoIndex == PhotosListBox.SelectedIndex)
            {
                PhotosListBox.SelectedIndex = SelectedPhotoIndex + 1;
                
            }
            else
            {
                PhotosListBox.SelectedIndex = SelectedPhotoIndex;
            }

            SelectedPhotoIndex++;
        }

        private void RBC_btn_Click(object sender, RoutedEventArgs e)
        {
            bool IsCanceled = false;

            if (PhotosListBox.SelectedItem != null)
            {
                DirectoryInfo currentDir = new DirectoryInfo(PhotCurrentDir.Text);
                DirectoryInfo upperDir = currentDir.Parent;
                string destinationDir = upperDir.FullName.ToString() + "\\" + "RBC";

                if(!(System.IO.Directory.Exists(destinationDir)))
                {
                    MessageBoxResult DestinationFolderCreationDecision =
                        MessageBox.Show("NOT exist, Press 'OK' to creat.", destinationDir.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    switch(DestinationFolderCreationDecision)    
                    {
                        case MessageBoxResult.Yes:
                            System.IO.Directory.CreateDirectory(destinationDir);
                            break;
                        case MessageBoxResult.No:
                            MessageBox.Show("New folder Creation Canceled, Annotation Canceled");
                            IsCanceled = true;
                            break;
                    }
                }

                if (!IsCanceled)
                {
                    if (PhotosListBox.SelectedItems.Count == 1)
                    {
                        Photo annotatingPhoto = (Photo)PhotosListBox.SelectedItem;

                        string sourceFile = annotatingPhoto.Source;

                        string title = annotatingPhoto.title;
                        string destFile = System.IO.Path.Combine(destinationDir, title);
                        System.IO.File.Copy(sourceFile, destFile, true);
                        annotatingPhoto.IsAnnotated = true;
                        annotatingPhoto.Category = "RBC";
                        NextImageButton_Click(sender, e);
                    }
                    else
                    {
                        foreach (var SingleItem in PhotosListBox.SelectedItems)
                        {
                            Photo SinglePhoto = (Photo)SingleItem;
                            string sourceFile = SinglePhoto.Source;
                            string title = SinglePhoto.title;
                            string destFile = System.IO.Path.Combine(destinationDir, title);
                            System.IO.File.Copy(sourceFile, destFile, true);
                            SinglePhoto.IsAnnotated = true;
                            SinglePhoto.Category = "RBC";
                        }
                        MessageBox.Show("Multi Annotatin Finished");
                    }
                }
                
            }
        }
    }

    public class TESTDataTemplateSelector:DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var fe = container as FrameworkElement;
            var obj = item as Photo;

            DataTemplate dt = null;

            if (obj != null && fe != null)
            {
                if (obj.IsAnnotated == true)
                {
                    dt = fe.FindResource("NoneTemplate") as DataTemplate;
                }
                else
                {
                    dt = fe.FindResource("FillTemplate") as DataTemplate;
                }
            }
            return dt;
        }
    }
}