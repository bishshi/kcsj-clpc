using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using kcsj.Models;
using kcsj.Services;

namespace kcsj.Views;

public partial class ResultWindow : Window
{
    public ResultWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ResultWindow(LeastSquaresResult result)
        : this()
    {
        this.FindControl<TextBox>("ResultTextBox")!.Text = Report.BuildResultText(result);
    }

    private void Close_Click(object? sender, RoutedEventArgs e) => Close();
}
