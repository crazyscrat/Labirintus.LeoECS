using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    /// <summary>
    /// Система обработки движения игрока
    /// </summary>
    internal sealed class PlayerMoveSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        private EcsSystems _systemsInit;
        private GameData _gameData = default;

        private EcsFilter<PlayerRef, PlayerInputEvent> _filter = default; //при вводе
        private EcsFilter<PlayerMoving> _filterMoving = default; //во время движения
        private EcsFilter<PlayerRef> _filterPlayer = default; //игрок
        private Config _config = default;

        private Vector3 newPosition = Vector3.zero;
        private Vector3 directionMoving = Vector3.zero;

        public void Run()
        {
            //если игрок не в движении
            if (_filterMoving.IsEmpty())
            {
                if (_gameData.NewGame) //если новая игра
                {
                    Vector2 startPos = _gameData.startPosition;
                    _gameData.currentPlayerPosition = new Vector2Int((int)startPos.x, (int)startPos.y);
                    _filter.Get1(0).transform.position = new Vector3(startPos.x, startPos.y, 0f); //ставим игрока на старт

                    _gameData.NewGame = false;
                    _world.NewEntity().Get<CheckObstacle>(); //добавляем компонент для проверки следующего хода

                    //устанавливаем анимацию покоя на старте
                    ref var player = ref _filterPlayer.Get1(0);
                    player.animator.SetInteger("Idle", 3);
                    player.animator.SetLayerWeight(0, 0f); //устанавливаем слой движения вторичным
                    player.animator.SetLayerWeight(1, 1f); //устанавливаем слой покоя основным
                }

                foreach (var i in _filter)
                {
                    ref var playerRef = ref _filter.Get1(i);
                    ref var inputEvent = ref _filter.Get2(i);

                    directionMoving = inputEvent.Direction; //получаем направление движения

                    var transform = playerRef.transform;
                    newPosition = transform.position + directionMoving; //вычисляем новую позицию

                    //делаем сдвиг в этом кадре, остальные в другой части кода
                    transform.position += new Vector3(inputEvent.Direction.x, inputEvent.Direction.y, 0) * _config.PlayerMoveSpeed * Time.deltaTime;

                    _gameData.currentPlayerPosition = new Vector2Int((int)newPosition.x, (int)newPosition.y); //сохраняем целевую позицию как текущую
                    _world.NewEntity().Get<PlayerMoving>(); //добавляем компонент о движении

                    //получаем направление для анимации покоя
                    int directionIdle = 3; //по умолчанию вниз
                    if (directionMoving.x > 0) directionIdle = 2; //вправо
                    if (directionMoving.x < 0) directionIdle = 4;//влево
                    if (directionMoving.y > 0) directionIdle = 1; //вверх
                    //устанавливаем анимации для движения и покоя
                    playerRef.animator.SetFloat("MoveX", directionMoving.x);
                    playerRef.animator.SetFloat("MoveY", directionMoving.y);
                    playerRef.animator.SetInteger("Idle", directionIdle);
                    playerRef.animator.SetLayerWeight(0, 1f); //устанавливаем слой движения основным
                    playerRef.animator.SetLayerWeight(1, 0f); //устанавливаем слой покоя вторичным
                }
            }
            else //если уже двигается
            {
                foreach (var i in _filterPlayer)
                {
                    ref var playerRef = ref _filter.Get1(i);

                    var transform = playerRef.transform;
                    //меняем позицию
                    //transform.position += directionMoving * _config.PlayerMoveSpeed * Time.deltaTime;
                    //transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _gameData.currentVelocity, _config.PlayerMoveSpeed);
                    transform.position = Vector3.MoveTowards(transform.position, newPosition, _config.PlayerMoveSpeed * Time.deltaTime);

                    //проверяем расстояние до точки пути для ПОШАГОВЫЙ РЕЖИМ
                    if ((transform.position - newPosition).magnitude < 0.01f)
                    {
                        transform.position = newPosition;

                        //проверка на достижение финиша
                        if (Mathf.Abs(newPosition.x - _gameData.finishPosition.x) < float.Epsilon && Mathf.Abs(newPosition.y - _gameData.finishPosition.y) < float.Epsilon)
                        {
                            Debug.Log("FINISH");
                            _world.NewEntity().Get<NextMazeButtonViewComponent>().isView = true; //показываем кнопку выхода
                        }

                        _filterMoving.GetEntity(0).Del<PlayerMoving>(); //удаляем компонент для прерывания движения

                        //настраиваем слои анимаций на анимацию покоя
                        playerRef.animator.SetLayerWeight(0, 0f); //устанавливаем слой движения вторичным
                        playerRef.animator.SetLayerWeight(1, 1f); //устанавливаем слой покоя основным
                    }

                    if ((newPosition - transform.position).magnitude != 0f)
                    {
                        _filterMoving.GetEntity(0).Get<CheckObstacle>(); //добавляем компонент для проверки следующего хода
                    }

                    //проверка расстояния до финиша
                    if ((_gameData.finishPositionV3 - transform.position).magnitude > 0.5f)
                    {
                        _world.NewEntity().Get<NextMazeButtonViewComponent>().isView = false; //прячем кнопку выхода

                    }
                }
            }
        }
    }

}