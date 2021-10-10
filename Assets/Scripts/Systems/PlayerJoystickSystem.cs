using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    /// <summary>
    /// Система обработки ввода дял движения
    /// </summary>
    internal sealed class PlayerJoystickSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private GameData _gameData = default;
        //фильтр по ссылке на игрока
        private EcsFilter<PlayerRef> _filterPlayer = default;
        private EcsFilter<PlayerMoving> _filterMoving = default;
        private EcsFilter<JoystickMoved> _filterJoystickMove = default;

        public void Init()
        {
            //устанавливаем анимации
            ref var player = ref _filterPlayer.Get1(0);
            player.animator.SetInteger("Idle", 3);
            player.animator.SetLayerWeight(0, 0f); //устанавливаем слой движения вторичным
            player.animator.SetLayerWeight(1, 1f); //устанавливаем слой покоя основным
        }

        public void Run()
        {
            if (_filterPlayer.IsEmpty()) return; //если пустой, то выходим

            if (!_filterMoving.IsEmpty()) return; //если игрок в движении

            var direction = Vector3.zero;
            //если на кнопки тача нажали
            if (!_filterJoystickMove.IsEmpty())
            {
                //получаем направление от джойстика
                direction = _filterJoystickMove.Get1(0).Direction;
                //Debug.Log(direction);
            }
            //#if UNITY_EDITOR
            //            else
            //            {
            //                //получаем ввод
            //                var h = Input.GetAxisRaw("Horizontal");
            //                var v = Input.GetAxisRaw("Vertical");
            //                if (Mathf.Abs(h) > Mathf.Abs(v))
            //                {
            //                    direction = new Vector3(h, 0, 0);
            //                }
            //                else if (Mathf.Abs(h) < Mathf.Abs(v))
            //                {
            //                    direction = new Vector3(0, v, 0);
            //                }
            //            }
            //#endif
            if (direction == Vector3.zero)
            {
                ref var playerRef = ref _filterPlayer.Get1(0);
                //устанавливаем анимации для движения и покоя
                playerRef.animator.SetFloat("MoveX", 0);
                playerRef.animator.SetFloat("MoveY", 0);
                //playerRef.animator.SetInteger("Idle", 3);
                playerRef.animator.SetLayerWeight(0, 0f); //устанавливаем слой движения вторичным
                playerRef.animator.SetLayerWeight(1, 1f); //устанавливаем слой покоя основным

                return;
            }
            ////получаем направление для анимации покоя
            //int directionIdle = 3; //по умолчанию вниз
            //if (direction.x > 0) directionIdle = 2; //вправо
            //if (direction.x < 0) directionIdle = 4;//влево
            //if (direction.y > 0) directionIdle = 1; //вверх
            ////устанавливаем анимации
            //ref var player = ref _filterPlayer.Get1(0);
            //player.animator.SetFloat("MoveX", direction.x);
            //player.animator.SetFloat("MoveY", direction.y);
            //player.animator.SetInteger("IdleDirection", directionIdle);
            //player.animator.SetLayerWeight(0, 1f); //устанавливаем слой движения основным
            //player.animator.SetLayerWeight(1, 0f); //устанавливаем слой покоя вторичным

            _gameData.moveDirection = new Vector2Int((int)direction.x, (int)direction.y); //запоминаем направление для атаки

            //проверяем ограничения движения
            //if (!_gameData.canMoveRight && direction.x > 0) direction.x = 0;
            //if (!_gameData.canMoveLeft && direction.x < 0) direction.x = 0;
            //if (!_gameData.canMoveUp && direction.y > 0) direction.y = 0;
            //if (!_gameData.canMoveDown && direction.y < 0) direction.y = 0;
            //Debug.Log(direction);

            if (direction == Vector3.zero) return;

            //если было нажатие
            if (direction.magnitude > 0f)
            {
                foreach (var i in _filterPlayer)
                {
                    //добавляем игроку компонент для движения
                    _filterPlayer.GetEntity(i).Get<PlayerInputEvent>().Direction = direction;
                }
            }
        }
    }

    //компонент для определения движения
    internal struct PlayerJoystickEvent
    {
        public Vector3 Direction; //вектор смещения
    }        
}