using System.Collections.Generic;

namespace kcsj.Models
{
    public class LeastSquaresResult
    {
        public int ObservationCount { get; set; }
        public int UnknownCount { get; set; }
        public int Redundancy { get; set; }

        /// <summary>
        /// 单位权中误差
        /// </summary>
        public double Sigma0 { get; set; }

        /// <summary>
        /// 未知点平差高程
        /// </summary>
        public Dictionary<string, double> UnknownElevations { get; } = new();

        /// <summary>
        /// 所有点平差后高程，包括已知点和未知点
        /// </summary>
        public Dictionary<string, double> AdjustedElevations { get; } = new();

        /// <summary>
        /// 未知点高程中误差
        /// </summary>
        public Dictionary<string, double> UnknownElevationErrors { get; } = new();

        /// <summary>
        /// 每条观测的改正数 v
        /// </summary>
        public List<double> Residuals { get; } = new();

        /// <summary>
        /// 每条观测的平差后高差
        /// </summary>
        public List<double> AdjustedHeightDiffs { get; } = new();

        /// <summary>
        /// 每条观测的详细结果
        /// </summary>
        public List<ObservationAdjustmentResult> ObservationResults { get; } = new();
    }

    public class ObservationAdjustmentResult
    {
        public int Index { get; set; }

        public string FromPoint { get; set; } = "";
        public string ToPoint { get; set; } = "";

        public double ObservedHeightDiff { get; set; }
        public double Distance { get; set; }
        public double Weight { get; set; }

        /// <summary>
        /// 改正数 v
        /// </summary>
        public double Residual { get; set; }

        /// <summary>
        /// 平差后高差 h + v
        /// </summary>
        public double AdjustedHeightDiff { get; set; }
    }
}