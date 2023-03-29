using System;
using System.Windows;
using System.Windows.Input;

namespace WPFTodo.AttachedProp;

static class MouseSwipe
{
    #region Properties
    private static readonly DependencyProperty SubscribedCountProperty = DependencyProperty.RegisterAttached(
        "SubscribedCount",
        typeof(int),
        typeof(MouseSwipe),
        new PropertyMetadata(0));
    private static void SetSubscribedCount(DependencyObject element, int value)
    {
        element.SetValue(SubscribedCountProperty, value);
    }
    private static int GetSubscribedCount(DependencyObject element)
    {
        return (int)element.GetValue(SubscribedCountProperty);
    }

    private static readonly DependencyProperty StartPositionProperty = DependencyProperty.RegisterAttached(
        "StartPosition",
        typeof(Point?),
        typeof(MouseSwipe),
        new PropertyMetadata(null));
    private static void SetStartPosition(DependencyObject element, Point? value)
    {
        element.SetValue(StartPositionProperty, value);
    }
    private static Point? GetStartPosition(DependencyObject element)
    {
        return (Point?)element.GetValue(StartPositionProperty);
    }

    public static readonly DependencyProperty SensitivityProperty = DependencyProperty.RegisterAttached(
        "Sensitivity",
        typeof(float),
        typeof(MouseSwipe),
        new PropertyMetadata(1f));
    public static void SetSensitivity(DependencyObject element, float value)
    {
        element.SetValue(SensitivityProperty, value);
    }
    public static float GetSensitivity(DependencyObject element)
    {
        return (float)element.GetValue(SensitivityProperty);
    }

    public static readonly DependencyProperty LeftCommandProperty = DependencyProperty.RegisterAttached(
        "LeftCommand",
        typeof(ICommand),
        typeof(MouseSwipe),
        new PropertyMetadata(default(ICommand), OnCommandChanged));
    public static void SetLeftCommand(DependencyObject element, ICommand value)
    {
        element.SetValue(LeftCommandProperty, value);
    }
    public static ICommand GetLeftCommand(DependencyObject element)
    {
        return (ICommand)element.GetValue(LeftCommandProperty);
    }

    public static readonly DependencyProperty RightCommandProperty = DependencyProperty.RegisterAttached(
        "RightCommand",
        typeof(ICommand),
        typeof(MouseSwipe),
        new PropertyMetadata(default(ICommand), OnCommandChanged));
    public static void SetRightCommand(DependencyObject element, ICommand value)
    {
        element.SetValue(RightCommandProperty, value);
    }
    public static ICommand GetRightCommand(DependencyObject element)
    {
        return (ICommand)element.GetValue(RightCommandProperty);
    }

    public static readonly DependencyProperty UpCommandProperty = DependencyProperty.RegisterAttached(
        "UpCommand",
        typeof(ICommand),
        typeof(MouseSwipe),
        new PropertyMetadata(default(ICommand), OnCommandChanged));
    public static void SetUpCommand(DependencyObject element, ICommand value)
    {
        element.SetValue(UpCommandProperty, value);
    }
    public static ICommand GetUpCommand(DependencyObject element)
    {
        return (ICommand)element.GetValue(UpCommandProperty);
    }

    public static readonly DependencyProperty DownCommandProperty = DependencyProperty.RegisterAttached(
        "DownCommand",
        typeof(ICommand),
        typeof(MouseSwipe),
        new PropertyMetadata(default(ICommand), OnCommandChanged));
    public static void SetDownCommand(DependencyObject element, ICommand value)
    {
        element.SetValue(DownCommandProperty, value);
    }
    public static ICommand GetDownCommand(DependencyObject element)
    {
        return (ICommand)element.GetValue(DownCommandProperty);
    }
    #endregion

    #region Event Handlers
    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var element = d as UIElement;
        if (element == null) return;

        bool IsNewCommand = e.NewValue is ICommand;
        bool IsOldCommand = e.OldValue is ICommand;

        //nothing changed
        if (IsNewCommand == IsOldCommand) return;

        int currSubscribed = GetSubscribedCount(element);

        //When command is changed to null
        if (!IsNewCommand && IsOldCommand)
        {
            SetSubscribedCount(element, currSubscribed - 1);

            //if no more listeners - unsubscribe
            if (currSubscribed - 1 == 0)
            {
                element.PreviewMouseLeftButtonDown -= OnPreviewMouseButtonDown;
                element.PreviewMouseLeftButtonUp -= OnPreviewMouseButtonUp;
                element.MouseLeave -= OnMouseLeave;
            }

            return;
        }

        //New command added
        SetSubscribedCount(element, currSubscribed + 1);

        //events have been already added
        if (currSubscribed != 0) return;

        element.PreviewMouseLeftButtonDown += OnPreviewMouseButtonDown;
        element.PreviewMouseLeftButtonUp += OnPreviewMouseButtonUp;
        element.MouseLeave += OnMouseLeave;
    }

    private static void OnMouseLeave(object sender, MouseEventArgs e)
    {
        var element = sender as UIElement;
        if (element == null) return;

        SetStartPosition(element, null);
    }

    private static void OnPreviewMouseButtonDown(object sender, MouseEventArgs e)
    {
        var element = sender as UIElement;
        if (element == null) return;

        Point start = e.GetPosition(element);
        SetStartPosition(element, start);
    }

    private static void OnPreviewMouseButtonUp(object sender, MouseEventArgs e)
    {
        var element = sender as UIElement;
        if (element == null) return;

        Point? startPosition = GetStartPosition(element);
        if (startPosition == null) return;
        Point start = startPosition.Value;

        Point end = e.GetPosition(element);
        float sensitivity = GetSensitivity(element);

        Vector travel = end - start;
        if (travel.Length < sensitivity) return;

        if (travel.X < -sensitivity)
        {
            GetLeftCommand(element)?.Execute(null);
        }
        else if (travel.X > sensitivity)
        {
            GetRightCommand(element)?.Execute(null);
        }

        if (travel.Y < -sensitivity)
        {
            GetUpCommand(element)?.Execute(null);
        }
        else if (travel.Y > sensitivity)
        {
            GetDownCommand(element)?.Execute(null);
        }

        SetStartPosition(element, null);
    }

    #endregion
}
