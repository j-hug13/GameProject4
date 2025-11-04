using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace GameProject4
{
    public static class SaveGame
    {
        private static string filePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "save.json");

        public static void Save(GameSaveData state)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(state, options);
            File.WriteAllText(filePath, json);
        }

        public static GameSaveData Load()
        {
            if(File.Exists(filePath) == false)
            {
                return null;
            }
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameSaveData>(json);
        }

        public static bool SaveExists() => File.Exists(filePath);
    }
}
