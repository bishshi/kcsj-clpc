using kcsj.Models;
using kcsj.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace kcsj.Forms
{
    public partial class FileInputForm : Form
    {
        // 保存解析后的已知点和观测值
        private List<KnownPoint> knownPoints = new List<KnownPoint>();
        private List<Observation> observations = new List<Observation>();

        public List<KnownPoint> ImportedKnownPoints { get; private set; }
        public List<Observation> ImportedObservations { get; private set; }

        public FileInputForm()
        {
            InitializeComponent();

            InitGridStyle();
        }

        // 初始化表格样式
        private void InitGridStyle()
        {
            dgvKnownPoints.ReadOnly = true;
            dgvKnownPoints.AllowUserToAddRows = false;
            dgvKnownPoints.AllowUserToDeleteRows = false;
            dgvKnownPoints.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKnownPoints.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvObservations.ReadOnly = true;
            dgvObservations.AllowUserToAddRows = false;
            dgvObservations.AllowUserToDeleteRows = false;
            dgvObservations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvObservations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // 通用选择 TXT 文件函数
        private string SelectTxtFile(string title)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = title;
            dialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }

            return "";
        }

        // 浏览已知点文件
        private void btnFileView1_Click(object sender, EventArgs e)
        {
            string path = SelectTxtFile("选择已知点高程文件");

            if (path != "")
            {
                txtKnownPath.Text = path;
            }
        }

        // 浏览观测值文件
        private void btnFileView2_Click(object sender, EventArgs e)
        {
            string path = SelectTxtFile("选择观测值文件");

            if (path != "")
            {
                txtObservationPath.Text = path;
            }
        }

        // 按空格、Tab、英文逗号、中文逗号分割
        private string[] SplitLine(string line)
        {
            return line.Split(new char[] { ' ', '\t', ',', '，' },
                StringSplitOptions.RemoveEmptyEntries);
        }

        // 读取已知点 TXT
        private List<KnownPoint> ReadKnownPoints(string filePath)
        {
            List<KnownPoint> result = new List<KnownPoint>();
            HashSet<string> pointNames = new HashSet<string>();

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("#") || line.StartsWith("//"))
                    continue;

                string[] parts = SplitLine(line);

                if (parts.Length != 2)
                {
                    throw new Exception($"已知点文件第 {i + 1} 行格式错误，应为：点名 高程");
                }

                string name = parts[0];

                if (!double.TryParse(parts[1], out double elevation))
                {
                    throw new Exception($"已知点文件第 {i + 1} 行高程不是有效数字");
                }

                if (pointNames.Contains(name))
                {
                    throw new Exception($"已知点文件第 {i + 1} 行点名重复：{name}");
                }

                pointNames.Add(name);
                result.Add(new KnownPoint(name, elevation));
            }

            if (result.Count == 0)
            {
                throw new Exception("已知点文件为空或没有有效数据");
            }

            return result;
        }

        // 读取观测值 TXT
        private List<Observation> ReadObservations(string filePath)
        {
            List<Observation> result = new List<Observation>();

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = SplitLine(line);

                if (parts.Length != 4)
                {
                    throw new Exception($"观测文件第 {i + 1} 行格式错误，应为：起点 终点 高差 距离");
                }

                string from = parts[0];
                string to = parts[1];

                if (from == to)
                {
                    throw new Exception($"观测文件第 {i + 1} 行起点和终点不能相同");
                }

                if (!double.TryParse(parts[2], out double heightDiff))
                {
                    throw new Exception($"观测文件第 {i + 1} 行高差不是有效数字");
                }

                if (!double.TryParse(parts[3], out double distance))
                {
                    throw new Exception($"观测文件第 {i + 1} 行距离不是有效数字");
                }

                if (distance <= 0)
                {
                    throw new Exception($"观测文件第 {i + 1} 行距离必须大于 0");
                }

                result.Add(new Observation(from, to, heightDiff, distance));
            }

            if (result.Count == 0)
            {
                throw new Exception("观测文件为空或没有有效数据");
            }

            return result;
        }

        // 设置表格表头
        private void SetGridHeader()
        {
            if (dgvKnownPoints.Columns.Count > 0)
            {
                dgvKnownPoints.Columns["Name"].HeaderText = "点名";
                dgvKnownPoints.Columns["Elevation"].HeaderText = "高程";
            }

            if (dgvObservations.Columns.Count > 0)
            {
                dgvObservations.Columns["FromPoint"].HeaderText = "起点";
                dgvObservations.Columns["ToPoint"].HeaderText = "终点";
                dgvObservations.Columns["HeightDiff"].HeaderText = "高差";
                dgvObservations.Columns["Distance"].HeaderText = "距离";
            }
        }

        // 点击“预览数据”
        private void btnViewData_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtKnownPath.Text))
                {
                    MessageBox.Show("请先选择已知点高程文件");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtObservationPath.Text))
                {
                    MessageBox.Show("请先选择观测值文件");
                    return;
                }

                knownPoints = ReadKnownPoints(txtKnownPath.Text);
                observations = ReadObservations(txtObservationPath.Text);

                dgvKnownPoints.DataSource = null;
                dgvKnownPoints.DataSource = knownPoints;

                dgvObservations.DataSource = null;
                dgvObservations.DataSource = observations;

                SetGridHeader();

                MessageBox.Show("数据预览成功！");
                LogService.AddLog("数据预览成功！");

            }
            catch (Exception ex)
            {

                dgvKnownPoints.DataSource = null;
                dgvObservations.DataSource = null;

                MessageBox.Show($"数据预览失败：{ex.Message} ");
                LogService.AddLog($"数据预览失败：{ex.Message} ");
            }
        }

        private void btnSubmitData_Click(object sender, EventArgs e)
        {
            try
            {
                // 如果还没有点“预览数据”，这里自动读取一次
                if (knownPoints == null || knownPoints.Count == 0 ||
                    observations == null || observations.Count == 0)
                {
                    if (string.IsNullOrWhiteSpace(txtKnownPath.Text))
                    {
                        MessageBox.Show("请先选择已知点高程文件");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtObservationPath.Text))
                    {
                        MessageBox.Show("请先选择观测值文件");
                        return;
                    }

                    knownPoints = ReadKnownPoints(txtKnownPath.Text);
                    observations = ReadObservations(txtObservationPath.Text);
                }

                // 把数据交给主窗体
                ImportedKnownPoints = knownPoints;
                ImportedObservations = observations;

                MessageBox.Show("数据导入成功！");
                LogService.AddLog("数据导入成功！");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败：" + ex.Message);
                LogService.AddLog("导入失败：" + ex.Message);
            }
        }
    }
}