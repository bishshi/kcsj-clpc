namespace kcsj.Forms
{
    partial class ManualInputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TitleManualInput = new Label();
            GroupKnown = new GroupBox();
            dgvKnownInput = new DataGridView();
            PointName = new DataGridViewTextBoxColumn();
            Elevation = new DataGridViewTextBoxColumn();
            GroupObservation = new GroupBox();
            dataGridView1 = new DataGridView();
            StartPoint = new DataGridViewTextBoxColumn();
            EndPoint = new DataGridViewTextBoxColumn();
            HeightDifference = new DataGridViewTextBoxColumn();
            Distance = new DataGridViewTextBoxColumn();
            btnOK = new Button();
            btnCancel = new Button();
            GroupKnown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKnownInput).BeginInit();
            GroupObservation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // TitleManualInput
            // 
            TitleManualInput.AutoSize = true;
            TitleManualInput.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            TitleManualInput.Location = new Point(570, 9);
            TitleManualInput.Name = "TitleManualInput";
            TitleManualInput.Size = new Size(138, 28);
            TitleManualInput.TabIndex = 0;
            TitleManualInput.Text = "手动输入数据";
            // 
            // GroupKnown
            // 
            GroupKnown.Controls.Add(dgvKnownInput);
            GroupKnown.Location = new Point(28, 50);
            GroupKnown.Name = "GroupKnown";
            GroupKnown.Size = new Size(426, 408);
            GroupKnown.TabIndex = 1;
            GroupKnown.TabStop = false;
            GroupKnown.Text = "已知点数据";
            // 
            // dgvKnownInput
            // 
            dgvKnownInput.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKnownInput.Columns.AddRange(new DataGridViewColumn[] { PointName, Elevation });
            dgvKnownInput.Location = new Point(18, 29);
            dgvKnownInput.Name = "dgvKnownInput";
            dgvKnownInput.RowHeadersWidth = 62;
            dgvKnownInput.Size = new Size(389, 355);
            dgvKnownInput.TabIndex = 0;
            // 
            // PointName
            // 
            PointName.HeaderText = "点名";
            PointName.MinimumWidth = 8;
            PointName.Name = "PointName";
            PointName.Width = 150;
            // 
            // Elevation
            // 
            Elevation.HeaderText = "高程/m";
            Elevation.MinimumWidth = 8;
            Elevation.Name = "Elevation";
            Elevation.Width = 150;
            // 
            // GroupObservation
            // 
            GroupObservation.Controls.Add(dataGridView1);
            GroupObservation.Location = new Point(481, 50);
            GroupObservation.Name = "GroupObservation";
            GroupObservation.Size = new Size(771, 408);
            GroupObservation.TabIndex = 2;
            GroupObservation.TabStop = false;
            GroupObservation.Text = "观测值";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { StartPoint, EndPoint, HeightDifference, Distance });
            dataGridView1.Location = new Point(15, 29);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(730, 355);
            dataGridView1.TabIndex = 0;
            // 
            // StartPoint
            // 
            StartPoint.HeaderText = "起点";
            StartPoint.MinimumWidth = 8;
            StartPoint.Name = "StartPoint";
            StartPoint.Width = 150;
            // 
            // EndPoint
            // 
            EndPoint.HeaderText = "终点";
            EndPoint.MinimumWidth = 8;
            EndPoint.Name = "EndPoint";
            EndPoint.Width = 150;
            // 
            // HeightDifference
            // 
            HeightDifference.HeaderText = "观测高差/m";
            HeightDifference.MinimumWidth = 8;
            HeightDifference.Name = "HeightDifference";
            HeightDifference.Width = 150;
            // 
            // Distance
            // 
            Distance.HeaderText = "路线长/km";
            Distance.MinimumWidth = 8;
            Distance.Name = "Distance";
            Distance.Width = 150;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(516, 485);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(112, 34);
            btnOK.TabIndex = 3;
            btnOK.Text = "提交";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.UseWaitCursor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(651, 485);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(112, 34);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // ManualInputForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1278, 556);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(GroupObservation);
            Controls.Add(GroupKnown);
            Controls.Add(TitleManualInput);
            Name = "ManualInputForm";
            Text = "手动输入数据";
            GroupKnown.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvKnownInput).EndInit();
            GroupObservation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label TitleManualInput;
        private GroupBox GroupKnown;
        private DataGridView dgvKnownInput;
        private GroupBox GroupObservation;
        private DataGridViewTextBoxColumn PointName;
        private DataGridViewTextBoxColumn Elevation;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn StartPoint;
        private DataGridViewTextBoxColumn EndPoint;
        private DataGridViewTextBoxColumn HeightDifference;
        private DataGridViewTextBoxColumn Distance;
        private Button btnOK;
        private Button btnCancel;
    }
}