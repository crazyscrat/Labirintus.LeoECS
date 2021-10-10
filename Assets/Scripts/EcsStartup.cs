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

        //������ �� �������� �� �����
        [SerializeField] private SceneData _sceneData = default;
        [SerializeField] private GameData _gameData = new GameData(); //���������� ���������, ��������
        [SerializeField] private Config _config = default;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private UIData _uiData = default;
        [SerializeField] private UIDebug _uiDebug = default; //����

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
                //.Add(new SaveLoadSystem()) //������� ��������-���������� ������
                .OneFrame<SaveComponent>() //������� ��������� ����������

                .Add(new MazeDisposeSystem()) //�������� ������� ���������
                .Add(new MazeGeneratorSystem()) //��������� ������ ���������
                .Add(new EnemyGeneratorSystem()) //��������� ������ ���������
                .Add(new ViewMazeConstructorSystem()) //��������� ������ ���������
                .OneFrame<MazeDispoze>() //������� ������� ���������
                .OneFrame<MazeGenerator>() //������� ��������� ���������

                // register your systems here, for example:
                .Add(new WeaponSystem()) //������� ������, �����������
                .Add(new PlayerInitSystem()) //�������� ������

                //.Add(new MoveButtonControlSystem()) //��������� ����������, ������ ��������-�����
                .Add(new MoveJoystickControlSystem()) //��������� ����������, ������ ��������-�����

                //.Add(new PlayerInputSystem()) //��������� ������ ����������
                .Add(new PlayerJoystickSystem()) //��������� ������ ����������

                //.Add(new PlayerMoveSystem()) //�������� ������
                .Add(new PlayerMoveJoystickSystem()) //�������� ������ ����������
                .Add(new HitSystem()) //��������� ������������
                .Add(new CameraFollowSystem()) //���������� ������ �� �������
                //.Add(new CheckObstacleSystem()) //��������� ������� ����
                //.Add(new CheckEnemySystem()) //��������� ������� ������

                .Add(new FovSystem()) //������� ���� ������

                .Add(new UserControlSystem()) //��������� �������� ������ � HUD
                
                //.Add(new InitAttackSystem()) //������� ���������� ���
                .Add(new InitRtAttackSystem()) //������� �������� ���
                //.Add(new CreateFightingEnemySystem()) //�������������� ����� ��� ���
                
                .Add(new WeaponShootSystem()) //������� �������� ��������
                .Add(new SpawnProjectileSystem()) //������� �������� ��������
                
                .Add(new ProjectileMoveSystem()) //������� �������� � ��������� �������
                .Add(new ProjectileHitSystem()) //������� ��������� ��������� �������

                .OneFrame<CheckObstacle>() //������� �������� ������������
                .OneFrame<CheckEnemy>() //������� �������� ������

                .Add(new BattleHealthSystem())
                .Add(new BattleAttackSystem())
                .Add(new BattleDefenseSystem())

                .Add(new DebugControlSystem()) //��������� �������� ������ � HUD ����

                .Add(new UpdateScreenInfoSystem()) //���������� ���� �� ������
                
                //.Add(new EndBattleSystem()) //���������� ���


                // register one-frame components (order is important), for example:

                .OneFrame<PlayerInputEvent>() //������� ����
                //.OneFrame<ButtonMovePressed>() //������� ���� � ����
                .OneFrame<JoystickMoved>() //������� ���� � ���������
                //.OneFrame<MoveButtonControl>() //������� �������� ������ ����������
                
                .OneFrame<HitEnterComponent>() //������� ������� ������������
                .OneFrame<HitExitComponent>() //������� ������� ������������
                .OneFrame<ProjectileHit>() //������� ������� ������������ �������
                .OneFrame<NextMazeComponent>() //������� ������� �������� �� ��������� ��������
                .OneFrame<NextMazeButtonViewComponent>() //������� �������� ������ ���������
                
                .OneFrame<StartBattleComponent>() //������� ��������� ������ �����
                .OneFrame<BattleHealthComponent>() //������� ��������� �������
                .OneFrame<BattleAttackComponent>() //������� ��������� �����
                .OneFrame<EndBattle>() //������� ��������� ���������� ��� 

                .OneFrame<UpdateScreenInfoComponent>() //������� ��������� ���������� �������� ������
                .OneFrame<UpdateBattleScreenInfoComponent>()  //������� ��������� ���������� ������ ��� 

                // inject service instances here (order doesn't important), for example:
                .Inject(_sceneData)
                .Inject(_config)
                .Inject(_playerData)
                .Inject(_gameData)
                .Inject(_uiData)
                .Inject(_uiDebug) //����
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