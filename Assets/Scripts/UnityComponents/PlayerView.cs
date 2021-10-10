using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    public class PlayerView : MonoBehaviour
    {
        public EcsWorld EcsWorld { get; set; }
        public EcsEntity Entity;

        public LayerMask targetMaskFOV; //скрываемые объекты
        public LayerMask obstacleMaskFOV; //препятствия
        public Transform transformFOV; //трансформ центральной точки
        public MeshFilter viewMeshFilterFOV; //мешфильтр поля        
        // Фактически PlayerView не нужен, достаточно только в ECS-компоненте
        // сохранить ссылку на transform игрока.
        // Но при таком подходе потом при вызове любого MonoBehaviour события
        // допустим OnCollisionEnter можно будет сразу добавить нужный компонент
        // на Entity игрока
        // private void OnCollisionEnter(Collision other) {
        //     Entity.Get<PlayerCollideEvent>();
        // }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("ItemPickUp"))
            {
                // instantly destroy coin to avoid multiple OnTriggerEnter() calls.
                collision.gameObject.SetActive(false);
            }
            var hit = EcsWorld.NewEntity();
            ref var hitComponent = ref hit.Get<HitEnterComponent>();

            hitComponent.first = transform.root.gameObject; //записываем себя
            hitComponent.other = collision.gameObject; //с кем столкнулись

        }
        private void OnTriggerExit2D(Collider2D collision)
        {           
            var hit = EcsWorld.NewEntity();
            ref var hitComponent = ref hit.Get<HitExitComponent>();

            hitComponent.first = transform.root.gameObject; //записываем себя
            hitComponent.other = collision.gameObject; //с кем столкнулись

        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    var hit = _ecsWorld.NewEntity();

        //    ref var hitComponent = ref hit.Get<HitComponent>();

        //    hitComponent.first = transform.root.gameObject;
        //    hitComponent.other = collision.gameObject;
        //}
    }
}
