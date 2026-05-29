using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RecipeBook
{
    internal static class JsonFileStore
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            IncludeFields = true // FIX: Ensures System.Text.Json can serialize/deserialize private fields safely
        };

        public static List<T> LoadList<T>(string filePath)
        {
            if (!File.Exists(filePath)) return new List<T>();
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
        }

        public static void SaveList<T>(string filePath, List<T> items)
        {
            string? dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            File.WriteAllText(filePath, JsonSerializer.Serialize(items, Options));
        }
    }
}