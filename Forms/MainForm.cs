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

            if (form.ShowDialog() == DialogResult.OK)
            {
                LogService.AddLog("主窗体已接收文件导入数据。");
                LogService.AddLog($"已知点数量：{DataStore.KnownPoints.Count}");
                LogService.AddLog($"观测数据数量：{DataStore.Observations.Count}");
            }
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
    }
}
