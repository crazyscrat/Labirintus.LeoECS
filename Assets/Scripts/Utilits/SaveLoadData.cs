using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Labirintus.ECS
{
    internal class SaveLoadData
    {
        //загрузка данных
        internal static PlayerData LoadData(string path)
        {
            if (File.Exists(path))
            {
                //формат для работы с бинарным файлом
                BinaryFormatter formatter = new BinaryFormatter();
                //поток для чтения файла
                FileStream file = File.Open(path, FileMode.Open);

                //читаем из бинарного файла
                PlayerData data = formatter.Deserialize(file) as PlayerData;
                //закрываем файл
                file.Close();

                return data;
            }

            return null;
        }

        //сохранение данных
        internal static void SaveData(PlayerData _playerData, string path)
        {
            //формат для работы с бинарным файлом
            BinaryFormatter formatter = new BinaryFormatter();
            //поток для записи файла
            FileStream file = File.Open(path, FileMode.OpenOrCreate);

            //создаем копию данных
            PlayerData copy = _playerData;
            //записываем в бинарный файл
            formatter.Serialize(file, copy);
            //закрываем файл
            file.Close();
        }
    }
}
