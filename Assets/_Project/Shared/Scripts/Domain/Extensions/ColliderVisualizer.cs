#if UNITY_EDITOR
using UnityEngine;

namespace LabDiner.LevelSystem
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [ExecuteAlways] // Chạy cả trong chế độ Edit Mode để Designer thấy ngay kết quả
    [RequireComponent(typeof(PolygonCollider2D))]
    public class ColliderVisualizer : MonoBehaviour
    {
        [Header("Gizmos Settings")]
    [Tooltip("Màu sắc vùng bên trong đa giác")]
    public Color zoneColor = new Color(0f, 0.8f, 1f, 0.25f);

    [Tooltip("Hiển thị đường viền sắc nét")]
    public bool drawWireframe = true;
    public Color wireframeColor = new Color(0f, 0.8f, 1f, 1f);

    private PolygonCollider2D polyCollider;

    private void OnDrawGizmos()
    {
        // Chỉ chạy trong Editor
        #if UNITY_EDITOR
        if (polyCollider == null)
            polyCollider = GetComponent<PolygonCollider2D>();

        if (polyCollider == null || !polyCollider.enabled) return;

        // Lưu ma trận cũ và áp dụng ma trận Transform của Object
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Matrix4x4 oldHandleMatrix = Handles.matrix;
        
        Gizmos.matrix = transform.localToWorldMatrix;
        Handles.matrix = transform.localToWorldMatrix;

        for (int i = 0; i < polyCollider.pathCount; i++)
        {
            Vector2[] pathPoints = polyCollider.GetPath(i);
            if (pathPoints.Length < 3) continue;

            // Chuyển đổi Vector2[] sang Vector3[] để vẽ
            Vector3[] points3D = new Vector3[pathPoints.Length];
            for (int j = 0; j < pathPoints.Length; j++)
            {
                // Đẩy nhẹ lên Z một chút để không bị đè bởi Sprite gốc
                points3D[j] = new Vector3(pathPoints[j].x, pathPoints[j].y, -0.01f);
            }

            // 1. Vẽ vùng đặc bằng Handles (Chống rác bộ nhớ, hiển thị mọi góc nhìn)
            Handles.color = zoneColor;
            Handles.DrawAAConvexPolygon(points3D);

            // 2. Vẽ đường viền sắc nét
            if (drawWireframe)
            {
                Gizmos.color = wireframeColor;
                for (int k = 0; k < points3D.Length; k++)
                {
                    Vector3 start = points3D[k];
                    Vector3 end = points3D[(k + 1) % points3D.Length];
                    start.z = end.z = -0.02f; // Nổi lên trên vùng đặc
                    Gizmos.DrawLine(start, end);
                }
            }
        }

        // Khôi phục ma trận
        Gizmos.matrix = oldMatrix;
        Handles.matrix = oldHandleMatrix;
        #endif
    }
    }
}

#endif