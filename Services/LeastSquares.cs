using System;
using System.Collections.Generic;
using System.Linq;
using kcsj.Models;

namespace kcsj.Services
{
    public static class LeastSquaresAdjustmentService
    {
        /// <summary>
        /// 水准网间接平差
        /// 
        /// 观测关系：
        /// h_AB = H_B - H_A
        /// 
        /// 误差方程：
        /// v = Bx - L
        /// 
        /// 法方程：
        /// N x = W
        /// N = B^T P B
        /// W = B^T P L
        /// x = N^-1 W
        /// </summary>
        public static LeastSquaresResult Adjust(
            List<KnownPoint> knownPoints,
            List<Observation> observations)
        {
            if (knownPoints == null || knownPoints.Count == 0)
            {
                throw new ArgumentException("已知点不能为空。");
            }

            if (observations == null || observations.Count == 0)
            {
                throw new ArgumentException("观测数据不能为空。");
            }

            Dictionary<string, double> knownElevations =
                knownPoints.ToDictionary(
                    p => p.Name.Trim(),
                    p => p.Elevation,
                    StringComparer.OrdinalIgnoreCase);

            HashSet<string> allPointNames = new(StringComparer.OrdinalIgnoreCase);

            foreach (Observation obs in observations)
            {
                if (string.IsNullOrWhiteSpace(obs.FromPoint) ||
                    string.IsNullOrWhiteSpace(obs.ToPoint))
                {
                    throw new ArgumentException("观测数据中存在空点名。");
                }

                allPointNames.Add(obs.FromPoint.Trim());
                allPointNames.Add(obs.ToPoint.Trim());
            }

            List<string> unknownNames = allPointNames
                .Where(name => !knownElevations.ContainsKey(name))
                .OrderBy(name => name)
                .ToList();

            if (unknownNames.Count == 0)
            {
                throw new InvalidOperationException("没有未知点，不需要进行间接平差。");
            }

            int observationCount = observations.Count;
            int unknownCount = unknownNames.Count;

            if (observationCount < unknownCount)
            {
                throw new InvalidOperationException("观测数小于未知数，无法进行最小二乘平差。");
            }

            Dictionary<string, int> unknownIndex = new(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < unknownNames.Count; i++)
            {
                unknownIndex[unknownNames[i]] = i;
            }

            MatrixOperations B = new MatrixOperations(observationCount, unknownCount);
            MatrixOperations L = new MatrixOperations(observationCount, 1);
            MatrixOperations P = new MatrixOperations(observationCount, observationCount);

            double[] weights = new double[observationCount];

            for (int i = 0; i < observationCount; i++)
            {
                Observation obs = observations[i];

                string from = obs.FromPoint.Trim();
                string to = obs.ToPoint.Trim();

                // h_AB = H_B - H_A
                // FromPoint 是未知点，系数为 -1
                if (unknownIndex.ContainsKey(from))
                {
                    B[i, unknownIndex[from]] = -1.0;
                }

                // ToPoint 是未知点，系数为 +1
                if (unknownIndex.ContainsKey(to))
                {
                    B[i, unknownIndex[to]] = 1.0;
                }

                // 已知点贡献：H_B(已知) - H_A(已知)
                double knownContribution = 0.0;

                if (knownElevations.ContainsKey(to))
                {
                    knownContribution += knownElevations[to];
                }

                if (knownElevations.ContainsKey(from))
                {
                    knownContribution -= knownElevations[from];
                }

                // L = h观测 - 已知点贡献
                L[i, 0] = obs.HeightDiff - knownContribution;

                // 水准测量常用定权：p = 1 / S
                // 距离越长，权越小
                double weight;

                if (obs.Distance <= 0)
                {
                    weight = 1.0;
                }
                else
                {
                    weight = 1.0 / obs.Distance;
                }

                P[i, i] = weight;
                weights[i] = weight;
            }

            MatrixOperations Bt = B.Transpose();

            MatrixOperations N = Bt.Multiply(P).Multiply(B);
            MatrixOperations W = Bt.Multiply(P).Multiply(L);

            MatrixOperations Qxx = N.Inverse();
            MatrixOperations X = Qxx.Multiply(W);

            // v = Bx - L
            MatrixOperations V = B.Multiply(X).Sub(L);

            // V^T P V
            MatrixOperations VtPV = V.Transpose().Multiply(P).Multiply(V);

            int redundancy = observationCount - unknownCount;

            double sigma0;

            if (redundancy > 0)
            {
                sigma0 = Math.Sqrt(VtPV[0, 0] / redundancy);
            }
            else
            {
                sigma0 = double.NaN;
            }

            LeastSquaresResult result = new LeastSquaresResult
            {
                ObservationCount = observationCount,
                UnknownCount = unknownCount,
                Redundancy = redundancy,
                Sigma0 = sigma0
            };

            foreach (KnownPoint point in knownPoints)
            {
                result.AdjustedElevations[point.Name.Trim()] = point.Elevation;
            }

            for (int i = 0; i < unknownCount; i++)
            {
                string pointName = unknownNames[i];
                double elevation = X[i, 0];

                result.UnknownElevations[pointName] = elevation;
                result.AdjustedElevations[pointName] = elevation;

                if (double.IsNaN(sigma0))
                {
                    result.UnknownElevationErrors[pointName] = double.NaN;
                }
                else
                {
                    result.UnknownElevationErrors[pointName] =
                        sigma0 * Math.Sqrt(Math.Abs(Qxx[i, i]));
                }
            }

            for (int i = 0; i < observationCount; i++)
            {
                Observation obs = observations[i];

                double residual = V[i, 0];
                double adjustedHeightDiff = obs.HeightDiff + residual;
                double adjustedHeightDiffError = double.NaN;

                if (!double.IsNaN(sigma0))
                {
                    double q = 0.0;

                    for (int row = 0; row < unknownCount; row++)
                    {
                        for (int column = 0; column < unknownCount; column++)
                        {
                            q += B[i, row] * Qxx[row, column] * B[i, column];
                        }
                    }

                    adjustedHeightDiffError = sigma0 * Math.Sqrt(Math.Abs(q));
                }

                result.Residuals.Add(residual);
                result.AdjustedHeightDiffs.Add(adjustedHeightDiff);

                result.ObservationResults.Add(new ObservationAdjustmentResult
                {
                    Index = i + 1,
                    FromPoint = obs.FromPoint,
                    ToPoint = obs.ToPoint,
                    ObservedHeightDiff = obs.HeightDiff,
                    Distance = obs.Distance,
                    Weight = weights[i],
                    Residual = residual,
                    AdjustedHeightDiff = adjustedHeightDiff,
                    AdjustedHeightDiffError = adjustedHeightDiffError
                });
            }

            LogService.AddLog("间接平差计算完成。");
            LogService.AddLog($"观测数：{observationCount}");
            LogService.AddLog($"未知点数：{unknownCount}");
            LogService.AddLog($"多余观测数：{redundancy}");

            if (double.IsNaN(sigma0))
            {
                LogService.AddLog("单位权中误差：无法计算，多余观测数为 0。");
            }
            else
            {
                LogService.AddLog($"单位权中误差：{sigma0:F6}");
            }

            return result;
        }
    }
}
