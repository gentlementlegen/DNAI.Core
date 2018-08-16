// NOTES
// https://www.codeproject.com/Articles/167873/A-WPF-File-ListView-and-ComboBox
// https://stackoverflow.com/questions/15104986/how-to-list-files-in-directory-c-sharp-wpf
// https://stackoverflow.com/questions/6415037/populate-treeview-from-list-of-file-paths-in-wpf
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace DNAIPluginPublisher.Model
{
    public class Item
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsSelected { get; set; }
        public ImageSource Icon { get; set; }
    }

    public class FileItem : Item
    {
    }

    public class DirectoryItem : Item
    {
        public List<Item> Items { get; set; }

        public DirectoryItem()
        {
            Items = new List<Item>();
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

        private List<Item> GetItemsInternal(string path)
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
                    Items = GetItemsInternal(directory.FullName)
                };

                items.Add(item);
            }

            foreach (var file in dirInfo.GetFiles())
            {
                var item = new FileItem
                {
                    Name = file.Name,
                    Path = file.FullName
                };
                if (!GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
                    item.Icon = ToImageSource(Icon.ExtractAssociatedIcon(file.FullName));

                items.Add(item);
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