using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, "savegame.json"); }
    }

    public static bool Exists()
    {
        return File.Exists(FilePath);
    }

    public static void Save(SaveData data)
    {
        string tempPath = FilePath + ".tmp";
        File.WriteAllText(tempPath, JsonUtility.ToJson(data, true));
        if (File.Exists(FilePath))
        {
            File.Replace(tempPath, FilePath, null);
        }
        else
        {
            File.Move(tempPath, FilePath);
        }
    }

    public static SaveData Load()
    {
        if (!Exists())
        {
            return null;
        }
        try
        {
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    public static void Delete()
    {
        if (Exists())
        {
            File.Delete(FilePath);
        }
    }
}
