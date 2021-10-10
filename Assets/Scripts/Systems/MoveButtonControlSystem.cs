using Leopotam.Ecs;
using LeopotamGroup.Globals;
using System;
using UnityEngine;

namespace Labirintus.ECS
{
    internal sealed class MoveButtonControlSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        //private GameData _gameData;
        private UIData _uiData;
        private UIDebug _uiDebug;

        private EcsFilter<MoveButtonControl> _filter;

        public void Init()
        {
            //вешаем события на кнопки
            _uiData.btnMoveLeft.onClick.AddListener(() =>
            {
                ref var component = ref _world.NewEntity().Get<ButtonMovePressed>();
                component.Direction = Vector3.left;
                component.StateAttack = StateAttack.LEFT;
            });
            _uiData.btnMoveRight.onClick.AddListener(() =>
            {
                ref var component = ref _world.NewEntity().Get<ButtonMovePressed>();
                component.Direction = Vector3.right;
                component.StateAttack = StateAttack.RIGHT;
            });
            _uiData.btnMoveUp.onClick.AddListener(() =>
            {
                ref var component = ref _world.NewEntity().Get<ButtonMovePressed>();
                component.Direction = Vector3.up;
                component.StateAttack = StateAttack.UP;
            });
            _uiData.btnMoveDown.onClick.AddListener(() =>
            {
                ref var component = ref _world.NewEntity().Get<ButtonMovePressed>();
                component.Direction = Vector3.down;
                component.StateAttack = StateAttack.DOWN;
            });
        }

        public void Run()
        {
            //если хода не было выходим
            if (_filter.IsEmpty()) return;
            //_uiDebug.textDebug.text = "Проверка кнопок" + System.DateTime.Now;
            CheckMoveButtons();
        }

        //установка активности кнопок управления при движении
        void CheckMoveButtons()
        {
            //_uiData.textBtnMoveLeft.text = "LEFT";
            //_uiData.textBtnMoveRight.text = "RIGHT";
            //_uiData.textBtnMoveUp.text = "UP";
            //_uiData.textBtnMoveDown.text = "DOWN";

            //влево
            if (!Service<GameData>.Get().canMoveLeft)
            {
                //если враг
                if (Service<GameData>.Get().canAttackLeft)
                {
                    //_uiData.textBtnMoveLeft.text = "АТАКА";
                    _uiData.btnMoveLeftImage.sprite = _uiData.attackSprite;
                    _uiData.btnMoveLeft.interactable = true;
                }
                else
                {
                    _uiData.btnMoveLeft.interactable = false;
                    _uiData.btnMoveLeftImage.sprite = _uiData.leftArrow;
                }
            }
            else
            {
                _uiData.btnMoveLeft.interactable = true;
                _uiData.btnMoveLeftImage.sprite = _uiData.leftArrow;
            }

            //вправо
            if (!Service<GameData>.Get().canMoveRight)
            {
                //если враг
                if (Service<GameData>.Get().canAttackRight)
                {
                    //_uiData.textBtnMoveRight.text = "АТАКА";
                    _uiData.btnMoveRightImage.sprite = _uiData.attackSprite;
                    _uiData.btnMoveRight.interactable = true;
                }
                else
                {
                    _uiData.btnMoveRight.interactable = false;
                    _uiData.btnMoveRightImage.sprite = _uiData.rightArrow;
                }
            }
            else
            {
                _uiData.btnMoveRight.interactable = true;
                _uiData.btnMoveRightImage.sprite = _uiData.rightArrow;
            }

            //вверх
            if (!Service<GameData>.Get().canMoveUp)
            {
                //если враг
                if (Service<GameData>.Get().canAttackUp)
                {
                    //_uiData.textBtnMoveUp.text = "АТАКА";
                    _uiData.btnMoveUpImage.sprite = _uiData.attackSprite;
                    _uiData.btnMoveUp.interactable = true;
                }
                else
                {
                    _uiData.btnMoveUp.interactable = false;
                    _uiData.btnMoveUpImage.sprite = _uiData.upArrow;
                }
            }
            else
            {
                _uiData.btnMoveUp.interactable = true;
                _uiData.btnMoveUpImage.sprite = _uiData.upArrow;
            }

            //вниз
            if (!Service<GameData>.Get().canMoveDown)
            {
                //если враг
                if (Service<GameData>.Get().canAttackDown)
                {
                    //_uiData.textBtnMoveDown.text = "АТАКА";
                    _uiData.btnMoveDownImage.sprite = _uiData.attackSprite;
                    _uiData.btnMoveDown.interactable = true;
                }
                else
                {
                    _uiData.btnMoveDown.interactable = false;
                    _uiData.btnMoveDownImage.sprite = _uiData.downArrow;
                }
            }
            else
            {
                _uiData.btnMoveDown.interactable = true;
                _uiData.btnMoveDownImage.sprite = _uiData.downArrow;
            }
        }
    }

    //компонент для проверки кнопок управления
    internal struct MoveButtonControl
    {
    }

    //компонент ввода управления с кнопок
    internal struct ButtonMovePressed
    {
        public Vector3 Direction;
        public StateAttack StateAttack;
    }
}