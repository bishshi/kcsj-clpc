namespace kcsj.Forms
{
    partial class FileInputForm
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
            KnownPointFile = new Label();
            KonwnSFile = new Label();
            btnSubmitData = new Button();
            txtKnownPath = new TextBox();
            txtObservationPath = new TextBox();
            btnFileView1 = new Button();
            btnFileView2 = new Button();
            btnViewData = new Button();
            dgvKnownPoints = new DataGridView();
            dgvObservations = new DataGridView();
            txtKnownPoints = new Label();
            txtGuance = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvKnownPoints).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvObservations).BeginInit();
            SuspendLayout();
            // 
            // KnownPointFile
            // 
            KnownPointFile.AutoSize = true;
            KnownPointFile.Location = new Point(37, 43);
            KnownPointFile.Name = "KnownPointFile";
            KnownPointFile.Size = new Size(136, 24);
            KnownPointFile.TabIndex = 0;
            KnownPointFile.Text = "已知点文件路径";
            // 
            // KonwnSFile
            // 
            KonwnSFile.AutoSize = true;
            KonwnSFile.Location = new Point(37, 89);
            KonwnSFile.Name = "KonwnSFile";
            KonwnSFile.Size = new Size(136, 24);
            KonwnSFile.TabIndex = 1;
            KonwnSFile.Text = "观测值文件路径";
            // 
            // btnSubmitData
            // 
            btnSubmitData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnSubmitData.Location = new Point(641, 38);
            btnSubmitData.Name = "btnSubmitData";
            btnSubmitData.Size = new Size(106, 83);
            btnSubmitData.TabIndex = 2;
            btnSubmitData.Text = "导入数据";
            btnSubmitData.UseVisualStyleBackColor = true;
            btnSubmitData.Click += btnSubmitData_Click;
            // 
            // txtKnownPath
            // 
            txtKnownPath.Location = new Point(190, 40);
            txtKnownPath.Name = "txtKnownPath";
            txtKnownPath.Size = new Size(194, 30);
            txtKnownPath.TabIndex = 3;
            // 
            // txtObservationPath
            // 
            txtObservationPath.Location = new Point(190, 91);
            txtObservationPath.Name = "txtObservationPath";
            txtObservationPath.Size = new Size(194, 30);
            txtObservationPath.TabIndex = 4;
            // 
            // btnFileView1
            // 
            btnFileView1.Location = new Point(390, 38);
            btnFileView1.Name = "btnFileView1";
            btnFileView1.Size = new Size(112, 34);
            btnFileView1.TabIndex = 5;
            btnFileView1.Text = "浏览";
            btnFileView1.UseVisualStyleBackColor = true;
            btnFileView1.Click += btnFileView1_Click;
            // 
            // btnFileView2
            // 
            btnFileView2.Location = new Point(390, 89);
            btnFileView2.Name = "btnFileView2";
            btnFileView2.Size = new Size(112, 34);
            btnFileView2.TabIndex = 6;
            btnFileView2.Text = "浏览";
            btnFileView2.UseVisualStyleBackColor = true;
            btnFileView2.Click += btnFileView2_Click;
            // 
            // btnViewData
            // 
            btnViewData.Location = new Point(529, 38);
            btnViewData.Name = "btnViewData";
            btnViewData.Size = new Size(106, 83);
            btnViewData.TabIndex = 7;
            btnViewData.Text = "预览数据";
            btnViewData.UseVisualStyleBackColor = true;
            btnViewData.Click += btnViewData_Click;
            // 
            // dgvKnownPoints
            // 
            dgvKnownPoints.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKnownPoints.Location = new Point(31, 154);
            dgvKnownPoints.Name = "dgvKnownPoints";
            dgvKnownPoints.RowHeadersWidth = 62;
            dgvKnownPoints.Size = new Size(710, 226);
            dgvKnownPoints.TabIndex = 8;
            // 
            // dgvObservations
            // 
            dgvObservations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvObservations.Location = new Point(31, 425);
            dgvObservations.Name = "dgvObservations";
            dgvObservations.RowHeadersWidth = 62;
            dgvObservations.Size = new Size(710, 245);
            dgvObservations.TabIndex = 9;
            // 
            // txtKnownPoints
            // 
            txtKnownPoints.AutoSize = true;
            txtKnownPoints.Location = new Point(354, 127);
            txtKnownPoints.Name = "txtKnownPoints";
            txtKnownPoints.Size = new Size(64, 24);
            txtKnownPoints.TabIndex = 10;
            txtKnownPoints.Text = "已知点";
            // 
            // txtGuance
            // 
            txtGuance.AutoSize = true;
            txtGuance.Location = new Point(350, 389);
            txtGuance.Name = "txtGuance";
            txtGuance.Size = new Size(64, 24);
            txtGuance.TabIndex = 11;
            txtGuance.Text = "观测值";
            // 
            // FileInputForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(769, 714);
            Controls.Add(txtGuance);
            Controls.Add(txtKnownPoints);
            Controls.Add(dgvObservations);
            Controls.Add(dgvKnownPoints);
            Controls.Add(btnViewData);
            Controls.Add(btnFileView2);
            Controls.Add(btnFileView1);
            Controls.Add(txtObservationPath);
            Controls.Add(txtKnownPath);
            Controls.Add(btnSubmitData);
            Controls.Add(KonwnSFile);
            Controls.Add(KnownPointFile);
            MaximizeBox = false;
            Name = "FileInputForm";
            Text = "文件读取";
            ((System.ComponentModel.ISupportInitialize)dgvKnownPoints).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvObservations).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label KnownPointFile;
        private Label KonwnSFile;
        private Button btnSubmitData;
        private TextBox txtKnownPath;
        private TextBox txtObservationPath;
        private Button btnFileView1;
        private Button btnFileView2;
        private Button btnViewData;
        private DataGridView dgvKnownPoints;
        private DataGridView dgvObservations;
        private Label txtKnownPoints;
        private Label txtGuance;
    }
}