using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    internal sealed class UserControlSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private UIData _uiData;

        private EcsFilter<NextMazeComponent> _nextMaze; //переход на след лабиринт
        private EcsFilter<NextMazeButtonViewComponent> _nextMazeBtnView; //показать кнопку перехода

        //private EcsFilter<StartBattleComponent> _attackFilter; //переход на панель битвы

        public void Init()
        {
            _uiData.btnNextMaze.gameObject.Deactivate(); //прячем кнопку нового лабиринта на старте

            //вешаем события на кнопки
            _uiData.btnNextMaze.onClick.AddListener(() =>
            {
                _world.NewEntity().Get<NextMazeComponent>();
                _uiData.btnNextMaze.gameObject.Deactivate(); //прячем кнопку
            });
        }

        public void Run()
        {
            //обработка нажатия на кнопку перехода на следующий лабиринт
            foreach (var i in _nextMaze)
            {
                Debug.Log("FINISH");
                _world.NewEntity().Get<MazeDispoze>();
                _world.NewEntity().Get<MazeGenerator>();
            }

            //отображение/скрытие кнопки следующий лабиринт
            foreach (var i in _nextMazeBtnView)
            {
                //если состояние противоположное
                if (_uiData.btnNextMaze.gameObject.activeInHierarchy != _nextMazeBtnView.Get1(0).isView)
                {
                    _uiData.btnNextMaze.gameObject.SetActive(_nextMazeBtnView.Get1(0).isView);
                }
            }
        }
    }

    public struct NextMazeComponent { }

    public struct NextMazeButtonViewComponent
    {
        public bool isView;
    }
}