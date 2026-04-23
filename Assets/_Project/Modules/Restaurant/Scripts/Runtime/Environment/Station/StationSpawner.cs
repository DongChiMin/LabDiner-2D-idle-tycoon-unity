using System.Collections.Generic;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Pooling;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    public class StationSpawner : MonoBehaviour, IStationUnboxer
    {
        public List<StationPosition> StationPos => _stationPos;
        [Header("Base Settings")]
        [SerializeField] private Station _stationPrefab;
        [SerializeField] private List<StationPosition> _stationPos;
        [SerializeField] private Transform _spawnParent;
        [SerializeField] protected bool _spawnInBox = true;

        protected List<Station> _spawnedStations = new List<Station>();

        #region API

        public Station RequestSpawn()
        {
            int nextIndex = _spawnedStations.Count;

            if (nextIndex < _stationPos.Count)
            {
                // Sinh máy mới ngay tại vị trí đã định sẵn, đặt lại vị trí làm việc
                Station newStation = Instantiate(_stationPrefab, _stationPos[nextIndex].spawnPos.position, Quaternion.identity, _spawnParent);
                newStation.SetWorkPos(_stationPos[nextIndex].workPos);
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
            foreach(StationPosition stationPos in _stationPos)
            {
                if( stationPos.spawnPos == null || stationPos.workPos == null)
                {
                    continue;
                }
                
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(stationPos.spawnPos.position, Vector3.one * 1.5f);
            
                Gizmos.color = Color.yellow;
                Vector3 pos = stationPos.workPos.position;
                    Vector3 center = pos + Vector3.up * 0.5f;
                    Gizmos.DrawWireCube(center, new Vector3(0.6f, 1.2f, 0.1f));
            }
        }
    }
}