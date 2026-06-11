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
        [SerializeField] private Transform _stationPosParent;
        [SerializeField] private Transform _spawnParent;
        [SerializeField] protected bool _spawnInBox = true;

        protected List<Station> _spawnedStations = new List<Station>();

        void Awake()
        {
            _stationPos = new List<StationPosition>(_stationPosParent.GetComponentsInChildren<StationPosition>());
        }

        void Start()
        {
            #if UNITY_EDITOR
            ValidateData();
            #endif
        }

        #region API

        public Station RequestSpawn(bool isFromLoadProgress = false)
        {
            int nextIndex = _spawnedStations.Count;

            if (nextIndex < _stationPos.Count)
            {
                // Sinh máy mới ngay tại vị trí đã định sẵn, đặt lại vị trí làm việc
                Station newStation = Instantiate(_stationPrefab, _stationPos[nextIndex].spawnPos.position, Quaternion.identity, _spawnParent);
                newStation.SetWorkPos(_stationPos[nextIndex].workPos);
                newStation.SetStatus(false);
                _spawnedStations.Add(newStation);

                if (_spawnInBox && !isFromLoadProgress)
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

        #region EDITOR ONLY
        #if UNITY_EDITOR

        private void ValidateData()
        {
            foreach(var stationPos in _stationPos)
            {
                if(stationPos == null)
                {
                    Debug.LogError($"StationSpawner '{gameObject.name}' có một StationPosition bị gán null, vui lòng kiểm tra lại dữ liệu để đảm bảo tất cả các StationPosition đều được gán hợp lệ!");
                    return;
                }

                if(stationPos.spawnPos == null || stationPos.workPos == null)
                {
                    Debug.LogError($"StationSpawner '{gameObject.name}' có StationPosition nào đó chưa được gán đầy đủ spawnPos hoặc workPos, vui lòng kiểm tra lại dữ liệu để đảm bảo tất cả các StationPosition đều có spawnPos và workPos hợp lệ!");
                    return;
                }
            }
        }
        #endif 
        #endregion
    }
}