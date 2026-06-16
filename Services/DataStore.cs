using System;
using System.Collections.Generic;
using System.Linq;
using kcsj.Models;

namespace kcsj.Services
{
    public static class DataStore
    {
        public static List<KnownPoint> KnownPoints { get; private set; } = new();
        public static List<Observation> Observations { get; private set; } = new();

        public static string DataSource { get; private set; } = "";

        public static bool HasData
        {
            get
            {
                return KnownPoints.Count > 0 && Observations.Count > 0;
            }
        }

        public static void SetData(
            List<KnownPoint> knownPoints,
            List<Observation> observations,
            string dataSource)
        {
            if (knownPoints == null || knownPoints.Count == 0)
            {
                throw new ArgumentException("已知点数据为空，不能保存数据。");
            }

            if (observations == null || observations.Count == 0)
            {
                throw new ArgumentException("观测数据为空，不能保存数据。");
            }

            KnownPoints = knownPoints.ToList();
            Observations = observations.ToList();
            DataSource = dataSource;

            LogService.AddLog($"数据已更新，来源：{dataSource}");
            LogService.AddLog($"已知点数量：{KnownPoints.Count}");
            LogService.AddLog($"观测数据数量：{Observations.Count}");
        }

        public static void Clear()
        {
            KnownPoints.Clear();
            Observations.Clear();
            DataSource = "";

            LogService.AddLog("数据已清空。");
        }
    }
}