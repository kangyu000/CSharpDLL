using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinaryLib
{
    public static class BinaryClass
    {
        public static void SavePublicMusicInfo(object obj, string FilePath)
        {
            if (File.Exists(FilePath) == true)
            {
                File.Delete(FilePath);
            }
            File.Create(FilePath).Close();
            Stream steam = File.Open(FilePath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(steam, obj);
            steam.Close();
            GC.Collect();
        }

        public static object ReadSystem(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                Stream steam2 = File.Open(FilePath, FileMode.Open,FileAccess.ReadWrite);
                BinaryFormatter bf2 = new BinaryFormatter();
                object o=bf2.Deserialize(steam2);
                steam2.Close();
                return o;
            }
            else 
            {
                return null;
            }
        }
    }
}
