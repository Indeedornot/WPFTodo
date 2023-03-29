using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPFTodo.AttachedProp;

internal static class UIHelper
{
    /// <summary>
    /// Finds a Child of a given item in the visual tree. 
    /// </summary>
    /// <param name="parent">A direct parent of the queried item.</param>
    /// <typeparam name="T">The type of the queried item.</typeparam>
    /// <param name="childName">x:Name or Name of child. </param>
    /// <returns>The first parent item that matches the submitted type parameter. 
    /// If not matching item can be found, 
    /// a null parent is being returned.</returns>
    public static T? FindChild<T>(DependencyObject parent)
       where T : DependencyObject
    {
        // Confirm parent and childName are valid. 
        if (parent == null) return null;

        T? foundChild = null;

        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            // If the child is not of the request child type child
            T? childType = child as T;
            if (childType != null)
            {
                // child element found.
                foundChild = (T)child;
                break;
            }

            // recursively drill down the tree
            foundChild = FindChild<T>(child);

            // If the child is found, break so we do not overwrite the found child. 
            if (foundChild != null) break;

        }

        return foundChild;
    }

    internal static T? FindParent<T>(DependencyObject element) where T : FrameworkElement
    {
        FrameworkElement? parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
        if (parent == null) return null;

        if (parent is T correctlyTyped)
        {
            return correctlyTyped;
        }

        return FindParent<T>(parent);
    }
}
