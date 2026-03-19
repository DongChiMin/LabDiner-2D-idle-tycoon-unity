using System.Collections;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestFlowManager : MonoBehaviour
    {
        [Header("Guest Settings")]
        [SerializeField] private GuestAI _guestPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _exitPoint;

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnInterval = 5f;
        [SerializeField] private float _initialDelay = 2f;

        void Start()
        {
            //chờ 2 giây, sau đó mỗi 5 giây thử spawn 1 lần
            StartCoroutine(SpawnLoop());
        }

        IEnumerator SpawnLoop()
        {
            yield return new WaitForSeconds(_initialDelay);

            while (true)
            {
                TrySpawnGuest();

                float timer = 0f;
                while (timer < _spawnInterval)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
        }

        void TrySpawnGuest()
        {
            Transform targetTable = SeatingManager.Instance.GetEmptyTable(out int index);

            if (targetTable != null)
            {
                GuestContext guest = PoolContext.Instance.GuestPool.Get( _spawnPoint.position, Quaternion.identity);
                guest.CtxAI.Setup(targetTable, index, _exitPoint);
            }
            else
            {
                Debug.Log("Hết bàn rồi, khách không vào!");
            }
        }
    }
}
