using System.IO;
using UnityEngine;

namespace Systems.SaveSystem
{
    public static class SaveSystem
    {
        private static string saveFileName = "savedata.json";

        
        public static void SaveGame(GameData data)
        {
            string json = JsonUtility.ToJson(data, true);
            
            string path = Path.Combine(Application.persistentDataPath, saveFileName);
            
            File.WriteAllText(path, json);
            
        }
        
        public static GameData LoadGame()
        {
            string path = Path.Combine(Application.persistentDataPath, saveFileName);
            
            if (!File.Exists(path))
            {
                return null;
            }
            
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);
        
            return data;
        }
    }
}