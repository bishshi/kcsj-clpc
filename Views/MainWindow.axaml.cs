using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using kcsj.Models;
using kcsj.Services;

namespace kcsj.Views;

public partial class MainWindow : Window
{
    private TextBox LogControl => this.FindControl<TextBox>("LogTextBox")!;
    private TextBlock DataStatusControl => this.FindControl<TextBlock>("DataStatusText")!;
    private LeastSquaresResult? _lastResult;

    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        LogService.OnLog += ShowLog;
        Closed += (_, _) => LogService.OnLog -= ShowLog;
        LogService.AddLog("程序启动成功。当前界面框架：Avalonia。");
        UpdateDataStatus();
    }

    private async void OpenDataInput_Click(object? sender, RoutedEventArgs e) =>
        await OpenDataInputAsync(0);

    private async void OpenFileInput_Click(object? sender, RoutedEventArgs e) =>
        await OpenDataInputAsync(0);

    private async void OpenManualInput_Click(object? sender, RoutedEventArgs e) =>
        await OpenDataInputAsync(1);

    private async Task OpenDataInputAsync(int selectedTab)
    {
        DataInputWindow window = new(selectedTab);
        bool saved = await window.ShowDialog<bool>(this);
        if (saved)
        {
            _lastResult = null;
            UpdateDataStatus();
        }
    }

    private async void CheckData_Click(object? sender, RoutedEventArgs e)
    {
        DataCheckResult result = DataCheck.Check(DataStore.KnownPoints, DataStore.Observations);
        LogService.AddLog(result.ToLogText());

        string message = result.IsValid
            ? "数据检查通过，可以进行平差计算。"
            : "数据检查未通过，请查看日志信息。";
        await DialogWindow.ShowMessageAsync(this, "数据检查", message);
    }

    private async void RunAdjustment_Click(object? sender, RoutedEventArgs e) =>
        await RunAdjustmentAsync();

    private async Task<bool> RunAdjustmentAsync()
    {
        try
        {
            if (!DataStore.HasData)
            {
                await DialogWindow.ShowMessageAsync(this, "开始平差", "请先导入或输入已知点和观测数据。");
                return false;
            }

            _lastResult = LeastSquaresAdjustmentService.Adjust(
                DataStore.KnownPoints,
                DataStore.Observations);

            foreach ((string pointName, double elevation) in _lastResult.UnknownElevations)
            {
                double error = _lastResult.UnknownElevationErrors[pointName];
                LogService.AddLog(
                    $"{pointName} 平差高程 = {elevation:F4} m，中误差 = ±{FormatValue(error, 4)} m");
            }

            foreach (ObservationAdjustmentResult item in _lastResult.ObservationResults)
            {
                LogService.AddLog(
                    $"第{item.Index}段 {item.FromPoint}->{item.ToPoint}：" +
                    $"v = {item.Residual:F6}，平差后高差 = {item.AdjustedHeightDiff:F6}，" +
                    $"中误差 = ±{FormatValue(item.AdjustedHeightDiffError, 6)} m");
            }

            await DialogWindow.ShowMessageAsync(this, "开始平差", "间接平差计算完成。");
            return true;
        }
        catch (Exception exception)
        {
            LogService.AddLog("平差失败：" + exception.Message);
            await DialogWindow.ShowMessageAsync(this, "平差失败", exception.Message);
            return false;
        }
    }

    private async void ShowResult_Click(object? sender, RoutedEventArgs e)
    {
        LeastSquaresResult? result = await GetResultOrRunAdjustmentAsync();
        if (result is not null)
        {
            await new ResultWindow(result).ShowDialog(this);
        }
    }

    private async Task<LeastSquaresResult?> GetResultOrRunAdjustmentAsync()
    {
        if (_lastResult is not null)
        {
            return _lastResult;
        }

        bool shouldRun = await DialogWindow.ConfirmAsync(
            this,
            "平差结果",
            "当前还没有平差结果，是否立即进行平差计算？");

        return shouldRun && await RunAdjustmentAsync() ? _lastResult : null;
    }

    private async void ExportReport_Click(object? sender, RoutedEventArgs e)
    {
        LeastSquaresResult? result = await GetResultOrRunAdjustmentAsync();
        if (result is null)
        {
            return;
        }

        IReadOnlyList<IStorageFolder> folders = await StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions
            {
                Title = "选择成果文件和报告文件的输出目录",
                AllowMultiple = false
            });

        string? outputFolder = folders.FirstOrDefault()?.Path.LocalPath;
        if (string.IsNullOrWhiteSpace(outputFolder))
        {
            return;
        }

        try
        {
            string heightPath = Path.Combine(outputFolder, "height.txt");
            string surveyPath = Path.Combine(outputFolder, "survey.txt");
            string reportPath = Path.Combine(outputFolder, "report.txt");

            await File.WriteAllTextAsync(heightPath, Report.BuildHeightText(result), Encoding.UTF8);
            await File.WriteAllTextAsync(surveyPath, Report.BuildSurveyText(result), Encoding.UTF8);
            await File.WriteAllTextAsync(
                reportPath,
                Report.BuildReportText(DataStore.KnownPoints, DataStore.Observations, result),
                Encoding.UTF8);

            LogService.AddLog($"高程成果文件已输出：{heightPath}");
            LogService.AddLog($"高差成果文件已输出：{surveyPath}");
            LogService.AddLog($"平差报告已输出：{reportPath}");

            bool shouldOpen = await DialogWindow.ConfirmAsync(
                this,
                "输出报告",
                "输出完成，是否打开输出目录？");
            if (shouldOpen)
            {
                PlatformLauncher.OpenFolder(outputFolder);
            }
        }
        catch (Exception exception)
        {
            LogService.AddLog("输出报告失败：" + exception.Message);
            await DialogWindow.ShowMessageAsync(this, "输出报告失败", exception.Message);
        }
    }

    private async void ClearData_Click(object? sender, RoutedEventArgs e)
    {
        bool confirmed = await DialogWindow.ConfirmAsync(this, "二次确认", "确定要清空当前数据吗？");
        if (!confirmed)
        {
            LogService.AddLog("用户取消清空数据。");
            return;
        }

        DataStore.Clear();
        _lastResult = null;
        UpdateDataStatus();
    }

    private async void About_Click(object? sender, RoutedEventArgs e) =>
        await DialogWindow.ShowMessageAsync(
            this,
            "关于",
            "水准测量间接平差程序\n使用 Avalonia 构建，支持 Windows、macOS 和 Linux。\n运行时：.NET 8");

    private void ClearLog_Click(object? sender, RoutedEventArgs e) => LogControl.Text = string.Empty;

    private void ShowLog(string message)
    {
        Dispatcher.UIThread.Post(() =>
        {
            LogControl.Text = string.IsNullOrEmpty(LogControl.Text)
                ? message
                : LogControl.Text + Environment.NewLine + message;
            LogControl.CaretIndex = LogControl.Text?.Length ?? 0;
        });
    }

    private void UpdateDataStatus()
    {
        DataStatusControl.Text = DataStore.HasData
            ? $"数据来源：{DataStore.DataSource}　已知点：{DataStore.KnownPoints.Count}　观测：{DataStore.Observations.Count}"
            : "当前未加载数据";
    }

    private static string FormatValue(double value, int digits) =>
        double.IsFinite(value) ? value.ToString($"F{digits}") : "无法计算";
}
