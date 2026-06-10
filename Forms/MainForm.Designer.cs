namespace kcsj.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            MainTitle = new Label();
            btnDataInput = new Button();
            btnDataInspect = new Button();
            btnStartAdjust = new Button();
            button2 = new Button();
            button3 = new Button();
            LOG = new RichTextBox();
            MainLog = new Label();
            ClearLog = new Button();
            menuStrip1 = new MenuStrip();
            数据ToolStripMenuItem = new ToolStripMenuItem();
            数据输入ToolStripMenuItem = new ToolStripMenuItem();
            从文件ToolStripMenuItem = new ToolStripMenuItem();
            手动输入ToolStripMenuItem = new ToolStripMenuItem();
            删除当前数据ToolStripMenuItem = new ToolStripMenuItem();
            数据检查ToolStripMenuItem = new ToolStripMenuItem();
            开始平差ToolStripMenuItem = new ToolStripMenuItem();
            查看结果ToolStripMenuItem = new ToolStripMenuItem();
            输出报告ToolStripMenuItem = new ToolStripMenuItem();
            关于ToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // MainTitle
            // 
            MainTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MainTitle.AutoSize = true;
            MainTitle.Font = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            MainTitle.Location = new Point(231, 55);
            MainTitle.Name = "MainTitle";
            MainTitle.Size = new Size(182, 31);
            MainTitle.TabIndex = 0;
            MainTitle.Text = "水准网平差程序";
            MainTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnDataInput
            // 
            btnDataInput.Location = new Point(30, 94);
            btnDataInput.Name = "btnDataInput";
            btnDataInput.Size = new Size(202, 64);
            btnDataInput.TabIndex = 1;
            btnDataInput.Text = "数据输入";
            btnDataInput.UseVisualStyleBackColor = true;
            btnDataInput.Click += btnDataInput_Click;
            // 
            // btnDataInspect
            // 
            btnDataInspect.Location = new Point(30, 164);
            btnDataInspect.Name = "btnDataInspect";
            btnDataInspect.Size = new Size(202, 64);
            btnDataInspect.TabIndex = 2;
            btnDataInspect.Text = "数据检查";
            btnDataInspect.UseVisualStyleBackColor = true;
            // 
            // btnStartAdjust
            // 
            btnStartAdjust.Location = new Point(30, 234);
            btnStartAdjust.Name = "btnStartAdjust";
            btnStartAdjust.Size = new Size(202, 64);
            btnStartAdjust.TabIndex = 3;
            btnStartAdjust.Text = "开始平差";
            btnStartAdjust.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(30, 304);
            button2.Name = "button2";
            button2.Size = new Size(202, 64);
            button2.TabIndex = 4;
            button2.Text = "查看结果";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(30, 374);
            button3.Name = "button3";
            button3.Size = new Size(202, 64);
            button3.TabIndex = 5;
            button3.Text = "输出报告";
            button3.UseVisualStyleBackColor = true;
            // 
            // LOG
            // 
            LOG.Location = new Point(275, 152);
            LOG.Name = "LOG";
            LOG.ReadOnly = true;
            LOG.Size = new Size(356, 286);
            LOG.TabIndex = 6;
            LOG.Text = "";
            // 
            // MainLog
            // 
            MainLog.AutoSize = true;
            MainLog.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            MainLog.Location = new Point(340, 101);
            MainLog.Name = "MainLog";
            MainLog.Size = new Size(96, 28);
            MainLog.TabIndex = 7;
            MainLog.Text = "运行日志";
            // 
            // ClearLog
            // 
            ClearLog.Location = new Point(456, 101);
            ClearLog.Name = "ClearLog";
            ClearLog.Size = new Size(112, 34);
            ClearLog.TabIndex = 8;
            ClearLog.Text = "清除日志";
            ClearLog.UseVisualStyleBackColor = true;
            ClearLog.Click += ClearLog_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { 数据ToolStripMenuItem, 开始平差ToolStripMenuItem, 查看结果ToolStripMenuItem, 输出报告ToolStripMenuItem, 关于ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(644, 32);
            menuStrip1.TabIndex = 9;
            menuStrip1.Text = "menuStrip1";
            // 
            // 数据ToolStripMenuItem
            // 
            数据ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 数据输入ToolStripMenuItem, 删除当前数据ToolStripMenuItem, 数据检查ToolStripMenuItem });
            数据ToolStripMenuItem.Name = "数据ToolStripMenuItem";
            数据ToolStripMenuItem.Size = new Size(98, 28);
            数据ToolStripMenuItem.Text = "数据管理";
            // 
            // 数据输入ToolStripMenuItem
            // 
            数据输入ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 从文件ToolStripMenuItem, 手动输入ToolStripMenuItem });
            数据输入ToolStripMenuItem.Name = "数据输入ToolStripMenuItem";
            数据输入ToolStripMenuItem.Size = new Size(218, 34);
            数据输入ToolStripMenuItem.Text = "数据输入";
            // 
            // 从文件ToolStripMenuItem
            // 
            从文件ToolStripMenuItem.Name = "从文件ToolStripMenuItem";
            从文件ToolStripMenuItem.Size = new Size(182, 34);
            从文件ToolStripMenuItem.Text = "从文件";
            从文件ToolStripMenuItem.Click += 从文件ToolStripMenuItem_Click;
            // 
            // 手动输入ToolStripMenuItem
            // 
            手动输入ToolStripMenuItem.Name = "手动输入ToolStripMenuItem";
            手动输入ToolStripMenuItem.Size = new Size(182, 34);
            手动输入ToolStripMenuItem.Text = "手动输入";
            手动输入ToolStripMenuItem.Click += 手动输入ToolStripMenuItem_Click;
            // 
            // 删除当前数据ToolStripMenuItem
            // 
            删除当前数据ToolStripMenuItem.Name = "删除当前数据ToolStripMenuItem";
            删除当前数据ToolStripMenuItem.Size = new Size(218, 34);
            删除当前数据ToolStripMenuItem.Text = "删除当前数据";
            // 
            // 数据检查ToolStripMenuItem
            // 
            数据检查ToolStripMenuItem.Name = "数据检查ToolStripMenuItem";
            数据检查ToolStripMenuItem.Size = new Size(218, 34);
            数据检查ToolStripMenuItem.Text = "数据检查";
            // 
            // 开始平差ToolStripMenuItem
            // 
            开始平差ToolStripMenuItem.Name = "开始平差ToolStripMenuItem";
            开始平差ToolStripMenuItem.Size = new Size(98, 28);
            开始平差ToolStripMenuItem.Text = "开始平差";
            开始平差ToolStripMenuItem.Click += 开始平差ToolStripMenuItem_Click;
            // 
            // 查看结果ToolStripMenuItem
            // 
            查看结果ToolStripMenuItem.Name = "查看结果ToolStripMenuItem";
            查看结果ToolStripMenuItem.Size = new Size(98, 28);
            查看结果ToolStripMenuItem.Text = "查看结果";
            查看结果ToolStripMenuItem.Click += 查看结果ToolStripMenuItem_Click;
            // 
            // 输出报告ToolStripMenuItem
            // 
            输出报告ToolStripMenuItem.Name = "输出报告ToolStripMenuItem";
            输出报告ToolStripMenuItem.Size = new Size(98, 28);
            输出报告ToolStripMenuItem.Text = "输出报告";
            输出报告ToolStripMenuItem.Click += 输出报告ToolStripMenuItem_Click;
            // 
            // 关于ToolStripMenuItem
            // 
            关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            关于ToolStripMenuItem.Size = new Size(62, 28);
            关于ToolStripMenuItem.Text = "关于";
            关于ToolStripMenuItem.Click += 关于ToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(644, 470);
            Controls.Add(ClearLog);
            Controls.Add(MainLog);
            Controls.Add(LOG);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(btnStartAdjust);
            Controls.Add(btnDataInspect);
            Controls.Add(btnDataInput);
            Controls.Add(MainTitle);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "水准网平差程序";
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label MainTitle;
        private Button btnDataInput;
        private Button btnDataInspect;
        private Button btnStartAdjust;
        private Button button2;
        private Button button3;
        private RichTextBox LOG;
        private Label MainLog;
        private Button ClearLog;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 数据ToolStripMenuItem;
        private ToolStripMenuItem 数据输入ToolStripMenuItem;
        private ToolStripMenuItem 从文件ToolStripMenuItem;
        private ToolStripMenuItem 手动输入ToolStripMenuItem;
        private ToolStripMenuItem 删除当前数据ToolStripMenuItem;
        private ToolStripMenuItem 数据检查ToolStripMenuItem;
        private ToolStripMenuItem 开始平差ToolStripMenuItem;
        private ToolStripMenuItem 查看结果ToolStripMenuItem;
        private ToolStripMenuItem 输出报告ToolStripMenuItem;
        private ToolStripMenuItem 关于ToolStripMenuItem;
    }
}