using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kcsj.Services
{
    public static class LogService
    {
        public static event Action<string> OnLog;
        public static void AddLog(string message)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            OnLog?.Invoke($"[{time}] {message}");
        }

    }
}
