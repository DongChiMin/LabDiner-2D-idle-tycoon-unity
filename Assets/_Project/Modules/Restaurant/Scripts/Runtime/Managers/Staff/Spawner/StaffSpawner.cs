using System.Collections.Generic;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Pooling;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public abstract class StaffSpawner<TStaff> : MonoBehaviour, IStaffUnboxer
        where TStaff : Component, IStaff
    {
        [Header("Events")]
        [SerializeField] private GlobalUpgradeEvent _onUpgradeAllStaffMoveSpeed;

        [Header("Base Settings")]
        [SerializeField] protected GlobalUpgradeEvent _onUpgradeQuantity;
        [SerializeField] protected TStaff _staffPrefab;
        [SerializeField] private Transform _spawnParent;

        // Dùng class base StaffManager trực tiếp để bỏ được tham số TManager thứ 3
        [SerializeField] protected List<MonoBehaviour> _mainManagers;

        [SerializeField] protected List<Transform> _restPositions;
        [SerializeField] protected int _initialCount = 1;
        [SerializeField] protected bool _spawnInBox = true;

        [Header("[DEBUG]")]

        [SerializeField] protected List<TStaff> _spawnedStaffs = new List<TStaff>();

        protected virtual void Start() => Spawn(_initialCount);

        protected virtual void OnEnable()
        {
            _onUpgradeQuantity.Register(HandleUpgrade);
            _onUpgradeAllStaffMoveSpeed.Register(HandleUpgradeAllStaffMoveSpeed);

        }
        protected virtual void OnDisable()
        {
            _onUpgradeQuantity.Unregister(HandleUpgrade);
            _onUpgradeAllStaffMoveSpeed.Unregister(HandleUpgradeAllStaffMoveSpeed);
        }

        private void HandleUpgrade(BaseUpgradeSO upgradeSO)
        {
            int amount = Mathf.RoundToInt(upgradeSO.UpgradeValue);
            OnUpgradeReceived(amount);
        }

        protected TStaff CreateInstance()
        {
            int index = _spawnedStaffs.Count;
            Transform restPoint = _restPositions[index % _restPositions.Count];

            if (index >= _restPositions.Count)
                Debug.LogWarning($"Not enough rest positions for {typeof(TStaff).Name}!");

            TStaff staff = Instantiate(_staffPrefab, restPoint.position, Quaternion.identity, _spawnParent);
            staff.RestPosition = restPoint;
            _spawnedStaffs.Add(staff);
            return staff;
        }

        protected void Spawn(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                TStaff staff = CreateInstance();
                // Spawn ban đầu thì active luôn và assign cho managers
                AssignToManagers(staff);
            }
        }

        protected void OnUpgradeReceived(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                TStaff staff = CreateInstance();

                if (_spawnInBox)
                {
                    staff.gameObject.SetActive(false);
                    var box = PoolContext.Instance.StaffBoxPool.Get(staff.RestPosition.position, Quaternion.identity);
                    box.Setup(staff, this);
                }
                else
                {
                    UnboxStaff(staff);
                }
            }
        }

        public virtual void UnboxStaff(Component staff)
        {
            if (staff is TStaff concreteStaff)
            {
                concreteStaff.gameObject.SetActive(true);
                AssignToManagers(concreteStaff);
            }
        }

        // Tách riêng hàm này để dùng chung cho cả Spawn và UnboxStaff
        private void AssignToManagers(TStaff staff)
        {
            foreach (var mono in _mainManagers)
            {
                if (mono is IStaffRegisterable<TStaff> manager)
                {
                    manager.AssignNewStaff(staff);
                }
            }
        }

        private void HandleUpgradeAllStaffMoveSpeed(BaseUpgradeSO upgradeSO)
        {
            foreach (IStaff staff in _spawnedStaffs)
            {
                staff.UpgradeMoveSpeed(upgradeSO.UpgradeValue);
            }
        }

        #region UNITY EDITOR
#if UNITY_EDITOR
        [Header("[GIZMOS SETTINGS]")]
        [SerializeField] private bool _showGizmos = true;
        [SerializeField] private Color _gizmoColor = Color.cyan;
        [SerializeField] private Vector3 _restPointDimensions = new Vector3(0.6f, 1.2f, 0.1f); // Hình vuông cao cao

        protected virtual void OnDrawGizmos()
        {
            if (!_showGizmos || _restPositions == null) return;

            Gizmos.color = _gizmoColor;

            for (int i = 0; i < _restPositions.Count; i++)
            {
                if (_restPositions[i] == null) continue;

                Vector3 pos = _restPositions[i].position;

                // Vẽ hình hộp đứng (đại diện cho vị trí nhân viên đứng nghỉ)
                // Center được offset lên một nửa chiều cao để hình nằm trên mặt sàn
                Vector3 center = pos + Vector3.up * (_restPointDimensions.y * 0.5f);
                Gizmos.DrawWireCube(center, _restPointDimensions);

                // Reset lại màu chính cho vòng lặp sau
                Gizmos.color = _gizmoColor;

                // Ghi số thứ tự điểm nghỉ (tùy chọn)
                UnityEditor.Handles.Label(pos + Vector3.up * (_restPointDimensions.y + 0.2f), $"Rest {i}");
            }
        }
#endif
        #endregion
    }
}