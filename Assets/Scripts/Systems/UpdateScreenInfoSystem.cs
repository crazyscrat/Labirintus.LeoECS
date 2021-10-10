using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Labirintus.ECS
{
    internal class UpdateScreenInfoSystem : IEcsRunSystem
    {
        private PlayerData _playerData;
        private UIData _uiData;

        private EcsFilter<UpdateScreenInfoComponent> _filterMainScreen;
        private EcsFilter<UpdateBattleScreenInfoComponent> _filterBattle;

        public void Run()
        {
            //обновление главного экрана
            foreach (var i in _filterMainScreen)
            {
                UpdateMainScreenInfo();
            }

            //обновление экрана боя
            foreach (var i in _filterBattle)
            {
                UpdateMainScreenInfo();
                UpdateBattleScreenInfo();
            }
        }

        //обновляем инфу на экране
        private void UpdateMainScreenInfo()
        {
            //энергии
            _uiData.textPowerBlueAmount.SetText(_playerData.PowerBlueAmountPlayer.ToString());
            _uiData.textPowerGreenAmount.SetText(_playerData.PowerGreenAmountPlayer.ToString());
            _uiData.textPowerPinkAmount.SetText(_playerData.PowerPinkAmountPlayer.ToString());
        }

        //обновляем инфу на экране
        private void UpdateBattleScreenInfo()
        {
            //обновляем здоровье игрока
            Service<GameData>.Get().panelBattleCharacter.hpBar.fillAmount = (float)_playerData.HealthPointPlayer / _playerData.MaxHealthPointPlayer;
            //обновляем защиту игрока
            Service<GameData>.Get().panelBattleCharacter.defenseBar.fillAmount = (float)_playerData.DefensePlayer / _playerData.MaxDefensePlayer;
            //обновляем здоровье врага
            Service<GameData>.Get().panelBattleEnemy.hpBar.fillAmount = 
                (float)Service<GameData>.Get().enemyData.HealthPoint / Service<GameData>.Get().enemyData.MaxHealthPoint;
            //обновляем защиту врага
            Service<GameData>.Get().panelBattleEnemy.defenseBar.fillAmount = 
                (float)Service<GameData>.Get().enemyData.DefenseEnemy / Service<GameData>.Get().enemyData.MaxDefenseEnemy;
        }
    }

    internal struct UpdateScreenInfoComponent
    {
    }
    internal struct UpdateBattleScreenInfoComponent
    {
    }
}