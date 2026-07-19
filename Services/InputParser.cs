using System.Globalization;
using System.Text;
using kcsj.Models;

namespace kcsj.Services;

public static class InputParser
{
    private static readonly char[] Separators = [' ', '\t', ',', '，'];

    public static List<KnownPoint> ReadKnownPointsFile(string filePath) =>
        ParseKnownPoints(File.ReadLines(filePath, Encoding.UTF8), Path.GetFileName(filePath));

    public static List<Observation> ReadObservationsFile(string filePath) =>
        ParseObservations(File.ReadLines(filePath, Encoding.UTF8), Path.GetFileName(filePath));

    public static List<KnownPoint> ParseKnownPointsText(string text) =>
        ParseKnownPoints(SplitText(text), "手动输入");

    public static List<Observation> ParseObservationsText(string text) =>
        ParseObservations(SplitText(text), "手动输入");

    public static string BuildKnownPointsPreview(IEnumerable<KnownPoint> points)
    {
        StringBuilder text = new("点名\t高程(m)\n");
        foreach (KnownPoint point in points)
        {
            text.AppendLine($"{point.Name}\t{point.Elevation:F4}");
        }

        return text.ToString();
    }

    public static string BuildObservationsPreview(IEnumerable<Observation> observations)
    {
        StringBuilder text = new("起点\t终点\t高差(m)\t距离(km)\n");
        foreach (Observation observation in observations)
        {
            text.AppendLine(
                $"{observation.FromPoint}\t{observation.ToPoint}\t" +
                $"{observation.HeightDiff:F6}\t{observation.Distance:F4}");
        }

        return text.ToString();
    }

    private static List<KnownPoint> ParseKnownPoints(IEnumerable<string> lines, string sourceName)
    {
        List<KnownPoint> result = [];
        HashSet<string> names = new(StringComparer.OrdinalIgnoreCase);
        int lineNumber = 0;

        foreach (string rawLine in lines)
        {
            lineNumber++;
            string line = rawLine.Trim();
            if (ShouldSkip(line))
            {
                continue;
            }

            string[] parts = SplitLine(line);
            if (parts.Length != 2)
            {
                throw new FormatException($"{sourceName} 第 {lineNumber} 行格式错误，应为：点名 高程");
            }

            string name = parts[0].Trim();
            if (!names.Add(name))
            {
                throw new FormatException($"{sourceName} 第 {lineNumber} 行点名重复：{name}");
            }

            if (!TryParseNumber(parts[1], out double elevation))
            {
                throw new FormatException($"{sourceName} 第 {lineNumber} 行高程不是有效数字");
            }

            result.Add(new KnownPoint(name, elevation));
        }

        if (result.Count == 0)
        {
            throw new FormatException($"{sourceName} 中没有有效的已知点数据");
        }

        return result;
    }

    private static List<Observation> ParseObservations(IEnumerable<string> lines, string sourceName)
    {
        List<Observation> result = [];
        int lineNumber = 0;

        foreach (string rawLine in lines)
        {
            lineNumber++;
            string line = rawLine.Trim();
            if (ShouldSkip(line))
            {
                continue;
            }

            string[] parts = SplitLine(line);
            if (parts.Length != 4)
            {
                throw new FormatException($"{sourceName} 第 {lineNumber} 行格式错误，应为：起点 终点 高差 距离");
            }

            string from = parts[0].Trim();
            string to = parts[1].Trim();
            if (string.Equals(from, to, StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException($"{sourceName} 第 {lineNumber} 行起点和终点不能相同");
            }

            if (!TryParseNumber(parts[2], out double heightDiff))
            {
                throw new FormatException($"{sourceName} 第 {lineNumber} 行高差不是有效数字");
            }

            if (!TryParseNumber(parts[3], out double distance) || distance <= 0)
            {
                throw new FormatException($"{sourceName} 第 {lineNumber} 行距离必须是大于 0 的数字");
            }

            result.Add(new Observation(from, to, heightDiff, distance));
        }

        if (result.Count == 0)
        {
            throw new FormatException($"{sourceName} 中没有有效的观测数据");
        }

        return result;
    }

    private static string[] SplitText(string text) =>
        text.Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\r', '\n').Split('\n');

    private static string[] SplitLine(string line) =>
        line.Split(Separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    private static bool ShouldSkip(string line) =>
        string.IsNullOrWhiteSpace(line) || line.StartsWith('#') || line.StartsWith("//", StringComparison.Ordinal);

    private static bool TryParseNumber(string text, out double value) =>
        double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) ||
        double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
}
