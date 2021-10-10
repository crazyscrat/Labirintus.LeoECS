using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Labirintus.ECS
{
    sealed class EcsStartup : MonoBehaviour
    {
        private EcsWorld _world;
        //private EcsSystems _systemsInit;
        private EcsSystems _systemsUpdate;
        //private EcsSystems _systemsFixedUpdate;

        //данные об объектах на сцене
        [SerializeField] private SceneData _sceneData = default;
        [SerializeField] private GameData _gameData = new GameData(); //глобальное хранилище, синглтон
        [SerializeField] private Config _config = default;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private UIData _uiData = default;
        [SerializeField] private UIDebug _uiDebug = default; //тест

        void Start()
        {
            // void can be switched to IEnumerator for support coroutines.

            _world = new EcsWorld();
            _systemsUpdate = new EcsSystems(_world);
            //_systemsInit = new EcsSystems(_world);
            //_systemsFixedUpdate = new EcsSystems(_world);

            Service<GameData>.Set(_gameData);

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systemsUpdate);
#endif

            //_systemsInit
            //    //.Add(new MazeConstructorSystem())

            //    .Inject(_config)
            //    .Inject(_sceneData)
            //    .Inject(_gameData)
            //    .Init();


            _systemsUpdate
                //.Add(new SaveLoadSystem()) //система загрузки-сохранения данных
                .OneFrame<SaveComponent>() //удаляем компонент сохранения

                .Add(new MazeDisposeSystem()) //удаление старого лабиринта
                .Add(new MazeGeneratorSystem()) //генерация нового лабиринта
                .Add(new EnemyGeneratorSystem()) //генерация нового лабиринта
                .Add(new ViewMazeConstructorSystem()) //генерация нового лабиринта
                .OneFrame<MazeDispoze>() //удаляем очистку лабиринта
                .OneFrame<MazeGenerator>() //удаляем генерацию лабиринта

                // register your systems here, for example:
                .Add(new WeaponSystem()) //система оружия, перезарядки
                .Add(new PlayerInitSystem()) //создание игрока

                //.Add(new MoveButtonControlSystem()) //обработка интерфейса, кнопки движения-атаки
                .Add(new MoveJoystickControlSystem()) //обработка интерфейса, кнопки движения-атаки

                //.Add(new PlayerInputSystem()) //получение данных управления
                .Add(new PlayerJoystickSystem()) //получение данных управления

                //.Add(new PlayerMoveSystem()) //движение игрока
                .Add(new PlayerMoveJoystickSystem()) //движение игрока джойстиком
                .Add(new HitSystem()) //обработка столкновений
                .Add(new CameraFollowSystem()) //следование камеры за игроком
                //.Add(new CheckObstacleSystem()) //обработка наличия пути
                //.Add(new CheckEnemySystem()) //обработка наличия врагов

                .Add(new FovSystem()) //система поля обзора

                .Add(new UserControlSystem()) //обработка действий игрока в HUD
                
                //.Add(new InitAttackSystem()) //система пошагового боя
                .Add(new InitRtAttackSystem()) //система реалтайм боя
                //.Add(new CreateFightingEnemySystem()) //инициализируем врага для боя
                
                .Add(new WeaponShootSystem()) //система создания выстрела
                .Add(new SpawnProjectileSystem()) //система создания снарядов
                
                .Add(new ProjectileMoveSystem()) //система движения и попадания снаряда
                .Add(new ProjectileHitSystem()) //система обработки попадания снаряда

                .OneFrame<CheckObstacle>() //удаляем проверку столкновений
                .OneFrame<CheckEnemy>() //удаляем проверку врагов

                .Add(new BattleHealthSystem())
                .Add(new BattleAttackSystem())
                .Add(new BattleDefenseSystem())

                .Add(new DebugControlSystem()) //обработка действий игрока в HUD ТЕСТ

                .Add(new UpdateScreenInfoSystem()) //обновление инфы на экране
                
                //.Add(new EndBattleSystem()) //завершение боя


                // register one-frame components (order is important), for example:

                .OneFrame<PlayerInputEvent>() //удаляем ввод
                //.OneFrame<ButtonMovePressed>() //удаляем ввод с тача
                .OneFrame<JoystickMoved>() //удаляем ввод с джойстика
                //.OneFrame<MoveButtonControl>() //удаляем проверку кнопок управления
                
                .OneFrame<HitEnterComponent>() //удаляем событие столкновения
                .OneFrame<HitExitComponent>() //удаляем событие столкновения
                .OneFrame<ProjectileHit>() //удаляем событие столкновения снаряда
                .OneFrame<NextMazeComponent>() //удаляем событие передода на следующий лабиринт
                .OneFrame<NextMazeButtonViewComponent>() //событие создания нового лабиринта
                
                .OneFrame<StartBattleComponent>() //удаляем компонент начала битвы
                .OneFrame<BattleHealthComponent>() //удаляем компонент лечения
                .OneFrame<BattleAttackComponent>() //удаляем компонент атаки
                .OneFrame<EndBattle>() //удаляем компонент завершения боя 

                .OneFrame<UpdateScreenInfoComponent>() //удаляем компонент обновления главного экрана
                .OneFrame<UpdateBattleScreenInfoComponent>()  //удаляем компонент обновления экрана боя 

                // inject service instances here (order doesn't important), for example:
                .Inject(_sceneData)
                .Inject(_config)
                .Inject(_playerData)
                .Inject(_gameData)
                .Inject(_uiData)
                .Inject(_uiDebug) //тест
                .Init();

            //_systemsFixedUpdate
            //    .Init();
        }

        void Update()
        {
            _systemsUpdate?.Run();
        }

        //private void FixedUpdate()
        //{
        //    _systemsFixedUpdate?.Run();
        //}

        void OnDestroy()
        {
            if (_systemsUpdate != null)
            {
                _systemsUpdate.Destroy();
                _systemsUpdate = null;
                _world.Destroy();
                _world = null;
            }
        }
    }
}