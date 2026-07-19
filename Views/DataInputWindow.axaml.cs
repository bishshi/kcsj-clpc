using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using kcsj.Models;
using kcsj.Services;

namespace kcsj.Views;

public partial class DataInputWindow : Window
{
    private static readonly FilePickerFileType TextFileType = new("文本文件")
    {
        Patterns = ["*.txt"],
        MimeTypes = ["text/plain"]
    };

    private TabControl TabsControl => this.FindControl<TabControl>("DataTabs")!;
    private TextBlock StatusControl => this.FindControl<TextBlock>("InputStatusText")!;
    private TextBox KnownPathControl => this.FindControl<TextBox>("KnownPathTextBox")!;
    private TextBox ObservationPathControl => this.FindControl<TextBox>("ObservationPathTextBox")!;
    private TextBox KnownPreviewControl => this.FindControl<TextBox>("KnownPreviewTextBox")!;
    private TextBox ObservationPreviewControl => this.FindControl<TextBox>("ObservationPreviewTextBox")!;
    private TextBox ManualKnownControl => this.FindControl<TextBox>("ManualKnownTextBox")!;
    private TextBox ManualObservationControl => this.FindControl<TextBox>("ManualObservationTextBox")!;

    private List<KnownPoint> _fileKnownPoints = [];
    private List<Observation> _fileObservations = [];

    public DataInputWindow()
        : this(0)
    {
    }

    public DataInputWindow(int selectedTab)
    {
        AvaloniaXamlLoader.Load(this);
        TabsControl.SelectedIndex = Math.Clamp(selectedTab, 0, 1);
    }

    private async void BrowseKnownFile_Click(object? sender, RoutedEventArgs e)
    {
        string? path = await PickTextFileAsync("选择已知点高程文件");
        if (path is not null)
        {
            KnownPathControl.Text = path;
            _fileKnownPoints = [];
        }
    }

    private async void BrowseObservationFile_Click(object? sender, RoutedEventArgs e)
    {
        string? path = await PickTextFileAsync("选择观测数据文件");
        if (path is not null)
        {
            ObservationPathControl.Text = path;
            _fileObservations = [];
        }
    }

    private async Task<string?> PickTextFileAsync(string title)
    {
        IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter = [TextFileType, FilePickerFileTypes.All]
            });

        return files.FirstOrDefault()?.Path.LocalPath;
    }

    private async void PreviewFiles_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            LoadFileData();
            ShowFilePreview();
            StatusControl.Text = $"预览完成：{_fileKnownPoints.Count} 个已知点，{_fileObservations.Count} 条观测。";
            LogService.AddLog("文件数据预览成功。");
        }
        catch (Exception exception)
        {
            ClearFilePreview();
            LogService.AddLog("数据预览失败：" + exception.Message);
            await DialogWindow.ShowMessageAsync(this, "数据预览失败", exception.Message);
        }
    }

    private async void ImportFiles_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            LoadFileData();
            DataStore.SetData(_fileKnownPoints, _fileObservations, "文件导入");
            LogService.AddLog("文件数据导入成功。");
            Close(true);
        }
        catch (Exception exception)
        {
            LogService.AddLog("导入失败：" + exception.Message);
            await DialogWindow.ShowMessageAsync(this, "导入失败", exception.Message);
        }
    }

    private async void PreviewManual_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            (List<KnownPoint> knownPoints, List<Observation> observations) = ParseManualData();
            StatusControl.Text = $"手动输入检查通过：{knownPoints.Count} 个已知点，{observations.Count} 条观测。";
            await DialogWindow.ShowMessageAsync(
                this,
                "手动输入预览",
                InputParser.BuildKnownPointsPreview(knownPoints) + "\n" +
                InputParser.BuildObservationsPreview(observations));
        }
        catch (Exception exception)
        {
            LogService.AddLog("手动输入检查失败：" + exception.Message);
            await DialogWindow.ShowMessageAsync(this, "输入错误", exception.Message);
        }
    }

    private async void SaveManual_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            (List<KnownPoint> knownPoints, List<Observation> observations) = ParseManualData();
            DataStore.SetData(knownPoints, observations, "手动输入");
            LogService.AddLog("手动输入数据已保存。");
            Close(true);
        }
        catch (Exception exception)
        {
            LogService.AddLog("手动输入数据时发生错误：" + exception.Message);
            await DialogWindow.ShowMessageAsync(this, "输入错误", exception.Message);
        }
    }

    private void LoadFileData()
    {
        string knownPath = KnownPathControl.Text?.Trim() ?? string.Empty;
        string observationPath = ObservationPathControl.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(knownPath))
        {
            throw new InvalidOperationException("请先选择已知点高程文件。");
        }

        if (string.IsNullOrWhiteSpace(observationPath))
        {
            throw new InvalidOperationException("请先选择观测数据文件。");
        }

        _fileKnownPoints = InputParser.ReadKnownPointsFile(knownPath);
        _fileObservations = InputParser.ReadObservationsFile(observationPath);
    }

    private void ShowFilePreview()
    {
        KnownPreviewControl.Text = InputParser.BuildKnownPointsPreview(_fileKnownPoints);
        ObservationPreviewControl.Text = InputParser.BuildObservationsPreview(_fileObservations);
    }

    private void ClearFilePreview()
    {
        _fileKnownPoints = [];
        _fileObservations = [];
        KnownPreviewControl.Text = string.Empty;
        ObservationPreviewControl.Text = string.Empty;
    }

    private (List<KnownPoint> KnownPoints, List<Observation> Observations) ParseManualData() =>
        (
            InputParser.ParseKnownPointsText(ManualKnownControl.Text ?? string.Empty),
            InputParser.ParseObservationsText(ManualObservationControl.Text ?? string.Empty)
        );

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close(false);
}
