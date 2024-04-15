using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Wxy.Tool { 
public sealed class SuperSaveData
{

    public static void SaveData<T>(string key, T value, DataFormat dataFormat = DataFormat.Binary)
    {

        try
        {
            string str = ".bytes";
            if (dataFormat == DataFormat.Binary)
            {
                str = ".bytes";
            }
            if (dataFormat == DataFormat.JSON)
            {
                str = ".json";
            }
            byte[] data = SerializationUtility.SerializeValue(value, dataFormat);

            // Get the persistent data path
            string directoryPath = DataTwo;


            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, $"{key}{str}");
            Debug.Log(filePath);
            // Save the bytes to the persistent data path
            File.WriteAllBytes(filePath, data);
        }
        catch (Exception e)
        {
            Debug.LogError($"SaveData Exception: {key}保存失败");
        }
    }

    public static T LoadData<T>(string key, T defaultValue = default, DataFormat dataFormat = DataFormat.Binary)
        {
        try
        {
            string str = ".bytes";
            if (dataFormat == DataFormat.Binary)
            {
                str = ".bytes";
            }
            if (dataFormat == DataFormat.JSON)
            {
                str = ".json";
            }
            // Get the persistent data path
            string directoryPath = Path.Combine(Application.persistentDataPath, "SuperSaveData");
            string filePath = Path.Combine(directoryPath, $"{key}{str}");

            // Read the bytes from the file
            byte[] data = File.ReadAllBytes(filePath);
            Debug.Log(filePath);
            // Deserialize the bytes to a PlayerData object
            return SerializationUtility.DeserializeValue<T>(data, dataFormat);
        }
        catch (Exception e)
        {
                Debug.Log($"{key}表示加载失败");
            return defaultValue; // 返回默认值，表示加载失败
        }
    }
    public static void DeleteData(string key, DataFormat dataFormat = DataFormat.Binary)
    {
        try
        {
            string str = ".bytes";
            if (dataFormat == DataFormat.Binary)
            {
                str = ".bytes";
            }
            if (dataFormat == DataFormat.JSON)
            {
                str = ".json";
            }

            // Get the persistent data path
            string directoryPath = Path.Combine(Application.persistentDataPath, "SuperSaveData");
            string filePath = Path.Combine(directoryPath, $"{key}{str}");

            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Delete the file
                File.Delete(filePath);
                Debug.Log($"DeleteData: {key}删除成功");

            }
            else
            {
                Debug.LogWarning($"DeleteData: {key}文件不存在，无需删除");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"DeleteData Exception: {key}删除失败 - {e.Message}");
        }
    }



    public static string AbsoluteDirectoryPath = Path.Combine(Application.persistentDataPath, "SuperSaveData");
    public static string DataOne = Path.Combine(AbsoluteDirectoryPath, "One");
    public static string DataTwo = Path.Combine(AbsoluteDirectoryPath, "Two");
    public const string FileName = "U1587wea";
    public static string FileFullName = Path.Combine(DataOne, FileName);
    public static DataFormat DataFormat = DataFormat.Binary;

    private static Dictionary<string, byte[]> KeyValues = new Dictionary<string, byte[]>();

    static SuperSaveData()
    {
        Debug.Log("Initializing SuperSaveData");
        if (!Directory.Exists(DataOne))
            Directory.CreateDirectory(DataOne);
        Load();
    }


    private static void Save()
    {
        byte[] bytes = SerializationUtility.SerializeValue(KeyValues, DataFormat);
        File.WriteAllBytes(FileFullName, bytes);
    }

    private static void Load()
    {
        KeyValues = File.Exists(FileFullName)
            ? SerializationUtility.DeserializeValue<Dictionary<string, byte[]>>(File.ReadAllBytes(FileFullName), DataFormat)
            : new Dictionary<string, byte[]>();
    }

    public static void DeleteAll()
    {
        KeyValues.Clear();
        Save();
    }

    public static void DeleteKey(string key)
    {
        if (KeyValues.Remove(key))
            Save();
        else
            Debug.LogWarning($"删除失败，没有找到指定Key:{key}");
    }

    public static T Get<T>(string key, T defaultValue = default)
    {
        if (KeyValues.TryGetValue(key, out byte[] bytes))
        {
            return SerializationUtility.DeserializeValue<T>(bytes, DataFormat);
        }
        else
        {
            return defaultValue;
        }
    }

    public static bool HasKey(string key)
    {

        if (KeyValues == null)
        {
            Debug.LogError("KeyValues is null. Did LoadData() method fail?");
            return false;
        }
        return KeyValues.ContainsKey(key);
    }

    public static void Set<T>(string key, T value)
    {
        byte[] data = SerializationUtility.SerializeValue(value, DataFormat);
        if (HasKey(key))
        {
            KeyValues[key] = data;
        }
        else
        {
            KeyValues.Add(key, data);
        }
        Save();
    }

}
}