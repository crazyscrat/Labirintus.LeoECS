using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    public class FieldOfView : MonoBehaviour
    {
        Mesh mesh;
        float fov = 360f;
        Vector3 origin = Vector3.zero;
        int rayCount = 50;
        float startingAngle;
        float angle;
        float angleIncrease;
        float viewDistance = 6f;

        [SerializeField] private LayerMask layerMask;
        [SerializeField] Transform target;

        void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            angleIncrease = fov / rayCount;
        }

        void LateUpdate()
        {
            angle = 0f;
            origin = target.position;

            Vector3[] vertices = new Vector3[rayCount + 1 + 1];
            Vector2[] uv = new Vector2[vertices.Length];
            int[] triangles = new int[rayCount * 3];

            vertices[0] = origin;

            int vertexIndex = 1;
            int triangleIndex = 0;
            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
                if (raycastHit2D.collider == null)
                {
                    //no hit
                    vertex = origin + GetVectorFromAngle(angle) * viewDistance;
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

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

        }

        public void SetOrigin(Vector3 origin)
        {
            this.origin = origin;
        }

        public void SetAimDirection(Vector3 aimDirection)
        {
            startingAngle = GetAngleFromVectorFloat(aimDirection) - fov / 2f;
        }

        public static Vector3 GetVectorFromAngle(float angle)
        {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }
    }
}
