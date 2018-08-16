// NOTES
// https://www.codeproject.com/Articles/167873/A-WPF-File-ListView-and-ComboBox
// https://stackoverflow.com/questions/15104986/how-to-list-files-in-directory-c-sharp-wpf
// https://stackoverflow.com/questions/6415037/populate-treeview-from-list-of-file-paths-in-wpf
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DNAIPluginPublisher.Model
{
    public class Item : ObservableObject
    {
        protected bool _isSelected;

        public string Name { get; set; }
        public string Path { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                Set(ref _isSelected, value);
                if (Parent != null && value == true) Parent.IsSelected = value;
            }
        }

        public ImageSource Icon { get; set; }

        public Item Parent { get; set; }
    }

    public class FileItem : Item
    {
        public new bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                Set(ref _isSelected, value);

                if (Parent != null && value == true) Parent.IsSelected = value;
            }
        }
    }

    public class DirectoryItem : Item
    {
        public List<Item> Items { get; set; }

        public DirectoryItem()
        {
            Items = new List<Item>();
        }

        public new bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                Set(ref _isSelected, value);

                if (value == true && Parent != null) Parent.IsSelected = value;

                foreach (var item in Items)
                {
                    if (item is DirectoryItem d)
                        d.IsSelected = value;
                    else
                        item.IsSelected = value;
                }
            }
        }
    }

    public class ItemProvider
    {
        private readonly ObservableCollection<Item> _items = new ObservableCollection<Item>();

        public IReadOnlyList<Item> Items => _items;

        public void GetItems(string path)
        {
            _items.Clear();
            foreach (var item in GetItemsInternal(path))
            {
                _items.Add(item);
            }
        }

        private List<Item> GetItemsInternal(string path, Item parent = null)
        {
            var items = new List<Item>();

            var dirInfo = new DirectoryInfo(path);

            foreach (var directory in dirInfo.GetDirectories())
            {
                var item = new DirectoryItem
                {
                    Name = directory.Name,
                    Path = directory.FullName,
                    Icon = null,
                    Parent = parent
                };

                item.Items = GetItemsInternal(directory.FullName, item);

                items.Add(item);
            }

            foreach (var file in dirInfo.GetFiles())
            {
                if (file.Extension != ".meta")
                {
                    var item = new FileItem
                    {
                        Name = file.Name,
                        Path = file.FullName,
                        Parent = parent
                    };
                    if (!GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
                        item.Icon = ToImageSource(Icon.ExtractAssociatedIcon(file.FullName));

                    items.Add(item);
                }
            }

            return items;
        }

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        private ImageSource ToImageSource(Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }
    }
}