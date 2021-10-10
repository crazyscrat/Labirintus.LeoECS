using Leopotam.Ecs;

namespace Labirintus.ECS
{
    //проверка наличия врагов на соседних клетках от игрока
    internal sealed class CheckEnemySystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private GameData _gameData = default;
        private UIDebug _uiDebug;

        private EcsFilter<CheckEnemy> _filter = default; //проверка врагов

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
                _gameData.stateAttack = StateAttack.NONE;

                MoveLeft();
                MoveRight();
                MoveUp();
                MoveDown();

                _world.NewEntity().Get<MoveButtonControl>(); //запустим проверку кнопок
                //_uiDebug.textDebug.text = "CheckEnemy" + System.DateTime.Now;
            }
        }

        //проверка влево
        private void MoveLeft()
        {
            _gameData.canAttackLeft = false;

            //проверяем можно ли двигаться в эту сторону
            if (!_gameData.canMoveLeft)
            {
                if (_gameData.dataMaze[currentY, currentX - 1] == 2)
                {
                    _gameData.canAttackLeft = true;
                }
            }
        }

        //проверка вправо
        private void MoveRight()
        {
            _gameData.canAttackRight = false;

            //проверяем можно ли двигаться в эту сторону
            if (!_gameData.canMoveRight)
            {
                if (_gameData.dataMaze[currentY, currentX + 1] == 2)
                {
                    _gameData.canAttackRight = true;
                }
            }
        }

        //проверка вверх
        private void MoveUp()
        {
            _gameData.canAttackUp = false;

            //проверяем можно ли двигаться в эту сторону
            if (!_gameData.canMoveUp)
            {
                if (_gameData.dataMaze[currentY + 1, currentX] == 2)
                {
                    _gameData.canAttackUp = true;
                }
            }
        }

        //проверка вниз
        private void MoveDown()
        {
            _gameData.canAttackDown = false;

            //проверяем можно ли двигаться в эту сторону
            if (!_gameData.canMoveDown)
            {
                if (_gameData.dataMaze[currentY - 1, currentX] == 2)
                {
                    _gameData.canAttackDown = true;
                }
            }
        }
    }

    //компонент проверки на наличие врагов
    public struct CheckEnemy
    {
    }
}