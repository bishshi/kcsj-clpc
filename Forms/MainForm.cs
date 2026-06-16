using kcsj.Models;
using kcsj.Services;
using System.Diagnostics;
using System.Text;

namespace kcsj.Forms
{
    public partial class MainForm : Form
    {
        private LeastSquaresResult? lastResult;

        public MainForm()
        {
            InitializeComponent();
            button3.Click += button3_Click;
        }

        private void btnDataInput_Click(object sender, EventArgs e)
        {
            DataInputForm form = new DataInputForm();
            form.ShowDialog();
            lastResult = null;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LogService.OnLog += ShowLog;
            LogService.AddLog("程序启动成功。");
        }

        private void ShowLog(string message)
        {
            LOG.AppendText(message + "\r\n");
            LOG.ScrollToCaret();
        }

        private void ClearLog_Click(object sender, EventArgs e)
        {
            LOG.Clear();
        }

        private void 从文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInputForm form = new FileInputForm();
            form.ShowDialog();
            lastResult = null;
        }

        private void 手动输入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManualInputForm form = new ManualInputForm();
            form.ShowDialog();
            lastResult = null;
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "水准测量间接平差程序");
        }

        private void 开始平差ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunAdjustment();
        }

        private void 查看结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAdjustmentResult();
        }

        private void 输出报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportReport();
        }

        private void btnDataInspect_Click(object sender, EventArgs e)
        {
            CheckData();
        }

        private void btnStartAdjust_Click(object sender, EventArgs e)
        {
            RunAdjustment();
        }

        private void 删除当前数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "确定要清空当前数据吗？",
                "二次确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                DataStore.Clear();
                lastResult = null;
                LogService.AddLog("当前数据已清空");
            }
            else
            {
                LogService.AddLog("用户取消清空数据");
            }
        }

        private void 数据检查ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowAdjustmentResult();
        }

        private void button3_Click(object? sender, EventArgs e)
        {
            ExportReport();
        }

        private void CheckData()
        {
            DataCheckResult result = DataCheck.Check(DataStore.KnownPoints, DataStore.Observations);

            LogService.AddLog(result.ToLogText());
            LogService.AddLog(Environment.NewLine);

            if (!result.IsValid)
            {
                MessageBox.Show(
                    "数据检查未通过，请查看日志信息。",
                    "数据检查",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            MessageBox.Show(
                "数据检查通过，可以进行平差计算。",
                "数据检查",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private bool RunAdjustment()
        {
            try
            {
                if (!DataStore.HasData)
                {
                    MessageBox.Show("请先导入或输入已知点和观测数据。");
                    return false;
                }

                LeastSquaresResult result = LeastSquaresAdjustmentService.Adjust(
                    DataStore.KnownPoints,
                    DataStore.Observations);

                lastResult = result;
                MessageBox.Show("间接平差计算完成。");

                foreach (KeyValuePair<string, double> item in result.UnknownElevations)
                {
                    string pointName = item.Key;
                    double elevation = item.Value;
                    double error = result.UnknownElevationErrors[pointName];

                    LogService.AddLog(
                        $"{pointName} 平差高程 = {elevation:F4} m，中误差 = ±{FormatValue(error, 4)} m");
                }

                foreach (ObservationAdjustmentResult obsResult in result.ObservationResults)
                {
                    LogService.AddLog(
                        $"第{obsResult.Index}段 {obsResult.FromPoint}->{obsResult.ToPoint}：" +
                        $"v = {obsResult.Residual:F6}，平差后高差 = {obsResult.AdjustedHeightDiff:F6}，" +
                        $"中误差 = ±{FormatValue(obsResult.AdjustedHeightDiffError, 6)} m");
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("平差失败：" + ex.Message);
                LogService.AddLog("平差失败：" + ex.Message);
                return false;
            }
        }

        private LeastSquaresResult? GetResultOrRunAdjustment()
        {
            if (lastResult != null)
            {
                return lastResult;
            }

            DialogResult choice = MessageBox.Show(
                "当前还没有平差结果，是否立即进行平差计算？",
                "平差结果",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (choice != DialogResult.Yes)
            {
                return null;
            }

            return RunAdjustment() ? lastResult : null;
        }

        private void ShowAdjustmentResult()
        {
            LeastSquaresResult? result = GetResultOrRunAdjustment();
            if (result == null)
            {
                return;
            }

            using ResultForm form = new ResultForm(result);
            form.ShowDialog(this);
        }

        private void ExportReport()
        {
            LeastSquaresResult? result = GetResultOrRunAdjustment();
            if (result == null)
            {
                return;
            }

            using FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "请选择成果文件和报告文件的输出目录",
                UseDescriptionForTitle = true
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            string heightPath = Path.Combine(dialog.SelectedPath, "height.txt");
            string surveyPath = Path.Combine(dialog.SelectedPath, "survey.txt");
            string reportPath = Path.Combine(dialog.SelectedPath, "report.txt");

            File.WriteAllText(heightPath, Report.BuildHeightText(result), Encoding.UTF8);
            File.WriteAllText(surveyPath, Report.BuildSurveyText(result), Encoding.UTF8);
            File.WriteAllText(
                reportPath,
                Report.BuildReportText(DataStore.KnownPoints, DataStore.Observations, result),
                Encoding.UTF8);

            LogService.AddLog($"高程成果文件已输出：{heightPath}");
            LogService.AddLog($"高差成果文件已输出：{surveyPath}");
            LogService.AddLog($"平差报告已输出：{reportPath}");

            DialogResult openChoice = MessageBox.Show(
                "输出完成，是否打开输出目录？",
                "输出报告",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (openChoice == DialogResult.Yes)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = dialog.SelectedPath,
                    UseShellExecute = true
                });
            }
        }

        private static string FormatValue(double value, int digits)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return "无法计算";
            }

            return value.ToString($"F{digits}");
        }
    }
}
