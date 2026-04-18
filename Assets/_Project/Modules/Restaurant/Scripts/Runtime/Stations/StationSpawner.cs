using System.Collections.Generic;
using LabDiner.Shared;
using LabDiner.Shared.SO;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class StationSpawner : MonoBehaviour, IStationUnboxer
    {
        #if UNITY_EDITOR
        public List<Transform> SpawnPoints => _spawnPoints;     //chỉ dùng cho debug 
        #endif

        [Header("Base Settings")]
        [SerializeField] private Station _stationPrefab;
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private Transform _spawnParent;
        [SerializeField] protected bool _spawnInBox = true;

        protected List<Station> _spawnedStations = new List<Station>();

        #region API

        public Station RequestSpawn()
        {
            int nextIndex = _spawnedStations.Count;

            if (nextIndex < _spawnPoints.Count)
            {
                // Sinh máy mới ngay tại vị trí đã định sẵn
                Station newStation = Instantiate(_stationPrefab, _spawnPoints[nextIndex].position, Quaternion.identity, _spawnParent);
                newStation.SetStatus(false);
                _spawnedStations.Add(newStation);

                if (_spawnInBox)
                {
                    newStation.gameObject.SetActive(false);
                    var box = PoolContext.Instance.StationBoxPool.Get(newStation.transform.position, Quaternion.identity);
                    box.Setup(newStation, this);
                }
                else
                {
                    newStation.SetStatus(true);
                }
                    

                return newStation;
            }
            else
            {
                Debug.LogWarning("Đã hết chỗ đặt máy trên bàn này!");
                return null;
            }
        }

        public void UnboxStation(Station station)
        {
            station.gameObject.SetActive(true);
            station.SetStatus(true);
        }

        #endregion


        void OnDrawGizmos()
        {
            foreach(Transform spawnPoint in _spawnPoints)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(spawnPoint.position, Vector3.one * 1.5f);
            }
        }
    }
}