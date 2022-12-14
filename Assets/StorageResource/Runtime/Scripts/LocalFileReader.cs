using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

namespace Wolffun.StorageResource
{
    public static class LocalFileManager
    {
        public static T ReadDataFromLocalFile<T>(string filePath)
        {
            bool isHasFile = false;
#if UNITY_WEBGL
            isHasFile = PlayerPrefs.HasKey(filePath);
#else
            isHasFile = File.Exists(filePath);
#endif

            if (isHasFile)
            {
                try
                {
                    string savedData = string.Empty;

#if UNITY_WEBGL
                    savedData = PlayerPrefs.GetString(dataLink);
#else
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(filePath, FileMode.Open);
                    savedData = (string)bf.Deserialize(file);
                    file.Close();
                    file = null;
#endif
                    T loadedData = JsonConvert.DeserializeObject<T>(savedData);
                    
                    return loadedData;
                }
                catch (Exception ex)
                {
                    Debug.LogError("Load file throw exception " + ex.Message);
                    return default;
                }
            }
            else
            {
                return default;

            }
        }

        public static void WriteDataToLocalFile<T>(string filePath, T data)
        {
            try
            {
                string saveData = JsonConvert.SerializeObject(data);

#if UNITY_WEBGL
                PlayerPrefs.SetString(filePath, saveData);
                PlayerPrefs.Save();
#else
                BinaryFormatter bf = new BinaryFormatter();

                FileStream file = File.Open(filePath, FileMode.OpenOrCreate);
                bf.Serialize(file, saveData);
                file.Close();
                file = null;
#endif
            }
            catch (Exception ex)
            {
                Debug.LogError("Save data --" + data.GetType() + "-- is error: " + ex.GetBaseException() + "\n" + ex.StackTrace);
            }
        }
    }
}