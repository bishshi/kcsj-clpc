using System.Text;
using kcsj.Models;

namespace kcsj.Services
{
    public static class Report
    {
        public static string BuildResultText(LeastSquaresResult result)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("========== 平差结果 ==========");
            builder.AppendLine($"观测数：{result.ObservationCount}");
            builder.AppendLine($"未知点数：{result.UnknownCount}");
            builder.AppendLine($"多余观测数：{result.Redundancy}");
            builder.AppendLine($"单位权中误差：{FormatValue(result.Sigma0, 6)}");
            builder.AppendLine();

            builder.AppendLine("一、未知点高程平差值");
            builder.AppendLine("点名\t平差高程(m)\t中误差(m)");
            foreach (KeyValuePair<string, double> item in result.UnknownElevations)
            {
                double error = result.UnknownElevationErrors.TryGetValue(item.Key, out double value)
                    ? value
                    : double.NaN;

                builder.AppendLine($"{item.Key}\t{item.Value:F4}\t{FormatValue(error, 4)}");
            }
            builder.AppendLine();

            builder.AppendLine("二、高差平差值");
            builder.AppendLine("序号\t起点\t终点\t观测高差(m)\t改正数(m)\t平差后高差(m)\t中误差(m)");
            foreach (ObservationAdjustmentResult observationResult in result.ObservationResults)
            {
                builder.AppendLine(
                    $"{observationResult.Index}\t{observationResult.FromPoint}\t{observationResult.ToPoint}\t" +
                    $"{observationResult.ObservedHeightDiff:F6}\t{observationResult.Residual:F6}\t" +
                    $"{observationResult.AdjustedHeightDiff:F6}\t" +
                    $"{FormatValue(observationResult.AdjustedHeightDiffError, 6)}");
            }

            return builder.ToString();
        }

        public static string BuildHeightText(LeastSquaresResult result)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("点名\t高程平差值(m)\t中误差(m)");
            foreach (KeyValuePair<string, double> item in result.UnknownElevations)
            {
                double error = result.UnknownElevationErrors.TryGetValue(item.Key, out double value)
                    ? value
                    : double.NaN;

                builder.AppendLine($"{item.Key}\t{item.Value:F4}\t{FormatValue(error, 4)}");
            }

            return builder.ToString();
        }

        public static string BuildSurveyText(LeastSquaresResult result)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("序号\t起点\t终点\t高差平差值(m)\t中误差(m)");
            foreach (ObservationAdjustmentResult observationResult in result.ObservationResults)
            {
                builder.AppendLine(
                    $"{observationResult.Index}\t{observationResult.FromPoint}\t{observationResult.ToPoint}\t" +
                    $"{observationResult.AdjustedHeightDiff:F6}\t" +
                    $"{FormatValue(observationResult.AdjustedHeightDiffError, 6)}");
            }

            return builder.ToString();
        }

        public static string BuildReportText(
            IReadOnlyList<KnownPoint> knownPoints,
            IReadOnlyList<Observation> observations,
            LeastSquaresResult result)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("水准网平差报告");
            builder.AppendLine($"生成时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            builder.AppendLine();

            builder.AppendLine("一、基本信息");
            builder.AppendLine($"数据来源：{DataStore.DataSource}");
            builder.AppendLine($"已知点数：{knownPoints.Count}");
            builder.AppendLine($"观测数：{result.ObservationCount}");
            builder.AppendLine($"未知点数：{result.UnknownCount}");
            builder.AppendLine($"多余观测数：{result.Redundancy}");
            builder.AppendLine($"单位权中误差：{FormatValue(result.Sigma0, 6)}");
            builder.AppendLine();

            builder.AppendLine("二、已知点数据");
            builder.AppendLine("点名\t高程(m)");
            foreach (KnownPoint point in knownPoints)
            {
                builder.AppendLine($"{point.Name}\t{point.Elevation:F4}");
            }
            builder.AppendLine();

            builder.AppendLine("三、原始观测数据");
            builder.AppendLine("序号\t起点\t终点\t观测高差(m)\t距离(km)");
            for (int i = 0; i < observations.Count; i++)
            {
                Observation observation = observations[i];
                builder.AppendLine(
                    $"{i + 1}\t{observation.FromPoint}\t{observation.ToPoint}\t" +
                    $"{observation.HeightDiff:F6}\t{observation.Distance:F4}");
            }
            builder.AppendLine();

            builder.AppendLine("四、高程成果");
            builder.Append(BuildHeightText(result));
            builder.AppendLine();

            builder.AppendLine("五、高差成果");
            builder.Append(BuildSurveyText(result));
            builder.AppendLine();

            builder.AppendLine("六、观测改正数");
            builder.AppendLine("序号\t起点\t终点\t观测高差(m)\t改正数(m)\t平差后高差(m)\t距离(km)\t权");
            foreach (ObservationAdjustmentResult observationResult in result.ObservationResults)
            {
                builder.AppendLine(
                    $"{observationResult.Index}\t{observationResult.FromPoint}\t{observationResult.ToPoint}\t" +
                    $"{observationResult.ObservedHeightDiff:F6}\t{observationResult.Residual:F6}\t" +
                    $"{observationResult.AdjustedHeightDiff:F6}\t{observationResult.Distance:F4}\t" +
                    $"{observationResult.Weight:F6}");
            }

            return builder.ToString();
        }

        private static string FormatValue(double value, int digits)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return "无法计算";
            }

            return value.ToString($"F{digits}");
        }
    }
}
