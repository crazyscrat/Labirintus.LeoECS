using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Labirintus.ECS
{
    internal sealed class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = default;
        private readonly SceneData _sceneData;
        private readonly PlayerData _playerData;
        private readonly UIDebug _uiDebug;

        public void Init()
        {
            var view = _sceneData._playerView; //получаем ссылку на компонент игрока
            var playerEntity = _world.NewEntity(); //создаем новую сущность
            ref var playerRef = ref playerEntity.Get<PlayerRef>(); //добавляем основной компонент игрока

            playerRef.View = view; //сохраняем на игрока ссылку, потом получим трансформ и т.д.
            playerRef.transform = view.transform;
            playerRef.collider2D = view.GetComponentInChildren<BoxCollider2D>();
            playerRef.rb = view.GetComponentInChildren<Rigidbody2D>();
            playerRef.animator = view.GetComponentInChildren<Animator>();
            playerRef.transformFOV = view.transformFOV;
            playerRef.viewMeshFilterFOV = view.viewMeshFilterFOV;
            playerRef.obstacleMaskFOV = view.obstacleMaskFOV;
            playerRef.behindFOV = view.targetMaskFOV;
            view.EcsWorld = _world;
            view.Entity = playerEntity; //сохраняем в игроке ссылку на его сущность

            //добавляем оружие игроку
            var weaponEntity = _world.NewEntity();
            var weaponView = view.GetComponentInChildren<WeaponSettings>();
            ref var weapon = ref weaponEntity.Get<Weapon>();
            weapon.owner = playerEntity;
            weapon.projectilePrefab = weaponView.projectilePrefab;
            weapon.projectileRadius = weaponView.projectileRadius;
            weapon.projectileSocket = weaponView.projectileSocket;
            weapon.projectileSpeed = weaponView.projectileSpeed;
            weapon.weaponDamage = weaponView.weaponDamage;
            weapon.reloadTime = weaponView.reloadTime;

            //_uiDebug.textDebug.text = _playerData.name;

            //при самом первом запуске игры
            if (_playerData.FirstStart)
            {
                _playerData.FirstStart = false;
            _uiDebug.textDebug.text = "Новая игра = " + _playerData.FirstStart;
                _playerData.HealthPointPlayer = 20; //_gameStateData.MaxHealthAmountPlayer;
                _playerData.DefensePlayer = _playerData.MaxDefensePlayer;
                _playerData.PowerBlueAmountPlayer = 0;
                _playerData.PowerGreenAmountPlayer = 0;
                _playerData.PowerPinkAmountPlayer = 0;
                _playerData.CoinsAmountPlayer = 0;
                _playerData.LevelPlayer = 0;
                _playerData.Damage = 10;
            }

            Vector2 startPos = Service<GameData>.Get().startPosition;
            Service<GameData>.Get().currentPlayerPosition = new Vector2Int((int)startPos.x, (int)startPos.y);
            playerRef.transform.position = new Vector3(startPos.x, startPos.y, 0f); //ставим игрока на старт

            _world.NewEntity().Get<CheckObstacle>();
        }

    }

    //компонент на сущности, хранит ссылку на игрока
    internal struct PlayerRef
    {
        public PlayerView View;
        public Transform transform;
        public BoxCollider2D collider2D;
        public Rigidbody2D rb;
        public Animator animator;

        //для поля обзора
        public LayerMask behindFOV; //скрываемые объекты
        public LayerMask obstacleMaskFOV; //препятствия
        public Transform transformFOV; //трансформ центральной точки
        public MeshFilter viewMeshFilterFOV; //мешфильтр поля        
    }
}