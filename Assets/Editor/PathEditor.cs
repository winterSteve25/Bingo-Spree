using NPC;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Path))]
    public class PathEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            Path path = (Path)target;
            Handles.color = Color.red;
            
            if (path.vertices == null || path.vertices.Length < 2) return;

            for (int i = 0; i < path.vertices.Length; i++)
            {
                Vector3 newOffset = Handles.PositionHandle(path.vertices[i], Quaternion.identity);
                
                if (newOffset != (Vector3)(path.vertices[i]))
                {
                    Undo.RecordObject(path, "Move vertex");
                    path.vertices[i] = newOffset;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Normalize"))
            {
                Path path = (Path)target;
                
                for (int i = 1; i < path.vertices.Length; i++)
                {
                    Vector2 prev = path.vertices[i - 1];
                    Vector2 curr = path.vertices[i];

                    float dx = Mathf.Abs(curr.x - prev.x);
                    float dy = Mathf.Abs(curr.y - prev.y);

                    if (dx > dy)
                    {
                        curr.y = prev.y;
                        path.vertices[i] = curr;
                    }
                    else
                    {
                        curr.x = prev.x;
                        path.vertices[i] = curr;
                    }
                }
            }
        }
    }
}