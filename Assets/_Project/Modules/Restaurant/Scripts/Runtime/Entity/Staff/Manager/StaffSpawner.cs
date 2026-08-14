using System.Collections.Generic;
using LabDiner.LevelSystem.Domain;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Pooling;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public class StaffSpawner : MonoBehaviour, IStaffUnboxer, ILevelInitializable, ILevelRebuildable
    {

        [Header("Data")]
        [SerializeField] private StaffPrefabRepository _staffRepository;

        [Header("Events")]
        [SerializeField] private StaffUpgradeEvent _onUpgradeStaff;
        [SerializeField] private StaffEvent _onStaffSpawn;
        [SerializeField] private StaffEvent _onStaffListClear;

        [Header("Base Settings")]
        [SerializeField] private bool _spawnInBox = true;

        [Header("References")]
        [SerializeField] private StaffRestPosController _restPosController;
        [SerializeField] private Transform _spawnParent;

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
            _spawnedStaffs.Clear();
            _onStaffListClear?.Raise(null);

            
            foreach (InitialStaffQuantity staffQuantity in config.InitialStaffs)
            {
                //Đọc dữ liệu từ levelConfig xem cần spawn các loại nhân viên nào
                int quantity = staffQuantity.Quantity;
                StaffType targetType = staffQuantity.StaffType;

                //Tìm prefab tương ứng với loại nhân viên trong StaffRepository
                Staff prefab = _staffRepository.GetStaffPrefab(targetType);
                if (prefab != null)
                {
                    //Sinh ra số lượng nhân viên tương ứng
                    for (int i = 0; i < quantity; i++)
                    {
                        Staff staff = CreateInstance(prefab);
                        staff.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogWarning($"No prefab found for staff type: {staffQuantity.StaffType}");
                }
            }
        }

        public void Rebuild()
        {
            _restPosController.Rebuild();
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
            bool isFromLoadProgress = upgradeSO.IsFromLoadProgress;
            int quantity = Mathf.RoundToInt(upgradeSO.UpgradeValue);
            StaffType target = upgradeSO.Target;
            List<Staff> staffToSpawn = new List<Staff>();

            //Sinh ra số lượng nhân viên mới dựa trên quantity và targetTypes

            if (target == StaffType.All)
            {
                List<Staff> _allPrefabs = _staffRepository.GetAllStaffPrefabs();
                //Nếu target là All, sinh ra nhân viên cho tất cả các loại trạm
                foreach (Staff prefab in _allPrefabs)
                {
                    List<Staff> staff = CreateInstance(prefab, quantity);
                    staffToSpawn.AddRange(staff);
                }
            }
            else
            {
                Staff prefab = _staffRepository.GetStaffPrefab(target);
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
            //Nếu load từ progress thì spawn trực tiếp nhân viên chứ ko spawn hộp
            if (_spawnInBox && !isFromLoadProgress)
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
            //Tìm vị trí nghỉ ngơi cho nhân viên mới dựa trên số lượng nhân viên đã spawn và staffType của vị trí đó
            StaffType staffType = prefab.StaffType;
            StaffRestPos restPos = _restPosController.GetAvailableRestPos(staffType);

            if(restPos != null)
            {
                restPos.SetOccupied(true);
                Transform restPoint = restPos.transform;
                Staff staff = Instantiate(prefab, restPoint.position, Quaternion.identity, _spawnParent);
                staff.RestPosition = restPoint;
                _spawnedStaffs.Add(staff);
                _onStaffSpawn?.Raise(staff);
                return staff;
            }

            Debug.LogWarning($"No available rest position for staff type: {staffType}");
            return null;
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