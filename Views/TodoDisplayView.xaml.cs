using System;
using System.Windows;
using System.Windows.Controls;

using WPFTodo.Commands;
using WPFTodo.Models;

namespace WPFTodo.Views;

/// <summary>
/// Interaction logic for TodoDisplay.xaml
/// </summary>
public partial class TodoDisplayView : UserControl
{
    public TodoDisplayView()
    {
        InitializeComponent();
    }

    public double TitleFontSize
    {
        get => (double)GetValue(TitleFontSizeProperty);
        set => SetValue(TitleFontSizeProperty, value);
    }

    // Using a DependencyProperty as the backing store for TitleFontSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleFontSizeProperty =
        DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(TodoDisplayView), new PropertyMetadata(default(double)));



    public double DescriptionFontSize
    {
        get => (double)GetValue(DescriptionFontSizeProperty);
        set => SetValue(DescriptionFontSizeProperty, value);
    }

    // Using a DependencyProperty as the backing store for DescriptionFontSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DescriptionFontSizeProperty =
        DependencyProperty.Register(nameof(DescriptionFontSize), typeof(double), typeof(TodoDisplayView), new PropertyMetadata(default(double)));


}
