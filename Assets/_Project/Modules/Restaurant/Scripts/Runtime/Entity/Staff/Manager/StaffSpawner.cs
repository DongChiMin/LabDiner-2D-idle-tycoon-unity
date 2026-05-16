using System.Collections.Generic;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Pooling;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public partial class StaffSpawner : MonoBehaviour, IStaffUnboxer, ILevelInitializable
    {
        [Header("Events")]
        [SerializeField] private StaffUpgradeEvent _onUpgradeStaff;

        [Header("Base Settings")]
        [SerializeField] private List<Staff> _prefabRepository; // Danh sách prefab nhân viên có thể spawn
        [SerializeField] private Transform _spawnParent;

        [SerializeField] private List<Transform> _restPositions;
        [SerializeField] private bool _spawnInBox = true;

        [Header("[Runtime]")]

        [SerializeField] private List<Staff> _spawnedStaffs = new List<Staff>();

        void OnEnable()
        {
            _onUpgradeStaff.Register(HandleUpgradeStaff);

        }
        void OnDisable()
        {
            _onUpgradeStaff.Unregister(HandleUpgradeStaff);
        }

        public void Init(LevelConfigSO config)
        {
            foreach (Staff staffPrefab in config.InitialStaffs)
            {
                Staff staff = CreateInstance(staffPrefab);
                staff.gameObject.SetActive(true);
            }
        }

        private void HandleUpgradeStaff(StaffUpgradeSO upgradeSO)
        {
            switch (upgradeSO.UpgradeType)
            {
                case StaffUpgradeType.Quantity:
                    UpgradeQUantity(upgradeSO);
                    break;
                case StaffUpgradeType.MoveSpeed:
                    UpgradeMoveSpeed(upgradeSO);
                    break;
                default:
                    Debug.LogWarning($"Unhandled staff upgrade type: {upgradeSO.UpgradeType}");
                    break;
            }
        }

        private void UpgradeQUantity(StaffUpgradeSO upgradeSO)
        {
            int quantity = Mathf.RoundToInt(upgradeSO.UpgradeValue);
            StaffType target = upgradeSO.Target;
            List<Staff> staffToSpawn = new List<Staff>();

            //Sinh ra số lượng nhân viên mới dựa trên quantity và targetTypes

                if (target == StaffType.All)
                {
                    //Nếu target là All, sinh ra nhân viên cho tất cả các loại trạm
                    foreach (Staff prefab in _prefabRepository)
                    {
                        List<Staff> staff = CreateInstance(prefab, quantity);
                        staffToSpawn.AddRange(staff);
                    }
                }
                else
                {
                    Staff prefab = _prefabRepository.Find(p => p.StaffType == target);
                    if (prefab != null && prefab.StaffType != StaffType.All)
                    {
                        List<Staff> staff = CreateInstance(prefab, quantity);
                        staffToSpawn.AddRange(staff);
                    }
                    else
                    {
                        Debug.LogWarning($"No prefab found for staff type: {target}");
                    }
                }

                //Nếu có nhân viên mới được sinh ra, kiểm tra xem có spawn trong hộp hay không
                if (_spawnInBox)
                {
                    foreach (var staff in staffToSpawn)
                    {
                        staff.gameObject.SetActive(false);
                        var box = PoolContext.Instance.StaffBoxPool.Get(staff.RestPosition.position, Quaternion.identity);
                        box.Setup(staff, this);
                    }
                }
                else
                {
                    foreach (var staff in staffToSpawn)
                    {
                        UnboxStaff(staff);
                    }
                }
            
        }

        private void UpgradeMoveSpeed(StaffUpgradeSO upgradeSO)
        {
            StaffType target = upgradeSO.Target;
            foreach (Staff staff in _spawnedStaffs)
            {
                if (target == staff.StaffType || target == StaffType.All)
                {
                    staff.UpgradeMoveSpeed(upgradeSO.UpgradeValue);
                }
            }
        }

        private List<Staff> CreateInstance(Staff prefab, int quantity)
        {
            List<Staff> createdStaffs = new List<Staff>();
            for (int i = 0; i < quantity; i++)
            {
                Staff staff = CreateInstance(prefab);
                createdStaffs.Add(staff);
            }
            return createdStaffs;
        }

        protected Staff CreateInstance(Staff prefab)
        {
            int index = _spawnedStaffs.Count;
            Transform restPoint = _restPositions[index % _restPositions.Count];

            if (index >= _restPositions.Count)
                Debug.LogWarning($"Not enough rest positions for {typeof(Staff).Name}!");

            Staff staff = Instantiate(prefab, restPoint.position, Quaternion.identity, _spawnParent);
            staff.RestPosition = restPoint;
            _spawnedStaffs.Add(staff);
            return staff;
        }

        public void UnboxStaff(Component staff)
        {
            if (staff is Staff concreteStaff)
            {
                concreteStaff.gameObject.SetActive(true);
            }
        }
    }
}