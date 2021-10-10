using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    //система загрузки-сохранения данных игры
    internal sealed class SaveLoadSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerData _playerData;
        private readonly Config _config;

        private EcsFilter<SaveComponent> _filter;

        private string path;

        public void Init()
        {
            //получаем путь к файлу с сохранениями
            path = $"{Application.persistentDataPath}/{_config.fileName}";
            Debug.Log(path);
            //загрузка при старте
            PlayerData load = SaveLoadData.LoadData(path);
            Debug.Log(load);
            if (load != null) _playerData = load; //если не пустой
        }

        public void Run()
        {
            if (_filter.IsEmpty()) return;

            SaveLoadData.SaveData(_playerData, path);
            //сохранение в процессе по компоненту
        }
    }

    internal struct SaveComponent
    {
    }
}