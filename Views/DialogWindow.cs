using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace kcsj.Views;

public sealed class DialogWindow : Window
{
    private DialogWindow(string title, string message, bool showCancel)
    {
        Title = title;
        CanResize = false;
        SizeToContent = SizeToContent.WidthAndHeight;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        MinWidth = 360;
        MaxWidth = 560;

        TextBlock messageText = new()
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap,
            MaxWidth = 500,
            FontSize = 15
        };

        Button confirmButton = new()
        {
            Content = "确定",
            MinWidth = 88,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        confirmButton.Click += (_, _) => Close(true);

        StackPanel buttons = new()
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 10
        };

        if (showCancel)
        {
            Button cancelButton = new()
            {
                Content = "取消",
                MinWidth = 88,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            cancelButton.Click += (_, _) => Close(false);
            buttons.Children.Add(cancelButton);
        }

        buttons.Children.Add(confirmButton);

        StackPanel content = new() { Spacing = 24 };
        content.Children.Add(messageText);
        content.Children.Add(buttons);

        Content = new Border
        {
            Padding = new Thickness(24),
            Background = Brushes.Transparent,
            Child = content
        };
    }

    public static Task ShowMessageAsync(Window owner, string title, string message) =>
        new DialogWindow(title, message, false).ShowDialog<bool>(owner);

    public static Task<bool> ConfirmAsync(Window owner, string title, string message) =>
        new DialogWindow(title, message, true).ShowDialog<bool>(owner);
}
