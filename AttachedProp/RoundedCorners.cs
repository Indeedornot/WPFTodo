using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFTodo.AttachedProp;
/// <summary>
/// This class is used to add a corner radius to a control.
/// <br/> Requires control to have a border as a child, otherwise it will not work.
/// <br/> Does not support visibility changes!
/// </summary>
public static class RoundedCorners {
    #region Radius
    public static readonly DependencyProperty RadiusProperty = DependencyProperty.RegisterAttached(
        "Radius",
        typeof(CornerRadius),
        typeof(RoundedCorners),
        new PropertyMetadata(default(CornerRadius), RadiusChanged));

    public static void SetRadius(DependencyObject element, CornerRadius value) {
        element.SetValue(RadiusProperty, value);
    }

    public static CornerRadius GetRadius(DependencyObject element) {
        return (CornerRadius)element.GetValue(RadiusProperty);
    }

    public static void RadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        Border? border = d as Border;
        if (border == null) {
            border = UIHelper.FindChild<Border>(d);
        }

        if (border == null) {
            InitialRadiusSet(d, e);
            return;
        }

        border.CornerRadius = (CornerRadius)e.NewValue;
    }

    private static void InitialRadiusSet(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var element = d as Control;
        if (element == null) return;

        element.Loaded -= LoadedHandler;
        element.IsVisibleChanged -= IsVisibleChangedHandler;

        if (!element.IsLoaded) {
            element.Loaded += LoadedHandler;
        }
        else {
            element.IsVisibleChanged += IsVisibleChangedHandler;
        }
    }

    private static void LoadedHandler(object sender, RoutedEventArgs e) {
        var element = sender as Control;
        if (element == null) return;

        //Pass it off to visibility handler
        if (!element.IsVisible) {
            element.IsVisibleChanged -= IsVisibleChangedHandler;
            element.IsVisibleChanged += IsVisibleChangedHandler;
            return;
        }

        //Ensure children are loaded
        element.UpdateLayout();

        var border = UIHelper.FindChild<Border>(element);
        if (border == null) return;

        var radius = GetRadius(element);
        border.CornerRadius = radius;
    }
    private static void IsVisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e) {
        var element = sender as Control;
        if (element == null) return;

        //Ensure children are loaded
        element.UpdateLayout();

        var border = UIHelper.FindChild<Border>(element);
        if (border == null) return;

        var radius = GetRadius(element);
        border.CornerRadius = radius;
    }

    public static readonly DependencyProperty TopLeftProperty = DependencyProperty.RegisterAttached(
      "TopLeft",
      typeof(double),
      typeof(RoundedCorners),
      new PropertyMetadata(default(double), TopLeftChanged));

    public static void SetTopLeft(DependencyObject element, double value) {
        element.SetValue(TopLeftProperty, value);
    }

    public static double GetTopLeft(DependencyObject element) {
        return (double)element.GetValue(TopLeftProperty);
    }

    private static void TopLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var cornerRadius = GetRadius(d);
        cornerRadius.TopLeft = (double)e.NewValue;
        SetRadius(d, cornerRadius);
    }
    #endregion

    #region TopRight
    public static readonly DependencyProperty TopRightProperty = DependencyProperty.RegisterAttached(
        "TopRight",
        typeof(double),
        typeof(RoundedCorners),
        new PropertyMetadata(default(double), TopRightChanged));

    public static void SetTopRight(DependencyObject element, double value) {
        element.SetValue(TopRightProperty, value);
    }

    public static double GetTopRight(DependencyObject element) {
        return (double)element.GetValue(TopRightProperty);
    }

    private static void TopRightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var cornerRadius = GetRadius(d);
        cornerRadius.TopRight = (double)e.NewValue;
        SetRadius(d, cornerRadius);
    }
    #endregion

    #region BottomLeft
    public static readonly DependencyProperty BottomLeftProperty = DependencyProperty.RegisterAttached(
        "BottomLeft",
        typeof(double),
        typeof(RoundedCorners),
        new PropertyMetadata(default(double), BottomLeftChanged));

    public static void SetBottomLeft(DependencyObject element, double value) {
        element.SetValue(BottomLeftProperty, value);
    }

    public static double GetBottomLeft(DependencyObject element) {
        return (double)element.GetValue(BottomLeftProperty);
    }

    private static void BottomLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var cornerRadius = GetRadius(d);
        cornerRadius.BottomLeft = (double)e.NewValue;
        SetRadius(d, cornerRadius);
    }
    #endregion

    #region BottomRight
    public static readonly DependencyProperty BottomRightProperty = DependencyProperty.RegisterAttached(
        "BottomRight",
        typeof(double),
        typeof(RoundedCorners),
        new PropertyMetadata(default(double), BottomRightChanged));

    public static void SetBottomRight(DependencyObject element, double value) {
        element.SetValue(BottomRightProperty, value);
    }

    public static double GetBottomRight(DependencyObject element) {
        return (double)element.GetValue(BottomRightProperty);
    }

    private static void BottomRightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var cornerRadius = GetRadius(d);
        cornerRadius.BottomRight = (double)e.NewValue;
        SetRadius(d, cornerRadius);
    }
    #endregion

    #region Left
    public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached(
        "Left",
        typeof(double),
        typeof(RoundedCorners),
        new PropertyMetadata(default(double), LeftChanged));

    public static void SetLeft(DependencyObject element, double value) {
        element.SetValue(LeftProperty, value);
    }

    public static double GetLeft(DependencyObject element) {
        return (double)element.GetValue(LeftProperty);
    }

    private static void LeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var cornerRadius = GetRadius(d);
        cornerRadius.TopLeft = (double)e.NewValue;
        cornerRadius.BottomLeft = (double)e.NewValue;
        SetRadius(d, cornerRadius);
    }
    #endregion

    #region Right
    public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached(
        "Right",
        typeof(double),
        typeof(RoundedCorners),
        new PropertyMetadata(default(double), RightChanged));

    public static void SetRight(DependencyObject element, double value) {
        element.SetValue(RightProperty, value);
    }

    public static double GetRight(DependencyObject element) {
        return (double)element.GetValue(RightProperty);
    }

    private static void RightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var cornerRadius = GetRadius(d);
        cornerRadius.TopRight = (double)e.NewValue;
        cornerRadius.BottomRight = (double)e.NewValue;
        SetRadius(d, cornerRadius);
    }
    #endregion

    #region Top
    public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached(
        "Top",
        typeof(double),
        typeof(RoundedCorners),
        new PropertyMetadata(default(double), TopChanged));

    public static void SetTop(DependencyObject element, double value) {
        element.SetValue(TopProperty, value);
    }

    public static double GetTop(DependencyObject element) {
        return (double)element.GetValue(TopProperty);
    }

    private static void TopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var cornerRadius = GetRadius(d);
        cornerRadius.TopLeft = (double)e.NewValue;
        cornerRadius.TopRight = (double)e.NewValue;
        SetRadius(d, cornerRadius);
    }
    #endregion

    #region Bottom
    public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached(
        "Bottom",
        typeof(double),
        typeof(RoundedCorners),
        new PropertyMetadata(default(double), BottomChanged));

    public static void SetBottom(DependencyObject element, double value) {

        element.SetValue(BottomProperty, value);
    }

    public static double GetBottom(UIElement element) {
        return (double)element.GetValue(BottomProperty);
    }

    private static void BottomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var cornerRadius = GetRadius(d);
        cornerRadius.BottomLeft = (double)e.NewValue;
        cornerRadius.BottomRight = (double)e.NewValue;
        SetRadius(d, cornerRadius);
    }
    #endregion
}
