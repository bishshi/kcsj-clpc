using kcsj.Models;
using kcsj.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kcsj.Forms
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDataInput_Click(object sender, EventArgs e)
        {
            DataInputForm form = new DataInputForm();
            form.ShowDialog();
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
        }

        private void 手动输入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManualInputForm form = new ManualInputForm();

            form.ShowDialog();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 开始平差ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DataStore.HasData)
                {
                    MessageBox.Show("请先导入或输入已知点和观测数据。");
                    return;
                }

                LeastSquaresResult result =
                    LeastSquaresAdjustmentService.Adjust(
                        DataStore.KnownPoints,
                        DataStore.Observations);

                MessageBox.Show("间接平差计算完成。");

                // 示例：把未知点结果输出到日志
                foreach (var item in result.UnknownElevations)
                {
                    string pointName = item.Key;
                    double elevation = item.Value;
                    double error = result.UnknownElevationErrors[pointName];

                    LogService.AddLog(
                        $"{pointName} 平差高程 = {elevation:F4} m，中误差 = ±{error:F4} m");
                }

                // 示例：输出观测改正数
                foreach (var obsResult in result.ObservationResults)
                {
                    LogService.AddLog(
                        $"第{obsResult.Index}段 {obsResult.FromPoint}->{obsResult.ToPoint}：" +
                        $"v = {obsResult.Residual:F6}，平差后高差 = {obsResult.AdjustedHeightDiff:F6}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("平差失败：" + ex.Message);
                LogService.AddLog("平差失败：" + ex.Message);
            }
        }

        private void 查看结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 输出报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnDataInspect_Click(object sender, EventArgs e)
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

        private void btnStartAdjust_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DataStore.HasData)
                {
                    MessageBox.Show("请先导入或输入已知点和观测数据。");
                    return;
                }

                LeastSquaresResult result =
                    LeastSquaresAdjustmentService.Adjust(
                        DataStore.KnownPoints,
                        DataStore.Observations);

                MessageBox.Show("间接平差计算完成。");

                // 示例：把未知点结果输出到日志
                foreach (var item in result.UnknownElevations)
                {
                    string pointName = item.Key;
                    double elevation = item.Value;
                    double error = result.UnknownElevationErrors[pointName];

                    LogService.AddLog(
                        $"{pointName} 平差高程 = {elevation:F4} m，中误差 = ±{error:F4} m");
                }

                // 示例：输出观测改正数
                foreach (var obsResult in result.ObservationResults)
                {
                    LogService.AddLog(
                        $"第{obsResult.Index}段 {obsResult.FromPoint}->{obsResult.ToPoint}：" +
                        $"v = {obsResult.Residual:F6}，平差后高差 = {obsResult.AdjustedHeightDiff:F6}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("平差失败：" + ex.Message);
                LogService.AddLog("平差失败：" + ex.Message);
            }
        }

        private void 删除当前数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "确定要清空当前数据吗？",
                "二次确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                DataStore.Clear();
                LogService.AddLog("当前数据已清空");
            }
            else
            {
                this.Close();
                LogService.AddLog("用户取消清空数据");
            }
        }

        private void 数据检查ToolStripMenuItem_Click(object sender, EventArgs e)
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
    }
}
