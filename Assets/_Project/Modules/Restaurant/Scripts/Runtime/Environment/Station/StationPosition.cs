using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    public class StationPosition : MonoBehaviour
    {
        [Header("References")]
        public Transform spawnPos;
        public Transform workPos;

        [Header("Gizmos")]
        [SerializeField] private Vector3 _spawnGizmoSize = Vector3.one * 1.5f;
        [SerializeField] private float _workGizmoCenterY = 0f;
        [SerializeField] private Vector3 _workGizmoSize = new Vector3(1.5f, 1.2f, 0.1f);

        void OnDrawGizmos()
        {
            if (spawnPos == null || workPos == null)
            {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnPos.position, _spawnGizmoSize);

            Gizmos.color = Color.yellow;
            Vector3 pos = workPos.position;
            Vector3 center = pos + Vector3.up * _workGizmoCenterY;
            Gizmos.DrawWireCube(center, _workGizmoSize);
        }

    }
}