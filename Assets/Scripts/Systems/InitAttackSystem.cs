using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Labirintus.ECS
{
    //обработка пошагового боя в окне
    internal class InitAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        //private GameData _gameData = default;
        private PlayerData _playerData;
        private UIData _uiData;

        private EcsFilter<ButtonMovePressed> _filterButtonMove = null;
        private EcsFilter<PlayerRef> _player = null;

        public void Init()
        {
            _uiData.panelBattle.gameObject.Deactivate(); //прячем панель битвы на старте

            //вешаем события на кнопквыхода из битвы
            _uiData.btnExitBattle.onClick.AddListener(() =>
            {
                Service<GameData>.Get().panelBattleCharacter.gameObject.Deactivate();
                Service<GameData>.Get().panelBattleEnemy.gameObject.Deactivate();
                _uiData.panelBattle.gameObject.Deactivate(); //прячем панель
            });
        }

        public void Run()
        {
            if (!_filterButtonMove.IsEmpty())
            {
                Service<GameData>.Get().stateAttack = StateAttack.NONE;
                StateAttack stateAttack = _filterButtonMove.Get1(0).StateAttack;

                //проверка на атаку врага
                if (stateAttack == StateAttack.LEFT && Service<GameData>.Get().canAttackLeft)
                {
                    Service<GameData>.Get().stateAttack = StateAttack.LEFT;
                }
                else if (stateAttack == StateAttack.RIGHT && Service<GameData>.Get().canAttackRight)
                {
                    Service<GameData>.Get().stateAttack = StateAttack.RIGHT;
                }
                else if (stateAttack == StateAttack.UP && Service<GameData>.Get().canAttackUp)
                {
                    Service<GameData>.Get().stateAttack = StateAttack.UP;
                }
                else if (stateAttack == StateAttack.DOWN && Service<GameData>.Get().canAttackDown)
                {
                    Service<GameData>.Get().stateAttack = StateAttack.DOWN;
                }

                //запускаем панель битвы
                if (Service<GameData>.Get().stateAttack != StateAttack.NONE)
                {
                    _uiData.panelBattle.gameObject.Activate(); //показываем панель

                    //если панели нет, то создаем
                    if (Service<GameData>.Get().panelBattleCharacter == null)
                    {
                        Service<GameData>.Get().panelBattleCharacter = GameObject.Instantiate(_uiData.prefabPanelBattleCharacter, _uiData.panelBattle.transform);
                    }
                    else
                    {
                        Service<GameData>.Get().panelBattleCharacter.gameObject.Activate();
                    }
                    Service<GameData>.Get().panelBattleCharacter.imageCharacter.sprite = _playerData.Sprite;
                    Service<GameData>.Get().panelBattleCharacter.hpBar.fillAmount = (float)_playerData.HealthPointPlayer / _playerData.MaxHealthPointPlayer;
                    Service<GameData>.Get().panelBattleCharacter.defenseBar.fillAmount = (float)_playerData.DefensePlayer / _playerData.MaxDefensePlayer;

                    _world.NewEntity().Get<StartBattleComponent>();
                    
                    //если панели нет, то создаем
                    if (Service<GameData>.Get().panelBattleButtons == null)
                    {
                        Service<GameData>.Get().panelBattleButtons = GameObject.Instantiate(_uiData.prefabPanelBattleButtons, _uiData.panelBattle.transform);
                        //атакуем
                        Service<GameData>.Get().panelBattleButtons.buttonAttack.onClick.AddListener(() =>
                        {
                            _player.GetEntity(0).Get<BattleAttackComponent>();
                        });
                        //защищаемся
                        Service<GameData>.Get().panelBattleButtons.buttonDefense.onClick.AddListener(() =>
                        {
                            _player.GetEntity(0).Get<BattleDefenseComponent>();
                        });
                        //лечимся
                        Service<GameData>.Get().panelBattleButtons.buttonHealth.onClick.AddListener(() =>
                        {
                            _player.GetEntity(0).Get<BattleHealthComponent>();
                        });
                    }
                }
            }
        }

    }
    internal struct StartBattleComponent
    {
    }
}