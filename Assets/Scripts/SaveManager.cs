using System.IO;
using UnityEngine;

public class SaveManager
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    public static void SaveProgress(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            Debug.Log("Saved progress at: " + SavePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Save failed: " + e.Message);
        }
    }

    public static SaveData LoadProgress()
    {       

        if (File.Exists(SavePath))
        {
            Debug.Log("save file exists");
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.Log("new save file");
            return new SaveData();
        }
    }
}
