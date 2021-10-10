using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    //система генерации выстрела
    internal class WeaponShootSystem : IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter<Weapon, Shoot> _filterSpawn;

        public void Run()
        {
            foreach (var i in _filterSpawn)
            {
                ref var weapon = ref _filterSpawn.Get1(i);
                ref var entity = ref _filterSpawn.GetEntity(i);
                ref var spawnProjectile = ref entity.Get<SpawnProjectile>();
                spawnProjectile.targetPosition = _filterSpawn.Get2(i).targetPosition;

                entity.Del<Shoot>();
            }
        }
    }

    internal struct Shoot
    {
        //позиция врага
        internal Vector3 targetPosition;
    }


    //компонент создания снаряда игрока
    internal struct SpawnProjectile
    {
        internal Vector3 targetPosition;
    }
}