using kcsj.Models;
using kcsj.Services;

namespace kcsj.Forms
{
    public partial class ResultForm : Form
    {
        private RichTextBox resultTextBox = null!;

        public ResultForm(LeastSquaresResult result)
        {
            InitializeComponent();
            resultTextBox.Text = Report.BuildResultText(result);
        }

        private void InitializeComponent()
        {
            Text = "平差结果";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(820, 560);
            MinimumSize = new Size(680, 420);

            resultTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 10.5F),
                WordWrap = false
            };

            Controls.Add(resultTextBox);
        }
    }
}
