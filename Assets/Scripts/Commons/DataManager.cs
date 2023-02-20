using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Commons
{

    public static class DataManager
    {
        public static void Save(string fileName, string location, object data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + location;
            Directory.CreateDirectory(path);
            FileStream file = File.Open(path + fileName, FileMode.OpenOrCreate);
            try
            {
                bf.Serialize(file, data);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {

                file.Close();

            }
        }

        public static T Load<T>(string fileName, string location)
        {
            FileStream file = null;
            try
            {
                string path = Application.persistentDataPath + "/" + location + fileName;
                file = File.Open(path, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(file);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
                return default(T);
            }
            finally
            {
                file?.Close();

            }
        }
    }
}
