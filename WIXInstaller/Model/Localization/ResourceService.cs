using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WIXInstaller.Model
{
    class ResourceService
    {
        public static void ReplaceWindowResource(Window view, ResourceDictionary oldDict, ResourceDictionary dict)
        {
            int ind = view.Resources.MergedDictionaries.IndexOf(oldDict);
            view.Resources.MergedDictionaries.Remove(oldDict);
            view.Resources.MergedDictionaries.Insert(ind, dict);
        }
        public static void AddWindowResource(Window view, ResourceDictionary dict)
        {
            view.Resources.MergedDictionaries.Add(dict);
        }

        public static void ReplacePageResource(Page view, ResourceDictionary oldDict, ResourceDictionary dict)
        {
            int ind = view.Resources.MergedDictionaries.IndexOf(oldDict);
            view.Resources.MergedDictionaries.Remove(oldDict);
            view.Resources.MergedDictionaries.Insert(ind, dict);
        }
        public static void AddPageResource(Page view, ResourceDictionary dict)
        {
            view.Resources.MergedDictionaries.Add(dict);
        }

        public static void ReplaceAllPageResource(ICollection<object> views, ResourceDictionary oldDict, ResourceDictionary dict)
        {
            foreach (var view in views)
            {
                ReplacePageResource((Page)view, oldDict, dict);
            }
        }
        public static void AddAllPageResource(ICollection<object> views, ResourceDictionary dict)
        {
            foreach (var view in views)
            {
                AddPageResource((Page)view, dict);
            }
        }
    }
}