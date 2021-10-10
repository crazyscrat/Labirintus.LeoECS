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

        public LayerMask targetMaskFOV; //���������� �������
        public LayerMask obstacleMaskFOV; //�����������
        public Transform transformFOV; //��������� ����������� �����
        public MeshFilter viewMeshFilterFOV; //��������� ����        
        // ���������� PlayerView �� �����, ���������� ������ � ECS-����������
        // ��������� ������ �� transform ������.
        // �� ��� ����� ������� ����� ��� ������ ������ MonoBehaviour �������
        // �������� OnCollisionEnter ����� ����� ����� �������� ������ ���������
        // �� Entity ������
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

            hitComponent.first = transform.root.gameObject; //���������� ����
            hitComponent.other = collision.gameObject; //� ��� �����������

        }
        private void OnTriggerExit2D(Collider2D collision)
        {           
            var hit = EcsWorld.NewEntity();
            ref var hitComponent = ref hit.Get<HitExitComponent>();

            hitComponent.first = transform.root.gameObject; //���������� ����
            hitComponent.other = collision.gameObject; //� ��� �����������

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
