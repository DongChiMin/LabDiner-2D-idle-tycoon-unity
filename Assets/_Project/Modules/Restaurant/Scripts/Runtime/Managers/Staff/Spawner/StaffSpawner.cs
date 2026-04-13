using System.Collections.Generic;
using LabDiner.Shared;
using LabDiner.Shared.SO;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public abstract class StaffSpawner<TStaff> : MonoBehaviour, IStaffUnboxer
        where TStaff : Component, IStaff
    {
        [Header("Base Settings")]
        [SerializeField] protected LevelUpgradeEvent _onUpgradeEvent;
        [SerializeField] protected TStaff _staffPrefab;
        
        // Dùng class base StaffManager trực tiếp để bỏ được tham số TManager thứ 3
        [SerializeField] protected List<MonoBehaviour> _mainManagers; 
        
        [SerializeField] protected List<Transform> _restPositions;
        [SerializeField] protected int _initialCount = 1;
        [SerializeField] protected bool _spawnInBox = true;

        protected List<TStaff> _spawnedStaffs = new List<TStaff>();

        protected virtual void Start() => Spawn(_initialCount);

        protected virtual void OnEnable() => _onUpgradeEvent.Register(HandleUpgrade);
        protected virtual void OnDisable() => _onUpgradeEvent.Unregister(HandleUpgrade);

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

            TStaff staff = Instantiate(_staffPrefab, restPoint.position, Quaternion.identity, transform);
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
            if(staff is TStaff concreteStaff)
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
    }
}