using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    public class ItemPickUp : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private int _amount;
        [SerializeField] private ItemPickUpType _itemType;

        public string Name { get => _name; private set => _name = value; }
        public int Amount { get => _amount; private set => _amount = value; }
        public ItemPickUpType ItemType { get => _itemType; private set => _itemType = value; }
    }

    public enum ItemPickUpType { COIN, HEALTH, POWER_BLUE, POWER_PINK, POWER_GREEN }
}
