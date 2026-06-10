using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using kcsj.Services;

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

        }

        private void 查看结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 输出报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
