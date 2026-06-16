using System;
using System.Collections.Generic;
using System.Linq;
using kcsj.Models;

namespace kcsj.Services
{
    public class DataCheckResult
    {
        public List<string> Errors { get; } = new();
        public List<string> Warnings { get; } = new();
        public List<string> Infos { get; } = new();

        public bool IsValid => Errors.Count == 0;

        public string ToLogText()
        {
            var lines = new List<string>();

            lines.Add("========== 数据检查结果 ==========");

            foreach (string info in Infos)
            {
                lines.Add("[信息] " + info);
            }

            foreach (string warning in Warnings)
            {
                lines.Add("[警告] " + warning);
            }

            foreach (string error in Errors)
            {
                lines.Add("[错误] " + error);
            }

            lines.Add(IsValid ? "数据检查通过。" : "数据检查未通过。");
            lines.Add("================================");

            return string.Join(Environment.NewLine, lines);
        }
    }

    public static class DataCheck
    {
        public static DataCheckResult Check(
            List<KnownPoint> knownPoints,
            List<Observation> observations)
        {
            var result = new DataCheckResult();

            if (knownPoints == null || knownPoints.Count == 0)
            {
                result.Errors.Add("未导入已知点数据。");
            }
            else
            {
                result.Infos.Add($"已知点数量：{knownPoints.Count}");
            }

            if (observations == null || observations.Count == 0)
            {
                result.Errors.Add("未导入观测数据。");
            }
            else
            {
                result.Infos.Add($"观测数据数量：{observations.Count}");
            }

            if (!result.IsValid)
            {
                return result;
            }

            CheckKnownPoints(knownPoints, result);
            CheckObservations(observations, result);
            CheckNetwork(knownPoints, observations, result);
            CheckRedundancy(knownPoints, observations, result);

            return result;
        }

        private static void CheckKnownPoints(
            List<KnownPoint> knownPoints,
            DataCheckResult result)
        {
            var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < knownPoints.Count; i++)
            {
                KnownPoint point = knownPoints[i];

                if (string.IsNullOrWhiteSpace(point.Name))
                {
                    result.Errors.Add($"第 {i + 1} 个已知点点名为空。");
                    continue;
                }

                string name = point.Name.Trim();

                if (!names.Add(name))
                {
                    result.Errors.Add($"已知点点名重复：{name}");
                }

                if (double.IsNaN(point.Elevation) || double.IsInfinity(point.Elevation))
                {
                    result.Errors.Add($"已知点 {name} 的高程不是有效数字。");
                }
            }
        }

        private static void CheckObservations(
            List<Observation> observations,
            DataCheckResult result)
        {
            var edges = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < observations.Count; i++)
            {
                Observation obs = observations[i];

                if (string.IsNullOrWhiteSpace(obs.FromPoint))
                {
                    result.Errors.Add($"第 {i + 1} 条观测起点为空。");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(obs.ToPoint))
                {
                    result.Errors.Add($"第 {i + 1} 条观测终点为空。");
                    continue;
                }

                string from = obs.FromPoint.Trim();
                string to = obs.ToPoint.Trim();

                if (string.Equals(from, to, StringComparison.OrdinalIgnoreCase))
                {
                    result.Errors.Add($"第 {i + 1} 条观测起点和终点相同：{from}");
                }

                if (double.IsNaN(obs.HeightDiff) || double.IsInfinity(obs.HeightDiff))
                {
                    result.Errors.Add($"第 {i + 1} 条观测高差不是有效数字。");
                }

                if (double.IsNaN(obs.Distance) || double.IsInfinity(obs.Distance))
                {
                    result.Errors.Add($"第 {i + 1} 条观测距离不是有效数字。");
                }
                else if (obs.Distance <= 0)
                {
                    result.Errors.Add($"第 {i + 1} 条观测距离必须大于 0。");
                }

                string edge1 = from + "-" + to;
                string edge2 = to + "-" + from;

                if (edges.Contains(edge1) || edges.Contains(edge2))
                {
                    result.Warnings.Add($"第 {i + 1} 条观测边可能重复：{from} - {to}");
                }
                else
                {
                    edges.Add(edge1);
                }
            }
        }

        private static void CheckNetwork(
            List<KnownPoint> knownPoints,
            List<Observation> observations,
            DataCheckResult result)
        {
            if (!result.IsValid)
            {
                return;
            }

            var knownNames = new HashSet<string>(
                knownPoints.Select(p => p.Name.Trim()),
                StringComparer.OrdinalIgnoreCase);

            var allNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (KnownPoint point in knownPoints)
            {
                allNames.Add(point.Name.Trim());
            }

            foreach (Observation obs in observations)
            {
                allNames.Add(obs.FromPoint.Trim());
                allNames.Add(obs.ToPoint.Trim());
            }

            var graph = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            foreach (string name in allNames)
            {
                graph[name] = new List<string>();
            }

            foreach (Observation obs in observations)
            {
                string from = obs.FromPoint.Trim();
                string to = obs.ToPoint.Trim();

                graph[from].Add(to);
                graph[to].Add(from);
            }

            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var queue = new Queue<string>();

            foreach (string knownName in knownNames)
            {
                if (graph.ContainsKey(knownName))
                {
                    visited.Add(knownName);
                    queue.Enqueue(knownName);
                }
            }

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();

                foreach (string next in graph[current])
                {
                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        queue.Enqueue(next);
                    }
                }
            }

            foreach (string name in allNames)
            {
                if (!visited.Contains(name))
                {
                    result.Errors.Add($"点 {name} 没有与任何已知点连通。");
                }
            }

            int unknownCount = allNames.Count(name => !knownNames.Contains(name));

            result.Infos.Add($"总点数：{allNames.Count}");
            result.Infos.Add($"未知点数：{unknownCount}");
        }

        private static void CheckRedundancy(
            List<KnownPoint> knownPoints,
            List<Observation> observations,
            DataCheckResult result)
        {
            if (!result.IsValid)
            {
                return;
            }

            var knownNames = new HashSet<string>(
                knownPoints.Select(p => p.Name.Trim()),
                StringComparer.OrdinalIgnoreCase);

            var allNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (KnownPoint point in knownPoints)
            {
                allNames.Add(point.Name.Trim());
            }

            foreach (Observation obs in observations)
            {
                allNames.Add(obs.FromPoint.Trim());
                allNames.Add(obs.ToPoint.Trim());
            }

            int unknownCount = allNames.Count(name => !knownNames.Contains(name));
            int observationCount = observations.Count;
            int redundancy = observationCount - unknownCount;

            result.Infos.Add($"多余观测数：r = {observationCount} - {unknownCount} = {redundancy}");

            if (observationCount < unknownCount)
            {
                result.Errors.Add("观测数少于未知点数，无法平差。");
            }
            else if (redundancy == 0)
            {
                result.Warnings.Add("多余观测数为 0，无法进行精度评定。");
            }
        }
    }
}