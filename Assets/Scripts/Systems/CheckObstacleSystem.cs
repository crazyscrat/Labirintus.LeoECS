using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Labirintus.ECS
{
    //система проверки препятствий на пути игрока
    internal sealed class CheckObstacleSystem : IEcsRunSystem
    {
        private GameData _gameData = default;
        private UIDebug _uiDebug;

        private EcsFilter<CheckObstacle> _filter = default;

        private int currentX = 0;
        private int currentY = 0;

        public void Run()
        {
            foreach (var i in _filter)
            {
                //получаем текущие кординаты
                currentX = _gameData.currentPlayerPosition.x;
                currentY = _gameData.currentPlayerPosition.y;

                //выполняем проверку
                //_gameData.stateAttack = StateAttack.NONE;

                MoveLeft();
                MoveRight();
                MoveUp();
                MoveDown();

                _filter.GetEntity(0).Get<CheckEnemy>(); //запускаем проверку на врагов
                //_uiDebug.textDebug.text = "CheckObstacle" + System.DateTime.Now;
            }
        }

        //проверка влево
        private void MoveLeft()
        {
            _gameData.canMoveLeft = false;

            //проверка на свободное пространство или путевую точку
            if (_gameData.dataMaze[currentY, currentX - 1] == 0 || _gameData.dataMaze[currentY, currentX - 1] == 9)
            {
                _gameData.canMoveLeft = true;
                //directionX = -1;
            }
        }

        //проверка вправо
        private void MoveRight()
        {
            _gameData.canMoveRight = false;
            //проверка на свободное пространство или путевую точку
            if (_gameData.dataMaze[currentY, currentX + 1] == 0 || _gameData.dataMaze[currentY, currentX + 1] == 9)
            {
                _gameData.canMoveRight = true;
                //directionX = 1;
            }
        }

        //проверка вверх
        private void MoveUp()
        {
            _gameData.canMoveUp = false;
            //проверка на свободное пространство или путевую точку
            if (_gameData.dataMaze[currentY + 1, currentX] == 0 || _gameData.dataMaze[currentY + 1, currentX] == 9)
            {
                _gameData.canMoveUp = true;
                //directionY = 1;
            }
        }

        //проверка вниз
        private void MoveDown()
        {
            _gameData.canMoveDown = false;
            //проверка на свободное пространство или путевую точку
            if (_gameData.dataMaze[currentY - 1, currentX] == 0 || _gameData.dataMaze[currentY - 1, currentX] == 9)
            {
                _gameData.canMoveDown = true;
                //directionY = -1;
            }
        }
    }

    //копонент проверки на препятствия
    internal struct CheckObstacle
    {
    }

    internal enum StateAttack { NONE, LEFT, RIGHT, UP, DOWN }
}