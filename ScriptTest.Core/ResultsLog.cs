using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ScriptTest.Core
{
    public static class ResultsLog
    {
        private static readonly string LogFile =
            Path.Combine(AppContext.BaseDirectory, "results.jsonl");

        
        public static void Append(
            Dictionary<string, object?> parameters,
            string winner,
            int generations,
            List<Dictionary<string, int>> history)
        {
            var record = new
            {
                timestamp = DateTime.UtcNow.ToString("o"),
                parameters,
                winner,
                generations,
                history
            };

            string json = JsonSerializer.Serialize(record);
            File.AppendAllText(LogFile, json + Environment.NewLine);
        }

        
        public static List<JsonElement> ReadAll()
        {
            var results = new List<JsonElement>();

            if (!File.Exists(LogFile))
                return results;

            foreach (var line in File.ReadAllLines(LogFile))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                using var doc = JsonDocument.Parse(line);
                results.Add(doc.RootElement.Clone());
            }

            return results;
        }
    }
}
