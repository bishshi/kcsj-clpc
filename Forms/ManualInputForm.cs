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
    public partial class ManualInputForm : Form
    {
        private string GetCellString(DataGridViewRow row, string columnName)
        {
            object value = row.Cells[columnName].Value;

            if (value == null)
            {
                return "";
            }

            return value.ToString().Trim();
        }

        private List<KnownPoint> ReadKnownPointsFromGrid()
        {
            List<KnownPoint> knownPoints = new List<KnownPoint>();

            foreach (DataGridViewRow row in dgvKnownInput.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                string pointName = GetCellString(row, "PointName");
                string elevationText = GetCellString(row, "Elevation");

                if (pointName == "" && elevationText == "")
                {
                    continue;
                }

                if (pointName == "")
                {
                    throw new Exception("已知点点名不能为空。");
                }

                double elevation;

                if (!double.TryParse(elevationText, out elevation))
                {
                    throw new Exception("已知点 " + pointName + " 的高程不是有效数字。");
                }

                KnownPoint point = new KnownPoint(pointName, elevation);
                knownPoints.Add(point);
            }

            if (knownPoints.Count == 0)
            {
                throw new Exception("至少需要输入一个已知点。");
            }

            return knownPoints;
        }

        private List<Observation> ReadObservationsFromGrid()
        {
            List<Observation> observations = new List<Observation>();

            foreach (DataGridViewRow row in dgvObservationInput.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                string fromPoint = GetCellString(row, "StartPoint");
                string toPoint = GetCellString(row, "EndPoint");
                string heightDiffText = GetCellString(row, "HeightDifference");
                string distanceText = GetCellString(row, "Distance");

                if (fromPoint == "" && toPoint == "" && heightDiffText == "" && distanceText == "")
                {
                    continue;
                }

                if (fromPoint == "")
                {
                    throw new Exception("观测数据起点不能为空。");
                }

                if (toPoint == "")
                {
                    throw new Exception("观测数据终点不能为空。");
                }

                double heightDiff;
                double distance;

                if (!double.TryParse(heightDiffText, out heightDiff))
                {
                    throw new Exception(fromPoint + " 到 " + toPoint + " 的高差不是有效数字。");
                }

                if (!double.TryParse(distanceText, out distance))
                {
                    throw new Exception(fromPoint + " 到 " + toPoint + " 的路线长不是有效数字。");
                }

                Observation observation = new Observation(fromPoint, toPoint, heightDiff, distance);
                observations.Add(observation);
            }

            if (observations.Count == 0)
            {
                throw new Exception("至少需要输入一条观测数据。");
            }

            return observations;
        }

        public ManualInputForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("您取消了输入！");
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                List<KnownPoint> knownPoints = ReadKnownPointsFromGrid();
                List<Observation> observations = ReadObservationsFromGrid();

                DataStore.SetData(knownPoints, observations, "手动输入");

                MessageBox.Show(
                    "手动输入数据已保存",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LogService.AddLog("手动输入数据。已知点数量: " + knownPoints.Count + "，观测数据数量: " + observations.Count);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                LogService.AddLog("手动输入数据时发生错误: " + ex.Message);
            }
        }
    }
}
