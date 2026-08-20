#if UNITY_EDITOR
using System.Collections.Generic;
using LabDiner.Restaurant.Humanoid;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public partial class GuestSpawner
    {

        partial void Debug_AddGuest(GuestContext guest)
        {
            _guests.Add(guest);
        }

        partial void Debug_RemoveGuest(GuestContext guest)
        {
            _guests.Remove(guest);
        }

        [Header("[DEBUG]")]
        [SerializeField] private Color _gizmoSpawnColor = Color.green;
        [SerializeField] private Color _gizmosExitColor = Color.red;
        [SerializeField] private Vector3 _restPointDimensions = new Vector3(0.6f, 1.2f, 0.1f); // Hình vuông cao cao

        protected virtual void OnDrawGizmos()
        {
            if (!_showGizmos || _spawnPoint == null) return;

            Gizmos.color = _gizmoSpawnColor;

            Vector3 pos = _spawnPoint.position;
            Vector3 center = pos + Vector3.up * _restPointDimensions.y * 0.5f;
            Gizmos.DrawWireCube(center, _restPointDimensions);

            UnityEditor.Handles.Label(pos + Vector3.up * (_restPointDimensions.y + 0.2f), $"Guest Spawn Point");

            Gizmos.color = _gizmosExitColor;

            pos = _exitPoint.position;

            center = pos + Vector3.up * _restPointDimensions.y * 0.5f;
            Gizmos.DrawWireCube(center, _restPointDimensions);

            UnityEditor.Handles.Label(pos + Vector3.up * (_restPointDimensions.y + 0.2f), $"Guest Exit Point");
        }

    }
}
#endif