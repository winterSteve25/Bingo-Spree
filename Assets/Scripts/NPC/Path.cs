using UnityEngine;

namespace NPC
{
    public class Path : MonoBehaviour
    {
        public Vector2[] vertices;

        private void OnDrawGizmosSelected()
        {
            if (vertices == null) return;
            if (vertices.Length < 2) return;

            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Gizmos.DrawLine(vertices[i], vertices[i + 1]);
            }
        }
    }
}