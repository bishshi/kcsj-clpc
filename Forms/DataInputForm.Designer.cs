namespace kcsj.Forms
{
    partial class DataInputForm
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
            DataFromFiles = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // DataFromFiles
            // 
            DataFromFiles.Location = new Point(57, 34);
            DataFromFiles.Name = "DataFromFiles";
            DataFromFiles.Size = new Size(165, 68);
            DataFromFiles.TabIndex = 0;
            DataFromFiles.Text = "从文件读取";
            DataFromFiles.UseVisualStyleBackColor = true;
            DataFromFiles.Click += DataFromFiles_Click;
            // 
            // button2
            // 
            button2.Location = new Point(57, 119);
            button2.Name = "button2";
            button2.Size = new Size(165, 74);
            button2.TabIndex = 1;
            button2.Text = "手动输入";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // DataInputForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(288, 218);
            Controls.Add(button2);
            Controls.Add(DataFromFiles);
            MaximizeBox = false;
            Name = "DataInputForm";
            Text = "数据输入";
            ResumeLayout(false);
        }

        #endregion

        private Button DataFromFiles;
        private Button button2;
    }
}