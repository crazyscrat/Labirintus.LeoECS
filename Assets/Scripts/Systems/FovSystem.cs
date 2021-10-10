using Leopotam.Ecs;
using LeopotamGroup.Globals;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    //Система отрисовки поля зрения персонажа
    internal class FovSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private UIData _uiData;
        private PlayerData _playerData;

        private EcsFilter<PlayerRef> _player;

        float _viewRadius; //радиус обзора
        float _viewAngle; //угол обзора
        int _rayCount; //качество поля зрения
        int _edgeResolveIterations; //количество итерация для точного определения угла препятствия
        float _edgeDistThreshold; //предельное расстояние между разными препятстсвиями

        Vector3 origin = Vector3.zero;
        float angle;
        float angleIncrease;        

        //LayerMask targetMask; //скрываемые объекты
        //LayerMask obstacleMask; //препятствия

        //Transform transform; //трансформ центральной точки
        //MeshFilter viewMeshFilter; //мешфильтр поля

        Mesh viewMesh; //меш поля обзора

        public void Init()
        {
            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            _player.Get1(0).viewMeshFilterFOV.mesh = viewMesh;

            _viewRadius = _playerData.viewRadius;
            _viewAngle = _playerData.viewAngle;
            _rayCount = _playerData.rayCount;
            _edgeResolveIterations = _playerData.edgeResolveIterations;
            _edgeDistThreshold = _playerData.edgeDistThreshold;

            angleIncrease = _viewAngle / _rayCount;
        }

        public void Run()
        {
            if (_player.IsEmpty()) return;

            //DrawFieldOfView();
            DrowFOV();
        }

        void DrowFOV()
        {
            angle = 0f;
            origin = _player.Get1(0).transform.position;

            Service<GameData>.Get().hashViewEnemies.Clear();

            Vector3[] vertices = new Vector3[_rayCount + 1 + 1];
            Vector2[] uv = new Vector2[vertices.Length];
            int[] triangles = new int[_rayCount * 3];

            vertices[0] = origin;

            int vertexIndex = 1;
            int triangleIndex = 0;
            for (int i = 0; i <= _rayCount; i++)
            {
                Vector3 vertex;
                Vector3 dir = GetVectorFromAngle(angle);
                
                //луч для обзора
                RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, dir, _viewRadius, _player.Get1(0).obstacleMaskFOV);
                if (raycastHit2D.collider == null)
                {
                    //no hit
                    vertex = origin + dir * _viewRadius;

                    //луч для обнаружения врагов
                    RaycastHit2D[] raycastObjects = Physics2D.RaycastAll(origin, dir, _viewRadius, _player.Get1(0).behindFOV);
                    if (raycastObjects.Length > 0) //есть объекты
                    {
                        for (int j = 0; j < raycastObjects.Length; j++)
                        {
                            if (raycastObjects[j].collider.tag == "Enemy") //если враг
                            {
                                Service<GameData>.Get().hashViewEnemies.Add(raycastObjects[j].transform);

                                break;
                            }
                        }
                    }
                }
                else
                {
                    //hit
                    vertex = raycastHit2D.point;
                }

                vertices[vertexIndex] = vertex;

                if (i > 0)
                {
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex;

                    triangleIndex += 3;
                }

                vertexIndex++;
                angle -= angleIncrease;
            }

            viewMesh.vertices = vertices;
            viewMesh.uv = uv;
            viewMesh.triangles = triangles;
            //viewMesh.RecalculateNormals();
            viewMesh.bounds = new Bounds(origin, Vector3.one * 1000f);
        }

        public static Vector3 GetVectorFromAngle(float angle)
        {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        void DrawFieldOfView()
        {
            ref var player = ref _player.Get1(0);

            int stepCount = Mathf.RoundToInt(_viewAngle * _rayCount);
            float stepAngleSize = _viewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();
            ViewCasInfo oldViewCast = new ViewCasInfo();
            

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = player.transformFOV.eulerAngles.y - _viewAngle / 2 + stepAngleSize * i;
                //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.blue);
                ViewCasInfo newViewCast = ViewCast(angle, player);

                if (i > 0)
                {
                    //определяем расстояние попаданий в разные препятствия между соседними лучами
                    bool edgeDistThresholdExceeded = Mathf.Abs(oldViewCast.dist - newViewCast.dist) > _edgeDistThreshold;

                    //проверка на не попадание одного из лучей, или попадание обоих, но в разные препятствия
                    //if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExceeded))
                    //{
                    //    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    //    if (edge.pointA != Vector3.zero)
                    //    {
                    //        viewPoints.Add(edge.pointA);
                    //    }
                    //    if (edge.pointB != Vector3.zero)
                    //    {
                    //        viewPoints.Add(edge.pointB);
                    //    }
                    //}
                }

                viewPoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }
                        
            int vertexCount = viewPoints.Count + 1; //число вершин
            Vector3[] vertices = new Vector3[vertexCount]; //вершины
            Vector2[] uv = new Vector2[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3]; //индексы вершин

            vertices[0] = Vector3.zero; //начальная вершина

            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = player.transformFOV.InverseTransformPoint(viewPoints[i]);// + Vector3.forward * maskCatwayDist;

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.uv = uv;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        //получение информации о результате луча
        ViewCasInfo ViewCast(float globalAngle, PlayerRef player)
        {
            Vector3 dir = DirFromAngle(globalAngle, false, player);

            RaycastHit2D hit = Physics2D.Raycast(player.transformFOV.position, dir, _viewRadius, player.obstacleMaskFOV);
            if (hit)
            {
                return new ViewCasInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCasInfo(false, player.transformFOV.position + dir * _viewRadius, _viewRadius, globalAngle);
            }
        }

        //вычисление направления по углу
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal, PlayerRef player)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += player.transformFOV.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        //структура каждого выстрела лучем
        internal struct ViewCasInfo
        {
            public bool hit;
            public Vector3 point;
            public float dist;
            public float angle;

            public ViewCasInfo(bool hit, Vector3 point, float dist, float angle)
            {
                this.hit = hit;
                this.point = point;
                this.dist = dist;
                this.angle = angle;
            }
        }
    }
}