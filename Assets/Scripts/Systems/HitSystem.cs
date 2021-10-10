using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    internal sealed class HitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private GameData _gameData;
        private PlayerData _playerData;
        private EcsFilter<HitEnterComponent> _filterHitsEnter; //фильтр столкновений
        private EcsFilter<HitExitComponent> _filterHitsExit; //фильтр выхода из столкновений
        private EcsFilter<PlayerRef> _player; //фильтр игрока

        public void Init()
        {
            //запускаем обновление экрана
            _world.NewEntity().Get<UpdateScreenInfoComponent>();
        }

        public void Run()
        {
            foreach (var i in _filterHitsEnter)
            {
                ref var hitComponent = ref _filterHitsEnter.Get1(i); //компонент столкновений

                foreach (var i2 in _player)
                {
                    ref var playerRef = ref _player.Get1(i2); //комонент игрока

                    //если это подбираемый предмет
                    if (hitComponent.other.CompareTag("ItemPickUp"))
                    {
                        ItemPickUp itemPickUp = hitComponent.other.GetComponent<ItemPickUp>(); //получаем данные предмета

                        //Debug.Log(itemPickUp.ItemType);
                        switch (itemPickUp.ItemType)
                        {
                            case ItemPickUpType.COIN:
                                _playerData.CoinsAmountPlayer += itemPickUp.Amount;
                                break;
                            case ItemPickUpType.HEALTH:
                                _playerData.HealthPointPlayer += itemPickUp.Amount;
                                break;
                            case ItemPickUpType.POWER_BLUE:
                                _playerData.PowerBlueAmountPlayer += itemPickUp.Amount;
                                break;
                            case ItemPickUpType.POWER_PINK:
                                _playerData.PowerPinkAmountPlayer += itemPickUp.Amount;
                                break;
                            case ItemPickUpType.POWER_GREEN:
                                _playerData.PowerGreenAmountPlayer += itemPickUp.Amount;
                                break;
                        }

                        //запускаем обновление экрана
                        _world.NewEntity().Get<UpdateScreenInfoComponent>();
                        _world.NewEntity().Get<SaveComponent>();
                    }
                    else if (hitComponent.other.CompareTag("Finish"))
                    {
                        _world.NewEntity().Get<NextMazeButtonViewComponent>().isView = true; //показываем кнопку выхода
                    }

                }
            }

            foreach (var i in _filterHitsExit)
            {
                ref var hitComponent = ref _filterHitsExit.Get1(i); //компонент столкновений

                if (hitComponent.other.CompareTag("Finish"))
                {
                    _world.NewEntity().Get<NextMazeButtonViewComponent>().isView = false; //показываем кнопку выхода
                }
            }
        }
    }

    internal struct HitEnterComponent
    {
        public GameObject first; //кто столкнулся
        public GameObject other; //с кем столкнулся
    }

    internal struct HitExitComponent
    {
        public GameObject first; //кто вышел
        public GameObject other; //из кого вышли
    }
}